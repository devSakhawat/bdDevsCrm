---
name: bddevs-crm-backend-crud
description: >
  Use this skill when generating any backend CRUD code for the bdDevsCrm project.
  Covers Controller, Service Interface, Service Implementation, and Repository layers
  following the exact patterns established in ModuleController, ModuleService,
  ModuleRepository, and RepositoryBase<T>.
  Triggers: "create CRUD for [Entity]", "generate backend for [Entity]",
  "add new entity [Entity]", "implement service for [Entity]".
project: bdDevsCrm
framework: .NET 10.0 + ASP.NET Core + EF Core 8+ + ADO.NET
---

# bdDevsCrm Backend CRUD Skill

## 1. Project Architecture Overview

```
bdDevsCrm/
├── Domain.Entities/          → Entity classes (POCO)
├── Domain.Contracts/         → Service interfaces (IXxxService)
├── Domain.Exceptions/        → Custom exception types (root namespace)
├── Application.Services/     → Service implementations (internal sealed)
│   └── Core/SystemAdmin/     → SystemAdmin module services
├── Infrastructure.Repositories/ → Repository implementations
├── Infrastructure.Sql/       → EF Core DbContext (CrmContext)
├── Presentation.Controller/  → Controllers (BaseApiController)
├── bdDevs.Shared/
│   ├── Records/Core/         → C# Records (Create/Update/Delete)
│   ├── Constants/            → RouteConstants.cs
│   ├── DataTransferObjects/  → DTOs
│   └── Grid/                 → GridEntity, GridOptions
```

---

## 2. Mandatory File Generation Order

For every new Entity, generate **exactly 4 files** in this order:

1. `RouteConstants` region (add to existing file)
2. `IXxxService.cs` — Service Interface
3. `XxxService.cs` — Service Implementation
4. `XxxRepository.cs` — Repository Implementation
5. `XxxController.cs` — Controller

---

## 3. RouteConstants Pattern

**File:** `bdDevs.Shared/Constants/RouteConstants.cs`

```csharp
#region EntityName
// CUD Operations
public const string CreateEntityName = "entity-name";
public const string UpdateEntityName = "entity-name/{key}";
public const string DeleteEntityName = "entity-name/{key}";

// Read Operations (High to Low Data Volume)
public const string EntityNameSummary = "entity-name-summary";    // Grid/Paginated (Largest)
public const string ReadEntityNames   = "entity-names";           // List All (Medium)
public const string EntityNameDDL     = "entity-names-ddl";       // Dropdown (Small)
public const string ReadEntityName    = "entity-name/{id:int}";   // Single by ID (Smallest)

// Specialized (add if entity has parent FK)
public const string EntityNamesByParentId = "entity-names-by-parent/{parentId:int}";
#endregion EntityName
```

**Rules:**
- Route strings: all lowercase, kebab-case (`entity-name`)
- `{key}` for CUD (no type constraint — accepts int or string)
- `{id:int}` for single read (type-constrained)
- `{parentId:int}` for filtered reads by FK
- DDL route ends with `-ddl`
- Summary route ends with `-summary`

---

## 4. Controller Pattern

**File:** `Presentation.Controller/Controllers/Core/SystemAdmin/EntityNameController.cs`

