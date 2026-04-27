# CRM v1 Implementation Task List

> **Project:** bdDevsCrm — Education CRM v1  
> **Created:** 2026-04-27  
> **Purpose:** Track every implementation task so nothing is lost between sessions.  
> **Update rule:** Mark `[x]` as soon as a task is finished and committed.

---

## Legend

| Symbol | Meaning |
|--------|---------|
| `[x]` | ✅ Done |
| `[ ]` | ⬜ Pending |
| `[~]` | 🔄 In Progress |
| `[!]` | ❌ Blocked |

---

## Phase 0 — Build Fix (Prerequisite for everything)

> The solution currently fails to build. Nothing can be tested until build passes.

- [ ] Run `dotnet restore` on the full solution
- [~] Fix all compilation errors (last count: ~163 errors)
  - [ ] Missing service method implementations
  - [ ] Missing / mismatched DTO classes
  - [ ] Property mismatches in Records (nullable, shape)
  - [ ] Missing repository methods (e.g., `ActiveDocumentParameterMappingsAsync`)
  - [ ] `GridDataSource` extension call not found
  - [ ] `LastUpdatedDate` entity property missing references
- [ ] Reduce nullable warnings (418 warnings → 0 warnings target)
- [ ] `dotnet build Presentation.Api/Presentation.Api.csproj` → **0 errors**
- [ ] `dotnet build Presentation.Mvc/Presentation.Mvc.csproj` → **0 errors**

---

## Phase 1 — CRM v1 New Module: Backend (DB-First)

> These are **net-new** tables and entities required by `EDUCATION_CRM_V1_IMPLEMENTATION_PACKAGE.md`. None of these exist yet.

### 1.1 SQL Schema (DB-First)

- [ ] `CrmAgent` — agency / partner master table
- [ ] `CrmCounsellingType` — counselling session type master
- [ ] `CrmCommissionType` — commission type master
- [ ] `CrmCommunicationTemplate` — message template master
- [ ] `CrmLead` — primary enquiry / lead record (with `BranchId` NOT NULL)
- [ ] `CrmLeadFollowUp` — follow-up schedule and history per lead
- [ ] `CrmCounsellingSession` — counselling interactions per lead
- [ ] `CrmStudent` — converted lead / enrolled student profile (with `BranchId` NOT NULL)
- [ ] `CrmStudentDocument` — uploaded & verified documents per student
- [ ] `CrmApplicationDocumentMap` — document-to-application linkage
- [ ] `CrmVisaApplication` — visa processing case per application
- [ ] `CrmPayment` — financial transaction per student / application
- [ ] `CrmCommission` — agent commission per application
- [ ] `CrmCommunicationLog` — call / email / SMS / WhatsApp history

> FK rules and locked business rules are documented in `EDUCATION_CRM_V1_IMPLEMENTATION_PACKAGE.md` §3–4.

### 1.2 EF Core — Scaffold / DB-First Refresh

- [ ] Run EF Core scaffold after SQL schema is applied
- [ ] Verify generated entities match expected shape
- [ ] Add data annotations / Fluent API configurations as needed

### 1.3 Domain Layer — New Entities

- [ ] `CrmAgent.cs` → `Domain.Entities/Entities/CRM/Education/MasterData/`
- [ ] `CrmCounsellingType.cs` → `...MasterData/`
- [ ] `CrmCommissionType.cs` → `...MasterData/`
- [ ] `CrmCommunicationTemplate.cs` → `...MasterData/`
- [ ] `CrmLead.cs` → `...Transactional/`
- [ ] `CrmLeadFollowUp.cs` → `...Transactional/`
- [ ] `CrmCounsellingSession.cs` → `...Transactional/`
- [ ] `CrmStudent.cs` → `...Transactional/`
- [ ] `CrmStudentDocument.cs` → `...Transactional/`
- [ ] `CrmApplicationDocumentMap.cs` → `...Transactional/`
- [ ] `CrmVisaApplication.cs` → `...Transactional/`
- [ ] `CrmPayment.cs` → `...Transactional/`
- [ ] `CrmCommission.cs` → `...Transactional/`
- [ ] `CrmCommunicationLog.cs` → `...Transactional/`

