# bdDevsCrm — Backend Developer Guide

> **Audience:** Every developer writing backend code in this repository.  
> **Goal:** One source of truth for architecture, coding patterns, repository usage, naming, DB workflow, and team rules.

---

## Table of Contents

1. [Architecture Overview](#1-architecture-overview)
2. [File & Folder Structure](#2-file--folder-structure)
3. [Coding Pattern — The Full CRUD Flow](#3-coding-pattern--the-full-crud-flow)
4. [Repository Pattern](#4-repository-pattern)
5. [Naming Conventions](#5-naming-conventions)
6. [Coding Style](#6-coding-style)
7. [Database Communication](#7-database-communication)
8. [DB-First Approach (How We Work with the Database)](#8-db-first-approach)
9. [What To Do ✅](#9-what-to-do-)
10. [What NOT To Do ❌](#10-what-not-to-do-)
11. [Quick Reference Checklist](#11-quick-reference-checklist)

---

## 1. Architecture Overview

We follow **Clean Architecture** with strict, one-directional layer dependencies.

```
Presentation Layer     (Presentation.Api / Presentation.Mvc / Presentation.Controller)
        │ depends on
Application Layer      (Application.Services)
        │ depends on
Infrastructure Layer   (Infrastructure.Repositories / Infrastructure.Sql / Infrastructure.Security / Infrastructure.Utilities)
        │ depends on
Domain Layer           (Domain.Entities / Domain.Contracts / Domain.Exceptions)
        ▲
        │ used by all layers
Shared Kernel          (bdDevs.Shared)
```

### Layer Responsibilities

| Layer | Project(s) | What It Does |
|---|---|---|
| **Domain** | `Domain.Entities`, `Domain.Contracts`, `Domain.Exceptions` | Business entities, repository interfaces, service interfaces, custom exceptions. **Zero** external dependencies. |
| **Application** | `Application.Services` | Business logic, use-case orchestration, input validation, mapping. No HTTP concerns; no direct DB access. |
| **Infrastructure** | `Infrastructure.Repositories`, `Infrastructure.Sql`, `Infrastructure.Security`, `Infrastructure.Utilities` | EF Core context, repository implementations, caching, JWT/password security, helpers. |
| **Presentation** | `Presentation.Controller`, `Presentation.Api`, `Presentation.Mvc` | Thin controllers, middleware pipeline, API bootstrapping, Swagger. No business logic. |
| **Shared Kernel** | `bdDevs.Shared` | `ApiResponse<T>`, C# Records, DTOs, constants, extension methods, grid utilities. |

### Hard Rules

```
✅  Outer layer → inner layer   (always OK)
❌  Domain → anything else      (NEVER)
❌  Application → Infrastructure (NEVER — use repository interfaces)
❌  Business logic in controllers (NEVER)
```

---

## 2. File & Folder Structure

```
bdDevsCrm/
│
├── Domain.Entities/
│   └── Entities/
│       ├── System/          # Company, Branch, Department …
│       ├── CRM/             # CrmCourse, CrmCountry …
│       └── DMS/             # DmsDocument …
│
├── Domain.Contracts/
│   ├── Repositories/        # IRepositoryManager, IRepositoryBase<T>
│   │   ├── Core/SystemAdmin/
│   │   ├── Core/HR/
│   │   ├── CRM/
│   │   └── DMS/
│   └── Services/            # IServiceManager, IXxxService
│       ├── Core/SystemAdmin/
│       ├── Core/HR/
│       ├── CRM/
│       └── DMS/
│
├── Domain.Exceptions/
│   ├── Base/                # AppException (root base class)
│   ├── BadRequest/          # BadRequestException, IdMismatchBadRequestException …
│   ├── NotFound/            # NotFoundException …
│   ├── Conflict/            # ConflictException …
│   ├── Unauthorized/        # UnauthorizedException …
│   ├── Forbidden/           # ForbiddenException …
│   └── Authentication/      # JwtSecurityException …
│
├── Application.Services/
│   ├── Core/
│   │   ├── SystemAdmin/     # CountryService, CompanyService …
│   │   └── HR/              # EmployeeService, BranchService …
│   ├── CRM/                 # CrmCourseService …
│   ├── DMS/                 # DmsDocumentService …
│   ├── Authentication/      # AuthenticationService …
│   ├── Caching/             # HybridCacheService, CacheManagementService …
│   ├── Mappings/            # Mapster configuration/profiles
│   ├── Validators/          # FluentValidation validators
│   └── ServiceManager.cs    # IServiceManager implementation (Lazy wiring)
│
├── Infrastructure.Repositories/
│   ├── RepositoryBase.cs    # Generic base with CRUD + ADO grid helpers
│   ├── RepositoryManager.cs # IRepositoryManager implementation (Lazy wiring)
│   ├── Core/SystemAdmin/    # Repository implementations
│   ├── Core/HR/
│   ├── CRM/
│   └── DMS/
│
├── Infrastructure.Sql/
│   └── Context/
│       ├── CrmContext.cs            # EF Core DbContext (DB-First generated)
│       └── Interceptors/            # AuditSaveChangesInterceptor, SlowQueryLoggingInterceptor
│
├── Infrastructure.Security/   # JWT, BCrypt password hashing
├── Infrastructure.Utilities/  # Helpers, extensions
│
├── Presentation.Controller/
│   └── Controllers/
│       ├── Base/            # BaseApiController
│       ├── Core/SystemAdmin/
│       ├── Core/HR/
│       ├── CRM/
│       └── DMS/
│
├── Presentation.Api/
│   ├── Program.cs           # DI registration + app pipeline
│   ├── Middleware/          # StandardExceptionMiddleware, CorrelationIdMiddleware …
│   └── Extensions/          # ConfigureRepositoryManager, ConfigureServiceManager …
│
├── bdDevs.Shared/
│   ├── ApiResponse/         # ApiResponse<T>, ApiResponseHelper
│   ├── Constants/           # RouteConstants, CacheKeyConstants, MessageConstants …
│   ├── DataTransferObjects/ # XxxDto classes
│   ├── Records/             # CreateXxxRecord, UpdateXxxRecord, DeleteXxxRecord
│   └── Extensions/          # MapsterExtensions (MapTo<T>, MapToList<T>)
│
└── Tests/
    ├── bdDevsCrm.UnitTests/
    └── bdDevsCrm.IntegrationTests/
```

> **Key rule:** Each new module (e.g., `Bonus`, `Payroll`) mirrors this structure — same subfolder under each layer.

---

## 3. Coding Pattern — The Full CRUD Flow

Every feature follows this **7-step recipe**, mirroring what `Country` already implements.

### Request Flow

```
HTTP Request
    └─► Controller  (Presentation.Controller)
            └─► IServiceManager.XxxService.MethodAsync(record)
                    └─► IRepositoryManager.XxxRepository.MethodAsync(entity)
                                └─► CrmContext (EF Core / ADO)
                                        └─► MS SQL Server
```

### Step-by-Step Example — Adding "Department"

#### Step 1: Domain Entity (already scaffolded from DB)

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

#### Step 2: CRUD Records (input contracts)

```csharp
// bdDevs.Shared/Records/Core/SystemAdmin/DepartmentRecords.cs
namespace bdDevs.Shared.Records.Core.SystemAdmin;

public record CreateDepartmentRecord(
    string DepartmentName,
    int CompanyId,
    int? StatusId);

public record UpdateDepartmentRecord(
    int DepartmentId,
    string DepartmentName,
    int CompanyId,
    int? StatusId);

public record DeleteDepartmentRecord(int DepartmentId);
```

#### Step 3: DTO (output contract)

```csharp
// bdDevs.Shared/DataTransferObjects/Core/SystemAdmin/DepartmentDto.cs
namespace bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

public class DepartmentDto
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public int CompanyId { get; set; }
    public int StatusId { get; set; }
}
```

#### Step 4: Repository Interface

```csharp
// Domain.Contracts/Repositories/Core/SystemAdmin/IDepartmentRepository.cs
namespace Domain.Contracts.Repositories.Core.SystemAdmin;

public interface IDepartmentRepository : IRepositoryBase<Department>
{
    Task<Department?> DepartmentAsync(int id, bool trackChanges, CancellationToken ct = default);
    Task<IEnumerable<Department>> DepartmentsAsync(bool trackChanges, CancellationToken ct = default);
}
```

#### Step 5: Service Interface

```csharp
// Domain.Contracts/Services/Core/SystemAdmin/IDepartmentService.cs
using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentDto>> DepartmentsAsync(bool trackChanges, CancellationToken ct = default);
    Task<DepartmentDto> DepartmentAsync(int id, bool trackChanges, CancellationToken ct = default);
    Task<GridEntity<DepartmentDto>> DepartmentSummaryAsync(GridOptions options, CancellationToken ct = default);
    Task<DepartmentDto> CreateAsync(CreateDepartmentRecord record, CancellationToken ct = default);
    Task<DepartmentDto> UpdateAsync(UpdateDepartmentRecord record, bool trackChanges, CancellationToken ct = default);
    Task DeleteAsync(DeleteDepartmentRecord record, bool trackChanges, CancellationToken ct = default);
}
```

#### Step 6: Repository Implementation

```csharp
// Infrastructure.Repositories/Core/SystemAdmin/DepartmentRepository.cs
namespace Infrastructure.Repositories.Core.SystemAdmin;

public class DepartmentRepository : RepositoryBase<Department>, IDepartmentRepository
{
    public DepartmentRepository(CrmContext context) : base(context) { }

    public async Task<Department?> DepartmentAsync(int id, bool trackChanges, CancellationToken ct = default)
        => await ByIdAsync(x => x.DepartmentId == id, trackChanges, ct);

    public async Task<IEnumerable<Department>> DepartmentsAsync(bool trackChanges, CancellationToken ct = default)
        => await FindAllAsync(trackChanges, ct);
}
```

#### Step 7: Service Implementation

```csharp
// Application.Services/Core/SystemAdmin/DepartmentService.cs
using bdDevs.Shared.Extensions;   // MapTo<T>, MapToList<T>
using Domain.Exceptions;

namespace Application.Services.Core.SystemAdmin;

internal sealed class DepartmentService : IDepartmentService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<DepartmentService> _logger;

    public DepartmentService(IRepositoryManager repository, ILogger<DepartmentService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentRecord record, CancellationToken ct = default)
    {
        if (record is null)
            throw new BadRequestException(nameof(CreateDepartmentRecord));

        _logger.LogInformation("Creating department: {Name}", record.DepartmentName);

        bool exists = await _repository.Departments.ExistsAsync(
            x => x.DepartmentName.Trim().ToLower() == record.DepartmentName.Trim().ToLower());

        if (exists)
            throw new ConflictException("Department with this name already exists.");

        var entity = record.MapTo<Department>();
        int id = await _repository.Departments.CreateAndIdAsync(entity, ct);
        await _repository.SaveAsync(ct);

        _logger.LogInformation("Department created: {Id}", id);
        var dto = entity.MapTo<DepartmentDto>();
        dto.DepartmentId = id;
        return dto;
    }

    public async Task<DepartmentDto> UpdateAsync(UpdateDepartmentRecord record, bool trackChanges, CancellationToken ct = default)
    {
        if (record is null)
            throw new BadRequestException(nameof(UpdateDepartmentRecord));

        var existing = await _repository.Departments.ByIdAsync(
            x => x.DepartmentId == record.DepartmentId, trackChanges: false, ct);

        if (existing is null)
            throw new NotFoundException("Department not found!");

        var entity = record.MapTo<Department>();
        _repository.Departments.UpdateByState(entity);
        await _repository.SaveAsync(ct);

        return entity.MapTo<DepartmentDto>();
    }

    public async Task DeleteAsync(DeleteDepartmentRecord record, bool trackChanges, CancellationToken ct = default)
    {
        if (record is null || record.DepartmentId <= 0)
            throw new BadRequestException("Invalid delete request.");

        var existing = await _repository.Departments.ByIdAsync(
            x => x.DepartmentId == record.DepartmentId, trackChanges: false);

        if (existing is null)
            throw new NotFoundException("Department not found!");

        await _repository.Departments.DeleteAsync(x => x.DepartmentId == record.DepartmentId, trackChanges: false, ct);
        await _repository.SaveAsync(ct);
    }

    public async Task<DepartmentDto> DepartmentAsync(int id, bool trackChanges, CancellationToken ct = default)
    {
        if (id <= 0) throw new BadRequestException("Invalid department ID.");
        var entity = await _repository.Departments.DepartmentAsync(id, trackChanges, ct);
        if (entity is null) throw new NotFoundException("Department not found!");
        return entity.MapTo<DepartmentDto>();
    }

    public async Task<IEnumerable<DepartmentDto>> DepartmentsAsync(bool trackChanges, CancellationToken ct = default)
    {
        var entities = await _repository.Departments.DepartmentsAsync(trackChanges, ct);
        return entities.MapToList<DepartmentDto>();
    }

    public async Task<GridEntity<DepartmentDto>> DepartmentSummaryAsync(GridOptions options, CancellationToken ct = default)
    {
        const string query = "SELECT * FROM Department";
        const string orderBy = "DepartmentName ASC";
        return await _repository.Departments.AdoGridDataAsync<DepartmentDto>(query, options, orderBy, "", ct);
    }
}
```

#### Step 8: Controller

```csharp
// Presentation.Controller/Controllers/Core/SystemAdmin/DepartmentController.cs
using bdDevs.Shared.Constants;

namespace Presentation.Controllers.Core.SystemAdmin;

[AuthorizeUser]
public class DepartmentController : BaseApiController
{
    public DepartmentController(IServiceManager serviceManager) : base(serviceManager) { }

    [HttpGet(RouteConstants.ReadDepartments)]
    public async Task<IActionResult> DepartmentsAsync(CancellationToken ct = default)
    {
        var list = await _serviceManager.Departments.DepartmentsAsync(trackChanges: false, ct);
        return Ok(ApiResponseHelper.Success(list, "Departments retrieved successfully."));
    }

    [HttpPost(RouteConstants.DepartmentSummary)]
    public async Task<IActionResult> DepartmentSummaryAsync([FromBody] GridOptions options, CancellationToken ct = default)
    {
        if (options is null) throw new NullModelBadRequestException(nameof(GridOptions));
        var grid = await _serviceManager.Departments.DepartmentSummaryAsync(options, ct);
        return Ok(ApiResponseHelper.Success(grid, "Department summary retrieved."));
    }

    [HttpPost(RouteConstants.CreateDepartment)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateDepartmentAsync([FromBody] CreateDepartmentRecord record, CancellationToken ct = default)
    {
        var created = await _serviceManager.Departments.CreateAsync(record, ct);
        if (created.DepartmentId <= 0)
            throw new InvalidCreateOperationException("Failed to create department.");
        return Ok(ApiResponseHelper.Created(created, "Department created successfully."));
    }

    [HttpPut(RouteConstants.UpdateDepartment)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateDepartmentAsync([FromRoute] int key, [FromBody] UpdateDepartmentRecord record, CancellationToken ct = default)
    {
        if (key != record.DepartmentId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateDepartmentRecord));
        var updated = await _serviceManager.Departments.UpdateAsync(record, trackChanges: false, ct);
        return Ok(ApiResponseHelper.Updated(updated, "Department updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteDepartment)]
    public async Task<IActionResult> DeleteDepartmentAsync([FromRoute] int key, CancellationToken ct = default)
    {
        var deleteRecord = new DeleteDepartmentRecord(key);
        await _serviceManager.Departments.DeleteAsync(deleteRecord, trackChanges: false, ct);
        return Ok(ApiResponseHelper.NoContent<object>("Department deleted successfully."));
    }
}
```

#### Step 9: Wire Up DI

```csharp
// In IRepositoryManager / RepositoryManager — add the new repository property
IDepartmentRepository Departments { get; }

// In IServiceManager / ServiceManager — add the new service property
IDepartmentService Departments { get; }
```

---

## 4. Repository Pattern

### Contract Location

```
Domain.Contracts/Repositories/Core/SystemAdmin/IDepartmentRepository.cs  ← interface
Infrastructure.Repositories/Core/SystemAdmin/DepartmentRepository.cs      ← implementation
```

### `IRepositoryBase<T>` — Available Methods (from `RepositoryBase<T>`)

```csharp
// Create
void Create(T entity);
Task CreateAsync(T entity, CancellationToken ct = default);
Task<int> CreateAndIdAsync(T entity, CancellationToken ct = default);   // returns new PK

// Read
Task<T?> ByIdAsync(Expression<Func<T, bool>> expression, bool trackChanges, CancellationToken ct = default);
Task<IEnumerable<T>> FindAllAsync(bool trackChanges, CancellationToken ct = default);
Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression, bool trackChanges, CancellationToken ct = default);
Task<bool> ExistsAsync(Expression<Func<T, bool>> expression, CancellationToken ct = default);

// Update
void UpdateByState(T entity);           // EF state-based update (no re-fetch needed)

// Delete
Task DeleteAsync(Expression<Func<T, bool>> expression, bool trackChanges, CancellationToken ct = default);

// Grid / reporting
Task<GridEntity<TDto>> AdoGridDataAsync<TDto>(string query, GridOptions options, string orderBy, string filter, CancellationToken ct = default);

// Transaction
Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default);
Task CommitTransactionAsync(IDbContextTransaction transaction);
Task RollbackTransactionAsync(IDbContextTransaction transaction);
```

### Repository Manager (Unit-of-Work)

Never instantiate repositories directly. Always access them via `IRepositoryManager`:

```csharp
// ✅ Correct
await _repository.Departments.CreateAsync(entity, ct);
await _repository.SaveAsync(ct);

// ❌ Wrong — bypasses unit-of-work
var repo = new DepartmentRepository(context);
```

Always call `SaveAsync` **after** the operation:

```csharp
await _repository.Departments.CreateAsync(entity, ct);
await _repository.SaveAsync(ct);   // ← must be explicit — don't let repo auto-save
```

### `trackChanges` Convention

| Scenario | `trackChanges` |
|---|---|
| Read-only (GET, grid, DDL) | `false` |
| Update (PUT) — load then modify | `true` |
| Update via `UpdateByState` | `false` — EF tracks by state change |

---

## 5. Naming Conventions

### C# Backend

| Element | Convention | Example |
|---|---|---|
| Class | PascalCase | `CrmCountryService` |
| Interface | I + PascalCase | `ICrmCountryService` |
| Method | PascalCase | `CountrySummaryAsync` |
| Async method | PascalCase + `Async` | `CreateAsync` |
| Property | PascalCase | `CountryName` |
| Private field | `_camelCase` | `_repository` |
| Parameter | camelCase | `record`, `trackChanges` |
| Local variable | camelCase | `countryDto` |
| Constant | UPPER_SNAKE_CASE | `MAX_LOGIN_ATTEMPTS` |

### Acronym Rule (Critical)

| Length | Rule | ✅ Correct | ❌ Wrong |
|---|---|---|---|
| 2 chars | ALL CAPS | `ID`, `UI` | `Id`, `Ui` |
| 3+ chars | PascalCase | `Crm`, `Dms`, `Ielts`, `Toefl`, `Pte`, `Gmat` | `CRM`, `DMS`, `IELTS` |

### Record Naming

```csharp
// ✅ Correct
CreateCountryRecord
UpdateCountryRecord
DeleteCountryRecord

// ❌ Wrong
CountryCreateRecord   // wrong order
CreateCountry         // missing Record suffix
```

### DTO Naming

```csharp
// ✅ Correct
CrmCountryDto
DepartmentDto
EmployeeDto

// ❌ Wrong
CountryDTO            // 3+ letter acronym must be PascalCase
```

### Namespace Rule

Namespace **must** mirror folder path:

```csharp
// ✅ Correct
namespace Application.Services.Core.SystemAdmin;
namespace Domain.Contracts.Services.CRM;
namespace Infrastructure.Repositories.Core.HR;
namespace Presentation.Controllers.Core.SystemAdmin;
```

---

## 6. Coding Style

### Dependency Injection — Constructor Injection Only

```csharp
// ✅ Correct
internal sealed class CrmCountryService : ICrmCountryService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmCountryService> _logger;
    private readonly IConfiguration _configuration;

    public CrmCountryService(
        IRepositoryManager repository,
        ILogger<CrmCountryService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _configuration = configuration;
    }
}

// ❌ Wrong — property injection, service locator, new keyword
public IRepositoryManager Repository { get; set; }   // property injection
var repo = serviceProvider.GetService<IRepositoryManager>();  // service locator
var service = new CrmCountryService(...);             // new keyword
```

### Guard Clauses — Fail Fast at the Top

```csharp
public async Task<CrmCountryDto> CreateAsync(CreateCountryRecord record, CancellationToken ct = default)
{
    // ✅ Guard at the very top — no indentation pyramid
    if (record is null)
        throw new BadRequestException(nameof(CreateCountryRecord));

    if (string.IsNullOrWhiteSpace(record.CountryName))
        throw new BadRequestException("Country name is required.");

    // ... normal logic below
}
```

### Exception Hierarchy (Use the Right One)

```csharp
using Domain.Exceptions;   // single using — all exceptions share this namespace

// 400 Bad Request — invalid input
throw new BadRequestException("Invalid request.");
throw new NullModelBadRequestException(nameof(CreateCountryRecord));
throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCountryRecord));
throw new IdParametersBadRequestException();

// 404 Not Found
throw new NotFoundException("Country not found!");
throw new NotFoundException("Country", "ID", id.ToString());

// 409 Conflict — duplicate data
throw new ConflictException("Country with this name already exists!");

// 401/403
throw new UnauthorizedException("Access denied.");
throw new ForbiddenException("You do not have permission.");
```

### Structured Logging (Required in Every Service Method)

```csharp
// ✅ Structured logging — named parameters, not string interpolation
_logger.LogInformation("Creating country: {CountryName}", record.CountryName);
_logger.LogInformation("Country created: {CountryId}", countryId);
_logger.LogWarning("Country not found: {CountryId}", countryId);

// ❌ Wrong — no structure, no searchability
_logger.LogInformation($"Creating country: {record.CountryName}");
_logger.LogInformation("Creating country: " + record.CountryName);
```

Log **intent** at the start and **outcome** at the end of each significant operation.

### Object Mapping — Mapster Only

```csharp
using bdDevs.Shared.Extensions;  // MapTo<T>, MapToList<T>

// Record → Entity
var entity = record.MapTo<CrmCountry>();

// Entity → DTO
var dto = entity.MapTo<CrmCountryDto>();

// Collection → DTO list
var dtoList = entities.MapToList<CrmCountryDto>();

// ❌ Wrong — old patterns
var entity = MyMapper.JsonClone<CrmCountry>(record);   // forbidden
var dto = _mapper.Map<CrmCountryDto>(entity);          // no AutoMapper
```

### API Response — Always `ApiResponseHelper`

```csharp
// ✅ Always use ApiResponseHelper factory methods
return Ok(ApiResponseHelper.Success(data, "Countries retrieved successfully."));
return Ok(ApiResponseHelper.Created(createdDto, "Country created successfully."));
return Ok(ApiResponseHelper.Updated(updatedDto, "Country updated successfully."));
return Ok(ApiResponseHelper.NoContent<object>("Country deleted successfully."));

// ❌ Wrong — manual construction, plain JSON
return Ok(new { success = true, data = result });       // no ApiResponse wrapper
return Ok(result);                                       // naked DTO
```

### Route Constants — No Magic Strings

```csharp
// ✅ Correct — use RouteConstants
[HttpGet(RouteConstants.ReadCountries)]
[HttpPost(RouteConstants.CountrySummary)]
[HttpPost(RouteConstants.CreateCountry)]
[HttpPut(RouteConstants.UpdateCountry)]
[HttpDelete(RouteConstants.DeleteCountry)]

// ❌ Wrong — magic strings
[HttpGet("countries")]
[HttpPost("country-summary")]
```

---

## 7. Database Communication

### Primary Path: EF Core via Repository

```
Service  →  IRepositoryManager.XxxRepository  →  RepositoryBase<T>  →  CrmContext  →  SQL Server
```

- The service **never** touches `CrmContext` directly.
- All EF queries go through repository methods.

### Read Queries — Always `trackChanges: false` (Default)

```csharp
// ✅ Correct for GET / list / DDL
var country = await _repository.Countries.ByIdAsync(x => x.CountryId == id, trackChanges: false, ct);

// Only use trackChanges: true when you plan to mutate the returned entity via EF change tracking
```

### Grid & Reporting — ADO Helper

For paginated grid data use the built-in `AdoGridDataAsync<TDto>` in `RepositoryBase<T>`:

```csharp
// Service method
public async Task<GridEntity<CrmCountryDto>> CountrySummaryAsync(GridOptions options, CancellationToken ct = default)
{
    const string query = "SELECT * FROM CrmCountry";
    const string orderBy = "CountryName ASC";
    return await _repository.Countries.AdoGridDataAsync<CrmCountryDto>(query, options, orderBy, "", ct);
}
```

> ⚠️ Raw SQL in `AdoGridDataAsync` is for **reporting/grid only**. For CRUD operations always use EF methods.

### Parameterized Queries Only

EF Core handles parameterization automatically. For raw SQL use `FromSqlInterpolated` or `SqlParameter`:

```csharp
// ✅ EF parameterized (automatic)
var country = await _context.CrmCountry.FirstOrDefaultAsync(x => x.CountryId == id);

// ✅ Raw SQL — parameterized
var result = await _context.Database.ExecuteSqlInterpolatedAsync(
    $"UPDATE CrmCountry SET Status = {status} WHERE CountryId = {id}", ct);

// ❌ String concatenation — SQL injection risk
var result = await _context.Database.ExecuteSqlRawAsync(
    "UPDATE CrmCountry SET Status = " + status);   // NEVER
```

### Multi-Tier Caching (L1 → L2 → L3)

Use the injected `IHybridCacheService` / `ICacheManagementService` — never cache inside a repository:

```csharp
// Caching belongs in the service layer (or Application.Services/Caching/)
var countries = await _hybridCache.GetOrSetAsync(
    CacheKeyConstants.CountriesDdl,
    () => _repository.Countries.ActiveCountriesAsync(false, ct),
    TimeSpan.FromMinutes(30));
```

---

## 8. DB-First Approach

### Why DB-First?

The database schema is the **source of truth**. Entities are scaffolded from the database, not the other way around.

```
SQL Server Database  →  Scaffold  →  CrmContext.cs (generated)  →  Domain Entities
```

### Scaffold Command

```bash
# Run from the Infrastructure.Sql project
dotnet ef dbcontext scaffold \
  "Server=.;Database=bdDevsCrm;Trusted_Connection=True;" \
  Microsoft.EntityFrameworkCore.SqlServer \
  --output-dir Context \
  --context CrmContext \
  --force \
  --no-build
```

### What Gets Generated — Don't Touch

| Generated File | Location | Rule |
|---|---|---|
| `CrmContext.cs` | `Infrastructure.Sql/Context/` | Never hand-edit — will be overwritten on re-scaffold |
| Entity classes (if scaffolded into entities) | `Domain.Entities/Entities/` | Never hand-edit |

### Add Custom Logic Safely (Partial Classes)

```csharp
// ✅ Correct — use partial class / partial method
// Infrastructure.Sql/Context/CrmContextExtensions.cs
public partial class CrmContext
{
    // Custom queries, overrides — not inside CrmContext.cs
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CrmCountry>()
            .Property(e => e.CountryName)
            .HasDefaultValue("");
    }
}
```

### Schema Change Workflow

```
1. Modify schema in SQL Server (add table / column / constraint)
        ↓
2. Re-run scaffold command (overwrites CrmContext.cs + entity classes)
        ↓
3. Build the solution — fix any compilation errors
        ↓
4. Update IRepositoryManager + RepositoryManager if new entity was added
        ↓
5. Update service interface + service implementation
        ↓
6. Update IServiceManager + ServiceManager
        ↓
7. Update/add CRUD Records + DTOs in bdDevs.Shared
        ↓
8. Build & test
```

> **Never** add migration files (`Add-Migration`) — we don't use code-first migrations.

---

## 9. What To Do ✅

| # | Rule |
|---|---|
| 1 | Keep controllers **thin** — one line to call a service method, one line to return a response. |
| 2 | Put **all** business rules, validation, and domain logic in the **service layer**. |
| 3 | Throw the right **domain exception** — let `StandardExceptionMiddleware` handle it. |
| 4 | Use **`CancellationToken`** in every async method — pass it through to repos. |
| 5 | Use **`trackChanges: false`** by default on every read query. |
| 6 | Use **`MapTo<T>()` / `MapToList<T>()`** (Mapster) for all object conversion. |
| 7 | Use **`ApiResponseHelper`** for all controller responses. |
| 8 | Use **`RouteConstants`** for all route strings — no magic strings in attributes. |
| 9 | Register services as **`Scoped`**; caches as **`Singleton`**. |
| 10 | Log **intent** + **outcome** in every service method using structured Serilog logging. |
| 11 | Add new entity repositories to `IRepositoryManager` / `RepositoryManager`. |
| 12 | Add new services to `IServiceManager` / `ServiceManager`. |
| 13 | Keep `CancellationToken ct = default` as the **last** parameter in async signatures. |
| 14 | Mark service classes as `internal sealed` — they are internal implementation details. |

---

## 10. What NOT To Do ❌

| # | Rule |
|---|---|
| 1 | **Never** put business logic, validation, or DB calls in a controller. |
| 2 | **Never** add a reference from `Domain.Entities` or `Domain.Contracts` to any Infrastructure project. |
| 3 | **Never** inject or use `CrmContext` directly in a service — go through `IRepositoryManager`. |
| 4 | **Never** call `SaveChangesAsync` inside a repository method — only via `IRepositoryManager.SaveAsync`. |
| 5 | **Never** return a domain **entity** from a service or controller — always map to a DTO first. |
| 6 | **Never** use `AutoMapper` or `MyMapper.JsonClone` — use Mapster `MapTo<T>()`. |
| 7 | **Never** use synchronous EF calls (`ToList()`, `FirstOrDefault()`) in web request paths. |
| 8 | **Never** catch exceptions in a controller just to re-wrap them — let middleware handle it. |
| 9 | **Never** return raw `Ok(result)` or anonymous objects — always use `ApiResponseHelper`. |
| 10 | **Never** hand-edit `CrmContext.cs` — use partial classes for customization. |
| 11 | **Never** add `Add-Migration` / code-first migrations — DB-First only. |
| 12 | **Never** use string concatenation in raw SQL — always use parameterized/interpolated queries. |
| 13 | **Never** use `$.ajax()`, `$.get()`, or `$.post()` in frontend code — use `fetch()` / `ApiClient`. |

---

## 11. Quick Reference Checklist

Use this before creating a PR for any new CRUD feature:

```
Feature Checklist
─────────────────────────────────────────────────────────────
Domain Layer
  [ ] Entity exists in Domain.Entities (DB-First scaffolded)
  [ ] Repository interface in Domain.Contracts/Repositories/
  [ ] Service interface in Domain.Contracts/Services/

Shared Kernel (bdDevs.Shared)
  [ ] CreateXxxRecord / UpdateXxxRecord / DeleteXxxRecord in Records/
  [ ] XxxDto in DataTransferObjects/
  [ ] Route constants added to RouteConstants.cs

Infrastructure
  [ ] Repository class implements interface + inherits RepositoryBase<T>
  [ ] Lazy<IXxxRepository> added to RepositoryManager
  [ ] IXxxRepository property exposed on IRepositoryManager

Application Services
  [ ] Service class is internal sealed, uses _repository + _logger
  [ ] Guard clauses at the top of every method
  [ ] MapTo<T>() / MapToList<T>() for all mapping
  [ ] Domain exceptions thrown (not raw Exception / ArgumentException)
  [ ] SaveAsync called after every write operation
  [ ] CancellationToken passed through
  [ ] Lazy<IXxxService> added to ServiceManager
  [ ] IXxxService property exposed on IServiceManager

Presentation
  [ ] Controller inherits BaseApiController
  [ ] [AuthorizeUser] applied at class level
  [ ] [ServiceFilter(typeof(EmptyObjectFilterAttribute))] on POST/PUT
  [ ] Route uses RouteConstants constants (no magic strings)
  [ ] Controller only calls service + returns ApiResponseHelper result
  [ ] ID mismatch check on PUT: key != record.XxxId → IdMismatchBadRequestException

Code Quality
  [ ] All async methods have Async suffix
  [ ] trackChanges: false on all read paths
  [ ] Structured logging in service (LogInformation start + outcome)
  [ ] No sync DB calls in web path
  [ ] No raw exceptions leaking to controller
─────────────────────────────────────────────────────────────
```

---

## 12. Logging & CORS Configuration

### Serilog Configuration

We use **Serilog** for structured logging with multi-sink output (Console + File) and enrichment for better observability.

#### Installed Package

```
Serilog.Sinks.File v7.0.0
```

#### Configuration (`appsettings.json`)

```json
{
  "Serilog": {
    "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.File" ],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.AspNetCore": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "outputTemplate": "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"
        }
      },
      {
        "Name": "File",
        "Args": {
          "path": "Logs/bdDevsCrm-.log",
          "rollingInterval": "Day",
          "retainedFileCountLimit": 30,
          "outputTemplate": "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] [{CorrelationId}] {Message:lj} {Properties:j}{NewLine}{Exception}"
        }
      }
    ],
    "Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId" ]
  }
}
```

#### Log File Details

| Property | Value |
|---|---|
| **Location** | `Presentation.Api/Logs/` |
| **File Pattern** | `bdDevsCrm-{Date}.log` |
| **Example** | `bdDevsCrm-20260420.log` |
| **Rolling Interval** | Daily |
| **Retention** | 30 days |
| **Enrichment** | CorrelationId, MachineName, ThreadId |

#### Log Levels

| Environment | Level |
|---|---|
| **Production** | Information |
| **Development** | Debug |

#### Enriched Log Format Example

```
[2026-04-20 15:30:45.123 +06:00] [INF] [abc-123-def] User login successful {"UserId": 5, "LoginId": "admin"}
```

- **Timestamp**: Full datetime with milliseconds and timezone
- **Level**: INF (Information), WRN (Warning), ERR (Error), DBG (Debug)
- **CorrelationId**: Unique request tracking ID (set by `CorrelationIdMiddleware`)
- **Structured Properties**: JSON object format for searchability

### CORS Configuration

Cross-Origin Resource Sharing (CORS) is configured to allow requests from the **Presentation.Mvc** frontend application.

#### Configuration (`appsettings.json`)

```json
{
  "Cors": {
    "AllowedOrigins": [
      "http://localhost:5086",
      "https://localhost:7274"
    ],
    "AllowCredentials": true,
    "PreflightMaxAge": 3600
  }
}
```

#### CORS Policy Details

| Property | Value | Purpose |
|---|---|---|
| **AllowedOrigins** | `http://localhost:5086`, `https://localhost:7274` | MVC frontend URLs (HTTP + HTTPS) |
| **AllowCredentials** | `true` | Allows cookies, authorization headers, and TLS client certificates |
| **PreflightMaxAge** | `3600` seconds (1 hour) | Browser caches preflight OPTIONS request for 1 hour |

#### CORS Registration (in `Program.cs`)

```csharp
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()!)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .SetPreflightMaxAge(TimeSpan.FromSeconds(builder.Configuration.GetValue<int>("Cors:PreflightMaxAge")));
    });
});

// In middleware pipeline
app.UseCors();  // Must be before UseAuthorization()
```

#### Security Notes

> ⚠️ **Production Update Required:**
> In production, replace `localhost` URLs with actual domain names (e.g., `https://app.bddevscrm.com`).
>
> ⚠️ **AllowCredentials:**
> When `AllowCredentials: true`, you **must** specify exact origins. Using `AllowAnyOrigin()` will cause a CORS policy error.

### Build Status

✅ **Last Build:** Successful (0 errors, 0 warnings — 2026-04-27)

---

## CRM v1 Module Status (2026-04-27)

### Phase Completion

| Phase | Description | Status |
|-------|-------------|--------|
| Phase 0 | Build fix (prerequisite) | ✅ Complete |
| Phase 1 | 14 new CRM v1 module backends | ✅ Complete |
| Phase 2 | 14 new CRM v1 module frontends | ✅ Complete |
| Phase 3 | Existing 20 CRM UI verification | ✅ Complete |
| Phase 4 | CRUD Records migration | ✅ Complete |
| Phase 5 | Security hardening | ✅ Complete |
| Phase 6 | Integration testing + docs | ✅ Complete |

### Phase 1 — 14 New CRM Backend Modules

All 14 modules have the full stack: Entity → Repository → Service → Controller → Records → DTO → Validator.

| Module | Controller | Service | Records |
|--------|-----------|---------|---------|
| CrmAgentType | `CrmAgentTypeController` | `CrmAgentTypeService` | `CreateCrmAgentTypeRecord` |
| CrmLeadSource | `CrmLeadSourceController` | `CrmLeadSourceService` | `CreateCrmLeadSourceRecord` |
| CrmLeadStatus | `CrmLeadStatusController` | `CrmLeadStatusService` | `CreateCrmLeadStatusRecord` |
| CrmStudentStatus | `CrmStudentStatusController` | `CrmStudentStatusService` | `CreateCrmStudentStatusRecord` |
| CrmOffice | `CrmOfficeController` | `CrmOfficeService` | `CreateCrmOfficeRecord` |
| CrmVisaType | `CrmVisaTypeController` | `CrmVisaTypeService` | `CreateCrmVisaTypeRecord` |
| CrmAgent | `CrmAgentController` | `CrmAgentService` | `CreateCrmAgentRecord` |
| CrmCounselor | `CrmCounselorController` | `CrmCounselorService` | `CreateCrmCounselorRecord` |
| CrmLead | `CrmLeadController` | `CrmLeadService` | `CreateCrmLeadRecord` |
| CrmStudent | `CrmStudentController` | `CrmStudentService` | `CreateCrmStudentRecord` |
| CrmEnquiry | `CrmEnquiryController` | `CrmEnquiryService` | `CreateCrmEnquiryRecord` |
| CrmFollowUp | `CrmFollowUpController` | `CrmFollowUpService` | `CreateCrmFollowUpRecord` |
| CrmNote | `CrmNoteController` | `CrmNoteService` | `CreateCrmNoteRecord` |
| CrmTask | `CrmTaskController` | `CrmTaskService` | `CreateCrmTaskRecord` |

### Phase 2 — 14 New CRM Frontend Modules

Each module has:
- `Presentation.Mvc/Controllers/CRM/{Name}Controller.cs`
- `Presentation.Mvc/Views/CRM/{Name}/Index.cshtml`
- `wwwroot/js/modules/crm/{name}/{name}Settings.js`
- `wwwroot/js/modules/crm/{name}/{name}Summary.js`
- `wwwroot/js/modules/crm/{name}/{name}Details.js`

All modules are wired into the CRM sidebar navigation group in `_Sidebar.cshtml`.

### Phase 5 — Security Hardening

Added `Presentation.Api/Extensions/SecurityExtensions.cs` with:
- **Rate Limiting:** 120 requests/minute per IP (global), 20 requests/minute per IP (AuthPolicy for auth endpoints)
- **Security Headers:** `X-Content-Type-Options: nosniff`, `X-Frame-Options: DENY`, `Referrer-Policy: strict-origin-when-cross-origin`, `X-XSS-Protection: 0`, `Permissions-Policy`, `Cache-Control: no-store` for API responses

### Phase 6 — Testing

Unit tests are in `bdDevsCrm.UnitTests/CRM/` using **xUnit + Moq**:
- `CrmAgentTypeServiceTests.cs` — 10 tests (CRUD + read operations)
- `CrmLeadSourceServiceTests.cs` — 8 tests (CRUD + read operations)

Run tests: `dotnet test bdDevsCrm.UnitTests/bdDevsCrm.UnitTests.csproj`

---

## Related Documentation

| Document | Path | Contents |
|---|---|---|
| Developer Guide | `DEVELOPER_GUIDE.md` | Full onboarding, setup, testing |
| Backend Design | `doc/backend_design.md` | Layer design, patterns, multi-tier cache |
| Coding Architecture | `doc/coding_architecture.md` | SOLID principles, layer diagrams |
| Naming Conventions | `doc/naming_conventions.md` | Full naming rules with examples |
| Frontend Design | `doc/frontend_design.md` | UI/UX, Kendo, JS patterns |
| UI Design Documentation | `doc/HRIS_UIDesign_Documentation.md` | Complete UI/UX design system + frontend implementation plan |

---

*Last updated: 2026-04-20 | Maintainer: bdDevs team*
