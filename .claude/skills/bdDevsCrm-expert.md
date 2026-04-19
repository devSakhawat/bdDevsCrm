# bdDevsCrm Expert - Claude Skill

## Skill Identity
**Name**: bdDevsCrm Expert
**Version**: 1.0.0
**Purpose**: Enterprise CRM development assistant for bdDevsCrm project following Clean Architecture and SOLID principles

---

## Project Context

### Project Overview
- **Name**: bdDevsCrm - Enterprise CRM System
- **Modules**: HRIS + BonusPayment + 30+ business modules
- **Scale**: 60,000+ concurrent users
- **Architecture**: Clean Architecture + SOLID Principles (strictly enforced)
- **Target Framework**: .NET 10.0
- **Current Status**: Backend ~65% complete, 0 build errors

### Technology Stack

**Backend:**
- .NET 10.0 Web API
- Entity Framework Core 8.0+ with MS SQL Server
- Mapster 10.0.7 (object mapping - NOT AutoMapper)
- FluentValidation 11.12.0 (input validation)
- Serilog (structured logging)
- JWT Bearer Authentication
- AES-256 two-way password encryption

**Caching:**
- Multi-tier: L1 (IMemoryCache) → L2 (Redis) → L3 (PostgreSQL)

**Frontend:**
- ASP.NET Core MVC + Razor Pages
- jQuery (DOM manipulation ONLY)
- Kendo UI 2024 Q4 (Grid, TabStrip, Window, etc.)
- JavaScript Fetch API (NEVER jQuery.ajax)

---

## Project Structure (Verified)

```
bdDevsCrm/
├── Domain.Entities/              # 93 entity files
├── Domain.Contracts/             # 81 service interfaces
├── Domain.Exceptions/            # Custom exceptions (root namespace)
├── Application.Services/         # Single simplified project
│   ├── Core/SystemAdmin/
│   ├── Core/HR/
│   ├── CRM/
│   ├── DMS/
│   ├── Authentication/
│   ├── Mappings/
│   └── Validators/
├── Infrastructure.Repositories/
├── Infrastructure.Sql/           # EF Core, DbContext
├── Infrastructure.Security/
├── Infrastructure.Utilities/
├── Presentation.Controller/      # 78 shared controllers
├── Presentation.Api/             # RESTful Web API
├── Presentation.Mvc/             # MVC + Razor
├── bdDevs.Shared/                # Shared Kernel
│   ├── Records/Core/             # 90 C# record types
│   ├── ApiResponse/
│   ├── DataTransferObjects/
│   └── Grid/
└── Tests/
    ├── bdDevsCrm.UnitTests/
    └── bdDevsCrm.IntegrationTests/
```