```csharp
using Presentation.AuthorizeAttributes;
using Domain.Contracts.Services;
using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Domain.Exceptions;
using bdDevs.Shared.Constants;
using Application.Shared.Grid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Presentation.ActionFilters;

namespace Presentation.Controllers.Core.SystemAdmin;

/// <summary>
/// EntityName management endpoints.
/// [AuthorizeUser] ensures all requests are authenticated.
/// Exceptions handled by StandardExceptionMiddleware.
/// </summary>
[AuthorizeUser]
public class EntityNameController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public EntityNameController(
        IServiceManager serviceManager,
        IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    #region CUD

    /// <summary>Creates a new EntityName.</summary>
    [HttpPost(RouteConstants.CreateEntityName)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateEntityNameAsync(
        [FromBody] EntityNameDto modelDto,
        CancellationToken cancellationToken = default)
    {
        var created = await _serviceManager.EntityNames
            .CreateEntityNameAsync(modelDto, cancellationToken);

        if (created.EntityNameId <= 0)
            throw new InvalidCreateOperationException("Failed to create EntityName record.");

        return Ok(ApiResponseHelper.Created(created, "EntityName created successfully."));
    }

    /// <summary>Updates an existing EntityName.</summary>
    [HttpPut(RouteConstants.UpdateEntityName)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateEntityNameAsync(
        [FromRoute] int key,
        [FromBody] EntityNameDto modelDto,
        CancellationToken cancellationToken = default)
    {
        if (key != modelDto.EntityNameId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(EntityNameDto));

        var updated = await _serviceManager.EntityNames
            .UpdateEntityNameAsync(key, modelDto, trackChanges: true, cancellationToken);

        return Ok(ApiResponseHelper.Updated(updated, "EntityName updated successfully."));
    }

    /// <summary>Deletes an EntityName by ID.</summary>
    [HttpDelete(RouteConstants.DeleteEntityName)]
    public async Task<IActionResult> DeleteEntityNameAsync(
        [FromRoute] int key,
        CancellationToken cancellationToken = default)
    {
        await _serviceManager.EntityNames
            .DeleteEntityNameAsync(key, trackChanges: true, cancellationToken);

        return Ok(ApiResponseHelper.NoContent<object>("EntityName deleted successfully."));
    }

    #endregion CUD

    #region Read

    /// <summary>Retrieves paginated summary grid of EntityNames.</summary>
    [HttpPost(RouteConstants.EntityNameSummary)]
    public async Task<IActionResult> EntityNameSummaryAsync(
        [FromBody] GridOptions options,
        CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var result = await _serviceManager.EntityNames
            .EntityNameSummaryAsync(trackChanges: false, options, cancellationToken);

        if (!result.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<EntityNameDto>(), "No records found."));

        return Ok(ApiResponseHelper.Success(result, "EntityNames retrieved successfully."));
    }

    /// <summary>Retrieves all EntityNames.</summary>
    [HttpGet(RouteConstants.ReadEntityNames)]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> EntityNamesAsync(CancellationToken cancellationToken = default)
    {
        var result = await _serviceManager.EntityNames
            .EntityNamesAsync(trackChanges: false, cancellationToken);

        if (!result.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<EntityNameDto>(), "No records found."));

        return Ok(ApiResponseHelper.Success(result, "EntityNames retrieved successfully."));
    }

    /// <summary>Retrieves a single EntityName by ID.</summary>
    [HttpGet(RouteConstants.ReadEntityName)]
    public async Task<IActionResult> EntityNameAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var result = await _serviceManager.EntityNames
            .EntityNameAsync(id, trackChanges: false, cancellationToken);

        return Ok(ApiResponseHelper.Success(result, "EntityName retrieved successfully."));
    }

    /// <summary>Retrieves EntityNames for dropdown list.</summary>
    [HttpGet(RouteConstants.EntityNameDDL)]
    public async Task<IActionResult> EntityNamesForDDLAsync(CancellationToken cancellationToken = default)
    {
        var result = await _serviceManager.EntityNames
            .EntityNameForDDLAsync(cancellationToken);

        if (!result.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<EntityNameForDDLDto>(), "No records found."));

        return Ok(ApiResponseHelper.Success(result, "EntityNames retrieved successfully."));
    }

    #endregion Read
}
```

**Controller Rules:**
- Class-level `[AuthorizeUser]` — NEVER method-level
- Constructor: `IServiceManager` + `IMemoryCache` always injected
- CUD methods: `[ServiceFilter(typeof(EmptyObjectFilterAttribute))]` on Create/Update
- Summary endpoint: `[HttpPost]` (receives `GridOptions` in body)
- List/Single endpoints: `[HttpGet]`
- `[ResponseCache(Duration = 60)]` on list-all endpoint
- Return `ApiResponseHelper.Created()` for POST, `ApiResponseHelper.Updated()` for PUT, `ApiResponseHelper.NoContent<object>()` for DELETE
- NEVER put business logic in controller — delegate to service

---

## 5. Service Interface Pattern

**File:** `Domain.Contracts/Services/Core/SystemAdmin/IEntityNameService.cs`

