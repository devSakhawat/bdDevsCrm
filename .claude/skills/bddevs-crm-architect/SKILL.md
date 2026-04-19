# bdDevsCrm Architect Skill

## Overview
This skill provides comprehensive knowledge about the bdDevsCrm enterprise CRM system architecture, coding standards, and best practices for working with Clean Architecture in .NET Core with ASP.NET MVC frontend.

## Metadata
- **Name**: bddevs-crm-architect
- **Version**: 1.0.0
- **Category**: architecture
- **Technologies**: .NET Core 8/10, ASP.NET MVC, Entity Framework Core, Kendo UI, jQuery, Redis, PostgreSQL
- **Architecture Pattern**: Clean Architecture with Shared Kernel

## Project Context

### System Overview
Enterprise-grade CRM system designed to support 60,000+ employees with:
- Clean Architecture (100% adherence)
- Multi-tier caching (L1: Memory, L2: Redis, L3: PostgreSQL)
- Two-way password encryption (AES-256)
- High scalability and maintainability focus

### Technology Stack

**Backend:**
- .NET Core 10 Web API
- Entity Framework Core 10.0
- MS SQL Server (primary database)
- JWT Bearer Authentication
- Multi-tier caching with Redis and PostgreSQL fallback

**Frontend:**
- ASP.NET Core MVC with Razor Pages
- jQuery (DOM manipulation only - NOT for HTTP calls)
- Kendo UI 2024 Q4 (Grid, TabStrip, Window, Validator)
- JavaScript Fetch API for all HTTP communication

## Architecture Layers

### Layer Dependency Flow
```
Presentation (API + MVC)
    ↓
Application.Services (simplified - single project)
    ↓
Infrastructure.Data + Infrastructure.Services
    ↓
Domain (Core)
    ↑
Shared.Kernel (Cross-cutting)
```

### 1. Domain Layer (Innermost)
**Purpose**: Core business entities and logic

**Contains:**
- `Entities/` - Domain entities with business logic
- `ValueObjects/` - Immutable value objects (Email, Address, Money)
- `Enums/` - Enumeration types
- `Interfaces/` - Repository interfaces (IUserRepository, IOrderRepository)
- `Exceptions/` - Custom domain exceptions (UserNotFoundException)

**Rules:**
- NO dependencies on other layers
- Pure C# classes with business logic
- Entities validate their own state
- NO database concerns (no EF attributes)

**Example:**
```csharp
public class User
{
    public int Id { get; private set; }
    public string Username { get; private set; }
    public Email Email { get; private set; }

    // Business logic
    public void ChangeEmail(Email newEmail)
    {
        if (newEmail == null)
            throw new ArgumentNullException(nameof(newEmail));
        Email = newEmail;
    }
}
```

### 2. Application Layer (Application.Services)
**Purpose**: Orchestrate business workflows and use cases

**Structure (Simplified - Single Project):**
```
Application.Services/
  ├── Services/          - Service implementations (UserService)
  ├── Contracts/         - Service interfaces (IUserService)
  ├── DTOs/              - Data Transfer Objects
  ├── Validators/        - FluentValidation validators
  └── Mappings/          - AutoMapper profiles
```

**Rules:**
- NO database access (use repository interfaces)
- NO HTTP concerns (no controllers, no HttpContext)
- Return DTOs, NEVER domain entities
- Use dependency injection for all dependencies

**Service Pattern:**
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

        // 2. Business logic - retrieve from repository
        var user = await _repository.Users.GetByIdAsync(userId);
        if (user == null)
            throw new UserNotFoundException(userId);

        // 3. Transform to DTO
        return _mapper.Map<UserDTO>(user);
    }
}
```

### 3. Infrastructure Layer
**Purpose**: Implement data access and external service integrations

**Contains:**
- `Infrastructure.Data/` - EF Core DbContext, Repositories, Migrations
- `Infrastructure.Services/` - Email, SMS, Caching, External APIs

**Rules:**
- Implements repository interfaces from Domain layer
- NO business logic (only data access)
- Use EF Core for database operations
- Implement multi-tier caching strategy

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

    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
}
```