**Removed Projects (don't suggest these):**
- ❌ Application.ServiceContracts
- ❌ Application.Shared
- ❌ Presentation.Logger

---

## Architecture Rules (CRITICAL)

### Clean Architecture Layers

```
Presentation (API + MVC + Controller)
    ↓ depends on
Application.Services (single project)
    ↓ depends on
Infrastructure (Repositories, Sql, Security, Utilities)
    ↓ depends on
Domain (Entities, Contracts, Exceptions)
    ↑ used by all
Shared Kernel (bdDevs.Shared)
```

**Dependency Rules:**
1. ✅ Outer layers CAN depend on inner layers
2. ❌ Domain NEVER depends on Infrastructure or Application
3. ✅ Application.Services is ONE project (simplified)
4. ✅ All projects can reference bdDevs.Shared

### SOLID Principles (Strictly Follow)

**Single Responsibility:**
```csharp
// ✅ Good
public class UserService : IUserService { }
public class EmailService : IEmailService { }

// ❌ Bad
public class UserService {
    public Task SendEmail() { } // Should be in EmailService
}
```

**Open/Closed:**
```csharp
// ✅ Good - use interfaces
public interface IPaymentProcessor { }
public class BkashProcessor : IPaymentProcessor { }
public class NagadProcessor : IPaymentProcessor { }
```

**Liskov Substitution:**
```csharp
// ✅ All implementations must be substitutable
public interface IRepository<T> { Task<T> GetByIdAsync(int id); }
```

**Interface Segregation:**
```csharp
// ✅ Good - separate interfaces
public interface IReadRepository<T> { }
public interface IWriteRepository<T> { }

// ❌ Bad - fat interface
public interface IRepository<T> {
    Task BulkInsertAsync(IEnumerable<T> items); // Not needed by all
}
```

**Dependency Inversion:**
```csharp
// ✅ Good - depend on abstraction
public UsersController(IUserService userService) { }

// ❌ Bad - depend on concrete class
public UsersController() {
    var service = new UserService(); // Tight coupling
}
```

---

## Code Generation Rules

### 1. Naming Conventions

**C# (Backend):**
```csharp
public class UserService : IUserService { }        // PascalCase
private readonly IUserRepository _userRepository;  // _camelCase (underscore)
public string Username { get; set; }               // PascalCase
public void CreateUser(string username) { }        // camelCase parameters
public const int MAX_LOGIN_ATTEMPTS = 5;           // UPPER_SNAKE_CASE

// Acronyms 3+ letters: PascalCase
public class CrmCourse { }      // ✅ Crm (NOT CRM)
public class DmsDocument { }    // ✅ Dms (NOT DMS)
public class IeltsScore { }     // ✅ Ielts (NOT IELTS)
```

**JavaScript (Frontend):**
```javascript
function saveEmployee() { }                        // camelCase
const employeeId = 123;                            // camelCase
const API_BASE_URL = "/api";                       // UPPER_SNAKE_CASE
class UserManager { }                              // PascalCase
const $saveBtn = $("#btnSave");                    // $ prefix for jQuery cache
```

**CSS (BEM-inspired):**
```css
.employee-form { }                    /* block */
.employee-form__header { }            /* element */
.employee-form--readonly { }          /* modifier */
.mt-16, .text-center { }              /* utility */
```

### 2. CRUD Records Pattern (In Progress)

**Location:** `bdDevs.Shared/Records/Core/{Area}/{Entity}Records.cs`

```csharp
namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>Record for creating a new Company.</summary>
public record CreateCompanyRecord(
    string CompanyName,
    string Code,
    int StatusId
);

/// <summary>Record for updating an existing Company.</summary>
public record UpdateCompanyRecord(
    int CompanyId,
    string CompanyName,
    string Code,
    int StatusId
);

/// <summary>Record for deleting a Company.</summary>
public record DeleteCompanyRecord(int CompanyId);
```

**Service Interface:**
```csharp
// Domain.Contracts/Services/Core/SystemAdmin/ICompanyService.cs
Task<CompanyDto> CreateAsync(CreateCompanyRecord record);
Task<CompanyDto> UpdateAsync(UpdateCompanyRecord record);
Task<bool> DeleteAsync(DeleteCompanyRecord record);
```

**Service Implementation:**
```csharp
// Application.Services/Core/SystemAdmin/CompanyService.cs
public async Task<CompanyDto> CreateAsync(CreateCompanyRecord record)
{
    // 1. Map record to entity using Mapster
    var company = record.Adapt<Company>();

    // 2. Business logic
    await _repository.Companies.CreateAsync(company);
    await _repository.SaveAsync();

    // 3. Return DTO (NOT entity)
    return company.Adapt<CompanyDto>();
}
```

**Controller:**
```csharp
// Presentation.Controller/Controllers/Core/SystemAdmin/CompanyController.cs
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateCompanyRecord record)
{
    var result = await _service.CreateAsync(record);
    return Ok(new ApiResponse<CompanyDto> {
        CorrelationId = HttpContext.TraceIdentifier,
        StatusCode = 200,
        Success = true,
        Message = "Company created successfully",
        Data = result,
        Timestamp = DateTime.UtcNow
    });
}
```

### 3. Object Mapping

**✅ Use Mapster (NOT AutoMapper, NOT MyMapper):**
```csharp
using Mapster;

// Entity to DTO
var dto = entity.Adapt<CompanyDto>();

// Collection mapping
var dtoList = entities.Adapt<List<CompanyDto>>();

// Record to Entity
var entity = record.Adapt<Company>();
```

**❌ NEVER use:**
```csharp
MyMapper.JsonClone(entity)           // Old pattern
_mapper.Map<CompanyDto>(entity)      // AutoMapper (not used)
```

### 4. Async/Await Pattern

**✅ Always use async for I/O:**
```csharp
public async Task<User> GetUserAsync(int id)
{
    return await _repository.Users.GetByIdAsync(id);
}

public async Task<IActionResult> GetUser(int id)
{
    var user = await _userService.GetUserByIdAsync(id);
    return Ok(user);
}
```

**❌ NEVER synchronous I/O:**
```csharp
public User GetUser(int id)
{
    return _repository.Users.GetById(id);  // BAD
}
```

### 5. Return DTOs, NOT Entities

**✅ Correct:**
```csharp
// Service returns DTO
public async Task<CompanyDto> GetByIdAsync(int id)
{
    var company = await _repository.Companies.GetByIdAsync(id);
    return company.Adapt<CompanyDto>();
}

// Controller returns ApiResponse<DTO>
public async Task<IActionResult> Get(int id)
{
    var dto = await _service.GetByIdAsync(id);
    return Ok(new ApiResponse<CompanyDto> { Data = dto });
}
```

**❌ Wrong:**
```csharp
// NEVER return entities
public async Task<Company> GetByIdAsync(int id)
{
    return await _repository.Companies.GetByIdAsync(id);
}
```

### 6. Dependency Injection

**✅ Always use constructor injection:**
```csharp
public class UserService : IUserService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IRepositoryManager repository,
        IMapper mapper,
        ILogger<UserService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }
}
```

**❌ NEVER use new keyword for services:**
```csharp
var service = new UserService();  // BAD
```

---

## Frontend Rules

### 1. Layout Dimensions (FIXED - from HRIS doc)

```
Header:  60px  (NOT 70px) - #1E5FA8 background
Footer:  48px  (NOT 20px)
Sidebar: 240px expanded / 64px collapsed - #1E293B background
Main:    Remaining space - 24px padding, #F1F5F9 background
```

### 2. Form Layout Types

**Type 1: Single Column (4-6 fields)**
```html
<div class="form-layout form-layout--single">
    <div class="form-group">
        <label>Full Name <span class="required-star">*</span></label>
        <input class="k-input" data-val="true" />
        <span class="field-error-msg"></span>
    </div>
</div>
```

**Type 2: Grid/Multi-Column (6-20+ fields)**
```html
<div class="form-layout form-layout--grid">
    <div class="form-group col-span-1">...</div>
    <div class="form-group col-span-1">...</div>
    <div class="form-group col-span-2">...</div> <!-- full width -->
</div>
```

**Type 3: Inline Filter**
```html
<div class="form-layout form-layout--inline">
    <div class="form-group">...</div>
    <button class="k-button btn-primary">Search</button>
</div>
```

### 3. API Communication

**✅ ALWAYS use Fetch API:**
```javascript
async function callApi(url, method = 'GET', data = null) {
    const token = localStorage.getItem('authToken');
    const config = {
        method: method,
        headers: {
            'Content-Type': 'application/json',
            'Authorization': token ? `Bearer ${token}` : ''
        }
    };
    if (data && ['POST','PUT','PATCH'].includes(method)) {
        config.body = JSON.stringify(data);
    }
    const response = await fetch(url, config);
    if (!response.ok) throw new Error(await response.text());
    return await response.json();
}
```

**❌ NEVER use jQuery Ajax:**
```javascript
$.ajax({ url: '/api/users' })      // BAD
$.get('/api/users')                // BAD
$.post('/api/users', data)         // BAD
```

### 4. Kendo Grid Configuration

**✅ ALWAYS use server-side paging:**
```javascript
$("#gridEmployee").kendoGrid({
    dataSource: {
        transport: {
            read: { url: "/Employee/GetList", type: "POST" }
        },
        serverPaging: true,      // REQUIRED
        serverSorting: true,     // REQUIRED
        serverFiltering: true,   // REQUIRED
        pageSize: 20
    },
    pageable: { pageSizes: [10, 20, 50, 100] },
    sortable: true,
    filterable: { mode: "row" },
    resizable: true
});
```

**❌ NEVER client-side paging:**
```javascript
serverPaging: false    // BAD for large datasets
```

### 5. Color Palette (ONLY use these)

```scss
// Primary
$color-primary:    #1E5FA8;  // Header, Primary Button
$color-primary-dk: #1E3A5F;  // Hover
$color-accent:     #2563EB;  // Active, Focus

// Neutral
$color-dark:       #1E293B;  // Headings
$color-mid:        #475569;  // Body text
$color-light:      #94A3B8;  // Helper text
$color-border:     #CBD5E1;  // Borders
$color-bg:         #F1F5F9;  // Background

// Semantic
$color-success:    #16A34A;  // Save, Approve
$color-danger:     #DC2626;  // Delete, Error
$color-warning:    #D97706;  // Warning
$color-info:       #0284C7;  // Info
```

### 6. Typography Scale (ONLY use these sizes)

```
22px bold      → Page Title (H1)
18px semibold  → Section Title (H2)
15px semibold  → Card Title (H3)
14px regular   → Important body text
13px regular   → Standard body, labels, buttons, table cells
12px regular   → Helper text, breadcrumb, footer
11px italic    → Validation error messages
```

---

## Common Mistakes to AVOID

### Backend:
1. ❌ Using `AutoMapper` or `MyMapper` → Use `Mapster`
2. ❌ Returning entities from services → Return `DTOs`
3. ❌ Business logic in controllers → Move to services
4. ❌ Synchronous I/O → Use `async/await`
5. ❌ `new ClassName()` → Use dependency injection
6. ❌ Domain depending on Infrastructure → Fix architecture
7. ❌ Sub-namespaces in Domain.Exceptions → Use root namespace

### Frontend:
1. ❌ `jQuery.ajax()` → Use `Fetch API`
2. ❌ Client-side paging for large data → Server-side paging
3. ❌ Random colors/fonts → Use design system
4. ❌ Inline styles → Use CSS classes
5. ❌ jQuery for HTTP → Use Fetch API

---

## Build Commands

```bash
# Build entire solution
dotnet build

# Build specific project
dotnet build Presentation.Mvc/Presentation.Mvc.csproj

# Current build status: 0 errors, 423 warnings (non-critical)
```

---

## Exception Handling

**All exceptions use root namespace:**
```csharp
using Domain.Exceptions;  // Single import

// Custom exceptions
throw new UserNotFoundException(userId);
throw new CompanyBadRequestException("Invalid data");
throw new UnauthorizedAccessException("Access denied");
```

---

## API Response Format

**ALWAYS return ApiResponse<T>:**
```csharp
public class ApiResponse<T>
{
    public string CorrelationId { get; set; }  // Request tracking
    public int StatusCode { get; set; }         // 200, 404, etc.
    public bool Success { get; set; }           // true/false
    public string Message { get; set; }         // User message
    public T Data { get; set; }                 // Payload
    public List<string> Errors { get; set; }    // Error details
    public DateTime Timestamp { get; set; }     // Response time
}
```

---

## Current Migration Status

**CRUD Records Pattern:**
- Records: 90/276 (32.6%)
- Validators: 88/276 (31.9%)
- Services: 54/80 using Records+Mapster (67.5%)
- Controllers: 54/78 using Records (69.2%)

**When adding new features:**
1. Create Records in `bdDevs.Shared/Records/`
2. Create Validators in `Application.Services/Validators/`
3. Use Mapster for mapping
4. Return DTOs from services
5. Use ApiResponse<T> in controllers

---

## Documentation References

- `/doc/PROJECT_VISION.md` - Project vision
- `/doc/backend_design.md` - Backend architecture
- `/doc/frontend_design.md` - Frontend patterns
- `/doc/coding_architecture.md` - SOLID principles
- `/doc/HRIS_UIDesign_Documentation.md` - Complete UI guide

---

**When in doubt:**
1. Follow Clean Architecture layers strictly
2. Use Mapster (not AutoMapper/MyMapper)
3. Return DTOs (not entities)
4. Use Fetch API (not jQuery Ajax)
5. Server-side paging for grids
6. Stick to the color/typography palette
7. Follow the 3 form layout types

**Last Updated:** 2026-04-19
**Version:** 1.0.0
