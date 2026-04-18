# CRUD Records Pattern Implementation Progress

**Project**: bdDevsCrm
**Started**: 2026-04-15
**Status**: 🚧 In Progress
**Current Focus**: Service/Controller migration & build fixes
**Latest Update**: 2026-04-18 - Fixed ApproverDetailsService and ICrmApplicantCourseService interface mismatches

---

## 📋 Overview

This document tracks the implementation of the CRUD Records pattern across all entities in the bdDevsCrm project. The goal is to replace DTOs with C# record types for all HTTP requests and implement proper validation using FluentValidation.

### Key Requirements
1. ✅ Use **Mapster** for mapping (NOT AutoMapper) - installed
2. ✅ Use **FluentValidation** for validation - installed
3. 🚧 Use **Create/Update/Delete XxxRecord** for every entity - partially done across modules
4. 🚧 Controllers use Records instead of DTOs - in progress
5. 🚧 Services use Mapster instead of MyMapper.JsonClone - in progress
6. ✅ Mapster available in bdDevs.Shared for shared mapping utilities

### Current Snapshot (2026-04-18)
- Records created: **90** total (SystemAdmin 53, HR 2, CRM 26, DMS 9)
- Validators created: **88** total (SystemAdmin 53, CRM 26, DMS 9) + BaseRecordValidator
- Services using Records: **54 / 80** (67.5%)
- Controllers using Records: **54 / 78** (69.2%)
- Build status: **FAILING** (115 errors, down from 162). Recent fixes: ApproverDetailsService and ICrmApplicantCourseService interface mismatches resolved. Primary blockers: missing validator type resolutions in several SystemAdmin services, missing repository method `ActiveDocumentParameterMappingsAsync`, missing entity members (`LastUpdatedDate`) referenced in services, and grid extension calls (e.g., `GridDataSource`) not present. Needs remediation before further migrations.

---

## 🎯 Implementation Strategy

### Phase 1: Infrastructure Setup ✅ Completed
- [x] Mapster 10.0.7 added to bdDevs.Shared & Application.Services
- [x] FluentValidation 11.12.0 (+ DI extensions) added
- [x] BaseRecordValidator<T>, MapsterConfig, MapsterExtensions created

### Phase 2: System Module - 🚧 In Progress
- [x] CRUD Records for SystemAdmin entities (53 files)
- [x] Validators for SystemAdmin Records (53 files)
- [ ] Services fully migrated to Records/Mapster (26 SystemAdmin services still using DTO/MyMapper; see priority list)
- [ ] Controllers fully migrated (12 SystemAdmin controllers still DTO-based)
- [ ] Build/test verification (current solution build failing)

### Phase 3: CRM Module - 🚧 In Progress
- [x] CRUD Records (26 files)
- [x] Validators (26 files)
- [ ] Services & controllers: majority migrated; remaining MyMapper usage to replace and endpoints to verify
- [ ] Build/test verification (blocked by global build failures)

### Phase 4: DMS Module - 🚧 In Progress
- [x] CRUD Records (9 files)
- [x] Validators (9 files)
- [ ] Services/controllers: DMS document services/controllers still DTO/MyMapper heavy
- [ ] Build/test verification (blocked)

### Phase 5: HR Module - 🚧 Not Started (beyond records)
- [x] Records for Branch, Department (2 files)
- [ ] Validators
- [ ] Service/controller migrations

### Phase 6: Final Testing & Documentation
- [ ] Build verification (0 errors)
- [ ] Integration testing
- [ ] Update API documentation
- [ ] Performance testing

---

## 📊 Coverage Summary

### Records & Validators
- **SystemAdmin Records**: 53 files present
- **HR Records**: 2 files present (Branch, Department)
- **CRM Records**: 26 files present
- **DMS Records**: 9 files present
- **Validators**: 88 files (53 SystemAdmin, 26 CRM, 9 DMS) + BaseRecordValidator

### Services & Controllers
- **Services using Records/Mapster**: 54 of 80 (67.5%)
- **Services still DTO/MyMapper** (26): ModuleService, AccessControlService, MenuService, CurrencyService, PropertyExtractionService, GroupService, SystemSettingsService, QueryAnalyzerService, StatusService, UsersService, CompanyService, BranchService, EmployeeService, DepartmentService, HttpContextService, CookieManagementService, CacheManagementService, AuthenticationService, HybridCacheService, DmsDocumentVersionService, DmsDocumentTagService, DmsDocumentAccessLogService, DmsDocumentTypeService, DmsDocumentFolderService, DmsDocumentService, DmsDocumentTagMapService.
- **Controllers using Records**: 54 of 78 (69.2%)
- **Controllers still DTO-based** (24): QueryAnalyzer, Users, Group, SystemSettings, AccessControl, WorkFlow, Companies, Currency, Common, Menu, Module, Branch, Employee, Department, BaseApi, Buggy, Home, Authentication, DmsDocumentTag, DmsDocumentType, DmsDocumentAccessLog, DmsDocument, DmsDocumentFolder, Test.

---

## 📝 Progress by Task Type

### Progress by Task Type
- **Records generation**: 90 files present across SystemAdmin/HR/CRM/DMS
- **Validators**: 88 files present across SystemAdmin/CRM/DMS
- **Service layer migration**: 54/80 services use Records + Mapster; 26 services remain on DTO/MyMapper
- **Controller migration**: 54/78 controllers accept Records; 24 remain on DTOs
- **Build**: currently failing (see Known Issues)

---

