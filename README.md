# ProductSphere

A production-ready RESTful Web API built with **ASP.NET Core 8** using **Clean Architecture** principles. 
The project demonstrates enterprise-level backend development practices including secure authentication, layered architecture, testing, logging, and containerization.

> GitHub Repository: https://github.com/pankajs5535/ProductSphere

---

# Project Overview

ProductSphere is a RESTful backend API developed as part of a technical assessment. It provides CRUD operations for **Products** and **Items**, 
while following modern software engineering principles to ensure scalability, maintainability, security, and testability.

The solution is organized using **Clean Architecture**, separating responsibilities into Domain, Application, Infrastructure, and API layers.
It also includes JWT-based authentication, role-based authorization, validation, exception handling, logging, comprehensive testing, and Docker support.

---

# Features

- Product CRUD Operations
- Item CRUD Operations
- JWT Authentication
- Refresh Token Authentication
- BCrypt Password Hashing
- Role-Based Authorization
- Generic Repository Pattern
- Repository Pattern
- Unit of Work Pattern
- AutoMapper
- FluentValidation
- Validation Filter
- Global Exception Handling Middleware
- Serilog Structured Logging
- API Versioning
- Swagger / OpenAPI Documentation
- SQL Server with Entity Framework Core
- Unit Testing (xUnit, Moq, FluentAssertions)
- Integration Testing (WebApplicationFactory)
- Docker Support

---

# Technology Stack

| Category | Technology |
|----------|------------|
| Framework | ASP.NET Core 8 Web API |
| Language | C# |
| Database | SQL Server |
| ORM | Entity Framework Core |
| Authentication | JWT + Refresh Token |
| Authorization | Role-Based Authorization |
| Mapping | AutoMapper |
| Validation | FluentValidation |
| Logging | Serilog |
| API Documentation | Swagger / OpenAPI |
| Unit Testing | xUnit, Moq, FluentAssertions |
| Integration Testing | WebApplicationFactory |
| Containerization | Docker |
| Architecture | Clean Architecture |


---

# Architecture

ProductSphere follows the **Clean Architecture** pattern, separating the application into independent layers with clear responsibilities. This approach improves maintainability, scalability, testability, and separation of concerns.

```
                 +----------------------+
                 |   ProductSphere.API  |
                 +----------+-----------+
                            |
                            v
                 +----------------------+
                 | ProductSphere.Application |
                 +----------+-----------+
                            |
                            v
                 +----------------------+
                 | ProductSphere.Domain |
                 +----------+-----------+
                            |
                            v
                 +----------------------+
                 | ProductSphere.Infrastructure |
                 +----------------------+
```

### API Layer

Responsible for handling HTTP requests and responses.

**Contains:**

- Controllers
- Filters
- Middleware
- Extensions
- Program Configuration
- Swagger Configuration

---

### Application Layer

Contains the application's business contracts and shared components.

**Contains:**

- DTOs
- Repository Interfaces
- Service Interfaces
- AutoMapper Profiles
- FluentValidation Validators

---

### Domain Layer

Represents the core business model of the application.

**Contains:**

- Entities
- Custom Exceptions

---

### Infrastructure Layer

Responsible for data access, authentication, and external implementations.

**Contains:**

- Entity Framework Core DbContext
- Repository Implementations
- Generic Repository
- Unit of Work
- Authentication Services
- Business Services
- Entity Configurations

---

# Project Structure