```csharp
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

/// <summary>
/// Service contract for EntityName management operations.
/// </summary>
public interface IEntityNameService
{
    /// <summary>
    /// Creates a new EntityName record after validating for null input and duplicate name.
    /// </summary>
    /// <param name="entityForCreate">DTO containing data for the new EntityName.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>The created <see cref="EntityNameDto"/> with newly assigned ID.</returns>
    /// <exception cref="BadRequestException">Thrown when input is null.</exception>
    /// <exception cref="DuplicateRecordException">Thrown when a duplicate record exists.</exception>
    Task<EntityNameDto> CreateEntityNameAsync(
        EntityNameDto entityForCreate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing EntityName record.
    /// </summary>
    /// <param name="entityNameId">The ID of the EntityName to update.</param>
    /// <param name="modelDto">DTO containing updated values.</param>
    /// <param name="trackChanges">Whether EF change tracking is enabled.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>The updated <see cref="EntityNameDto"/>.</returns>
    /// <exception cref="BadRequestException">Thrown when input is null or IDs mismatch.</exception>
    /// <exception cref="NotFoundException">Thrown when record is not found.</exception>
    /// <exception cref="DuplicateRecordException">Thrown when duplicate name exists.</exception>
    Task<EntityNameDto> UpdateEntityNameAsync(
        int entityNameId,
        EntityNameDto modelDto,
        bool trackChanges,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an EntityName record by ID.
    /// </summary>
    /// <param name="entityNameId">The ID of the EntityName to delete.</param>
    /// <param name="trackChanges">Whether EF change tracking is enabled.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>Number of affected rows.</returns>
    /// <exception cref="IdParametersBadRequestException">Thrown when ID is zero or negative.</exception>
    /// <exception cref="NotFoundException">Thrown when record is not found.</exception>
    Task<int> DeleteEntityNameAsync(
        int entityNameId,
        bool trackChanges,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single EntityName record by ID.
    /// </summary>
    /// <param name="id">The ID to retrieve.</param>
    /// <param name="trackChanges">Whether EF change tracking is enabled.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>The <see cref="EntityNameDto"/> matching the ID.</returns>
    /// <exception cref="NotFoundException">Thrown when record is not found.</exception>
    Task<EntityNameDto> EntityNameAsync(
        int id,
        bool trackChanges,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all EntityName records.
    /// </summary>
    /// <param name="trackChanges">Whether EF change tracking is enabled.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>Collection of all <see cref="EntityNameDto"/> records.</returns>
    Task<IEnumerable<EntityNameDto>> EntityNamesAsync(
        bool trackChanges,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated summary grid of EntityNames.
    /// </summary>
    /// <param name="trackChanges">Whether EF change tracking is enabled.</param>
    /// <param name="options">Grid options including pagination, filtering, sorting.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="GridEntity{EntityNameDto}"/> with paged data.</returns>
    Task<GridEntity<EntityNameDto>> EntityNameSummaryAsync(
        bool trackChanges,
        GridOptions options,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a lightweight list for dropdown binding.
    /// Returns only ID and Name fields, ordered by Name.
    /// </summary>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>Collection of <see cref="EntityNameForDDLDto"/> for dropdown.</returns>
    Task<IEnumerable<EntityNameForDDLDto>> EntityNameForDDLAsync(
        CancellationToken cancellationToken = default);
}
```

---

## 6. Service Implementation Pattern

**File:** `Application.Services/Core/SystemAdmin/EntityNameService.cs`

