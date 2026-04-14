# Coding Architecture

## Overview
This document elaborates on the high-level coding architecture used in this repository, which is designed to promote modularity and maintainability. The project follows **Clean Architecture** principles with a **Shared Kernel** pattern for cross-cutting concerns.

## Architecture Overview

```
┌─────────────────────────────────────────────────────────────┐
│                      Presentation Layer                      │
│            (ASP.NET Core Web API + MVC)                      │
│  ┌──────────────────────┐   ┌──────────────────────────┐   │
│  │   API Controllers    │   │    MVC Controllers       │   │
│  │   (RESTful APIs)     │   │   (Views + Razor)        │   │
│  └──────────────────────┘   └──────────────────────────┘   │
└────────────────────────┬────────────────────────────────────┘
                         │
                         ▼
┌─────────────────────────────────────────────────────────────┐
│                     Application Layer                        │
│                  (Application.Services)                      │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  Services/      - Service implementations           │   │
│  │  Contracts/     - Service interfaces (IUserService) │   │
│  │  DTOs/          - Data Transfer Objects             │   │
│  │  Validators/    - FluentValidation validators       │   │
│  │  Mappings/      - AutoMapper profiles               │   │
│  └──────────────────────────────────────────────────────┘   │
└────────────────────────┬────────────────────────────────────┘
                         │
                         ▼
┌─────────────────────────────────────────────────────────────┐
│                     Infrastructure Layer                     │
│           (Data Access + External Services)                  │
│  ┌──────────────────────┐   ┌──────────────────────────┐   │
│  │  Repositories/       │   │  External Services/      │   │
│  │  DbContext           │   │  Email, SMS, etc.        │   │
│  │  Migrations/         │   │  Caching (Redis)         │   │
│  └──────────────────────┘   └──────────────────────────┘   │
└────────────────────────┬────────────────────────────────────┘
                         │
                         ▼
┌─────────────────────────────────────────────────────────────┐
│                        Domain Layer                          │
│                    (Core Business Logic)                     │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  Entities/          - Domain entities (User, Order)  │   │
│  │  ValueObjects/      - Value objects                  │   │
│  │  Enums/             - Enumeration types              │   │
│  │  Interfaces/        - Repository interfaces          │   │
│  │  Exceptions/        - Custom domain exceptions       │   │
│  └──────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
                         ▲
                         │
         ┌───────────────┴───────────────┐
         │                               │
┌────────▼────────┐           ┌──────────▼──────────┐
│  Shared Kernel  │           │   Common/Utilities  │
│  (Cross-cutting)│           │   (Helper classes)  │
└─────────────────┘           └─────────────────────┘
```

## Layer Details

### 1. Domain Layer (Core)
**Purpose**: Contains the core business logic and domain entities.

**Dependencies**: None (Independent, innermost layer)

**Structure**:
```
Domain/
  ├── Entities/               - Domain entities with business logic
  ├── ValueObjects/           - Immutable value objects
  ├── Enums/                  - Enumeration types
  ├── Interfaces/             - Repository interfaces (IUserRepository)
  ├── Exceptions/             - Custom domain exceptions
  └── Events/                 - Domain events (optional)
```

**Responsibilities**:
- Define core business entities (User, Product, Order, etc.)
- Encapsulate business rules and invariants
- Define repository interfaces
- Domain-specific exceptions

**Key Principles**:
- No dependencies on other layers
- Pure C# classes with business logic
- Entities should validate their own state
- Use value objects for complex types (Email, Address, Money)

**Example Entity**:
```csharp
public class User
{
    public int Id { get; private set; }
    public string Username { get; private set; }
    public Email Email { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Business logic method
    public void ChangeEmail(Email newEmail)
    {
        if (newEmail == null)
            throw new ArgumentNullException(nameof(newEmail));

        Email = newEmail;
    }
}
```

### 2. Application Layer (Business Logic)
**Purpose**: Orchestrates business logic and use cases.

**Dependencies**: Domain Layer only

**Structure** (Simplified - Single Project):
```
Application.Services/
  ├── Services/               - Service implementations (UserService)
  ├── Contracts/              - Service interfaces (IUserService)
  ├── DTOs/                   - Data Transfer Objects
  ├── Validators/             - FluentValidation validators
  └── Mappings/               - AutoMapper profiles
```