```text
ProductSphere
│
├── Src
│   │
│   ├── ProductSphere.API
│   │   ├── Controllers
│   │   │   ├── ProductController.cs
│   │   │   ├── ItemController.cs
│   │   │   └── AuthController.cs
│   │   │
│   │   ├── Extensions
│   │   │   ├── ServiceCollectionExtensions.cs
│   │   │   └── ApplicationBuilderExtensions.cs
│   │   │
│   │   ├── Filters
│   │   │   └── ValidationFilter.cs
│   │   │
│   │   ├── Middleware
│   │   │   └── GlobalExceptionMiddleware.cs
│   │   │
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   └── ProductSphere.API.csproj
│   │
│   ├── ProductSphere.Application
│   │   ├── DTOs
│   │   │   ├── ProductDtos
│   │   │   │   ├── ProductDto.cs
│   │   │   │   ├── CreateProductDto.cs
│   │   │   │   └── UpdateProductDto.cs
│   │   │   │
│   │   │   ├── ItemDtos
│   │   │   │   ├── ItemDto.cs
│   │   │   │   ├── CreateItemDto.cs
│   │   │   │   └── UpdateItemDto.cs
│   │   │   │
│   │   │   └── AuthDtos
│   │   │       ├── RegisterRequestDto.cs
│   │   │       ├── LoginRequestDto.cs
│   │   │       ├── RefreshTokenRequestDto.cs
│   │   │       └── AuthResponseDto.cs
│   │   │
│   │   ├── Interfaces
│   │   │   ├── IRepositories
│   │   │   │   ├── IGenericRepository.cs
│   │   │   │   ├── IProductRepository.cs
│   │   │   │   ├── IItemRepository.cs
│   │   │   │   └── IUnitOfWork.cs
│   │   │   │
│   │   │   └── IServices
│   │   │       ├── IProductService.cs
│   │   │       ├── IItemService.cs
│   │   │       ├── IAuthService.cs
│   │   │       └── IJwtTokenService.cs
│   │   │
│   │   ├── Mapping
│   │   │   └── ProductProfile.cs
│   │   │
│   │   └── Validators
│   │       ├── Product
│   │       │   ├── CreateProductDtoValidator.cs
│   │       │   └── UpdateProductDtoValidator.cs
│   │       │
│   │       ├── Item
│   │       │   ├── CreateItemDtoValidator.cs
│   │       │   └── UpdateItemDtoValidator.cs
│   │       │
│   │       └── Auth
│   │           ├── RegisterRequestDtoValidator.cs
│   │           ├── LoginRequestDtoValidator.cs
│   │           └── RefreshTokenRequestDtoValidator.cs
│   │
│   ├── ProductSphere.Domain
│   │   ├── Entities
│   │   │   ├── Product.cs
│   │   │   ├── Item.cs
│   │   │   ├── User.cs
│   │   │   ├── Role.cs
│   │   │   └── RefreshToken.cs
│   │   │
│   │   └── Exceptions
│   │       ├── BadRequestException.cs
│   │       ├── NotFoundException.cs
│   │       └── UnauthorizedException.cs
│   │
│   └── ProductSphere.Infrastructure
│       ├── Data
│       │   ├── Configurations
│       │   │   ├── ProductConfiguration.cs
│       │   │   ├── ItemConfiguration.cs
│       │   │   ├── UserConfiguration.cs
│       │   │   ├── RoleConfiguration.cs
│       │   │   └── RefreshTokenConfiguration.cs
│       │   │
│       │   ├── Repositories
│       │   │   ├── GenericRepository.cs
│       │   │   ├── ProductRepository.cs
│       │   │   ├── ItemRepository.cs
│       │   │   └── UnitOfWork.cs
│       │   │
│       │   └── ApplicationDbContext.cs
│       │
│       ├── Identity
│       │   ├── AuthService.cs
│       │   ├── JwtTokenService.cs
│       │   └── JwtSettings.cs
│       │
│       └── Services
│           ├── ProductService.cs
│           └── ItemService.cs
│
├── Tests
│   │
│   ├── ProductSphere.Application.Tests
│   │   ├── Services
│   │   │   ├── ProductServiceTests.cs
│   │   │   ├── ItemServiceTests.cs
│   │   │   └── AuthServiceTests.cs
│   │   │
│   │   └── ProductSphere.Application.Tests.csproj
│   │
│   ├── ProductSphere.Infrastructure.Tests
│   │   ├── Data
│   │   │   ├── ApplicationDbContextTests.cs
│   │   │   └── UnitOfWorkTests.cs
│   │   │
│   │   ├── Helpers
│   │   │   └── DbContextFactory.cs
│   │   │
│   │   ├── Repositories
│   │   │   └── GenericRepositoryTests.cs
│   │   │
│   │   └── ProductSphere.Infrastructure.Tests.csproj
│   │
│   └── ProductSphere.API.Tests
│       ├── CustomWebApplicationFactory.cs
│       ├── ProductControllerTests.cs
│       ├── ItemControllerTests.cs
│       ├── AuthControllerTests.cs
│       └── ProductSphere.API.Tests.csproj
│
├── Dockerfile
├── docker-compose.yml
├── ProductSphere.sln
└── README.md
```