### 1.4 Domain Contracts — Repository Interfaces

- [ ] `ICrmAgentRepository.cs`
- [ ] `ICrmCounsellingTypeRepository.cs`
- [ ] `ICrmCommissionTypeRepository.cs`
- [ ] `ICrmCommunicationTemplateRepository.cs`
- [ ] `ICrmLeadRepository.cs`
- [ ] `ICrmLeadFollowUpRepository.cs`
- [ ] `ICrmCounsellingSessionRepository.cs`
- [ ] `ICrmStudentRepository.cs` *(new version replacing education one)*
- [ ] `ICrmStudentDocumentRepository.cs`
- [ ] `ICrmApplicationDocumentMapRepository.cs`
- [ ] `ICrmVisaApplicationRepository.cs`
- [ ] `ICrmPaymentRepository.cs`
- [ ] `ICrmCommissionRepository.cs`
- [ ] `ICrmCommunicationLogRepository.cs`

> All under `Domain.Contracts/Repositories/CRM/`

### 1.5 Domain Contracts — Service Interfaces

- [ ] `ICrmAgentService.cs`
- [ ] `ICrmCounsellingTypeService.cs`
- [ ] `ICrmCommissionTypeService.cs`
- [ ] `ICrmCommunicationTemplateService.cs`
- [ ] `ICrmLeadService.cs`
- [ ] `ICrmLeadFollowUpService.cs`
- [ ] `ICrmCounsellingSessionService.cs`
- [ ] `ICrmStudentService.cs`
- [ ] `ICrmStudentDocumentService.cs`
- [ ] `ICrmApplicationDocumentMapService.cs`
- [ ] `ICrmVisaApplicationService.cs`
- [ ] `ICrmPaymentService.cs`
- [ ] `ICrmCommissionService.cs`
- [ ] `ICrmCommunicationLogService.cs`

> All under `Domain.Contracts/Services/CRM/`

### 1.6 Shared — Records & Validators

- [ ] `CrmAgentRecords.cs` (Create/Update/Delete records)
- [ ] `CrmCounsellingTypeRecords.cs`
- [ ] `CrmCommissionTypeRecords.cs`
- [ ] `CrmCommunicationTemplateRecords.cs`
- [ ] `CrmLeadRecords.cs` — `CreateCrmLeadRecord` must have `BranchId` (NOT NULL)
- [ ] `CrmLeadFollowUpRecords.cs`
- [ ] `CrmCounsellingSessionRecords.cs`
- [ ] `CrmStudentRecords.cs` — `CreateCrmStudentRecord` must have `BranchId` (NOT NULL)
- [ ] `CrmStudentDocumentRecords.cs`
- [ ] `CrmApplicationDocumentMapRecords.cs`
- [ ] `CrmVisaApplicationRecords.cs`
- [ ] `CrmPaymentRecords.cs`
- [ ] `CrmCommissionRecords.cs`
- [ ] `CrmCommunicationLogRecords.cs`

> All under `bdDevs.Shared/Records/CRM/`

- [ ] FluentValidation validator for each Record above (under `Application.Services/Validators/CRM/`)

### 1.7 Infrastructure — Repositories

- [ ] `CrmAgentRepository.cs`
- [ ] `CrmCounsellingTypeRepository.cs`
- [ ] `CrmCommissionTypeRepository.cs`
- [ ] `CrmCommunicationTemplateRepository.cs`
- [ ] `CrmLeadRepository.cs`
- [ ] `CrmLeadFollowUpRepository.cs`
- [ ] `CrmCounsellingSessionRepository.cs`
- [ ] `CrmStudentRepository.cs`
- [ ] `CrmStudentDocumentRepository.cs`
- [ ] `CrmApplicationDocumentMapRepository.cs`
- [ ] `CrmVisaApplicationRepository.cs`
- [ ] `CrmPaymentRepository.cs`
- [ ] `CrmCommissionRepository.cs`
- [ ] `CrmCommunicationLogRepository.cs`

