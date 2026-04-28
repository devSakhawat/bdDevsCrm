# Technical Report

## 1. Purpose and Audience
এই technical report টি developer-friendly reference হিসেবে তৈরি করা হয়েছে যাতে একজন developer repository খুলেই বুঝতে পারে:
- solution structure কী
- কোন project কী কাজ করে
- কোন page কোথায় আছে
- কোন page-এর design pattern কেমন
- frontend → API → service → repository flow কীভাবে যায়
- কোন change করলে কোন file set খুলতে হবে

## 2. Solution Snapshot
- Projects: 14
- Domain entities: 99
- Service interfaces: 89
- Repository interfaces: 90
- Services: 88
- Repositories: 90
- MVC controllers/views: 81 / 94
- API controllers: 85
- Frontend module scripts: 243
- Validators: 89

## 3. Project Map
| Project | Responsibility |
| --- | --- |
| Application.Services | Application layer: services, validators, mappings, orchestration |
| Domain.Contracts | Service and repository contracts / abstractions |
| Domain.Entities | Domain entities and core business objects |
| Domain.Exceptions | Domain-specific exception definitions |
| Infrastructure.Repositories | Repository implementations and persistence adapters |
| Infrastructure.Security | JWT, encryption, and security services |
| Infrastructure.Sql | DbContext, EF Core, interceptors, and SQL access |
| Infrastructure.Utilities | Utility helpers and infrastructure support code |
| Presentation.Api | API host and composition root |
| Presentation.Controller | Shared API controllers under /bdDevs-crm |
| Presentation.Mvc | MVC frontend host, Razor views, shared shell, and JS modules |
| bdDevs.Shared | Shared kernel: ApiResponse, DTOs, records, constants, grid helpers |
| bdDevsCrm.UnitTests | Unit test suite |
| bdDevsCrm.IntegrationTests | Integration test suite |

## 4. Runtime Architecture Scenario

### 4.1 High-Level Flow
1. **Browser request** আসে `Presentation.Mvc`-এ
2. Shared layout `_Layout.cshtml` header, sidebar, page header, content zone, footer load করে
3. Core JS layer (`app.config.js`, `app.api.js`, `app.sidebar.js`, `app.header.js`, `app.modal.js`, `app.grid.js`, `app.form.js`) page behavior establish করে
4. Feature-specific MVC page scripts module অনুযায়ী কাজ করে
5. API call `/bdDevs-crm/...` route-এ যায়
6. `Presentation.Controller` API controller request receive করে
7. Controller `IServiceManager` ভিত্তিক application service call করে
8. Application service repository contract invoke করে
9. `Infrastructure.Repositories` + `Infrastructure.Sql` persistence layer data access সম্পন্ন করে
10. Response `ApiResponse<T>` envelope-এ frontend-এ ফিরে আসে

### 4.2 Layer Responsibility Map
| Layer | Main Projects | Developer Mental Model |
| --- | --- | --- |
| Presentation (MVC) | `Presentation.Mvc` | Screen, Razor, shell, browser script, user interaction |
| Presentation (API) | `Presentation.Api`, `Presentation.Controller` | API host, middleware, auth, endpoint surface |
| Application | `Application.Services` | Use case orchestration, validation, mapping, service contracts |
| Domain | `Domain.Entities`, `Domain.Contracts`, `Domain.Exceptions` | Core business model and contracts |
| Infrastructure | `Infrastructure.Repositories`, `Infrastructure.Sql`, `Infrastructure.Security`, `Infrastructure.Utilities` | DB access, security, helpers, concrete adapters |
| Shared Kernel | `bdDevs.Shared` | Shared DTO/record/response/constants used across layers |

## 5. Key Technical Conventions

### 5.1 API Route Convention
- Shared API controller base route: `/bdDevs-crm`
- Source of truth: `Presentation.Controller/Controllers/BaseApiController.cs`
- Frontend route base config: `Presentation.Mvc/wwwroot/js/core/app.config.js`