# Design Patterns Used

The application follows several enterprise design patterns to ensure maintainability, scalability, and separation of concerns.

## Clean Architecture

Separates the application into independent layers:

- API
- Application
- Domain
- Infrastructure

---

## Repository Pattern

Encapsulates data access logic and provides a clean abstraction over Entity Framework Core.

Repositories:

- GenericRepository
- ProductRepository
- ItemRepository

---

## Generic Repository Pattern

Provides reusable CRUD operations that can be shared across multiple entities, reducing code duplication.

---

## Unit of Work Pattern

Coordinates multiple repositories and ensures changes are committed as a single transaction.

---

## Dependency Injection

All services, repositories, and framework components are registered using ASP.NET Core's built-in Dependency Injection container.

---

# Authentication & Authorization

The API uses **JWT (JSON Web Token)** authentication with **Refresh Token Rotation** for secure access.

## Authentication Features

- User Registration
- User Login
- JWT Access Token Generation
- Refresh Token Generation
- Refresh Token Rotation
- BCrypt Password Hashing

---

## Authorization

Role-Based Authorization is implemented to protect secured endpoints.

Supported Roles:

- Admin

Protected endpoints require a valid JWT access token.

---

## Authentication Flow

```text
User
 │
 │ Register / Login
 ▼
AuthController
 │
 ▼
AuthService
 │
 ▼
Validate Credentials
 │
 ▼
Generate JWT Access Token
 │
 ▼
Generate Refresh Token
 │
 ▼
Return Tokens
 │
 ▼
Client Stores Tokens
 │
 ▼
Access Protected APIs
 │
 ▼
Access Token Expired
 │
 ▼
Refresh Token Endpoint
 │
 ▼
Generate New Access Token
 │
 ▼
Continue Access
```

---

# API Endpoints

## Authentication

| Method | Endpoint | Description |
|---------|----------|-------------|
| POST | `/api/v1/Auth/register` | Register a new user |
| POST | `/api/v1/Auth/login` | Authenticate user |
| POST | `/api/v1/Auth/refresh-token` | Generate new access token |

---

## Product

| Method | Endpoint | Description |
|---------|----------|-------------|
| GET | `/api/v1/Product` | Get all products |
| GET | `/api/v1/Product/{id}` | Get product by Id |
| POST | `/api/v1/Product` | Create product |
| PUT | `/api/v1/Product` | Update product |
| DELETE | `/api/v1/Product/{id}` | Delete product |

---

## Item

| Method | Endpoint | Description |
|---------|----------|-------------|
| GET | `/api/v1/Item` | Get all items |
| GET | `/api/v1/Item/{id}` | Get item by Id |
| POST | `/api/v1/Item` | Create item |
| PUT | `/api/v1/Item` | Update item |
| DELETE | `/api/v1/Item/{id}` | Delete item |

---

# Database Schema

The application uses **SQL Server** with **Entity Framework Core**.

## Tables

### Product

| Column | Type |
|---------|------|
| Id | int |
| ProductName | nvarchar(255) |
| CreatedBy | nvarchar(100) |
| CreatedOn | datetime |
| ModifiedBy | nvarchar(100) |
| ModifiedOn | datetime |

---

### Item

| Column | Type |
|---------|------|
| Id | int |
| ProductId | int (FK) |
| Quantity | int |

---

### User

Stores application users.

---

### Role

Stores application roles.

---

### RefreshToken

Stores refresh tokens used for JWT token rotation.

---

# Entity Relationships

```text
Role
 │
 └──────< User
            │
            └──────< RefreshToken

Product
 │
 └──────< Item
```

---

# Getting Started

## Prerequisites

Before running the project, ensure the following software is installed:

- .NET 8 SDK
- SQL Server
- Visual Studio 2022
- Git
- Docker Desktop (Optional)