**Responsibilities**:
- Implement use cases and business workflows
- Coordinate domain objects
- Transform domain entities to DTOs and vice versa
- Validate input data
- Transaction management

**Key Principles**:
- No database access (use repositories via interfaces)
- No HTTP concerns (no controllers, routes, etc.)
- Return DTOs, not domain entities
- Use dependency injection for repositories

**Example Service**:
```csharp
public class UserService : IUserService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public async Task<UserDTO> GetUserByIdAsync(int userId)
    {
        // 1. Validation
        if (userId <= 0)
            throw new ArgumentException("Invalid user ID");

        // 2. Retrieve from repository
        var user = await _repository.Users.GetByIdAsync(userId);

        if (user == null)
            throw new UserNotFoundException(userId);

        // 3. Map to DTO
        return _mapper.Map<UserDTO>(user);
    }
}
```

### 3. Infrastructure Layer (Data Access + External Services)
**Purpose**: Implements data access and external service integrations.

**Dependencies**: Domain Layer, Application Layer

**Structure**:
```
Infrastructure.Data/
  ├── DbContext/              - Entity Framework DbContext
  ├── Repositories/           - Repository implementations
  ├── Configurations/         - EF entity configurations
  └── Migrations/             - Database migrations

Infrastructure.Services/
  ├── Email/                  - Email service implementation
  ├── SMS/                    - SMS service implementation
  ├── Caching/                - Multi-tier caching (Memory, Redis, PostgreSQL)
  └── External/               - Third-party API integrations
```

**Responsibilities**:
- Implement repository interfaces from Domain
- Configure Entity Framework mappings
- Implement caching strategies (L1/L2/L3)
- Connect to external services (email, SMS, payment gateways)
- File storage operations

**Key Principles**:
- Implement interfaces defined in Domain layer
- Use dependency injection
- No business logic (only data access)
- Handle database migrations

**Example Repository**:
```csharp
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public async Task<User> GetByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
}
```

**Multi-Tier Caching Strategy**:
```csharp
public async Task<T> GetWithCacheAsync<T>(string key)
{
    // L1: Memory Cache (fastest)
    if (_memoryCache.TryGetValue(key, out T value))
        return value;

    // L2: Redis Cache
    var redisValue = await _redis.GetAsync(key);
    if (redisValue != null)
    {
        _memoryCache.Set(key, redisValue, TimeSpan.FromMinutes(5));
        return redisValue;
    }

    // L3: PostgreSQL Cache (fallback)
    var dbValue = await _postgresCache.GetAsync(key);
    if (dbValue != null)
    {
        _redis.SetAsync(key, dbValue, TimeSpan.FromMinutes(30));
        _memoryCache.Set(key, dbValue, TimeSpan.FromMinutes(5));
        return dbValue;
    }

    return default;
}
```

### 4. Presentation Layer (API + MVC)
**Purpose**: Exposes application functionality via REST APIs and web UI.

**Dependencies**: Application Layer, Infrastructure Layer

**Structure**:
```
Presentation.API/
  ├── Controllers/            - API controllers (RESTful)
  ├── Filters/                - Action filters, exception filters
  ├── Middleware/             - Custom middleware
  └── Models/                 - API-specific models (requests/responses)

Presentation.Web/
  ├── Controllers/            - MVC controllers
  ├── Views/                  - Razor views
  ├── wwwroot/                - Static files (CSS, JS, images)
  │   ├── js/                 - JavaScript (jQuery, Kendo UI)
  │   ├── css/                - Stylesheets
  │   └── lib/                - Third-party libraries
  └── Models/                 - View models
```

**Responsibilities**:
- Handle HTTP requests/responses
- Route mapping
- Authentication and authorization
- Input validation
- Error handling and logging
- Render views (MVC)

**Key Principles**:
- Thin controllers (delegate to services)
- Use standard HTTP status codes
- Return unified ApiResponse<T> format
- Handle exceptions gracefully
- No business logic in controllers

**Example API Controller**:
```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);

            return Ok(new ApiResponse<UserDTO>
            {
                CorrelationId = HttpContext.TraceIdentifier,
                StatusCode = 200,
                Success = true,
                Message = "User retrieved successfully",
                Data = user,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new ApiResponse<UserDTO>
            {
                CorrelationId = HttpContext.TraceIdentifier,
                StatusCode = 404,
                Success = false,
                Message = ex.Message,
                Errors = new List<string> { ex.Message },
                Timestamp = DateTime.UtcNow
            });
        }
    }
}
```