```csharp
using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;
using Domain.Contracts.Services.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Domain.Exceptions;
using Application.Shared.Grid;
using Application.Services.Mappings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.Core.SystemAdmin;

/// <summary>
/// EntityName service implementing business logic for EntityName management.
/// </summary>
internal sealed class EntityNameService : IEntityNameService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<EntityNameService> _logger;
    private readonly IConfiguration _configuration;

    public EntityNameService(
        IRepositoryManager repository,
        ILogger<EntityNameService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>Creates a new EntityName record.</summary>
    public async Task<EntityNameDto> CreateEntityNameAsync(
        EntityNameDto entityForCreate,
        CancellationToken cancellationToken = default)
    {
        if (entityForCreate is null)
            throw new BadRequestException(nameof(EntityNameDto));

        // Duplicate check — adjust field name as needed
        bool exists = await _repository.EntityNames.ExistsAsync(
            x => x.Name.Trim().ToLower() == entityForCreate.Name.Trim().ToLower(),
            cancellationToken: cancellationToken);

        if (exists)
            throw new DuplicateRecordException("EntityName", "Name");

        EntityName entity = MyMapper.JsonClone<EntityNameDto, EntityName>(entityForCreate);

        await _repository.EntityNames.CreateAsync(entity, cancellationToken);
        int affected = await _repository.SaveChangesAsync(cancellationToken);

        if (affected <= 0)
            throw new InvalidOperationException("EntityName could not be saved to the database.");

        _logger.LogInformation(
            "EntityName created. ID: {Id}, Name: {Name}, Time: {Time}",
            entity.EntityNameId, entity.Name, DateTime.UtcNow);

        return MyMapper.JsonClone<EntityName, EntityNameDto>(entity);
    }

    /// <summary>Updates an existing EntityName record.</summary>
    public async Task<EntityNameDto> UpdateEntityNameAsync(
        int entityNameId,
        EntityNameDto modelDto,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        if (modelDto is null)
            throw new BadRequestException(nameof(EntityNameDto));

        if (entityNameId != modelDto.EntityNameId)
            throw new BadRequestException(entityNameId.ToString(), nameof(EntityNameDto));

        EntityName existing = await _repository.EntityNames
            .FirstOrDefaultAsync(x => x.EntityNameId == entityNameId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("EntityName", "EntityNameId", entityNameId.ToString());

        bool duplicateExists = await _repository.EntityNames.ExistsAsync(
            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower()
                 && x.EntityNameId != entityNameId,
            cancellationToken: cancellationToken);

        if (duplicateExists)
            throw new DuplicateRecordException("EntityName", "Name");

        EntityName updated = MyMapper.MergeChangedValues<EntityName, EntityNameDto>(existing, modelDto);
        _repository.EntityNames.UpdateByState(updated);

        int affected = await _repository.SaveChangesAsync(cancellationToken);
        if (affected <= 0)
            throw new NotFoundException("EntityName", "EntityNameId", entityNameId.ToString());

        _logger.LogInformation(
            "EntityName updated. ID: {Id}, Name: {Name}, Time: {Time}",
            updated.EntityNameId, updated.Name, DateTime.UtcNow);

        return MyMapper.JsonClone<EntityName, EntityNameDto>(updated);
    }

    /// <summary>Deletes an EntityName record.</summary>
    public async Task<int> DeleteEntityNameAsync(
        int entityNameId,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        if (entityNameId <= 0)
            throw new BadRequestException(entityNameId.ToString(), nameof(EntityNameDto));

        EntityName entity = await _repository.EntityNames
            .FirstOrDefaultAsync(x => x.EntityNameId == entityNameId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("EntityName", "EntityNameId", entityNameId.ToString());

        await _repository.EntityNames.DeleteAsync(
            x => x.EntityNameId == entityNameId, trackChanges, cancellationToken);

        int affected = await _repository.SaveChangesAsync(cancellationToken);

        if (affected <= 0)
            throw new NotFoundException("EntityName", "EntityNameId", entityNameId.ToString());

        _logger.LogWarning(
            "EntityName deleted. ID: {Id}, Name: {Name}, Time: {Time}",
            entity.EntityNameId, entity.Name, DateTime.UtcNow);

        return affected;
    }

    /// <summary>Retrieves a single EntityName by ID.</summary>
    public async Task<EntityNameDto> EntityNameAsync(
        int id,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        EntityName entity = await _repository.EntityNames
            .EntityNameAsync(id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("EntityName", "EntityNameId", id.ToString());

        _logger.LogInformation(
            "EntityName fetched. ID: {Id}, Time: {Time}",
            entity.EntityNameId, DateTime.UtcNow);

        return MyMapper.JsonClone<EntityName, EntityNameDto>(entity);
    }

    /// <summary>Retrieves all EntityNames.</summary>
    public async Task<IEnumerable<EntityNameDto>> EntityNamesAsync(
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        IEnumerable<EntityName> entities = await _repository.EntityNames
            .EntityNamesAsync(trackChanges, cancellationToken);

        if (!entities.Any())
        {
            _logger.LogWarning("No EntityNames found.");
            return Enumerable.Empty<EntityNameDto>();
        }

        _logger.LogInformation("EntityNames fetched successfully.");
        return MyMapper.JsonCloneIEnumerableToIEnumerable<EntityName, EntityNameDto>(entities);
    }

    /// <summary>Retrieves paginated summary grid.</summary>
    public async Task<GridEntity<EntityNameDto>> EntityNameSummaryAsync(
        bool trackChanges,
        GridOptions options,
        CancellationToken cancellationToken = default)
    {
        const string query =
            @"SELECT
                EntityNameId,
                Name,
                -- add other columns here
                IsActive
              FROM EntityName";  // adjust table name

        const string orderBy = "Name ASC";

        return await _repository.EntityNames
            .AdoGridDataAsync<EntityNameDto>(query, options, orderBy, "", cancellationToken);
    }

    /// <summary>Retrieves lightweight list for dropdown.</summary>
    public async Task<IEnumerable<EntityNameForDDLDto>> EntityNameForDDLAsync(
        CancellationToken cancellationToken = default)
    {
        IEnumerable<EntityName> entities = await _repository.EntityNames.ListWithSelectAsync(
            x => new EntityName
            {
                EntityNameId = x.EntityNameId,
                Name = x.Name
            },
            orderBy: x => x.Name,
            trackChanges: false,
            cancellationToken: cancellationToken);

        if (!entities.Any())
        {
            _logger.LogWarning("No EntityNames found for DDL.");
            return Enumerable.Empty<EntityNameForDDLDto>();
        }

        _logger.LogInformation("EntityNames fetched for DDL.");
        return MyMapper.JsonCloneIEnumerableToIEnumerable<EntityName, EntityNameForDDLDto>(entities);
    }
}
```