---

# Clone the Repository

```bash
git clone https://github.com/pankajs5535/ProductSphere.git
```

```bash
cd ProductSphere
```

---

# Restore Packages

```bash
dotnet restore
```

---

# Configure SQL Server

Update the connection string in:

```
Src/ProductSphere.API/appsettings.json
```

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=ProductSphereDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

---

# Apply Database Migrations

```bash
dotnet ef database update \
--project Src/ProductSphere.Infrastructure \
--startup-project Src/ProductSphere.API
```

---

# Run the Application

```bash
dotnet run --project Src/ProductSphere.API
```

The API will be available at:

```
https://localhost:5001
```

Swagger UI:

```
https://localhost:5001/swagger
```

---

# Configuration

Configuration files are located in:

```
Src/ProductSphere.API
```

- appsettings.json
- appsettings.Development.json

These files contain:

- SQL Server Connection String
- JWT Settings
- Serilog Configuration
- Logging Settings
- Allowed Hosts

---

# Docker

The application supports Docker for containerized deployment.

## Build Docker Image

```bash
docker build -t productsphere-api .
```

---

## Run Docker Container

```bash
docker run -d -p 8080:80 productsphere-api
```

---

## Using Docker Compose

```bash
docker compose up --build
```

This starts the API and all configured services using the `docker-compose.yml` file.

---

 # Testing

The solution includes comprehensive testing across the Application, Infrastructure, and API layers.

## Application Tests

- ProductServiceTests
- ItemServiceTests
- AuthServiceTests

Frameworks:

- xUnit
- Moq
- FluentAssertions

---

## Infrastructure Tests

Covered Test Classes:

- GenericRepositoryTests
- ApplicationDbContextTests
- UnitOfWorkTests

---

## API Integration Tests

Implemented using **WebApplicationFactory**.

Covered Controllers:

- AuthController
- ProductController
- ItemController

---

All unit and integration tests pass successfully.

---

# Logging

The project uses **Serilog** for structured logging.

Logging captures:

- Application startup
- API requests
- Exceptions
- Errors
- Warnings
- Information logs

---

# Validation

Input validation is implemented using **FluentValidation**.

Validated DTOs include:

- Product DTOs
- Item DTOs
- Authentication DTOs

Validation is executed automatically through the **ValidationFilter** before requests reach the controllers.

---

# Exception Handling

A centralized **Global Exception Middleware** provides consistent error responses.

Handled Exceptions:

- NotFoundException
- BadRequestException
- UnauthorizedException
- Unhandled Exceptions

---

# Security

Implemented security features include:

- JWT Authentication
- Refresh Token Rotation
- BCrypt Password Hashing
- Role-Based Authorization
- HTTPS Support
- Input Validation

---

| Test Type | Status |
|------------|--------|
| Application Unit Tests | ✅ Passed |
| Infrastructure Unit Tests | ✅ Passed |
| API Integration Tests | ✅ Passed (26/26) |

---

# Implemented Features

- ✅ Clean Architecture
- ✅ Repository Pattern
- ✅ Generic Repository
- ✅ Unit of Work
- ✅ Entity Framework Core
- ✅ SQL Server
- ✅ AutoMapper
- ✅ FluentValidation
- ✅ Validation Filter
- ✅ Global Exception Middleware
- ✅ JWT Authentication
- ✅ Refresh Token Rotation
- ✅ BCrypt Password Hashing
- ✅ Role-Based Authorization
- ✅ API Versioning
- ✅ Swagger
- ✅ Serilog Logging
- ✅ Unit Testing
- ✅ Integration Testing
- ✅ Docker

---

# Future Enhancements

Possible future improvements include:

- Redis Caching
- Health Checks
- CI/CD Pipeline
- Azure Deployment
- Email Notifications
- Background Jobs (Hangfire)
- Monitoring & Telemetry

---

# Author

**Pankaj Suryawanshi**

GitHub: https://github.com/pankajs5535
GitHub: https://github.com/pankajs5535/ProductSphere

---

# License

This project was developed as part of a technical assessment. 
It demonstrates enterprise backend development practices using ASP.NET Core 8 and Clean Architecture.