### 5. Shared Kernel (Cross-Cutting Concerns)
**Purpose**: Shared utilities and cross-cutting functionality used across all layers.

**Dependencies**: None (Referenced by all layers)

**Structure**:
```
Shared.Kernel/
  ├── Constants/              - Application-wide constants
  ├── Extensions/             - Extension methods
  ├── Helpers/                - Helper classes
  ├── Models/                 - Shared models (ApiResponse<T>)
  └── Utilities/              - Utility functions
```

**Responsibilities**:
- Common constants (error codes, configuration keys)
- Extension methods (string extensions, collection extensions)
- Helper classes (DateHelper, StringHelper)
- Shared response models
- Utility functions

**Example - ApiResponse Model**:
```csharp
public class ApiResponse<T>
{
    public string CorrelationId { get; set; }
    public int StatusCode { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public List<string> Errors { get; set; }
    public DateTime Timestamp { get; set; }
}
```

## SOLID Principles Implementation

### 1. Single Responsibility Principle (SRP)
Each class has one reason to change.

**Good Example**:
```csharp
// ✅ UserService handles user business logic only
public class UserService : IUserService
{
    public async Task<UserDTO> GetUserAsync(int id) { }
    public async Task<UserDTO> CreateUserAsync(CreateUserDTO dto) { }
}

// ✅ EmailService handles email operations only
public class EmailService : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body) { }
}
```

**Bad Example**:
```csharp
// ❌ UserService doing too many things
public class UserService
{
    public async Task<User> GetUser(int id) { }
    public async Task SendWelcomeEmail(User user) { }  // Should be in EmailService
    public async Task LogUserActivity(User user) { }   // Should be in LoggingService
}
```

### 2. Open/Closed Principle (OCP)
Classes should be open for extension but closed for modification.

**Good Example**:
```csharp
// ✅ Use abstraction to extend behavior
public interface IPaymentProcessor
{
    Task<bool> ProcessPaymentAsync(decimal amount);
}

public class CreditCardProcessor : IPaymentProcessor { }
public class PayPalProcessor : IPaymentProcessor { }
public class BkashProcessor : IPaymentProcessor { }  // New processor without modifying existing code
```

### 3. Liskov Substitution Principle (LSP)
Derived classes must be substitutable for their base classes.

**Good Example**:
```csharp
// ✅ All implementations can be used interchangeably
public interface IRepository<T>
{
    Task<T> GetByIdAsync(int id);
}

public class UserRepository : IRepository<User> { }
public class ProductRepository : IRepository<Product> { }
```

### 4. Interface Segregation Principle (ISP)
Clients should not depend on interfaces they don't use.

**Good Example**:
```csharp
// ✅ Separate interfaces for different concerns
public interface IReadRepository<T>
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
}

public interface IWriteRepository<T>
{
    Task<T> CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}
```

**Bad Example**:
```csharp
// ❌ Fat interface forcing unnecessary implementations
public interface IRepository<T>
{
    Task<T> GetByIdAsync(int id);
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task BulkInsertAsync(IEnumerable<T> entities);  // Not needed by all
    Task ExecuteRawSqlAsync(string sql);            // Not needed by all
}
```

### 5. Dependency Inversion Principle (DIP)
Depend on abstractions, not concretions.

**Good Example**:
```csharp
// ✅ Controller depends on abstraction (IUserService)
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;  // Abstraction

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
}
```

**Bad Example**:
```csharp
// ❌ Controller depends on concrete class
public class UsersController : ControllerBase
{
    private readonly UserService _userService;  // Concrete class

    public UsersController()
    {
        _userService = new UserService();  // Tight coupling
    }
}
```

## Naming Conventions

### C# (Backend)
- **Classes, Interfaces, Methods**: `PascalCase`
  ```csharp
  public class UserService { }
  public interface IUserRepository { }
  public async Task<User> GetUserByIdAsync(int id) { }
  ```

- **Private Fields**: `_camelCase` (underscore prefix)
  ```csharp
  private readonly IUserRepository _userRepository;
  ```

