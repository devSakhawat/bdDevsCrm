# Developer Guide - bdDevsCrm Project

**Welcome to bdDevsCrm!** This comprehensive guide will help you understand the project architecture, coding standards, and development workflow.

---

## 📋 Table of Contents

1. [Project Overview](#project-overview)
2. [Getting Started](#getting-started)
3. [Architecture & Design](#architecture--design)
4. [Coding Standards](#coding-standards)
5. [Development Workflow](#development-workflow)
6. [Common Patterns](#common-patterns)
7. [UI/UX Guidelines](#uiux-guidelines)
8. [Testing](#testing)
9. [Troubleshooting](#troubleshooting)
10. [FAQs](#faqs)

---

## 📖 Project Overview

### What is bdDevsCrm?

**bdDevsCrm** is an enterprise-grade Customer Relationship Management (CRM) system designed to support **60,000+ concurrent users** with 30+ business modules including:

- **HRIS (Human Resource Information System)**
- **Bonus Payment Management**
- **CRM (Customer Relationship Management)**
- **DMS (Document Management System)**
- **30+ additional business modules**

### Technology Stack

#### Backend
- **.NET 10.0** - Latest LTS framework
- **Entity Framework Core 8.0+** - ORM with MS SQL Server
- **Mapster 10.0.7** - High-performance object mapping
- **FluentValidation 11.12.0** - Input validation
- **Serilog** - Structured logging
- **JWT Bearer** - Authentication
- **Multi-tier Caching** - IMemoryCache → Redis → PostgreSQL

#### Frontend
- **ASP.NET Core MVC** - Server-side rendering
- **Razor Pages** - View engine
- **jQuery** - DOM manipulation (NOT for Ajax)
- **Kendo UI 2024 Q4** - Enterprise UI components
- **JavaScript Fetch API** - HTTP communication

### Project Stats

- **14 Projects** in solution
- **93 Domain Entities**
- **81 Service Interfaces**
- **78 Controllers**
- **90 CRUD Record Types**
- **0 Build Errors** (423 warnings - mostly nullable references)
- **~65% Backend Complete**

---

## 🚀 Getting Started

### Prerequisites

1. **.NET 10.0 SDK** or later
2. **Visual Studio 2022** (17.8+) or **VS Code** with C# extension
3. **MS SQL Server 2019+** or **SQL Server Express**
4. **Redis** (for distributed caching)
5. **PostgreSQL** (optional - for cache fallback)
6. **Git** for version control

### Initial Setup

1. **Clone the repository:**
```bash
git clone https://github.com/devSakhawat/bdDevsCrm.git
cd bdDevsCrm
```

2. **Restore NuGet packages:**
```bash
dotnet restore
```

3. **Build the solution:**
```bash
dotnet build
```

4. **Update connection strings:**
   - Edit `Presentation.Api/appsettings.json`
   - Edit `Presentation.Mvc/appsettings.json`
   - Configure your SQL Server connection

5. **Run migrations:**
```bash
dotnet ef database update --project Infrastructure.Sql
```

6. **Run the application:**
```bash
# API
dotnet run --project Presentation.Api

# MVC
dotnet run --project Presentation.Mvc
```

### Project Structure

```
bdDevsCrm/
├── Domain.Entities/              # Core business entities (93 files)
├── Domain.Contracts/             # Service interfaces (81 files)
├── Domain.Exceptions/            # Custom exceptions
│
├── Application.Services/         # Business logic (SINGLE project)
│   ├── Core/SystemAdmin/         # System admin services
│   ├── Core/HR/                  # HR services
│   ├── CRM/                      # CRM services
│   ├── DMS/                      # Document services
│   ├── Authentication/           # Auth services
│   ├── Mappings/                 # Mapster profiles
│   └── Validators/               # FluentValidation validators
│
├── Infrastructure.Repositories/  # Data access implementations
├── Infrastructure.Sql/           # EF Core, DbContext, Migrations
├── Infrastructure.Security/      # Authentication, Encryption
├── Infrastructure.Utilities/     # Helper functions
│
├── Presentation.Controller/      # Shared controllers (78 files)
├── Presentation.Api/             # RESTful Web API
├── Presentation.Mvc/             # MVC + Razor frontend
│
├── bdDevs.Shared/                # Shared Kernel
│   ├── Records/                  # C# record types (90 files)
│   ├── ApiResponse/              # Unified response format
│   ├── DataTransferObjects/      # DTOs
│   └── Grid/                     # Grid utilities
│
└── Tests/
    ├── bdDevsCrm.UnitTests/
    └── bdDevsCrm.IntegrationTests/
```

---

## 🏗️ Architecture & Design

### Clean Architecture

This project strictly follows **Clean Architecture** principles with clear layer separation:

```
┌─────────────────────────────────────────────┐
│         Presentation Layer                  │
│    (API + MVC + Shared Controllers)         │
└──────────────────┬──────────────────────────┘
                   │ depends on
┌──────────────────▼──────────────────────────┐
│       Application Layer                     │
│      (Application.Services)                 │
└──────────────────┬──────────────────────────┘
                   │ depends on
┌──────────────────▼──────────────────────────┐
│       Infrastructure Layer                  │
│  (Repositories, Sql, Security, Utilities)   │
└──────────────────┬──────────────────────────┘
                   │ depends on
┌──────────────────▼──────────────────────────┐
│         Domain Layer                        │
│    (Entities, Contracts, Exceptions)        │
└─────────────────────────────────────────────┘
                   ▲
                   │ used by all
┌──────────────────┴──────────────────────────┐
│         Shared Kernel                       │
│          (bdDevs.Shared)                    │
└─────────────────────────────────────────────┘
```

### Layer Responsibilities

#### 1. Domain Layer (Core)
**Location:** `Domain.Entities`, `Domain.Contracts`, `Domain.Exceptions`

**Responsibilities:**
- Define core business entities
- Define repository interfaces
- Define service interfaces
- Define custom exceptions
- Business rules and invariants

**Rules:**
- ✅ NO external dependencies
- ✅ Pure C# business logic
- ❌ NEVER depend on Infrastructure or Application
- ✅ Entities should validate their own state

**Example:**
```csharp
// Domain.Entities/Entities/System/Company.cs
public class Company
{
    public int CompanyId { get; private set; }
    public string CompanyName { get; private set; }
    public string Code { get; private set; }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Company name cannot be empty");
        CompanyName = newName;
    }
}
```

#### 2. Application Layer
**Location:** `Application.Services` (SINGLE simplified project)

**Responsibilities:**
- Implement business workflows
- Orchestrate domain objects
- Transform entities to DTOs
- Validate input using FluentValidation
- Transaction management

**Rules:**
- ✅ Depend on Domain layer only
- ✅ Return DTOs (NEVER entities)
- ✅ Use repository interfaces (NOT implementations)
- ❌ NO database access directly
- ❌ NO HTTP concerns

**Example:**
```csharp
// Application.Services/Core/SystemAdmin/CompanyService.cs
public class CompanyService : ICompanyService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CompanyService> _logger;

    public async Task<CompanyDto> CreateAsync(CreateCompanyRecord record)
    {
        // 1. Map record to entity
        var company = record.Adapt<Company>();

        // 2. Business logic
        await _repository.Companies.CreateAsync(company);
        await _repository.SaveAsync();

        // 3. Return DTO
        return company.Adapt<CompanyDto>();
    }
}
```

#### 3. Infrastructure Layer
**Location:** `Infrastructure.Repositories`, `Infrastructure.Sql`, etc.

**Responsibilities:**
- Implement repository interfaces
- Configure EF Core mappings
- Implement caching strategies
- External service integrations

**Rules:**
- ✅ Implement interfaces from Domain
- ✅ Use dependency injection
- ❌ NO business logic
- ✅ Handle database migrations

**Example:**
```csharp
// Infrastructure.Repositories/Core/SystemAdmin/CompanyRepository.cs
public class CompanyRepository : ICompanyRepository
{
    private readonly ApplicationDbContext _context;

    public async Task<Company> GetByIdAsync(int id)
    {
        return await _context.Companies
            .Include(c => c.Status)
            .FirstOrDefaultAsync(c => c.CompanyId == id);
    }
}
```

#### 4. Presentation Layer
**Location:** `Presentation.Api`, `Presentation.Mvc`, `Presentation.Controller`

**Responsibilities:**
- Handle HTTP requests/responses
- Route mapping
- Authentication/Authorization
- Input validation
- Return unified ApiResponse<T>

**Rules:**
- ✅ Thin controllers (delegate to services)
- ✅ Use standard HTTP status codes
- ✅ Return ApiResponse<T>
- ❌ NO business logic

**Example:**
```csharp
// Presentation.Controller/Controllers/Core/SystemAdmin/CompanyController.cs
[ApiController]
[Route("api/[controller]")]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _service;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCompanyRecord record)
    {
        var result = await _service.CreateAsync(record);
        return Ok(new ApiResponse<CompanyDto>
        {
            CorrelationId = HttpContext.TraceIdentifier,
            StatusCode = 200,
            Success = true,
            Message = "Company created successfully",
            Data = result,
            Timestamp = DateTime.UtcNow
        });
    }
}
```

### SOLID Principles

This project strictly follows SOLID principles:

#### Single Responsibility Principle (SRP)
Each class has ONE reason to change.

```csharp
// ✅ Good
public class UserService : IUserService { }
public class EmailService : IEmailService { }

// ❌ Bad
public class UserService {
    public Task SendEmail() { } // Should be in EmailService
}
```

#### Open/Closed Principle (OCP)
Open for extension, closed for modification.

```csharp
// ✅ Good
public interface IPaymentProcessor { }
public class BkashProcessor : IPaymentProcessor { }
public class NagadProcessor : IPaymentProcessor { }
```

#### Liskov Substitution Principle (LSP)
Derived classes must be substitutable for base classes.

```csharp
// ✅ Good
public interface IRepository<T> {
    Task<T> GetByIdAsync(int id);
}
```

#### Interface Segregation Principle (ISP)
No client should depend on methods it doesn't use.

```csharp
// ✅ Good
public interface IReadRepository<T> { }
public interface IWriteRepository<T> { }

// ❌ Bad
public interface IRepository<T> {
    Task BulkInsertAsync(IEnumerable<T> items); // Not needed by all
}
```

#### Dependency Inversion Principle (DIP)
Depend on abstractions, not concretions.

```csharp
// ✅ Good
public CompanyController(ICompanyService service) { }

// ❌ Bad
public CompanyController() {
    var service = new CompanyService(); // Tight coupling
}
```

---

## 💻 Coding Standards

### Naming Conventions

#### C# (Backend)

```csharp
// Classes, Interfaces, Methods: PascalCase
public class UserService : IUserService { }
public async Task<UserDto> GetUserByIdAsync(int id) { }

// Private fields: _camelCase (underscore prefix)
private readonly IUserRepository _userRepository;

// Properties: PascalCase
public string Username { get; set; }

// Parameters, Local Variables: camelCase
public void CreateUser(string username, int age)
{
    var isValid = true;
}

// Constants: UPPER_SNAKE_CASE
public const int MAX_LOGIN_ATTEMPTS = 5;
public const string DEFAULT_ROLE = "User";

// Acronyms 3+ letters: PascalCase (NOT ALL CAPS)
public class CrmCourse { }      // ✅ Crm (NOT CRM)
public class DmsDocument { }    // ✅ Dms (NOT DMS)
public class IeltsScore { }     // ✅ Ielts (NOT IELTS)
public class ToeflTest { }      // ✅ Toefl (NOT TOEFL)
```

#### JavaScript (Frontend)

```javascript
// Functions, Variables: camelCase
function saveEmployee() { }
const employeeId = 123;
let userName = "John";

// Classes: PascalCase
class UserManager { }

// Constants: UPPER_SNAKE_CASE
const API_BASE_URL = "/api";
const MAX_RETRY_ATTEMPTS = 3;

// jQuery cache: $ prefix
const $saveBtn = $("#btnSave");
const $grid = $("#gridEmployee");

// Event handlers: on + EventType + Target
function onClickSaveBtn() { }
function onChangeStatus() { }
```

#### CSS (BEM-inspired)

```css
/* Block */
.employee-form { }
.bonus-grid { }

/* Block__Element */
.employee-form__header { }
.bonus-grid__toolbar { }

/* Block--Modifier */
.employee-form--readonly { }
.btn--primary { }

/* Utility classes */
.mt-16 { }
.text-center { }
.hidden { }
```

### File Naming

| Type | Convention | Example |
|------|-----------|---------|
| C# Class | PascalCase.cs | `UserService.cs` |
| C# Interface | IPascalCase.cs | `IUserService.cs` |
| JavaScript | camelCase.js | `employeeGrid.js` |
| Razor View | PascalCase.cshtml | `Index.cshtml` |
| Partial View | _PascalCase.cshtml | `_EmployeeForm.cshtml` |
| CSS/SCSS | kebab-case.scss | `form-layout.scss` |

---

## 🔄 Development Workflow

### 1. Adding a New Feature (CRUD Entity)

**Step 1: Create Domain Entity**
```csharp
// Domain.Entities/Entities/System/Department.cs
public class Department
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public int CompanyId { get; set; }
    public int StatusId { get; set; }
}
```

**Step 2: Create CRUD Records**
```csharp
// bdDevs.Shared/Records/Core/SystemAdmin/DepartmentRecords.cs
namespace bdDevs.Shared.Records.Core.SystemAdmin;

public record CreateDepartmentRecord(
    string DepartmentName,
    int CompanyId,
    int StatusId
);

public record UpdateDepartmentRecord(
    int DepartmentId,
    string DepartmentName,
    int CompanyId,
    int StatusId
);

public record DeleteDepartmentRecord(int DepartmentId);
```

**Step 3: Create Validators**
```csharp
// Application.Services/Validators/Core/SystemAdmin/DepartmentRecordValidators.cs
public class CreateDepartmentRecordValidator
    : AbstractValidator<CreateDepartmentRecord>
{
    public CreateDepartmentRecordValidator()
    {
        RuleFor(x => x.DepartmentName)
            .NotEmpty().WithMessage("Department name is required")
            .MaximumLength(100);

        RuleFor(x => x.CompanyId)
            .GreaterThan(0).WithMessage("Company is required");
    }
}
```

**Step 4: Create Service Interface**
```csharp
// Domain.Contracts/Services/Core/SystemAdmin/IDepartmentService.cs
public interface IDepartmentService
{
    Task<DepartmentDto> CreateAsync(CreateDepartmentRecord record);
    Task<DepartmentDto> UpdateAsync(UpdateDepartmentRecord record);
    Task<bool> DeleteAsync(DeleteDepartmentRecord record);
    Task<DepartmentDto> GetByIdAsync(int id);
    Task<List<DepartmentDto>> GetAllAsync();
}
```

**Step 5: Implement Service**
```csharp
// Application.Services/Core/SystemAdmin/DepartmentService.cs
public class DepartmentService : IDepartmentService
{
    private readonly IRepositoryManager _repository;

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentRecord record)
    {
        var department = record.Adapt<Department>();
        await _repository.Departments.CreateAsync(department);
        await _repository.SaveAsync();
        return department.Adapt<DepartmentDto>();
    }
}
```

**Step 6: Create Controller**
```csharp
// Presentation.Controller/Controllers/Core/SystemAdmin/DepartmentController.cs
[ApiController]
[Route("api/[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _service;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentRecord record)
    {
        var result = await _service.CreateAsync(record);
        return Ok(new ApiResponse<DepartmentDto> { Data = result });
    }
}
```

**Step 7: Register in DI Container**
```csharp
// Presentation.Api/Extensions/ServiceExtensions.cs
services.AddScoped<IDepartmentService, DepartmentService>();
```

### 2. Object Mapping

**✅ ALWAYS use Mapster:**
```csharp
using Mapster;

// Entity to DTO
var dto = entity.Adapt<DepartmentDto>();

// Collection mapping
var dtoList = entities.Adapt<List<DepartmentDto>>();

// Record to Entity
var entity = record.Adapt<Department>();
```

**❌ NEVER use:**
```csharp
MyMapper.JsonClone(entity)           // Old pattern
_mapper.Map<DepartmentDto>(entity)   // AutoMapper
```

### 3. Exception Handling

**All exceptions use root namespace:**
```csharp
using Domain.Exceptions;

// Throw custom exceptions
throw new DepartmentNotFoundException(id);
throw new DepartmentBadRequestException("Invalid data");
throw new UnauthorizedAccessException("Access denied");
```

### 4. API Response Format

**ALWAYS return ApiResponse<T>:**
```csharp
return Ok(new ApiResponse<DepartmentDto>
{
    CorrelationId = HttpContext.TraceIdentifier,
    StatusCode = 200,
    Success = true,
    Message = "Department created successfully",
    Data = result,
    Timestamp = DateTime.UtcNow
});
```

---

## 🎨 UI/UX Guidelines

### Layout Dimensions (FIXED)

```
┌────────────────────────────────────────┐
│     Header: 60px (#1E5FA8)             │
├──────────┬─────────────────────────────┤
│ Sidebar  │   Main Content Area         │
│ 240px    │   (24px padding)            │
│ (#1E293B)│   (#F1F5F9 background)      │
│ (64px    │                             │
│collapsed)│                             │
├──────────┴─────────────────────────────┤
│     Footer: 48px                       │
└────────────────────────────────────────┘
```

### Color Palette (ONLY use these)

```scss
// Primary
$color-primary:    #1E5FA8;  // Header, Primary Button
$color-primary-dk: #1E3A5F;  // Hover state
$color-accent:     #2563EB;  // Active, Focus ring
$color-primary-lt: #DBEAFE;  // Selected row

// Neutral/Gray
$color-dark:       #1E293B;  // Headings, body text
$color-mid:        #475569;  // Secondary text
$color-light:      #94A3B8;  // Helper text
$color-border:     #CBD5E1;  // All borders
$color-bg:         #F1F5F9;  // Page background

// Semantic
$color-success:    #16A34A;  // Save, Approve
$color-danger:     #DC2626;  // Delete, Error
$color-warning:    #D97706;  // Warning
$color-info:       #0284C7;  // Info
```

### Typography Scale (ONLY use these)

```
22px bold      → H1 Page Title
18px semibold  → H2 Section Title
15px semibold  → H3 Card Title
14px regular   → Important body text
13px regular   → Standard (body, labels, buttons, table cells)
12px regular   → Helper text, breadcrumb, footer
11px italic    → Validation error messages
```

### Form Layout Types

#### Type 1: Single Column (4-6 fields)
**Use for:** Simple forms, modals, leave applications

```html
<div class="form-layout form-layout--single">
    <div class="form-group">
        <label>Full Name <span class="required-star">*</span></label>
        <input type="text" class="k-input"
               data-val="true"
               data-required-msg="Full Name is required" />
        <span class="field-error-msg"></span>
    </div>
</div>
```

#### Type 2: Grid/Multi-Column (6-20 fields)
**Use for:** Complex forms, employee profiles

```html
<div class="form-layout form-layout--grid">
    <div class="form-group col-span-1">
        <label>First Name</label>
        <input class="k-input" />
    </div>
    <div class="form-group col-span-1">
        <label>Last Name</label>
        <input class="k-input" />
    </div>
    <div class="form-group col-span-2">
        <label>Address (Full Width)</label>
        <textarea class="k-input"></textarea>
    </div>
</div>
```

#### Type 3: Inline Filter
**Use for:** Search bars, filter panels

```html
<div class="form-layout form-layout--inline">
    <div class="form-group">
        <label>Department</label>
        <select class="k-dropdown"></select>
    </div>
    <div class="form-group">
        <label>Status</label>
        <select class="k-dropdown"></select>
    </div>
    <button class="k-button btn-primary">Search</button>
</div>
```

### Kendo Grid Standard Configuration

```javascript
$("#gridEmployee").kendoGrid({
    dataSource: {
        transport: {
            read: { url: "/Employee/GetList", type: "POST" }
        },
        serverPaging: true,      // ✅ REQUIRED
        serverSorting: true,     // ✅ REQUIRED
        serverFiltering: true,   // ✅ REQUIRED
        pageSize: 20,
        schema: {
            data: "data",
            total: "total"
        }
    },
    pageable: {
        pageSizes: [10, 20, 50, 100],
        buttonCount: 5
    },
    sortable: true,
    filterable: { mode: "row" },
    resizable: true,
    columns: [
        { field: "employeeCode", title: "Code", width: 120 },
        { field: "fullName", title: "Name", width: 200 },
        { field: "departmentName", title: "Department", width: 160 },
        {
            title: "Actions",
            width: 140,
            template: function(dataItem) {
                return `
                    <button class="k-button btn-sm btn-secondary btn-edit"
                            data-id="${dataItem.id}">Edit</button>
                    <button class="k-button btn-sm btn-danger btn-delete"
                            data-id="${dataItem.id}">Delete</button>
                `;
            }
        }
    ]
});
```

### Frontend API Calls (Fetch API ONLY)

```javascript
// ✅ ALWAYS use Fetch API
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
    if (!response.ok) {
        const error = await response.json();
        throw new Error(error.message || 'Request failed');
    }
    return await response.json();
}

// Usage
const user = await callApi('/api/users/123', 'GET');
const newUser = await callApi('/api/users', 'POST', {
    name: 'John Doe',
    email: 'john@example.com'
});
```

**❌ NEVER use jQuery Ajax:**
```javascript
$.ajax({ url: '/api/users' })      // BAD
$.get('/api/users')                // BAD
$.post('/api/users', data)         // BAD
```

---

## 🧪 Testing

### Unit Testing

```csharp
// bdDevsCrm.UnitTests/Services/CompanyServiceTests.cs
public class CompanyServiceTests
{
    private readonly Mock<IRepositoryManager> _mockRepo;
    private readonly CompanyService _service;

    public CompanyServiceTests()
    {
        _mockRepo = new Mock<IRepositoryManager>();
        _service = new CompanyService(_mockRepo.Object);
    }

    [Fact]
    public async Task CreateAsync_ValidRecord_ReturnsDto()
    {
        // Arrange
        var record = new CreateCompanyRecord("Test Co", "TST", 1);

        // Act
        var result = await _service.CreateAsync(record);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Co", result.CompanyName);
    }
}
```

### Integration Testing

```csharp
// bdDevsCrm.IntegrationTests/Controllers/CompanyControllerTests.cs
public class CompanyControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    [Fact]
    public async Task Create_ValidData_Returns200()
    {
        // Arrange
        var record = new CreateCompanyRecord("Test", "TST", 1);

        // Act
        var response = await _client.PostAsJsonAsync("/api/company", record);

        // Assert
        response.EnsureSuccessStatusCode();
    }
}
```

---

## 🔧 Troubleshooting

### Common Issues

#### Build Errors

**Problem:** `The type or namespace name 'X' could not be found`
**Solution:** Check project references and using statements

**Problem:** `Mapster not found`
**Solution:** Install Mapster package:
```bash
dotnet add package Mapster
```

#### Runtime Errors

**Problem:** `No service for type 'IXxxService' has been registered`
**Solution:** Register service in DI container:
```csharp
services.AddScoped<IXxxService, XxxService>();
```

**Problem:** `Nullable reference warning`
**Solution:** Add null checks or use null-forgiving operator `!`

#### Frontend Issues

**Problem:** Grid not loading data
**Solution:** Check server-side paging configuration and API endpoint

**Problem:** Validation not working
**Solution:** Ensure `data-val="true"` attribute is set on inputs

---

## ❓ FAQs

### Q: Should I use AutoMapper or Mapster?
**A:** ALWAYS use **Mapster**. AutoMapper and MyMapper are not used in this project.

### Q: Can I return entities from controllers?
**A:** NO. ALWAYS return DTOs wrapped in `ApiResponse<T>`.

### Q: Should I use jQuery for Ajax calls?
**A:** NO. Use JavaScript **Fetch API** for all HTTP calls.

### Q: Can I use client-side paging for Kendo Grid?
**A:** NO. ALWAYS use **server-side paging** for performance.

### Q: Where should I put business logic?
**A:** In **Service classes** in `Application.Services` layer, NOT in controllers.

### Q: Can Domain layer depend on Infrastructure?
**A:** NO. Domain must be **independent** of all outer layers.

### Q: What naming convention for acronyms?
**A:** 3+ letter acronyms use **PascalCase** (Crm, Dms, NOT CRM, DMS).

### Q: Should I create separate projects for ServiceContracts?
**A:** NO. Everything is in **Application.Services** (simplified architecture).

---

## 📚 Additional Resources

### Documentation
- `/doc/PROJECT_VISION.md` - Project vision and goals
- `/doc/backend_design.md` - Backend architecture details
- `/doc/frontend_design.md` - Frontend patterns and guidelines
- `/doc/coding_architecture.md` - SOLID principles and layer structure
- `/doc/HRIS_UIDesign_Documentation.md` - Complete UI component guide

### External Links
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [SOLID Principles](https://www.digitalocean.com/community/conceptual_articles/s-o-l-i-d-the-first-five-principles-of-object-oriented-design)
- [Mapster Documentation](https://github.com/MapsterMapper/Mapster)
- [Kendo UI Documentation](https://docs.telerik.com/kendo-ui/)
- [FluentValidation](https://docs.fluentvalidation.net/)

---

## 🤝 Contributing

### Before You Start
1. Read this guide thoroughly
2. Understand Clean Architecture principles
3. Follow coding standards strictly
4. Write tests for new features
5. Update documentation if needed

### Code Review Checklist
- [ ] Follows Clean Architecture layers
- [ ] Uses Mapster for mapping
- [ ] Returns DTOs (not entities)
- [ ] Uses async/await for I/O
- [ ] Uses dependency injection
- [ ] Follows naming conventions
- [ ] Includes validation
- [ ] Has unit tests
- [ ] Frontend uses Fetch API
- [ ] Grid uses server-side paging

---

## 📝 Summary

### Key Takeaways

**Backend:**
1. ✅ Use **Mapster** (NOT AutoMapper/MyMapper)
2. ✅ Return **DTOs** (NOT entities)
3. ✅ Use **async/await** for all I/O
4. ✅ Use **dependency injection**
5. ✅ Keep controllers **thin**
6. ✅ Domain is **independent**

**Frontend:**
1. ✅ Use **Fetch API** (NOT jQuery Ajax)
2. ✅ Use **server-side paging** for grids
3. ✅ Follow **3 form layout types**
4. ✅ Use **color palette** only
5. ✅ Use **typography scale** only
6. ✅ jQuery for **DOM only**

**Remember:** When in doubt, check existing code patterns and documentation!

---

**Last Updated:** 2026-04-19
**Version:** 1.0.0
**Maintainer:** Development Team

**Happy Coding! 🚀**
