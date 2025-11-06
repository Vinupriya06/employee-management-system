using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Employee> Employees => Set<Employee>();

        /// <summary>Configures entity relationships and model settings.</summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var employees = modelBuilder.Entity<Employee>();

            employees.ToTable("Employees");

            employees.HasIndex(e => e.Email).IsUnique();

            employees.Property(e => e.FirstName).HasMaxLength(50).IsRequired();
            employees.Property(e => e.LastName).HasMaxLength(50).IsRequired();
            employees.Property(e => e.Email).HasMaxLength(100).IsRequired();

            employees.Property(e => e.IsActive).HasDefaultValue(true);
            employees.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
        }
    }
}
