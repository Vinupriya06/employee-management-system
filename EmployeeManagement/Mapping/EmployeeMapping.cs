using EmployeeManagement.Api.DTOs;
using EmployeeManagement.Core.Entities;

namespace EmployeeManagement.Api.Mapping
{
    public static class EmployeeMapping
    {
        /// <summary>Converts an Employee entity to an EmployeeReadDto.</summary>
        public static EmployeeReadDto ToReadDto(this Employee e) => new()
        {
            EmployeeId = e.EmployeeId,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            DateOfBirth = e.DateOfBirth,
            IsActive = e.IsActive,
            CreatedDate = e.CreatedDate
        };

        /// <summary>Converts an EmployeeCreateDto to an Employee entity.</summary>
        public static Employee ToEntity(this EmployeeCreateDto dto) => new()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            DateOfBirth = dto.DateOfBirth,
            IsActive = dto.IsActive
        };

        /// <summary>Maps the values from EmployeeUpdateDto onto an existing Employee entity.</summary>
        public static void MapToEntity(this EmployeeUpdateDto dto, Employee entity)
        {
            entity.FirstName = dto.FirstName;
            entity.LastName = dto.LastName;
            entity.Email = dto.Email;
            entity.DateOfBirth = dto.DateOfBirth;
            entity.IsActive = dto.IsActive;
        }
    }
}