> All under `Infrastructure.Repositories/CRM/`

### 1.8 Application Services

- [ ] `CrmAgentService.cs`
- [ ] `CrmCounsellingTypeService.cs`
- [ ] `CrmCommissionTypeService.cs`
- [ ] `CrmCommunicationTemplateService.cs`
- [ ] `CrmLeadService.cs` — enforce duplicate phone block, mandatory follow-up on create
- [ ] `CrmLeadFollowUpService.cs`
- [ ] `CrmCounsellingSessionService.cs`
- [ ] `CrmStudentService.cs` — enforce lead conversion rules
- [ ] `CrmStudentDocumentService.cs`
- [ ] `CrmApplicationDocumentMapService.cs`
- [ ] `CrmVisaApplicationService.cs` — enforce visa dependency on application
- [ ] `CrmPaymentService.cs`
- [ ] `CrmCommissionService.cs` — enforce application dependency
- [ ] `CrmCommunicationLogService.cs` — append-only enforcement

> All under `Application.Services/CRM/`

### 1.9 API Controllers

- [ ] `CrmAgentController.cs`
- [ ] `CrmCounsellingTypeController.cs`
- [ ] `CrmCommissionTypeController.cs`
- [ ] `CrmCommunicationTemplateController.cs`
- [ ] `CrmLeadController.cs`
- [ ] `CrmLeadFollowUpController.cs`
- [ ] `CrmCounsellingSessionController.cs`
- [ ] `CrmStudentController.cs`
- [ ] `CrmStudentDocumentController.cs`
- [ ] `CrmApplicationDocumentMapController.cs`
- [ ] `CrmVisaApplicationController.cs`
- [ ] `CrmPaymentController.cs`
- [ ] `CrmCommissionController.cs`
- [ ] `CrmCommunicationLogController.cs`

> All under `Presentation.Controller/Controllers/CRM/`  
> Must extend `BaseApiController` → route prefix `/bdDevs-crm`

### 1.10 DI Registration

- [ ] Register all new repositories in `IRepositoryManager` / `RepositoryManager`
- [ ] Register all new services in `IServiceManager` / `ServiceManager` (or `Program.cs`)

### 1.11 Build Verification (Phase 1)

- [ ] `dotnet build Presentation.Api/Presentation.Api.csproj` → 0 errors

---

## Phase 2 — CRM v1 New Module: Frontend (MVC)

> Create Razor views + 3-file JS pattern for each new module.

### 2.1 Master Data UIs (Grid + Modal)

| Status | Module | View Path | JS Path |
|--------|--------|-----------|---------|
| `[ ]` | CrmAgent | `Views/CRM/Agent/Index.cshtml` | `wwwroot/js/modules/crm/agent/` |
| `[ ]` | CrmCounsellingType | `Views/CRM/CounsellingType/Index.cshtml` | `wwwroot/js/modules/crm/counsellingtype/` |
| `[ ]` | CrmCommissionType | `Views/CRM/CommissionType/Index.cshtml` | `wwwroot/js/modules/crm/commissiontype/` |
| `[ ]` | CrmCommunicationTemplate | `Views/CRM/CommunicationTemplate/Index.cshtml` | `wwwroot/js/modules/crm/communicationtemplate/` |

### 2.2 Transactional Module UIs