### 4. Presentation Layer
**Purpose**: Expose functionality via REST APIs and web UI

**Contains:**
- `Presentation.API/` - API controllers (RESTful)
- `Presentation.Web/` - MVC controllers, Views, wwwroot

**Rules:**
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

### 5. Shared Kernel
**Purpose**: Cross-cutting concerns used by all layers

**Contains:**
- `Constants/` - Application-wide constants
- `Extensions/` - Extension methods
- `Helpers/` - Helper classes
- `Models/` - Shared models (ApiResponse<T>)
- `Utilities/` - Utility functions

## API Communication Standards

### Unified API Response Model
**ALWAYS use this structure for all API responses:**
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

### Frontend API Client (JavaScript)
**ALWAYS use Fetch API - NEVER jQuery.ajax():**
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
            throw new Error(error.message || `HTTP ${response.status}: ${response.statusText}`);
        }
        const result = await response.json();
        return result;
    } catch (error) {
        console.error('API Error:', error);
        throw error;
    }
}

// Usage examples:
const user = await callApi('/api/users/123', 'GET');
const newUser = await callApi('/api/users', 'POST', { username: 'john', email: 'john@example.com' });
const updated = await callApi('/api/users/123', 'PUT', { username: 'john_updated' });
await callApi('/api/users/123', 'DELETE');
```

## Frontend Design Patterns

### Layout Dimensions (FIXED)
- **Header**: 70px height (fixed, sticky at top)
- **Footer**: 20px height (fixed at bottom)
- **Sidebar**: 260px width (collapsible to ~50px icon-only)
- **Main Content**: Remaining space (responsive)

### Three Form Design Types

#### 1. Inline Form (Grid-based) - 2-5 fields
**Use Case**: Simple data with few fields (User list, Product catalog)

**Pattern**: Kendo Grid with inline editing
```javascript
$("#grid").kendoGrid({
    dataSource: {
        transport: {
            read: { url: "/api/users", type: "GET" },
            create: { url: "/api/users", type: "POST" },
            update: { url: "/api/users", type: "PUT" },
            destroy: { url: "/api/users", type: "DELETE" }
        },
        schema: { model: { id: "id" } }
    },
    editable: "inline",
    toolbar: ["create"],
    columns: [
        { field: "username", title: "Username" },
        { field: "email", title: "Email" },
        { command: ["edit", "destroy"], title: "&nbsp;", width: 200 }
    ]
});
```

#### 2. Modal Form (Popup) - 5-15 fields
**Use Case**: Medium complexity forms (User details, Order entry)

**Pattern**: Kendo Window with form validation
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
        email: function(input) {
            if (input.is("[name='email']")) {
                return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(input.val());
            }
            return true;
        }
    },
    messages: {
        email: "Please enter a valid email address"
    }
}).data("kendoValidator");

async function saveUser() {
    if (validator.validate()) {
        const formData = {
            username: $("#username").val(),
            email: $("#email").val(),
            phone: $("#phone").val()
        };

        const result = await callApi('/api/users', 'POST', formData);
        if (result.success) {
            window.close();
            $("#grid").data("kendoGrid").dataSource.read();
        }
    }
}
```

#### 3. Complex Tabbed Form - 15+ fields
**Use Case**: Complex data entry with multiple sections (Employee profile, Project details)

**Pattern**: Kendo TabStrip with lazy loading
```javascript
$("#tabstrip").kendoTabStrip({
    animation: { open: { effects: "fadeIn" } },
    select: async function(e) {
        const tabName = $(e.item).find("> .k-link").text();

        // Lazy load tab data
        if (tabName === "Personal Info") {
            await loadPersonalInfo();
        } else if (tabName === "Employment") {
            await loadEmploymentInfo();
        } else if (tabName === "Documents") {
            await loadDocuments();
        }
    }
});

async function loadPersonalInfo() {
    const data = await callApi('/api/users/123/personal-info', 'GET');
    // Populate form fields
    $("#firstName").val(data.data.firstName);
    $("#lastName").val(data.data.lastName);
    // ... more fields
}
```