### 5.2 API Response Convention
- Every API response should align to `bdDevs.Shared/ApiResponse/ApiResponse.cs`
- Core fields: `StatusCode`, `Success`, `Message`, `Version`, `Timestamp`, `Data`, `Error`, `Pagination`, `Links`, `CorrelationId`

### 5.3 Authentication Convention
- API host registers JWT authentication and authorization policies in `Presentation.Api/Program.cs`
- MVC app uses `AuthenticationCheckMiddleware`, but current implementation is mostly pass-through and relies on client-side token flow awareness
- Login page lives at `Presentation.Mvc/Views/Account/Login.cshtml`

### 5.4 Shared Frontend Shell Convention
- Layout: `Presentation.Mvc/Views/Shared/_Layout.cshtml`
- Header: `Presentation.Mvc/Views/Shared/_Header.cshtml`
- Sidebar: `Presentation.Mvc/Views/Shared/_Sidebar.cshtml`
- Sidebar behavior: `Presentation.Mvc/wwwroot/js/core/app.sidebar.js`
- Global core scripts: `Presentation.Mvc/wwwroot/js/core/`

### 5.5 CRUD Page Convention
- Legacy pages commonly use page-specific inline styles + grid container + popup window
- Newer simple pages reuse `Presentation.Mvc/wwwroot/js/modules/crm/common/crmSimpleCrudFactory.js`
- Complex pages use tabbed form layout (`Employee`, `ApplicantInfo`, `Application`)
- Document-centric pages add upload, tagging, access, or version concepts

## 6. Page Design Scenarios

### 6.1 Scenario A — Login Page
**What a developer sees:** full-screen centered card, credential form, remember-me checkbox, toast/loading behavior.

**Open these files:**
- `Presentation.Mvc/Views/Account/Login.cshtml`
- `Presentation.Mvc/wwwroot/js/modules/account/login.js`
- `Presentation.Mvc/wwwroot/js/core/app.auth.js`

**When to open this scenario:** authentication UI, token bootstrap, login UX, entry-point branding.

### 6.2 Scenario B — Dashboard Shell
**What a developer sees:** KPI cards, inline filter form, preview table, rollout checklist, form pattern showcase.

**Open these files:**
- `Presentation.Mvc/Controllers/HomeController.cs`
- `Presentation.Mvc/Views/Home/Index.cshtml`
- `Presentation.Mvc/wwwroot/js/modules/home/homeDashboard.js`

**When to open this scenario:** shell refinement, dashboard widget design, reusable layout examples.

### 6.3 Scenario C — Simple Master Data Page
**What a developer sees:** top page header, breadcrumb, Add/Refresh actions, grid body, modal window form.

**Representative files:**
- `Presentation.Mvc/Views/Core/SystemAdmin/Country.cshtml`
- `Presentation.Mvc/Views/CRM/AdditionalDocument/Index.cshtml`
- `Presentation.Mvc/Views/DMS/DocumentAccessLog/Index.cshtml`

**Developer takeaway:** most operational pages follow this pattern; if a page is not deeply transactional, this is the default screen archetype.

### 6.4 Scenario D — Complex HR Workflow Page
**What a developer sees:** employee grid plus 12-tab data capture flow (basic info, personal, contact, employment, department/designation, salary, bank, emergency, education, experience, documents, additional info).

**Open these files:**
- `Presentation.Mvc/Views/Core/HR/Employee.cshtml`
- `Presentation.Mvc/Controllers/Core/HR/EmployeeController.cs`
- related module scripts under `Presentation.Mvc/wwwroot/js/modules/core/hr/employee/`

**Developer takeaway:** this is the model for large, segmented enterprise forms.

### 6.5 Scenario E — CRM Applicant/Application Workflow
**What a developer sees:**
- Applicant page: profile onboarding + contact/passport tabs
- Application page: applicant selection, status, education/work/documents/course/review flow