- **Properties**: `PascalCase`
  ```csharp
  public string Username { get; set; }
  ```

- **Parameters, Local Variables**: `camelCase`
  ```csharp
  public void CreateUser(string username, int age) { }
  ```

- **Constants**: `UPPER_SNAKE_CASE` or `PascalCase`
  ```csharp
  public const string DEFAULT_ROLE = "User";
  public const int MAX_LOGIN_ATTEMPTS = 5;
  ```

### JavaScript (Frontend)
- **Functions, Variables**: `camelCase`
  ```javascript
  function getUserById(userId) { }
  const userName = "John";
  ```

- **Classes**: `PascalCase`
  ```javascript
  class UserManager { }
  ```

- **Constants**: `UPPER_SNAKE_CASE`
  ```javascript
  const API_BASE_URL = "https://api.example.com";
  const MAX_RETRY_ATTEMPTS = 3;
  ```

- **Private Properties** (convention): `_camelCase`
  ```javascript
  class User {
      _privateField = null;
  }
  ```

## Code Organization Best Practices

### 1. File Naming
- One class per file
- File name matches class name
- Example: `UserService.cs` contains `UserService` class

### 2. Folder Structure
- Group by feature/domain, not by type
  ```
  ✅ Good:
  Users/
    ├── UserService.cs
    ├── IUserService.cs
    ├── UserDTO.cs
    └── UserValidator.cs

  ❌ Bad:
  Services/
    └── UserService.cs
  Interfaces/
    └── IUserService.cs
  DTOs/
    └── UserDTO.cs
  ```

### 3. Dependency Injection
- Register all services in `Program.cs` or `Startup.cs`
- Use appropriate lifetimes:
  - `Scoped`: Services that are created once per request (DbContext, Repositories)
  - `Transient`: Services that are created each time they're requested (lightweight services)
  - `Singleton`: Services that are created once for the application lifetime (caching, configuration)

```csharp
// Program.cs
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddSingleton<ICacheService, CacheService>();
```

### 4. Error Handling
- Use custom exceptions for domain errors
- Use global exception handler in Presentation layer
- Log exceptions with correlation IDs

```csharp
// Domain Layer
public class UserNotFoundException : Exception
{
    public UserNotFoundException(int userId)
        : base($"User with ID {userId} not found") { }
}

// Presentation Layer - Global Exception Handler
public class GlobalExceptionHandler : IExceptionHandler
{
    public async Task<bool> TryHandleAsync(HttpContext context, Exception exception)
    {
        var response = new ApiResponse<object>
        {
            CorrelationId = context.TraceIdentifier,
            StatusCode = exception switch
            {
                UserNotFoundException => 404,
                ValidationException => 400,
                UnauthorizedException => 401,
                _ => 500
            },
            Success = false,
            Message = exception.Message,
            Errors = new List<string> { exception.Message },
            Timestamp = DateTime.UtcNow
        };

        context.Response.StatusCode = response.StatusCode;
        await context.Response.WriteAsJsonAsync(response);
        return true;
    }
}
```

## Key Architecture Rules

1. **Dependencies Flow Inward**: Outer layers can depend on inner layers, never the reverse
   - ✅ Application → Domain
   - ✅ Infrastructure → Domain
   - ✅ Presentation → Application
   - ❌ Domain → Infrastructure (NEVER)

2. **No Business Logic in Presentation Layer**: Controllers should be thin, delegating to services

3. **No Data Access in Application Layer**: Use repository interfaces, let Infrastructure implement them

4. **Domain Layer is Independent**: No external dependencies, pure C# business logic

5. **Use DTOs for Data Transfer**: Never expose domain entities directly to clients

6. **Async/Await for I/O Operations**: All database and external service calls should be async

7. **Validate Early**: Validate input at the presentation layer, validate business rules in domain/application layer

8. **Use Dependency Injection**: Never use `new` keyword for services, inject dependencies

## Summary

This architecture ensures:
- **Separation of Concerns**: Each layer has a clear responsibility
- **Testability**: Layers can be tested independently
- **Maintainability**: Changes in one layer don't ripple through the entire codebase
- **Scalability**: Easy to add new features without breaking existing code
- **Clean Code**: Following SOLID principles results in readable, maintainable code
