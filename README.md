# Employee Management System (ASP.NET Core + EF Core)

A clean, production-style API to manage employees with CRUD, search, sorting, pagination, counts, and global exception handling.

## Tech Stack
- ASP.NET Core Web API • Entity Framework Core • SQL Server • jQuery (sample UI)

## Prerequisites
- .NET SDK 8 (or your project’s version)
- SQL Server (LocalDB/Express/Developer)
- EF Core tools: `dotnet tool install -g dotnet-ef` (if needed)

## Setup

### 1) Configure Database
Update `EmployeeManagement/appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=EmployeeDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}

docs: update README
