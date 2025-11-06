using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Core.Services;
using EmployeeManagement.Api.DTOs;
using EmployeeManagement.Api.Mapping;
using EmployeeManagement.Core.Entities;

namespace EmployeeManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _service;

        public EmployeesController(IEmployeeService service)
        {
            _service = service;
        }

        /// <summary>
        /// Returns a paginated list of items with optional search and sorting.
        /// Allows filtering by search term and sorting by a specified column and direction.
        /// Supports pagination through page number and page size parameters.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search,[FromQuery] string? sortColumn, [FromQuery] string? sortDirection, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var (data, total) = await _service.SearchAndPaginateAsync(search, sortColumn, sortDirection, pageNumber, pageSize);
            var result = new
            {
                totalCount = total,
                items = data.Select(e => e.ToReadDto())
            };
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a single record based on the provided ID.
        /// Returns the item if found, otherwise returns a not found response.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return Ok(entity.ToReadDto());
        }

        /// <summary>
        /// Creates a new employee record using the provided details.
        /// Accepts the data from the request body and adds it to the database.
        /// Returns the created record or an appropriate error response.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmployeeCreateDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var entity = dto.ToEntity();
            var created = await _service.CreateAsync(entity);
            var read = created.ToReadDto();

            return CreatedAtAction(nameof(GetById), new { id = read.EmployeeId }, read);
        }

        /// <summary>
        /// Updates an existing employee record based on the provided ID.
        /// Accepts updated details from the request body.
        /// Returns the updated record or an appropriate error response.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] EmployeeUpdateDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var dummy = new Employee();                
            dto.MapToEntity(dummy);

            var updated = await _service.UpdateAsync(id, dummy);
            return Ok(updated.ToReadDto());
        }

        /// <summary>
        /// Deletes an employee record based on the provided ID.
        /// Removes the record from the database if it exists.
        /// Returns a success or not found response.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Retrieves the total count of records.
        /// Useful for displaying summary or dashboard statistics.
        /// Returns the count value in the response.
        /// </summary>
        [HttpGet("counts")]
        public async Task<IActionResult> GetCounts()
        {
            var (total, active) = await _service.GetCountsAsync();
            return Ok(new { total, active });
        }


    }
}
