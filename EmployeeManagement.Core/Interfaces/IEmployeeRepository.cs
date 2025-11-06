using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeManagement.Core.Entities;

namespace EmployeeManagement.Core.Interfaces
{
    public interface IEmployeeRepository
    {
        /// <summary>Retrieves all employees.</summary>
        Task<IEnumerable<Employee>> GetAllAsync();

        /// <summary>Retrieves an employee by ID.</summary>
        Task<Employee?> GetByIdAsync(int id);

        /// <summary>Adds a new employee.</summary>
        Task AddAsync(Employee employee);

        /// <summary>Updates an existing employee.</summary>
        Task UpdateAsync(Employee employee);

        /// <summary>Deletes an employee by ID.</summary>

        Task DeleteAsync(int id);

        /// <summary>Checks if an email already exists, optionally excluding a specific employee.</summary>
        Task<bool> EmailExistsAsync(string email, int? excludeEmployeeId = null);

        /// <summary>Returns filtered and paginated employee data along with total count.</summary>
        Task<(IEnumerable<Employee> Data, int TotalCount)> SearchAndPaginateAsync(string? searchText, string? sortColumn, string? sortDirection,int pageNumber,int pageSize);
    }
}
