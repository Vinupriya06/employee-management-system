using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Interfaces;
using EmployeeManagement.Core.Services;
using System.Linq;


namespace EmployeeManagement.Infrastructure.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repo;

        public EmployeeService(IEmployeeRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Retrieves and returns all employee records.
        /// </summary>
        public Task<IEnumerable<Employee>> GetAllAsync() => _repo.GetAllAsync();

        /// <summary>
        /// Retrieves a specific employee record by its ID.
        /// </summary>
        public Task<Employee?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        /// <summary>
        /// Creates a new employee record in the database.
        /// </summary>
        public async Task<Employee> CreateAsync(Employee employee)
        {
            if (await _repo.EmailExistsAsync(employee.Email))
                throw new InvalidOperationException("Email already exists.");

            await _repo.AddAsync(employee);
            return employee;
        }

        /// <summary>
        /// Updates an existing employee record based on the provided ID.
        /// </summary>
        public async Task<Employee> UpdateAsync(int id, Employee employee)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing is null)
                throw new KeyNotFoundException("Employee not found.");

            if (await _repo.EmailExistsAsync(employee.Email, excludeEmployeeId: id))
                throw new InvalidOperationException("Email already exists.");

            existing.FirstName = employee.FirstName;
            existing.LastName = employee.LastName;
            existing.Email = employee.Email;
            existing.DateOfBirth = employee.DateOfBirth;
            existing.IsActive = employee.IsActive;

            await _repo.UpdateAsync(existing);
            return existing;
        }

        /// <summary>
        /// Deletes an employee record based on the provided ID.
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing is null)
                throw new KeyNotFoundException("Employee not found.");

            await _repo.DeleteAsync(id);
        }

        /// <summary>
        /// Returns filtered, sorted, and paginated employee data along with the total count.
        /// </summary>
        public Task<(IEnumerable<Employee> Data, int TotalCount)> SearchAndPaginateAsync( string? searchText, string? sortColumn, string? sortDirection, int pageNumber, int pageSize) => _repo.SearchAndPaginateAsync(searchText, sortColumn, sortDirection, pageNumber, pageSize);

        /// <summary>
        /// Retrieves total employee count and active employee count.
        /// </summary>
        public async Task<(int total, int active)> GetCountsAsync()
        {
            var all = await _repo.GetAllAsync();
            var total = all.Count();
            var active = all.Count(e => e.IsActive);
            return (total, active);
        }



    }
}
