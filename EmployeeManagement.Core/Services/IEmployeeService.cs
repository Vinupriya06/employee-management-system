using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeManagement.Core.Entities;

namespace EmployeeManagement.Core.Services
{
    public interface IEmployeeService
    {
        /// <summary>Retrieves all employees.</summary>
        Task<IEnumerable<Employee>> GetAllAsync();

        /// <summary>Retrieves an employee by its ID.</summary>
        Task<Employee?> GetByIdAsync(int id);

        /// <summary>Creates a new employee record.</summary>
        Task<Employee> CreateAsync(Employee employee);

        /// <summary>Updates an existing employee using the given ID.</summary>
        Task<Employee> UpdateAsync(int id, Employee employee);

        /// <summary>Deletes an employee by its ID.</summary>
        Task DeleteAsync(int id);

        /// <summary>Retrieves paginated and optionally filtered and sorted employee data.</summary>
        Task<(IEnumerable<Employee> Data, int TotalCount)> SearchAndPaginateAsync(string? searchText, string? sortColumn, string? sortDirection, int pageNumber, int pageSize);

        /// <summary>Gets total employee count and active employee count.</summary>
        Task<(int total, int active)> GetCountsAsync();

    }
}
