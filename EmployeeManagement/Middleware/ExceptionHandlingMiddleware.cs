using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invokes the next middleware in the pipeline and handles any exceptions.
        /// Wraps the request execution in a try-catch block.
        /// Routes exceptions to the custom handler.
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Handles exceptions by mapping them to appropriate HTTP status codes and messages.
        /// Creates a standardized JSON response for the client.
        /// Writes the response to the HTTP context.
        /// </summary>
        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var (code, message) = ex switch
            {
                KeyNotFoundException => (HttpStatusCode.NotFound, ex.Message),
                InvalidOperationException => (HttpStatusCode.BadRequest, ex.Message), 
                DbUpdateException => (HttpStatusCode.Conflict, "A database error occurred."),
                _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
            };

            var payload = new
            {
                statusCode = (int)code,
                message,
                details = context.Request.Path.Value
            };

            var json = JsonSerializer.Serialize(payload);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(json);
        }
    }

    /// <summary>Adds the global exception handling middleware to the pipeline.</summary>
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
            => app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
