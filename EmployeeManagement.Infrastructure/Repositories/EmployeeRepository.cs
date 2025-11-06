using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Interfaces;
using EmployeeManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _db;

        public EmployeeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Retrieves and returns all employee records from the database.
        /// </summary>
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _db.Employees.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Retrieves a single employee record based on the provided ID.
        /// </summary>
        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _db.Employees.FindAsync(id);
        }

        /// <summary>
        /// Adds a new employee record to the database.
        /// </summary>
        public async Task AddAsync(Employee employee)
        {
            await _db.Employees.AddAsync(employee);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing employee record in the database.
        /// </summary>
        public async Task UpdateAsync(Employee employee)
        {
            _db.Employees.Update(employee);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes an existing employee record from the database by ID.
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var entity = await _db.Employees.FindAsync(id);
            if (entity != null)
            {
                entity.IsActive = false; 
                _db.Employees.Update(entity);
                await _db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Checks whether an email is already associated with an employee, with an option to exclude a specific employee ID.
        /// </summary>
        public async Task<bool> EmailExistsAsync(string email, int? excludeEmployeeId = null)
        {
            return await _db.Employees
                .AnyAsync(e => e.Email == email && (!excludeEmployeeId.HasValue || e.EmployeeId != excludeEmployeeId.Value));
        }

        /// <summary>
        /// Retrieves filtered, sorted, and paginated employee data along with the total record count.
        /// </summary>
        public async Task<(IEnumerable<Employee> Data, int TotalCount)> SearchAndPaginateAsync(string? searchText,  string? sortColumn, string? sortDirection, int pageNumber, int pageSize)
        {
            var query = _db.Employees.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var s = searchText.Trim();
                query = query.Where(e =>
                    e.FirstName.Contains(s) ||
                    e.LastName.Contains(s) ||
                    e.Email.Contains(s));
            }

            bool desc = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);
            query = sortColumn switch
            {
                "FirstName" => desc ? query.OrderByDescending(e => e.FirstName) : query.OrderBy(e => e.FirstName),
                "LastName" => desc ? query.OrderByDescending(e => e.LastName) : query.OrderBy(e => e.LastName),
                "Email" => desc ? query.OrderByDescending(e => e.Email) : query.OrderBy(e => e.Email),
                "DateOfBirth" => desc ? query.OrderByDescending(e => e.DateOfBirth) : query.OrderBy(e => e.DateOfBirth),
                "IsActive" => desc ? query.OrderByDescending(e => e.IsActive) : query.OrderBy(e => e.IsActive),
                "CreatedDate" => desc ? query.OrderByDescending(e => e.CreatedDate) : query.OrderBy(e => e.CreatedDate),
                _ => query.OrderBy(e => e.EmployeeId) // default
            };

            var total = await query.CountAsync();

            int skip = (pageNumber <= 1 ? 0 : (pageNumber - 1) * pageSize);
            var data = await query.Skip(skip).Take(pageSize).ToListAsync();

            return (data, total);
        }
    }
}
