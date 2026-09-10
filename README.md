# 👥 Employee Management System

A full-featured Employee Management System built with **ASP.NET Core MVC, using Entity Framework Core with ASP.NET Core Identity for authentication, custom policy-based authorization, and claims-based access control.

## 🚀 Tech Stack

- .NET 8
- ASP.NET Core MVC
- Entity Framework Core
- ASP.NET Core Identity (Authentication, Roles & Claims)
- SQL Server
- Bootstrap (UI)

## ✨ Features

- User Authentication — Login/Register with ASP.NET Core Identity, enforced strong password policy (min. 10 characters, 3 unique characters)
- Domain-Restricted Registration — Custom validation attribute restricts sign-ups to a specific company email domain, with live email-availability check (AJAX remote validation)
- Employee Management — Add, edit, delete, and view employees with photo upload and department assignment (HR / IT / Payroll)
- Role-Based Admin Panel — Create, edit, delete roles; assign/remove users from roles
- Claims-Based Permissions — Fine-grained access control using custom claims (Create Role, Edit Role, Delete Role)
- Custom Authorization Policies — Policy-based access using custom `IAuthorizationHandler` implementations:
  - `SuperAdminHandler` — grants full access to Super Admins
  - `CanEditOnlyOtherAdminRolesAndClaimsHandler` — prevents an Admin from editing their own roles/claims
- Centralized Error Handling — Custom error pages for 404 and unhandled exceptions via a dedicated `ErrorController` and status-code re-execution middleware
- Repository Pattern — Clean separation of data access logic (`IEmployeeRepository`) with both SQL (`SQLEmployeeRepository`) and in-memory mock (`MockEmployeeRepository`) implementations, useful for testing

## 🏗️ Architecture

Authentication and authorization are built on ASP.NET Core Identity, extended with custom authorization policies and handlers to implement enterprise-style access control — for example, ensuring an Admin cannot modify their own permissions, while a Super Admin can manage everyone.

Employee data access follows the Repository Pattern, decoupling controllers from EF Core, which makes it easy to swap between a real SQL Server-backed repository and an in-memory mock for development/testing.

## 📂 Project Structure

- Controllers/ — AccountController (auth), AdministrationController (roles/claims/users), HomeController (employee CRUD), ErrorController (custom error handling)
- Models/ — Employee, Dept (enum), ApplicationUser, AppDbContext, Repository implementations
- ViewModels/ — Strongly-typed models for Login, Register, Employee Create/Edit, Role & Claims management
- Security/ — Custom authorization handlers and requirements
- Utilities/ — Custom validation attributes (e.g., domain-restricted email)
- Views/ — Razor views for Account, Home (Employee), and Administration modules
- Migrations/ — EF Core Migrations

## ⚙️ Getting Started

1. Clone the repository
2. Update the connection string in `appsettings.json` with your SQL Server details
3. Run migrations: `dotnet ef database update`
4. Run the project: `dotnet run`

## 📌 Note
Connection strings in `appsettings.json` are set to placeholder values for security. Replace them with your own SQL Server credentials before running locally.