**Service Rules:**
- Class: `internal sealed` — NEVER `public`
- Fields: `_camelCase` prefix (private readonly)
- Null check FIRST in every CUD method
- Duplicate check: before Create and Update
- `MyMapper.JsonClone<TSource, TDest>()` for mapping
- `MyMapper.MergeChangedValues<TEntity, TDto>()` for partial update
- `_logger.LogInformation()` for successful operations
- `_logger.LogWarning()` for deletions and empty results
- Grid summary: uses `AdoGridDataAsync<TDto>()` with raw SQL
- DDL: uses `ListWithSelectAsync()` — only ID + Name fields

---

## 7. Repository Pattern

**File:** `Infrastructure.Repositories/Core/SystemAdmin/EntityNameRepository.cs`

```csharp
using Domain.Entities.Entities.System;
using Domain.Contracts.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.Core.SystemAdmin;

/// <summary>
/// Repository for EntityName data access operations.
/// </summary>
public class EntityNameRepository : RepositoryBase<EntityName>, IEntityNameRepository
{
    public EntityNameRepository(CrmContext context) : base(context) { }

    /// <summary>Retrieves all EntityNames.</summary>
    public async Task<IEnumerable<EntityName>> EntityNamesAsync(
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await ListAsync(x => x.EntityNameId, trackChanges, cancellationToken);
    }

    /// <summary>Retrieves a single EntityName by ID.</summary>
    public async Task<EntityName?> EntityNameAsync(
        int entityNameId,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(
            x => x.EntityNameId.Equals(entityNameId),
            trackChanges,
            cancellationToken);
    }

    /// <summary>Retrieves EntityNames by a collection of IDs.</summary>
    public async Task<IEnumerable<EntityName>> EntityNamesByIdsAsync(
        IEnumerable<int> ids,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await ListByIdsAsync(
            x => ids.Contains(x.EntityNameId),
            trackChanges,
            cancellationToken);
    }

    // Add specialized queries here as needed
    // Example: by parent FK
    // public async Task<IEnumerable<EntityName>> EntityNamesByParentIdAsync(
    //     int parentId, bool trackChanges, CancellationToken cancellationToken = default)
    // {
    //     string query = $"SELECT * FROM EntityName WHERE ParentId = {parentId} ORDER BY Name ASC";
    //     return await AdoExecuteListQueryAsync<EntityName>(query, null, cancellationToken);
    // }
}
```