## 🔧 Technical Details

### File Structure
```
bdDevs.Shared/
  Records/
    Core/
      SystemAdmin/  <- System module records here
        CompanyRecords.cs
        BranchRecords.cs
        DepartmentRecords.cs
        ... (51 more to create)
      HR/
      CRM/
      DMS/
```

### Record Pattern Template
```csharp
namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new {Entity}.
/// </summary>
public record Create{Entity}Record(
    // Required and optional properties
);

/// <summary>
/// Record for updating an existing {Entity}.
/// </summary>
public record Update{Entity}Record(
    int {Entity}Id,
    // Other properties
);

/// <summary>
/// Record for deleting a {Entity}.
/// </summary>
public record Delete{Entity}Record(int {Entity}Id);
```

### Validator Pattern Template
```csharp
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

public class Create{Entity}RecordValidator : AbstractValidator<Create{Entity}Record>
{
    public Create{Entity}RecordValidator()
    {
        RuleFor(x => x.PropertyName)
            .NotEmpty().WithMessage("PropertyName is required")
            .MaximumLength(100).WithMessage("PropertyName must not exceed 100 characters");
    }
}
```

---

## 📈 Completion Metrics

### Overall Progress: 14.6% (39/267 records created)

| Module | Entities | Records Created | Validators | Services Updated | Controllers Updated |
|--------|----------|-----------------|------------|------------------|---------------------|
| System | 54 | 39/162 (24.1%) | 0/54 (0%) | 0/15 (0%) | 0/15 (0%) |
| CRM | ~30 | 0/90 (0%) | 0/30 (0%) | 1/10 (10%) | 0/10 (0%) |
| DMS | ~8 | 0/24 (0%) | 0/8 (0%) | 0/3 (0%) | 0/3 (0%) |
| **Total** | **~92** | **39/276** | **0/92** | **1/28** | **0/28** |

---

## 🚀 Priority Task List
1. **Unblock the build**: fix missing validator type resolutions, add/adjust repository methods (e.g., `ActiveDocumentParameterMappingsAsync`), align entity models with service expectations (`LastUpdatedDate`, nullable checks, `GridDataSource` helpers).
2. **Migrate remaining SystemAdmin services/controllers** to Records + Mapster (Module, AccessControl, Menu, Currency, Group, Users, SystemSettings, Company, Status/Workflow, QueryAnalyzer).
3. **Migrate remaining DMS services/controllers** (Document, DocumentFolder, DocumentType, DocumentTag, DocumentAccessLog, DocumentTagMap, DocumentVersion) off DTO/MyMapper to Records + Mapster.
4. **Migrate HR services/controllers** (Branch, Department, Employee) to CRUD Records with validators.
5. **Finish Mapster adoption** by removing MyMapper usage in CRM/DMS legacy services and ensuring validators are wired in DI.
6. **Re-run full build/tests** once above are addressed; then proceed to integration testing and documentation refresh.

---

## 📚 References

- **Naming Conventions**: `/doc/naming_conventions.md`
- **Backend Design**: `/doc/backend_design.md`
- **Project Vision**: `/doc/PROJECT_VISION.md`
- **Existing Records Example**: `/bdDevs.Shared/Records/Core/SystemAdmin/CountryRecords.cs`
- **Existing Service Example**: `/Application.Services/Core/SystemAdmin/CountryService.cs`

---

## 🐛 Known Issues

1. **MyMapper vs Mapster**: Current services use `MyMapper.JsonClone` (Newtonsoft.Json based). Need to migrate to Mapster for better performance.
2. **DTOs in Controllers**: Controllers currently accept DTOs instead of Records. Need systematic replacement.
3. **No Validation**: No FluentValidation currently implemented. Need to add for all Records.

---

## ✅ Quality Checklist

Before marking an entity as complete, verify:
- [ ] CreateXxxRecord created with all required properties
- [ ] UpdateXxxRecord created with ID + all properties
- [ ] DeleteXxxRecord created with ID only
- [ ] Validator class created with appropriate rules
- [ ] Service interface updated to use Records
- [ ] Service implementation uses Mapster (not MyMapper)
- [ ] Controller updated to accept Records
- [ ] Build succeeds with 0 errors
- [ ] Manual API testing completed
- [ ] HATEOAS links work correctly

---

## 🐛 Known Issues (blocking build)
1. Missing validator types referenced in services (e.g., Create/Update/Delete validators for AppsTransactionLog, AssignApprover, ApproverOrder, ApproverType, ApproverHistory, AuditLog/AuditTrail, AppsTokenInfo) despite validator files existing—verify namespaces, DI registration, and using statements.
2. Repository/API gaps: `IDocumentParameterMappingRepository.ActiveDocumentParameterMappingsAsync` referenced but not implemented.
3. Entity shape mismatches: several services expect `LastUpdatedDate` on entities (AssignApprover, ApproverOrder, ApproverType, ApproverHistory, ApproverTypeToGroupMapping) and nullable `int.HasValue`/`Value` usages in `AuditTypeService`; grid helper `GridDataSource` extension referenced in BoardInstitute/AuditType services missing.
4. Build result (2026-04-17): `dotnet build` => **112 errors**, **366 warnings**.

---

**Last Updated**: 2026-04-17 06:53 UTC  
**Updated By**: Codex Agent  
**Next Review**: After build blockers are resolved and remaining services/controllers are migrated  
**Current Progress**: Records 90 | Validators 88 | Services migrated 54/80 | Controllers migrated 54/78