### jQuery Usage Guidelines
**✅ USE jQuery for:**
- DOM manipulation: `$("#element").hide()`, `.addClass()`, `.css()`
- Event handling: `$("#btn").on("click", handler)`
- Kendo UI widget initialization

**❌ NEVER use jQuery for:**
- HTTP calls: NO `$.ajax()`, `$.get()`, `$.post()` - Use `fetch()` instead
- Complex logic: Use vanilla JavaScript or TypeScript
- State management: Use proper patterns, not DOM storage

## Naming Conventions

### C# (Backend)
```csharp
// Classes, Interfaces, Methods: PascalCase
public class UserService { }
public interface IUserRepository { }
public async Task<User> GetUserByIdAsync(int id) { }

// Private fields: _camelCase (underscore prefix)
private readonly IUserRepository _userRepository;

// Properties: PascalCase
public string Username { get; set; }

// Parameters, Local variables: camelCase
public void CreateUser(string username, int age) { }

// Constants: UPPER_SNAKE_CASE or PascalCase
public const int MAX_LOGIN_ATTEMPTS = 5;
```

### JavaScript (Frontend)
```javascript
// Functions, Variables: camelCase
function getUserById(userId) { }
const userName = "John";

// Classes: PascalCase
class UserManager { }

// Constants: UPPER_SNAKE_CASE
const API_BASE_URL = "https://api.example.com";
const MAX_RETRY_ATTEMPTS = 3;
```

## SOLID Principles (Quick Reference)

### 1. Single Responsibility Principle (SRP)
```csharp
// ✅ Good - Each class has one responsibility
public class UserService : IUserService { }
public class EmailService : IEmailService { }

// ❌ Bad - UserService doing multiple things
public class UserService {
    public Task SendEmail() { } // Should be in EmailService
}
```

### 2. Open/Closed Principle (OCP)
```csharp
// ✅ Good - Open for extension via abstraction
public interface IPaymentProcessor { }
public class CreditCardProcessor : IPaymentProcessor { }
public class BkashProcessor : IPaymentProcessor { } // Add new without modifying existing
```

### 3. Liskov Substitution Principle (LSP)
```csharp
// ✅ Good - All implementations are interchangeable
public interface IRepository<T> { Task<T> GetByIdAsync(int id); }
public class UserRepository : IRepository<User> { }
public class ProductRepository : IRepository<Product> { }
```

### 4. Interface Segregation Principle (ISP)
```csharp
// ✅ Good - Small, focused interfaces
public interface IReadRepository<T> { Task<T> GetByIdAsync(int id); }
public interface IWriteRepository<T> { Task CreateAsync(T entity); }

// ❌ Bad - Fat interface with unused methods
public interface IRepository<T> {
    Task BulkInsertAsync(IEnumerable<T> entities); // Not needed by all
}
```

### 5. Dependency Inversion Principle (DIP)
```csharp
// ✅ Good - Depend on abstraction
public class UsersController {
    private readonly IUserService _userService; // Interface
    public UsersController(IUserService userService) { }
}

// ❌ Bad - Depend on concrete class
public class UsersController {
    private readonly UserService _userService = new UserService(); // Tight coupling
}
```