**Open these files:**
- `Presentation.Mvc/Views/CRM/ApplicantInfo/Index.cshtml`
- `Presentation.Mvc/Views/CRM/Application/Index.cshtml`
- scripts under `Presentation.Mvc/wwwroot/js/modules/crm/applicantinfo/` and `.../application/`

**Developer takeaway:** CRM workflow is split between master applicant profile and transactional application submission.

### 6.6 Scenario F — Document Workflow Page
**What a developer sees:** document grid plus upload/metadata/tag/version/access concepts.

**Open these files:**
- `Presentation.Mvc/Views/DMS/Document/Index.cshtml`
- `Presentation.Mvc/Controllers/DMS/DocumentController.cs`
- scripts under `Presentation.Mvc/wwwroot/js/modules/dms/document/`

**Developer takeaway:** DMS pages extend CRUD with file lifecycle and compliance traceability.

## 7. Developer Navigation Map
| If you need to change... | Open these code locations first |
| --- | --- |
| Shared page chrome (header/sidebar/footer/layout) | `Presentation.Mvc/Views/Shared/_Layout.cshtml`, `_Header.cshtml`, `_Sidebar.cshtml`, `_Footer.cshtml`, `Presentation.Mvc/wwwroot/css`, `Presentation.Mvc/wwwroot/js/core/` |
| Sidebar data/load behavior | `_Sidebar.cshtml`, `Presentation.Mvc/wwwroot/js/core/app.sidebar.js`, `Presentation.Mvc/wwwroot/js/core/app.config.js` |
| Header quick links, profile, badges | `_Header.cshtml`, `Presentation.Mvc/wwwroot/js/core/app.header.js`, API summary/menu endpoints |
| API base route or response model | `Presentation.Controller/Controllers/BaseApiController.cs`, `bdDevs.Shared/ApiResponse/ApiResponse.cs`, `bdDevs.Shared/Constants/RouteConstants.cs`, `Presentation.Mvc/wwwroot/js/core/app.config.js` |
| Login flow | `Presentation.Mvc/Views/Account/Login.cshtml`, `Presentation.Mvc/wwwroot/js/modules/account/login.js`, auth controllers/services |
| Dashboard shell | `Presentation.Mvc/Controllers/HomeController.cs`, `Presentation.Mvc/Views/Home/Index.cshtml`, `Presentation.Mvc/wwwroot/js/modules/home/homeDashboard.js` |
| Simple CRM/DMS CRUD page | Feature view + feature controller + `crmSimpleCrudFactory.js` + feature `settings.js`, `summary.js`, `details.js` |
| Core HR complex form | `Presentation.Mvc/Views/Core/HR/Employee.cshtml` and matching module JS/API/service/repository stack |
| API endpoint behavior | Matching controller under `Presentation.Controller/Controllers`, service in `Application.Services`, repository in `Infrastructure.Repositories` |
| Validation rules | `Application.Services/Validators/` and corresponding DTO/record definitions in `bdDevs.Shared` |

