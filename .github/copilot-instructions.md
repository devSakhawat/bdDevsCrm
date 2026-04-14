# GitHub Copilot Instructions for bdDevsCrm

## Project Overview
This is an enterprise-grade CRM system built with **Clean Architecture** principles, designed to support 60,000+ employees with high scalability, maintainability, and security.

### Technology Stack
**Backend:**
- .NET Core 8/10 Web API
- Entity Framework Core 8.0
- MS SQL Server (primary database)
- Multi-tier caching: L1 (IMemoryCache) → L2 (Redis) → L3 (PostgreSQL fallback)
- JWT Bearer Authentication
- Two-way AES-256 password encryption

**Frontend:**
- ASP.NET Core MVC with Razor Pages
- jQuery (DOM manipulation only)
- Kendo UI 2024 Q4 (Grid, TabStrip, Window, Validator)
- JavaScript Fetch API for HTTP calls (NOT jQuery Ajax)

## Architecture Constraints

### Layer Structure & Dependencies
```
Presentation (API + MVC) → Application.Services → Infrastructure.Data
                                    ↓                      ↓
                                  Domain  ←  Shared.Kernel
```

**CRITICAL RULES:**
1. ✅ Outer layers depend on inner layers
2. ❌ Domain layer NEVER depends on Infrastructure or Application
3. ✅ Application.Services is simplified to a single project containing:
   - `Services/` - Service implementations
   - `Contracts/` - Service interfaces
   - `DTOs/` - Data Transfer Objects
   - `Validators/` - FluentValidation validators
   - `Mappings/` - AutoMapper profiles

### Domain Layer
- Pure C# business logic
- No external dependencies
- Contains: Entities, ValueObjects, Enums, Repository Interfaces, Custom Exceptions
- Entities should validate their own state
- Use value objects for complex types (Email, Address, Money)

**Example Entity:**
```csharp
public class User
{
    public int Id { get; private set; }
    public string Username { get; private set; }

    public void ChangeUsername(string newUsername)
    {
        if (string.IsNullOrWhiteSpace(newUsername))
            throw new ArgumentException("Username cannot be empty");
        Username = newUsername;
    }
}
```

### Application Layer (Application.Services)
- Orchestrates business workflows
- Returns DTOs, never domain entities
- No database access (use repository interfaces)
- No HTTP concerns (no controllers)

**Service Pattern:**
```csharp
public class UserService : IUserService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public async Task<UserDTO> GetUserByIdAsync(int userId)
    {
        // 1. Validation
        if (userId <= 0) throw new ArgumentException("Invalid user ID");

        // 2. Retrieve from repository
        var user = await _repository.Users.GetByIdAsync(userId);
        if (user == null) throw new UserNotFoundException(userId);

        // 3. Map to DTO
        return _mapper.Map<UserDTO>(user);
    }
}
```

### Infrastructure Layer
- Implements repository interfaces from Domain
- Contains EF Core DbContext and configurations
- Multi-tier caching implementation
- External service integrations (Email, SMS, etc.)
- NO business logic