## Multi-Tier Caching Pattern
```csharp
public async Task<T> GetWithCacheAsync<T>(string key)
{
    // L1: Memory Cache (fastest - ~1ms)
    if (_memoryCache.TryGetValue(key, out T value))
        return value;

    // L2: Redis Cache (~5-10ms)
    var redisValue = await _redis.GetAsync<T>(key);
    if (redisValue != null)
    {
        _memoryCache.Set(key, redisValue, TimeSpan.FromMinutes(5));
        return redisValue;
    }

    // L3: PostgreSQL Cache (fallback - ~50-100ms)
    var dbValue = await _postgresCache.GetAsync<T>(key);
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

### ❌ DON'T DO THIS:
1. **jQuery Ajax**: `$.ajax()`, `$.get()`, `$.post()` → Use `fetch()` instead
2. **Expose Entities**: Return domain entities from controllers → Use DTOs
3. **Business Logic in Controllers**: Controllers should be thin → Delegate to services
4. **Synchronous I/O**: `GetUser()` → Use `async Task<User> GetUserAsync()`
5. **New Keyword for Services**: `new UserService()` → Use dependency injection
6. **Database Access in Application Layer**: NO direct DbContext → Use repositories
7. **Domain Depends on Infrastructure**: Domain should be independent
8. **Mixed Concerns**: Keep layers separated

### ✅ DO THIS:
1. **Use Fetch API**: `await callApi('/api/users', 'GET')`
2. **Return DTOs**: `return Ok(new ApiResponse<UserDTO> { Data = userDto })`
3. **Thin Controllers**: `var user = await _userService.GetUserByIdAsync(id);`
4. **Async I/O**: `public async Task<User> GetUserByIdAsync(int id)`
5. **Dependency Injection**: `public UsersController(IUserService userService)`
6. **Repository Interfaces**: `await _repository.Users.GetByIdAsync(id)`
7. **Independent Domain**: No EF, no HTTP, no external dependencies
8. **Single Responsibility**: Each class does one thing well

## Example: Complete CRUD Implementation

### 1. Domain Entity
```csharp
public class Product
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentException("Price must be positive");
        Price = newPrice;
    }
}
```

### 2. Repository Interface (Domain)
```csharp
public interface IProductRepository
{
    Task<Product> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product> CreateAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
}
```

### 3. Repository Implementation (Infrastructure)
```csharp
public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public async Task<Product> GetByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<Product> CreateAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }
}
```

### 4. Service (Application.Services)
```csharp
public class ProductService : IProductService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public async Task<ProductDTO> GetProductByIdAsync(int id)
    {
        var product = await _repository.Products.GetByIdAsync(id);
        if (product == null)
            throw new ProductNotFoundException(id);
        return _mapper.Map<ProductDTO>(product);
    }

    public async Task<ProductDTO> CreateProductAsync(CreateProductDTO dto)
    {
        var product = _mapper.Map<Product>(dto);
        var created = await _repository.Products.CreateAsync(product);
        await _repository.SaveAsync();
        return _mapper.Map<ProductDTO>(created);
    }
}
```

### 5. API Controller (Presentation)
```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(id);
            return Ok(new ApiResponse<ProductDTO>
            {
                CorrelationId = HttpContext.TraceIdentifier,
                StatusCode = 200,
                Success = true,
                Data = product,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (ProductNotFoundException ex)
        {
            return NotFound(new ApiResponse<ProductDTO>
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

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO dto)
    {
        var product = await _productService.CreateProductAsync(dto);
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id },
            new ApiResponse<ProductDTO> { Success = true, Data = product });
    }
}
```

### 6. Frontend (JavaScript + Kendo Grid)
```javascript
// Initialize Grid
$("#productsGrid").kendoGrid({
    dataSource: {
        transport: {
            read: async (options) => {
                const result = await callApi('/api/products', 'GET');
                options.success(result.data);
            },
            create: async (options) => {
                const result = await callApi('/api/products', 'POST', options.data);
                options.success(result.data);
            },
            update: async (options) => {
                const result = await callApi(`/api/products/${options.data.id}`, 'PUT', options.data);
                options.success(result.data);
            },
            destroy: async (options) => {
                await callApi(`/api/products/${options.data.id}`, 'DELETE');
                options.success();
            }
        },
        schema: { model: { id: "id" } }
    },
    editable: "inline",
    toolbar: ["create"],
    columns: [
        { field: "name", title: "Product Name" },
        { field: "price", title: "Price", format: "{0:c}" },
        { command: ["edit", "destroy"], width: 200 }
    ]
});
```

## Performance Targets
- **Page Load**: < 2 seconds (initial load)
- **API Response**: < 500ms (95th percentile)
- **Grid Rendering**: < 1 second (1000 rows)
- **Search**: < 300ms (with caching)
- **Concurrent Users**: 1000+ simultaneous users

## Documentation References
- `/doc/PROJECT_VISION.md` - Overall project vision
- `/doc/backend_design.md` - Backend architecture details
- `/doc/frontend_design.md` - Frontend UI/UX design system
- `/doc/coding_architecture.md` - Layer structure and SOLID principles
- `/doc/form_and_grid_design.md` - Form and grid design patterns