| Status | Module | Notes |
|--------|--------|-------|
| `[ ]` | CrmLead | Full CRUD grid + tabbed detail form (Lead + FollowUp tabs) |
| `[ ]` | CrmLeadFollowUp | Embedded sub-grid inside Lead detail |
| `[ ]` | CrmCounsellingSession | Sub-grid inside Lead detail |
| `[ ]` | CrmStudent | Full CRUD grid + Documents/Application tabs |
| `[ ]` | CrmStudentDocument | Sub-grid inside Student detail |
| `[ ]` | CrmVisaApplication | Sub-grid inside Application detail |
| `[ ]` | CrmPayment | Grid with filters per student/application |
| `[ ]` | CrmCommission | Grid with filters per agent/application |
| `[ ]` | CrmCommunicationLog | Append-only log grid per lead/student/application |

> Each UI must follow the **3-file JS pattern** (`*Settings.js`, `*Details.js`, `*Summary.js`)  
> Use `AppConfig.apiRouteBaseUrl` for all API calls (resolves to `/bdDevs-crm`)

### 2.3 Sidebar Navigation

- [ ] Add "CRM v1" menu group or sub-items to `Views/Shared/_Sidebar.cshtml`
  - Lead Management
  - Student Management
  - Agent Management
  - Application / Visa
  - Finance (Payment, Commission)
  - Communication Log
  - Setup (CounsellingType, CommissionType, CommunicationTemplate)

---

## Phase 3 — Existing CRM Modules: UI Completion

> These 20 controllers already have backend + JS files. Verify each is fully wired and working.

### 3.1 Reference / Setup Modules (Grid + Modal)

| Status | Controller | Notes |
|--------|-----------|-------|
| `[ ]` | `CrmInstituteTypeController` | Verify grid + create/edit/delete modal |
| `[ ]` | `CrmPaymentMethodController` | JS files exist — smoke test |
| `[ ]` | `CrmMonthController` | Verify grid + modal |
| `[ ]` | `CrmYearController` | Verify grid + modal |
| `[ ]` | `CrmIntakeMonthController` | Verify grid + modal |
| `[ ]` | `CrmIntakeYearController` | Verify grid + modal |

### 3.2 Relationship Modules

| Status | Controller | Notes |
|--------|-----------|-------|
| `[ ]` | `CrmCourseIntakeController` | Verify FK filter by courseId |
| `[ ]` | `CrmApplicantCourseController` | Verify FK filter by applicationId |

### 3.3 Applicant Detail Modules (Application Sub-tabs)

| Status | Controller | Notes |
|--------|-----------|-------|
| `[ ]` | `CrmPresentAddressController` | Verify form + save by applicantId |
| `[ ]` | `CrmPermanentAddressController` | Verify form + save by applicantId |
| `[ ]` | `CrmEducationHistoryController` | Verify grid + modal |
| `[ ]` | `CrmWorkExperienceController` | Verify grid + modal |
| `[ ]` | `CrmApplicantReferenceController` | Verify grid + modal |
| `[ ]` | `CrmIeltsInformationController` | Verify form + scores |
| `[ ]` | `CrmToeflInformationController` | Verify form + scores |
| `[ ]` | `CrmGmatInformationController` | Verify form + scores |
| `[ ]` | `CrmOthersInformationController` | Verify form + scores |
| `[ ]` | `CrmStatementOfPurposeController` | Verify textarea + save |
| `[ ]` | `CrmAdditionalInfoController` | Verify form |
| `[ ]` | `CrmAdditionalDocumentController` | Verify file-selection + metadata |

---

## Phase 4 — CRUD Records Migration (Existing Modules)

> Migrate remaining services and controllers from DTO/MyMapper to Records + Mapster.  
> Current state: 54/80 services migrated, 54/78 controllers migrated.

### 4.1 Remaining SystemAdmin Services (26 to migrate)

- [ ] `ModuleService`
- [ ] `AccessControlService`
- [ ] `MenuService`
- [ ] `CurrencyService`
- [ ] `GroupService`
- [ ] `SystemSettingsService`
- [ ] `QueryAnalyzerService`
- [ ] `StatusService`
- [ ] `UsersService`
- [ ] `CompanyService`
- [ ] `BranchService`
- [ ] `EmployeeService`
- [ ] `DepartmentService`