## 8. Page-to-Code Inventory
| Area | Page | Screen Archetype | Controller Path | View Path |
| --- | --- | --- | --- | --- |
| Account | Login | Standalone authentication page | `Presentation.Mvc/Controllers/AccountController.cs` | `Presentation.Mvc/Views/Account/Login.cshtml` |
| Home | Dashboard | Dashboard shell | `Presentation.Mvc/Controllers/HomeController.cs` | `Presentation.Mvc/Views/Home/Index.cshtml` |
| Home | Privacy | Static informational page | `Presentation.Mvc/Controllers/HomeController.cs` | `Presentation.Mvc/Views/Home/Privacy.cshtml` |
| Core > HR | Branch | Grid + modal CRUD | `Presentation.Mvc/Controllers/Core/HR/BranchController.cs` | `Presentation.Mvc/Views/Core/HR/Branch.cshtml` |
| Core > HR | Department | Grid + modal CRUD | `Presentation.Mvc/Controllers/Core/HR/DepartmentController.cs` | `Presentation.Mvc/Views/Core/HR/Department.cshtml` |
| Core > HR | Designation | Grid + modal CRUD | `Presentation.Mvc/Controllers/Core/HR/DesignationController.cs` | `Presentation.Mvc/Views/Core/HR/Designation.cshtml` |
| Core > HR | Employee | Grid + 12-tab workflow form | `Presentation.Mvc/Controllers/Core/HR/EmployeeController.cs` | `Presentation.Mvc/Views/Core/HR/Employee.cshtml` |
| Core > HR | Shift | Grid + modal CRUD | `Presentation.Mvc/Controllers/Core/HR/ShiftController.cs` | `Presentation.Mvc/Views/Core/HR/Shift.cshtml` |
| Core > SystemAdmin | Access Control | Governance CRUD page | `Presentation.Mvc/Controllers/Core/SystemAdmin/AccessControlController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/AccessControl.cshtml` |
| Core > SystemAdmin | Access Restriction | Governance CRUD page | `Presentation.Mvc/Controllers/Core/SystemAdmin/AccessRestrictionController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/AccessRestriction.cshtml` |
| Core > SystemAdmin | Approver Details | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/Core/SystemAdmin/ApproverDetailsController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/ApproverDetails.cshtml` |
| Core > SystemAdmin | Approver History | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/Core/SystemAdmin/ApproverHistoryController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/ApproverHistory.cshtml` |
| Core > SystemAdmin | Approver Order | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/Core/SystemAdmin/ApproverOrderController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/ApproverOrder.cshtml` |
| Core > SystemAdmin | Approver Type | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/Core/SystemAdmin/ApproverTypeController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/ApproverType.cshtml` |
| Core > SystemAdmin | Approver Type To Group Mapping | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/Core/SystemAdmin/ApproverTypeToGroupMappingController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/ApproverTypeToGroupMapping.cshtml` |
| Core > SystemAdmin | Apps Token Info | Review grid + modal | `Presentation.Mvc/Controllers/Core/SystemAdmin/AppsTokenInfoController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/AppsTokenInfo.cshtml` |
| Core > SystemAdmin | Apps Transaction Log | Review grid + modal | `Presentation.Mvc/Controllers/Core/SystemAdmin/AppsTransactionLogController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/AppsTransactionLog.cshtml` |
| Core > SystemAdmin | Assign Approver | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/Core/SystemAdmin/AssignApproverController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/AssignApprover.cshtml` |
| Core > SystemAdmin | Audit Log | Review grid + modal | `Presentation.Mvc/Controllers/Core/SystemAdmin/AuditLogController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/AuditLog.cshtml` |
| Core > SystemAdmin | Audit Trail | Review grid + modal | `Presentation.Mvc/Controllers/Core/SystemAdmin/AuditTrailController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/AuditTrail.cshtml` |
| Core > SystemAdmin | Audit Type | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/Core/SystemAdmin/AuditTypeController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/AuditType.cshtml` |
| Core > SystemAdmin | Board Institute | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/Core/SystemAdmin/BoardInstituteController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/BoardInstitute.cshtml` |
| Core > SystemAdmin | Common | Grid + modal CRUD | `Presentation.Mvc/Controllers/Core/SystemAdmin/CommonController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/Common.cshtml` |
| Core > SystemAdmin | Company | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/Core/SystemAdmin/CompanyController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/Company.cshtml` |
| Core > SystemAdmin | Competencies | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/Core/SystemAdmin/CompetenciesController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/Competencies.cshtml` |
| Core > SystemAdmin | Competency Level | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/Core/SystemAdmin/CompetencyLevelController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/CompetencyLevel.cshtml` |
| Core > SystemAdmin | Country | Grid + modal CRUD | `Presentation.Mvc/Controllers/Core/SystemAdmin/CountryController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/Country.cshtml` |
| Core > SystemAdmin | Currency | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/Core/SystemAdmin/CurrencyController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/Currency.cshtml` |
| Core > SystemAdmin | Currency Rate | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/Core/SystemAdmin/CurrencyRateController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/CurrencyRate.cshtml` |
| Core > SystemAdmin | Document Parameter | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/Core/SystemAdmin/DocumentParameterController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/DocumentParameter.cshtml` |
| Core > SystemAdmin | Document Parameter Mapping | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/Core/SystemAdmin/DocumentParameterMappingController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/DocumentParameterMapping.cshtml` |
| Core > SystemAdmin | Document Query Mapping | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/Core/SystemAdmin/DocumentQueryMappingController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/DocumentQueryMapping.cshtml` |
| Core > SystemAdmin | Document Template | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/Core/SystemAdmin/DocumentTemplateController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/DocumentTemplate.cshtml` |
| Core > SystemAdmin | Document Type | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/Core/SystemAdmin/DocumentTypeController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/DocumentType.cshtml` |
| Core > SystemAdmin | Group | Grid + modal CRUD | `Presentation.Mvc/Controllers/Core/SystemAdmin/GroupController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/Group.cshtml` |
| Core > SystemAdmin | Holiday | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/Core/SystemAdmin/HolidayController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/Holiday.cshtml` |
| Core > SystemAdmin | Marital Status | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/Core/SystemAdmin/MaritalStatusController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/MaritalStatus.cshtml` |
| Core > SystemAdmin | Menu | Grid + modal CRUD | `Presentation.Mvc/Controllers/Core/SystemAdmin/MenuController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/Menu.cshtml` |
| Core > SystemAdmin | Module | Grid + modal CRUD | `Presentation.Mvc/Controllers/Core/SystemAdmin/ModuleController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/Module.cshtml` |
| Core > SystemAdmin | Password History | Review grid + modal | `Presentation.Mvc/Controllers/Core/SystemAdmin/PasswordHistoryController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/PasswordHistory.cshtml` |
| Core > SystemAdmin | Query Analyzer | Diagnostic review page | `Presentation.Mvc/Controllers/Core/SystemAdmin/QueryAnalyzerController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/QueryAnalyzer.cshtml` |
| Core > SystemAdmin | System Settings | Configuration CRUD page | `Presentation.Mvc/Controllers/Core/SystemAdmin/SystemSettingsController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/SystemSettings.cshtml` |
| Core > SystemAdmin | Thana | Grid + modal CRUD | `Presentation.Mvc/Controllers/Core/SystemAdmin/ThanaController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/Thana.cshtml` |
| Core > SystemAdmin | Timesheet | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/Core/SystemAdmin/TimesheetController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/Timesheet.cshtml` |
| Core > SystemAdmin | Token Blacklist | Security review page | `Presentation.Mvc/Controllers/Core/SystemAdmin/TokenBlacklistController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/TokenBlacklist.cshtml` |
| Core > SystemAdmin | Users | Administrative CRUD page | `Presentation.Mvc/Controllers/Core/SystemAdmin/UsersController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/Users.cshtml` |
| Core > SystemAdmin | Workflow | Grid + tabbed workflow form | `Presentation.Mvc/Controllers/Core/SystemAdmin/WorkflowController.cs` | `Presentation.Mvc/Views/Core/SystemAdmin/Workflow.cshtml` |
| CRM | Additional Document | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/CRM/AdditionalDocumentController.cs` | `Presentation.Mvc/Views/CRM/AdditionalDocument/Index.cshtml` |
| CRM | Additional Info | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/CRM/AdditionalInfoController.cs` | `Presentation.Mvc/Views/CRM/AdditionalInfo/Index.cshtml` |
| CRM | Applicant Course | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/CRM/ApplicantCourseController.cs` | `Presentation.Mvc/Views/CRM/ApplicantCourse/Index.cshtml` |
| CRM | Applicant Info | Grid + 3-tab workflow form | `No dedicated MVC controller found (view present)` | `Presentation.Mvc/Views/CRM/ApplicantInfo/Index.cshtml` |
| CRM | Applicant Reference | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/CRM/ApplicantReferenceController.cs` | `Presentation.Mvc/Views/CRM/ApplicantReference/Index.cshtml` |
| CRM | Application | Grid + 7-tab workflow form | `Presentation.Mvc/Controllers/CRM/ApplicationController.cs` | `Presentation.Mvc/Views/CRM/Application/Index.cshtml` |
| CRM | Application Status | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/CRM/ApplicationStatusController.cs` | `Presentation.Mvc/Views/CRM/ApplicationStatus/Index.cshtml` |
| CRM | Communication Type | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/CRM/CommunicationTypeController.cs` | `Presentation.Mvc/Views/CRM/CommunicationType/Index.cshtml` |
| CRM | Course | Grid + modal CRUD | `No dedicated MVC controller found (view present)` | `Presentation.Mvc/Views/CRM/Course/Index.cshtml` |
| CRM | Course Intake | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/CRM/CourseIntakeController.cs` | `Presentation.Mvc/Views/CRM/CourseIntake/Index.cshtml` |
| CRM | Document Type | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/CRM/DocumentTypeController.cs` | `Presentation.Mvc/Views/CRM/DocumentType/Index.cshtml` |
| CRM | Education History | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/CRM/EducationHistoryController.cs` | `Presentation.Mvc/Views/CRM/EducationHistory/Index.cshtml` |
| CRM | Gmat Information | Grid + modal CRUD | `Presentation.Mvc/Controllers/CRM/GmatInformationController.cs` | `Presentation.Mvc/Views/CRM/GmatInformation/Index.cshtml` |
| CRM | Ielts Information | Grid + modal CRUD | `Presentation.Mvc/Controllers/CRM/IeltsInformationController.cs` | `Presentation.Mvc/Views/CRM/IeltsInformation/Index.cshtml` |
| CRM | Institute | Grid + modal CRUD | `No dedicated MVC controller found (view present)` | `Presentation.Mvc/Views/CRM/Institute/Index.cshtml` |
| CRM | Institute Type | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/CRM/InstituteTypeController.cs` | `Presentation.Mvc/Views/CRM/InstituteType/Index.cshtml` |
| CRM | Intake Month | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/CRM/IntakeMonthController.cs` | `Presentation.Mvc/Views/CRM/IntakeMonth/Index.cshtml` |
| CRM | Intake Year | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/CRM/IntakeYearController.cs` | `Presentation.Mvc/Views/CRM/IntakeYear/Index.cshtml` |
| CRM | Lead Source | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/CRM/LeadSourceController.cs` | `Presentation.Mvc/Views/CRM/LeadSource/Index.cshtml` |
| CRM | Lead Stage | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/CRM/LeadStageController.cs` | `Presentation.Mvc/Views/CRM/LeadStage/Index.cshtml` |
| CRM | Month | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/CRM/MonthController.cs` | `Presentation.Mvc/Views/CRM/Month/Index.cshtml` |
| CRM | Others Information | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/CRM/OthersInformationController.cs` | `Presentation.Mvc/Views/CRM/OthersInformation/Index.cshtml` |
| CRM | Payment Method | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/CRM/PaymentMethodController.cs` | `Presentation.Mvc/Views/CRM/PaymentMethod/Index.cshtml` |
| CRM | Permanent Address | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/CRM/PermanentAddressController.cs` | `Presentation.Mvc/Views/CRM/PermanentAddress/Index.cshtml` |
| CRM | Present Address | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/CRM/PresentAddressController.cs` | `Presentation.Mvc/Views/CRM/PresentAddress/Index.cshtml` |
| CRM | Statement Of Purpose | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/CRM/StatementOfPurposeController.cs` | `Presentation.Mvc/Views/CRM/StatementOfPurpose/Index.cshtml` |
| CRM | Toefl Information | Grid + modal CRUD | `Presentation.Mvc/Controllers/CRM/ToeflInformationController.cs` | `Presentation.Mvc/Views/CRM/ToeflInformation/Index.cshtml` |
| CRM | Visa Status | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/CRM/VisaStatusController.cs` | `Presentation.Mvc/Views/CRM/VisaStatus/Index.cshtml` |
| CRM | Work Experience | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/CRM/WorkExperienceController.cs` | `Presentation.Mvc/Views/CRM/WorkExperience/Index.cshtml` |
| CRM | Year | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/CRM/YearController.cs` | `Presentation.Mvc/Views/CRM/Year/Index.cshtml` |
| DMS | Document | Document workflow page | `Presentation.Mvc/Controllers/DMS/DocumentController.cs` | `Presentation.Mvc/Views/DMS/Document/Index.cshtml` |
| DMS | Document Access Log | Review grid + modal | `Presentation.Mvc/Controllers/DMS/DocumentAccessLogController.cs` | `Presentation.Mvc/Views/DMS/DocumentAccessLog/Index.cshtml` |
| DMS | Document Folder | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/DMS/DocumentFolderController.cs` | `Presentation.Mvc/Views/DMS/DocumentFolder/Index.cshtml` |
| DMS | Document Tag | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/DMS/DocumentTagController.cs` | `Presentation.Mvc/Views/DMS/DocumentTag/Index.cshtml` |
| DMS | Document Type | Reusable grid + modal CRUD | `Presentation.Mvc/Controllers/DMS/DocumentTypeController.cs` | `Presentation.Mvc/Views/DMS/DocumentType/Index.cshtml` |
| DMS | File Update History | Review grid + modal | `Presentation.Mvc/Controllers/DMS/FileUpdateHistoryController.cs` | `Presentation.Mvc/Views/DMS/FileUpdateHistory/Index.cshtml` |
| Test | Session Test | Diagnostic utility page | `Presentation.Mvc/Controllers/TestController.cs` | `Presentation.Mvc/Views/Test/SessionTest.cshtml` |