**Repository Pattern:**
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
}
```

### Presentation Layer (API + MVC)
- Thin controllers - delegate to services
- Return unified `ApiResponse<T>` format
- Handle exceptions with global exception handler
- NO business logic in controllers

**API Controller Pattern:**
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

## API Communication Standards

### Unified API Response Model
**Always use this structure for all API responses:**
```csharp
public class ApiResponse<T>
{
    public string CorrelationId { get; set; }      // Request tracking ID
    public int StatusCode { get; set; }            // HTTP status code
    public bool Success { get; set; }              // Success flag
    public string Message { get; set; }            // User-friendly message
    public T Data { get; set; }                    // Response data
    public List<string> Errors { get; set; }       // Error details
    public DateTime Timestamp { get; set; }        // Response timestamp
}
```

### Frontend API Calling Pattern
**ALWAYS use Fetch API, NEVER jQuery.ajax():**
```javascript
async function callApi(url, method = 'GET', data = null, options = {}) {
    const token = localStorage.getItem('authToken');

    const config = {
        method: method,
        headers: {
            'Content-Type': 'application/json',
            'Authorization': token ? `Bearer ${token}` : '',
            ...options.headers
        }
    };

    if (data && (method === 'POST' || method === 'PUT' || method === 'PATCH')) {
        config.body = JSON.stringify(data);
    }

    try {
        const response = await fetch(url, config);
        if (!response.ok) {
            const error = await response.json();
            throw new Error(error.message || `HTTP ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error('API Error:', error);
        throw error;
    }
}

// Usage:
const user = await callApi('/api/users/123', 'GET');
const newUser = await callApi('/api/users', 'POST', { username: 'john', email: 'john@example.com' });
```

## Frontend Design Standards

### Layout Dimensions (Fixed)
- **Header**: 70px height (fixed, sticky at top)
- **Footer**: 20px height (fixed at bottom)
- **Sidebar**: 260px width (collapsible to ~50px icon-only)
- **Main Content**: Remaining space (responsive)

### Three Form Types

#### 1. Inline Form (Grid-based) - 2-5 fields
Use Kendo Grid with inline editing:
```javascript
$("#grid").kendoGrid({
    dataSource: dataSource,
    editable: "inline",
    toolbar: ["create"],
    columns: [
        { field: "username", title: "Username" },
        { field: "email", title: "Email" },
        { command: ["edit", "destroy"], title: "&nbsp;" }
    ]
});
```

#### 2. Modal Form (Popup) - 5-15 fields
Use Kendo Window with form validation:
```javascript
const window = $("#formWindow").kendoWindow({
    width: "600px",
    title: "User Details",
    visible: false,
    modal: true,
    actions: ["Close"]
}).data("kendoWindow");

const validator = $("#userForm").kendoValidator({
    rules: {
        custom: function(input) {
            if (input.is("[name='email']")) {
                return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(input.val());
            }
            return true;
        }
    }
}).data("kendoValidator");
```

#### 3. Complex Tabbed Form - 15+ fields
Use Kendo TabStrip with lazy loading:
```javascript
$("#tabstrip").kendoTabStrip({
    animation: { open: { effects: "fadeIn" } },
    select: function(e) {
        const tabName = $(e.item).find("> .k-link").text();
        loadTabData(tabName); // Load data when tab is selected
    }
});
```

### jQuery Usage
**Use jQuery ONLY for:**
- DOM manipulation (`$("#element").hide()`, `.addClass()`, etc.)
- Event handling (`$("#btn").on("click", handler)`)
- Kendo UI widget initialization

**NEVER use jQuery for:**
- HTTP calls (use `fetch()` instead of `$.ajax()`)
- Complex logic (use vanilla JavaScript)

## Naming Conventions

### C# (Backend)
- **Classes, Interfaces, Methods**: `PascalCase`
  - `UserService`, `IUserRepository`, `GetUserByIdAsync()`
- **Private Fields**: `_camelCase` (underscore prefix)
  - `private readonly IUserRepository _userRepository;`
- **Properties**: `PascalCase`
  - `public string Username { get; set; }`
- **Parameters, Local Variables**: `camelCase`
  - `public void CreateUser(string username, int age)`
- **Constants**: `UPPER_SNAKE_CASE` or `PascalCase`
  - `public const int MAX_LOGIN_ATTEMPTS = 5;`

### JavaScript (Frontend)
- **Functions, Variables**: `camelCase`
  - `function getUserById(userId) { }`, `const userName = "John";`
- **Classes**: `PascalCase`
  - `class UserManager { }`
- **Constants**: `UPPER_SNAKE_CASE`
  - `const API_BASE_URL = "https://api.example.com";`

## SOLID Principles

### 1. Single Responsibility Principle (SRP)
Each class has one reason to change.
```csharp
// ✅ Good
public class UserService : IUserService { }
public class EmailService : IEmailService { }

// ❌ Bad
public class UserService {
    public Task SendEmail() { } // Should be in EmailService
}
```

### 2. Open/Closed Principle (OCP)
Open for extension, closed for modification.
```csharp
// ✅ Good - Add new payment processors without modifying existing code
public interface IPaymentProcessor { }
public class CreditCardProcessor : IPaymentProcessor { }
public class BkashProcessor : IPaymentProcessor { }
```

### 3. Liskov Substitution Principle (LSP)
Derived classes must be substitutable for base classes.
```csharp
// ✅ Good - All implementations can be used interchangeably
public interface IRepository<T> { Task<T> GetByIdAsync(int id); }
public class UserRepository : IRepository<User> { }
```

### 4. Interface Segregation Principle (ISP)
Clients should not depend on unused interfaces.
```csharp
// ✅ Good - Separate interfaces
public interface IReadRepository<T> { Task<T> GetByIdAsync(int id); }
public interface IWriteRepository<T> { Task CreateAsync(T entity); }

// ❌ Bad - Fat interface
public interface IRepository<T> {
    Task<T> GetByIdAsync(int id);
    Task BulkInsertAsync(IEnumerable<T> entities); // Not needed by all
}
```

### 5. Dependency Inversion Principle (DIP)
Depend on abstractions, not concretions.
```csharp
// ✅ Good - Depend on interface
public class UsersController {
    private readonly IUserService _userService; // Abstraction
}

// ❌ Bad - Depend on concrete class
public class UsersController {
    private readonly UserService _userService = new UserService(); // Tight coupling
}
```

## Code Generation Rules

### Dependency Injection
**Always use constructor injection:**
```csharp
public class UserService : IUserService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    public UserService(IRepositoryManager repository, IMapper mapper, ILogger<UserService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }
}
```

**Register services in Program.cs:**
```csharp
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddSingleton<ICacheService, CacheService>();
```

### Async/Await for I/O
**Always use async for database and external service calls:**
```csharp
// ✅ Good
public async Task<User> GetUserAsync(int id)
{
    return await _repository.Users.GetByIdAsync(id);
}

// ❌ Bad
public User GetUser(int id)
{
    return _repository.Users.GetById(id); // Synchronous
}
```

### Error Handling
**Use custom exceptions for domain errors:**
```csharp
// Domain Layer
public class UserNotFoundException : Exception
{
    public UserNotFoundException(int userId)
        : base($"User with ID {userId} not found") { }
}

// Service Layer
if (user == null)
    throw new UserNotFoundException(userId);

// Presentation Layer - Global Exception Handler
public class GlobalExceptionHandler : IExceptionHandler
{
    public async Task<bool> TryHandleAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            UserNotFoundException => 404,
            ValidationException => 400,
            UnauthorizedException => 401,
            _ => 500
        };

        var response = new ApiResponse<object>
        {
            CorrelationId = context.TraceIdentifier,
            StatusCode = statusCode,
            Success = false,
            Message = exception.Message,
            Errors = new List<string> { exception.Message },
            Timestamp = DateTime.UtcNow
        };

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(response);
        return true;
    }
}
```

### Validation
**Use FluentValidation for input validation:**
```csharp
public class CreateUserDTOValidator : AbstractValidator<CreateUserDTO>
{
    public CreateUserDTOValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .Length(3, 50).WithMessage("Username must be between 3 and 50 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
    }
}
```

## Multi-Tier Caching Pattern
**Use L1 (Memory) → L2 (Redis) → L3 (PostgreSQL) fallback:**
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
        await _redis.SetAsync(key, dbValue, TimeSpan.FromMinutes(30));
        _memoryCache.Set(key, dbValue, TimeSpan.FromMinutes(5));
        return dbValue;
    }

    return default;
}
```

## Common Mistakes to Avoid

### ❌ DON'T:
1. Use `jQuery.ajax()` or `$.get()` - Use `fetch()` instead
2. Expose domain entities directly from controllers - Use DTOs
3. Put business logic in controllers - Delegate to services
4. Use synchronous methods for I/O operations
5. Create repositories directly with `new` - Use dependency injection
6. Access database from Application layer - Use repository interfaces
7. Add dependencies from Domain to Infrastructure
8. Mix concerns across layers

### ✅ DO:
1. Use `fetch()` API for all HTTP calls
2. Return DTOs from services and controllers
3. Keep controllers thin - delegate to services
4. Use async/await for all I/O operations
5. Inject dependencies via constructor
6. Define repository interfaces in Domain, implement in Infrastructure
7. Keep Domain layer independent (no external dependencies)
8. Follow Single Responsibility Principle

## Security Considerations
- **Authentication**: JWT Bearer tokens stored in `localStorage`
- **Password Encryption**: Two-way AES-256 encryption (client requirement)
- **Authorization**: Role-based access control (RBAC)
- **Input Validation**: Validate at both presentation and application layers
- **SQL Injection**: Use parameterized queries (EF Core handles this)
- **XSS Prevention**: Sanitize user input, use Razor encoding

## Performance Targets
- **Page Load**: < 2 seconds (initial load)
- **API Response**: < 500ms (95th percentile)
- **Grid Rendering**: < 1 second (for 1000 rows)
- **Search**: < 300ms (with caching)
- **Concurrent Users**: Support 1000+ simultaneous users

## Testing Approach
- **Unit Tests**: Service layer, repository layer
- **Integration Tests**: API endpoints with in-memory database
- **E2E Tests**: Critical user flows (login, CRUD operations)

## Documentation References
For detailed information, refer to:
- `/doc/PROJECT_VISION.md` - Overall project vision and goals
- `/doc/backend_design.md` - Backend architecture and patterns
- `/doc/frontend_design.md` - Frontend UI/UX design system
- `/doc/coding_architecture.md` - Layer structure and SOLID principles