### 4.2 Remaining DMS Services (7 to migrate)

- [ ] `DmsDocumentService`
- [ ] `DmsDocumentFolderService`
- [ ] `DmsDocumentTypeService`
- [ ] `DmsDocumentTagService`
- [ ] `DmsDocumentAccessLogService`
- [ ] `DmsDocumentVersionService`
- [ ] `DmsDocumentTagMapService`

### 4.3 Remaining Controllers (24 to migrate)

- [ ] `QueryAnalyzerController`
- [ ] `UsersController`
- [ ] `GroupController`
- [ ] `SystemSettingsController`
- [ ] `AccessControlController`
- [ ] `WorkFlowController`
- [ ] `CompaniesController`
- [ ] `CurrencyController`
- [ ] `MenuController`
- [ ] `ModuleController`
- [ ] `BranchController`
- [ ] `EmployeeController`
- [ ] `DepartmentController`
- [ ] `DmsDocumentController`
- [ ] `DmsDocumentTagController`
- [ ] `DmsDocumentTypeController`
- [ ] `DmsDocumentAccessLogController`
- [ ] `DmsDocumentFolderController`

---

## Phase 5 — Security & Quality

- [ ] Add rate limiting middleware
- [ ] Add Content Security Policy headers
- [ ] Review and tighten input validation across all endpoints
- [ ] Audit logging for security-sensitive events (login, permission changes)
- [ ] Ensure all CRM endpoints require `[Authorize]`

---

## Phase 6 — Final Testing & Documentation

- [ ] Integration test: Lead → FollowUp → Counselling → Student conversion flow
- [ ] Integration test: Student → Application → Visa flow
- [ ] Integration test: Application → Commission calculation
- [ ] Smoke test all 30 existing CRM module UIs (grid + CRUD)
- [ ] Smoke test all 14 new CRM v1 module UIs
- [ ] Update `API_ENDPOINTS_DOCUMENTATION.md` with new v1 endpoints
- [ ] Update `HRIS_UIDesign_Documentation.md` checkboxes as done

---

## Progress Summary

| Phase | Total Tasks | Done | Remaining |
|-------|------------|------|-----------|
| Phase 0 — Build Fix | ~10 | 0 | ~10 |
| Phase 1 — New Backend | ~90 | 0 | ~90 |
| Phase 2 — New Frontend UI | ~18 | 0 | ~18 |
| Phase 3 — Existing UI Verification | 20 | 0 | 20 |
| Phase 4 — CRUD Records Migration | ~45 | 54 (prev) | ~45 remaining |
| Phase 5 — Security | 5 | 0 | 5 |
| Phase 6 — Testing & Docs | ~10 | 0 | ~10 |
| **Total** | **~198** | **54 (prev)** | **~198** |

---

## Key Reference Files

| File | Purpose |
|------|---------|
| `doc/EDUCATION_CRM_V1_IMPLEMENTATION_PACKAGE.md` | Scope, ERD, business rules, naming conventions |
| `doc/BACKEND_DEVELOPER_GUIDE.md` | 7-step CRUD flow, repo pattern, naming |
| `doc/crud_records_implementation_progress.md` | Records migration tracker |
| `doc/HRIS_UIDesign_Documentation.md` | Frontend UI backlog (20 remaining CRM UIs) |
| `bdDevs.Shared/Constants/RouteConstants.cs` | API base route constants |
| `Presentation.Mvc/wwwroot/js/modules/crm/common/crmSimpleCrudFactory.js` | Reusable CRM CRUD factory |
| `Presentation.Controller/Controllers/BaseApiController.cs` | Base controller (route: `/bdDevs-crm`) |

---

*Last updated: 2026-04-27 — update this table after each session*