**Repository Rules:**
- Inherits `RepositoryBase<TEntity>`
- Constructor: only `CrmContext context` parameter
- Simple queries: use EF Core base methods (`ListAsync`, `FirstOrDefaultAsync`)
- Complex/filtered queries: use ADO.NET (`AdoExecuteListQueryAsync`, `AdoExecuteSingleDataAsync`)
- Grid/paginated queries: use `AdoGridDataAsync<TDto>()` (called from Service, not Repository)

---

## 8. Exception Types Reference

| Exception | When to Use |
|-----------|-------------|
| `BadRequestException(nameof(Dto))` | DTO is null |
| `BadRequestException(id.ToString(), nameof(Dto))` | ID mismatch or invalid |
| `IdParametersBadRequestException()` | ID ≤ 0 in controller |
| `IdMismatchBadRequestException(key, nameof(Dto))` | Route key ≠ DTO ID |
| `NullModelBadRequestException(nameof(T))` | GridOptions is null |
| `NotFoundException("Entity", "Field", value)` | Record not found |
| `DuplicateRecordException("Entity", "Field")` | Duplicate name/code |
| `InvalidCreateOperationException(message)` | Created ID ≤ 0 |

---

## 9. ApiResponseHelper Reference

| Method | HTTP Status | When to Use |
|--------|-------------|-------------|
| `ApiResponseHelper.Created(data, msg)` | 201 | Successful POST/Create |
| `ApiResponseHelper.Updated(data, msg)` | 200 | Successful PUT/Update |
| `ApiResponseHelper.NoContent<object>(msg)` | 204 | Successful DELETE |
| `ApiResponseHelper.Success(data, msg)` | 200 | Successful GET |
| `ApiResponseHelper.Success(empty, msg)` | 200 | Empty list (not 404) |

---

## 10. Naming Conventions

### C# Naming
| Element | Convention | Example |
|---------|-----------|---------|
| Class | PascalCase | `ModuleService` |
| Private field | `_camelCase` | `_repository`, `_logger` |
| Method | PascalCase + Async suffix | `CreateModuleAsync` |
| Parameter | camelCase | `entityForCreate`, `trackChanges` |
| Interface | `I` prefix + PascalCase | `IModuleService` |
| DTO | PascalCase + Dto suffix | `ModuleDto`, `ModuleForDDLDto` |

### Method Naming Pattern
| Operation | Pattern | Example |
|-----------|---------|---------|
| Create | `Create[Entity]Async` | `CreateModuleAsync` |
| Update | `Update[Entity]Async` | `UpdateModuleAsync` |
| Delete | `Delete[Entity]Async` | `DeleteModuleAsync` |
| Get Single | `[Entity]Async` | `ModuleAsync` |
| Get List | `[Entities]Async` | `ModulesAsync` |
| Get Summary | `[Entity]SummaryAsync` | `ModuleSummaryAsync` |
| Get DDL | `[Entity]ForDDLAsync` | `ModuleForDDLAsync` |

---

## 11. XML Documentation Rules

Every public/internal method MUST have:
```csharp
/// <summary>One-line description.</summary>
/// <param name="paramName">Description.</param>
/// <returns>Return value description.</returns>
/// <exception cref="ExceptionType">When thrown.</exception>
```

- Language: English ONLY
- No Bangla inside code or XML comments
- Summary: single sentence, action-oriented

---

## 12. Anti-Patterns to Avoid

❌ **NEVER do these:**
- Business logic in Controller
- Synchronous I/O operations (no `.Result`, `.Wait()`)
- `new ClassName()` — use DI
- AutoMapper (use `MyMapper`)
- Return Entity from Service (always return DTO)
- `public` service class (must be `internal sealed`)
- Domain layer depending on Infrastructure
- Client-side paging
- Missing null checks in service
- Missing duplicate check on Create/Update

✅ **ALWAYS do these:**
- `async/await` for all I/O
- Constructor injection
- `trackChanges: false` for read-only queries
- `trackChanges: true` for CUD operations
- `CancellationToken cancellationToken = default` as last parameter
- Full XML documentation on all methods