## 9. Important Current-State Notes
1. **Sidebar is API-driven** – current sidebar markup is a shell; menu tree is built client-side from menu data.
2. **Header is interactive** – quick links, notifications, approvals, messages, birthdays, and profile context are header panel concepts.
3. **Kendo asset references are scaffold-ready but commented** – `_Layout.cshtml` keeps placeholders for licensed assets.
4. **MVC routes are controller-name based** – multiple namespaces organize code, so developers should use full file path/module context when tracing a page.
5. **API remains the main security gate** – MVC middleware currently does not fully enforce route blocking by itself.
6. **Shared CRUD factory exists for newer pages** – use it before inventing page-specific CRUD plumbing.

## 10. Developer Workflow Recommendation
1. Identify the page/module from the functional document.
2. Open the Razor view and matching MVC controller.
3. Check whether the page is legacy-specific JS or shared-factory based.
4. Trace the API endpoint from `app.config.js` or feature settings.
5. Move into `Presentation.Controller` → `Application.Services` → `Infrastructure.Repositories` as needed.
6. Validate response shape against `ApiResponse<T>`.
7. Update documentation when page purpose, flow, or architecture changes.

## 11. Fast Orientation for New Developers
- **Want to understand the UI shell?** Start with `_Layout.cshtml`, `_Header.cshtml`, `_Sidebar.cshtml`
- **Want to understand route/response conventions?** Start with `BaseApiController.cs` and `ApiResponse.cs`
- **Want to inspect a simple page pattern?** Open `CRM/AdditionalDocument/Index.cshtml`
- **Want to inspect a large workflow page?** Open `Core/HR/Employee.cshtml` or `CRM/Application/Index.cshtml`
- **Want to inspect DMS behavior?** Open `DMS/Document/Index.cshtml`
- **Want to understand frontend runtime?** Explore `Presentation.Mvc/wwwroot/js/core/`
- **Want to understand backend composition?** Start at `Presentation.Api/Program.cs`
