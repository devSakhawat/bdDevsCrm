# Functional Documentation

## 1. Purpose
এই ডকুমেন্টটি bdDevsCrm-এর বর্তমান functional landscape-কে business capability, actor, workflow, এবং page-by-page responsibility অনুযায়ী ব্যাখ্যা করে। Developer, BA, QA, UI designer এবং module owner—সবার জন্য এটি একটি common reference।

## 2. Product Functional Scope
প্রজেক্টটি বর্তমানে নিচের business area-গুলোকে cover করে:
- **Account / Entry** – login এবং session bootstrapping
- **Home** – dashboard shell, privacy/compliance landing
- **Core HR** – employee and organizational master data maintenance
- **Core System Administration** – security, approval, document config, audit, token, and global setup
- **CRM** – applicant, application, academic profile, contact/address, document and supporting master data
- **DMS** – document repository, tags, folders, types, access log, update history
- **Test/Diagnostic** – session behavior validation page

## 3. Primary Actors
| Actor | Functional Goal |
| --- | --- |
| System Administrator | System settings, access control, menus, modules, approvals, token/audit governance পরিচালনা করা |
| HR Operator | Employee, branch, department, designation, shift, holiday, timesheet-type configuration maintain করা |
| CRM Operator / Counselor | Applicant onboarding, application processing, academic/test/document workflow maintain করা |
| DMS Operator | Document repository, folder, type, tag, access log, history maintain করা |
| Compliance / Audit Team | Audit log, access log, password history, token blacklist, transaction trail review করা |
| End User / Employee | Login, dashboard visibility, quick link navigation, profile-context workspace use করা |
| Developer / Support Engineer | কোন page কোথায়, কী purpose, কোন layer impact—তা দ্রুত শনাক্ত করা |

## 4. End-to-End Functional Flows

### 4.1 Authentication and Entry Flow
1. User `Account/Login` page-এ credentials দেয়
2. Frontend login script API-auth route call করে
3. Token/session established হলে user workspace shell-এ প্রবেশ করে
4. Header, sidebar, quick links, and profile context load হয়

### 4.2 Dashboard Flow
1. User landing dashboard-এ summary cards, quick filters, preview table দেখে
2. Dashboard reusable shell pattern দেখায়: KPI + filter + action + table + form section
3. এখান থেকে user অন্যান্য module page-এ navigate করে

### 4.3 Master Data Maintenance Flow
1. User sidebar/module থেকে একটি master page খোলে
2. Grid-এ current records দেখে
3. Add/Edit action থেকে modal বা popup form খোলে
4. Save/Refresh/Delete cycle-এর মাধ্যমে data maintain করে
5. Updated state grid-এ reflect হয়

### 4.4 Complex Workflow Flow
1. User complex entity page (Employee / Applicant / Application / Document) খোলে
2. Grid থেকে existing record select অথবা new workflow initiate করে
3. Multi-tab বা document-focused form ব্যবহার করে segmented data capture করে
4. Review/submit/update action-এর মাধ্যমে lifecycle progress হয়

### 4.5 Audit and Governance Flow
1. Compliance/Admin user audit/log/history/token page খোলে
2. Grid/review screen থেকে event records inspect করে
3. Suspicious access, transaction, password or token event trace করা যায়

## 5. Functional UI Archetypes
| Archetype | Typical Use |
| --- | --- |
| Standalone authentication page | Login and access entry |
| Dashboard shell | KPI, shortcut, quick filter, preview insights |
| Grid + modal CRUD | Reference/master data maintenance |
| Reusable grid + modal CRUD | New shared-page pattern using common JS factory |
| Grid + tabbed workflow form | High-data-density business workflow page |
| Document workflow page | File, metadata, tag, access, history-centric page |
| Review grid | Audit, log, token, history, diagnostic pages |

## 6. Page Inventory by Module
## Account

| Page | MVC Controller | Business Purpose | Primary Activities | UI Pattern | View Path |
| --- | --- | --- | --- | --- | --- |
| Login | AccountController | User authentication, token bootstrap, and workspace entry | Enter credentials, remember session, submit login, start authenticated workflow | Standalone authentication page | `Presentation.Mvc/Views/Account/Login.cshtml` |

## Home

| Page | MVC Controller | Business Purpose | Primary Activities | UI Pattern | View Path |
| --- | --- | --- | --- | --- | --- |
| Dashboard | HomeController | Dashboard landing page for the workspace shell | Review KPI cards, use quick filters, inspect preview table, trigger dashboard actions | Dashboard shell | `Presentation.Mvc/Views/Home/Index.cshtml` |
| Privacy | HomeController | Privacy, compliance, and policy communication page | Review privacy statement, compliance guidance, and governance notice | Static informational page | `Presentation.Mvc/Views/Home/Privacy.cshtml` |

## Core > HR

| Page | MVC Controller | Business Purpose | Primary Activities | UI Pattern | View Path |
| --- | --- | --- | --- | --- | --- |
| Branch | BranchController | Branch master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Grid + modal CRUD | `Presentation.Mvc/Views/Core/HR/Branch.cshtml` |
| Department | DepartmentController | Department master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Grid + modal CRUD | `Presentation.Mvc/Views/Core/HR/Department.cshtml` |
| Designation | DesignationController | Designation master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Grid + modal CRUD | `Presentation.Mvc/Views/Core/HR/Designation.cshtml` |
| Employee | EmployeeController | End-to-end employee lifecycle management | List employees, open complex profile form, maintain HR data across 12 tabs, update status and related information | Grid + 12-tab workflow form | `Presentation.Mvc/Views/Core/HR/Employee.cshtml` |
| Shift | ShiftController | Shift master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Grid + modal CRUD | `Presentation.Mvc/Views/Core/HR/Shift.cshtml` |

## Core > SystemAdmin

| Page | MVC Controller | Business Purpose | Primary Activities | UI Pattern | View Path |
| --- | --- | --- | --- | --- | --- |
| Access Control | AccessControlController | Role and permission matrix administration | Review access rules, maintain permission mapping, align security policy with modules | Governance CRUD page | `Presentation.Mvc/Views/Core/SystemAdmin/AccessControl.cshtml` |
| Access Restriction | AccessRestrictionController | Policy-based access restriction administration | Configure restriction rules, review exceptions, update enforcement settings | Governance CRUD page | `Presentation.Mvc/Views/Core/SystemAdmin/AccessRestriction.cshtml` |
| Approver Details | ApproverDetailsController | Approver Details master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/Core/SystemAdmin/ApproverDetails.cshtml` |
| Approver History | ApproverHistoryController | Approver History review, traceability, and compliance support | List records, filter/search, inspect entries, support audit and incident review | Reusable grid + modal CRUD | `Presentation.Mvc/Views/Core/SystemAdmin/ApproverHistory.cshtml` |
| Approver Order | ApproverOrderController | Approver Order master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/Core/SystemAdmin/ApproverOrder.cshtml` |
| Approver Type | ApproverTypeController | Approver Type master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/Core/SystemAdmin/ApproverType.cshtml` |
| Approver Type To Group Mapping | ApproverTypeToGroupMappingController | Approver Type To Group Mapping master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/Core/SystemAdmin/ApproverTypeToGroupMapping.cshtml` |
| Apps Token Info | AppsTokenInfoController | Token issuance and token metadata review | Inspect issued tokens, support session investigation, review token status | Review grid + modal | `Presentation.Mvc/Views/Core/SystemAdmin/AppsTokenInfo.cshtml` |
| Apps Transaction Log | AppsTransactionLogController | Application transaction log review | Inspect system transactions, monitor business events, support troubleshooting | Review grid + modal | `Presentation.Mvc/Views/Core/SystemAdmin/AppsTransactionLog.cshtml` |
| Assign Approver | AssignApproverController | Assign Approver master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/Core/SystemAdmin/AssignApprover.cshtml` |
| Audit Log | AuditLogController | Operational audit log review | Search logs, inspect change records, support investigation and compliance reporting | Review grid + modal | `Presentation.Mvc/Views/Core/SystemAdmin/AuditLog.cshtml` |
| Audit Trail | AuditTrailController | Detailed entity-level audit trail review | Inspect before/after traces, follow business events, support traceability and compliance | Review grid + modal | `Presentation.Mvc/Views/Core/SystemAdmin/AuditTrail.cshtml` |
| Audit Type | AuditTypeController | Audit Type review, traceability, and compliance support | List records, filter/search, inspect entries, support audit and incident review | Reusable grid + modal CRUD | `Presentation.Mvc/Views/Core/SystemAdmin/AuditType.cshtml` |
| Board Institute | BoardInstituteController | Board Institute master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/Core/SystemAdmin/BoardInstitute.cshtml` |
| Common | CommonController | Common operational maintenance page | Review records, add new entries, update existing data, and keep module data current | Grid + modal CRUD | `Presentation.Mvc/Views/Core/SystemAdmin/Common.cshtml` |
| Company | CompanyController | Company master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/Core/SystemAdmin/Company.cshtml` |
| Competencies | CompetenciesController | Competencies master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/Core/SystemAdmin/Competencies.cshtml` |
| Competency Level | CompetencyLevelController | Competency Level master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/Core/SystemAdmin/CompetencyLevel.cshtml` |
| Country | CountryController | Country master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Grid + modal CRUD | `Presentation.Mvc/Views/Core/SystemAdmin/Country.cshtml` |
| Currency | CurrencyController | Currency master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/Core/SystemAdmin/Currency.cshtml` |
| Currency Rate | CurrencyRateController | Currency Rate master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/Core/SystemAdmin/CurrencyRate.cshtml` |
| Document Parameter | DocumentParameterController | Document Parameter master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/Core/SystemAdmin/DocumentParameter.cshtml` |
| Document Parameter Mapping | DocumentParameterMappingController | Document Parameter Mapping master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/Core/SystemAdmin/DocumentParameterMapping.cshtml` |
| Document Query Mapping | DocumentQueryMappingController | Document Query Mapping master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/Core/SystemAdmin/DocumentQueryMapping.cshtml` |
| Document Template | DocumentTemplateController | Document Template master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/Core/SystemAdmin/DocumentTemplate.cshtml` |
| Document Type | DocumentTypeController | System-level document type configuration | Maintain shared document type metadata for cross-module configuration | Reusable grid + modal CRUD | `Presentation.Mvc/Views/Core/SystemAdmin/DocumentType.cshtml` |
| Group | GroupController | Group master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Grid + modal CRUD | `Presentation.Mvc/Views/Core/SystemAdmin/Group.cshtml` |
| Holiday | HolidayController | Holiday master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/Core/SystemAdmin/Holiday.cshtml` |
| Marital Status | MaritalStatusController | Marital Status master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/Core/SystemAdmin/MaritalStatus.cshtml` |
| Menu | MenuController | Menu master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Grid + modal CRUD | `Presentation.Mvc/Views/Core/SystemAdmin/Menu.cshtml` |
| Module | ModuleController | Module master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Grid + modal CRUD | `Presentation.Mvc/Views/Core/SystemAdmin/Module.cshtml` |
| Password History | PasswordHistoryController | Password history and credential governance review | Inspect password change history, support security review, monitor policy compliance | Review grid + modal | `Presentation.Mvc/Views/Core/SystemAdmin/PasswordHistory.cshtml` |
| Query Analyzer | QueryAnalyzerController | Diagnostic query review and optimization support | Inspect query metadata, review analysis results, support performance troubleshooting | Diagnostic review page | `Presentation.Mvc/Views/Core/SystemAdmin/QueryAnalyzer.cshtml` |
| System Settings | SystemSettingsController | Global application settings and configuration management | Review configuration values, update system behavior controls, maintain operational settings | Configuration CRUD page | `Presentation.Mvc/Views/Core/SystemAdmin/SystemSettings.cshtml` |
| Thana | ThanaController | Thana master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Grid + modal CRUD | `Presentation.Mvc/Views/Core/SystemAdmin/Thana.cshtml` |
| Timesheet | TimesheetController | Timesheet master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/Core/SystemAdmin/Timesheet.cshtml` |
| Token Blacklist | TokenBlacklistController | Blocked token governance page | Review revoked tokens, maintain blacklist status, support forced logout scenarios | Security review page | `Presentation.Mvc/Views/Core/SystemAdmin/TokenBlacklist.cshtml` |
| Users | UsersController | System user directory and administrative maintenance | Review user accounts, manage identity records, update status and administrative assignments | Administrative CRUD page | `Presentation.Mvc/Views/Core/SystemAdmin/Users.cshtml` |
| Workflow | WorkflowController | Workflow master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Grid + tabbed workflow form | `Presentation.Mvc/Views/Core/SystemAdmin/Workflow.cshtml` |

## CRM

| Page | MVC Controller | Business Purpose | Primary Activities | UI Pattern | View Path |
| --- | --- | --- | --- | --- | --- |
| Additional Document | AdditionalDocumentController | Additional supporting document maintenance for CRM workflow | Capture supporting document metadata, update linked records, and keep document requirements current | Reusable grid + modal CRUD | `Presentation.Mvc/Views/CRM/AdditionalDocument/Index.cshtml` |
| Additional Info | AdditionalInfoController | Supplementary applicant information maintenance | Store extra applicant metadata, edit supplemental fields, and keep profile completeness updated | Reusable grid + modal CRUD | `Presentation.Mvc/Views/CRM/AdditionalInfo/Index.cshtml` |
| Applicant Course | ApplicantCourseController | Applicant preferred course mapping maintenance | Associate applicants with course choices, edit preferences, and maintain selection data | Reusable grid + modal CRUD | `Presentation.Mvc/Views/CRM/ApplicantCourse/Index.cshtml` |
| Applicant Info | No dedicated MVC controller found | Applicant onboarding and master profile capture | List applicants, capture personal/contact/passport data, edit applicant records, maintain recruitment pipeline inputs | Grid + 3-tab workflow form | `Presentation.Mvc/Views/CRM/ApplicantInfo/Index.cshtml` |
| Applicant Reference | ApplicantReferenceController | Applicant reference contact maintenance | Capture referee information, update relationship details, and maintain reference contacts | Reusable grid + modal CRUD | `Presentation.Mvc/Views/CRM/ApplicantReference/Index.cshtml` |
| Application | ApplicationController | CRM application processing and submission workflow | Create applications, assign status, attach documents, choose course intake, review and submit records | Grid + 7-tab workflow form | `Presentation.Mvc/Views/CRM/Application/Index.cshtml` |
| Application Status | ApplicationStatusController | Application Status master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/CRM/ApplicationStatus/Index.cshtml` |
| Communication Type | CommunicationTypeController | Communication Type master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/CRM/CommunicationType/Index.cshtml` |
| Course | No dedicated MVC controller found | Course master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Grid + modal CRUD | `Presentation.Mvc/Views/CRM/Course/Index.cshtml` |
| Course Intake | CourseIntakeController | Course Intake master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/CRM/CourseIntake/Index.cshtml` |
| Document Type | DocumentTypeController | CRM-facing document category maintenance | Maintain CRM document categories used by applicant and application workflows | Reusable grid + modal CRUD | `Presentation.Mvc/Views/CRM/DocumentType/Index.cshtml` |
| Education History | EducationHistoryController | Applicant academic history maintenance | Capture previous academic records, edit qualifications, and keep study history complete | Reusable grid + modal CRUD | `Presentation.Mvc/Views/CRM/EducationHistory/Index.cshtml` |
| Gmat Information | GmatInformationController | GMAT score capture for applicant evaluation | Store, edit, and review GMAT exam result information | Grid + modal CRUD | `Presentation.Mvc/Views/CRM/GmatInformation/Index.cshtml` |
| Ielts Information | IeltsInformationController | IELTS score capture for applicant evaluation | Store, edit, and review IELTS exam result information | Grid + modal CRUD | `Presentation.Mvc/Views/CRM/IeltsInformation/Index.cshtml` |
| Institute | No dedicated MVC controller found | Institute master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Grid + modal CRUD | `Presentation.Mvc/Views/CRM/Institute/Index.cshtml` |
| Institute Type | InstituteTypeController | Institute Type master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/CRM/InstituteType/Index.cshtml` |
| Intake Month | IntakeMonthController | Intake Month master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/CRM/IntakeMonth/Index.cshtml` |
| Intake Year | IntakeYearController | Intake Year master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/CRM/IntakeYear/Index.cshtml` |
| Lead Source | LeadSourceController | Lead Source master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/CRM/LeadSource/Index.cshtml` |
| Lead Stage | LeadStageController | Lead Stage master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/CRM/LeadStage/Index.cshtml` |
| Month | MonthController | Month master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/CRM/Month/Index.cshtml` |
| Others Information | OthersInformationController | Supplementary miscellaneous applicant information maintenance | Store extra narrative or uncategorized details, edit information, and keep applicant profile complete | Reusable grid + modal CRUD | `Presentation.Mvc/Views/CRM/OthersInformation/Index.cshtml` |
| Payment Method | PaymentMethodController | Payment Method master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/CRM/PaymentMethod/Index.cshtml` |
| Permanent Address | PermanentAddressController | Applicant permanent address maintenance | Capture permanent address details, update location fields, and maintain mailing information | Reusable grid + modal CRUD | `Presentation.Mvc/Views/CRM/PermanentAddress/Index.cshtml` |
| Present Address | PresentAddressController | Applicant present address maintenance | Capture current address details, update location fields, and maintain contactability | Reusable grid + modal CRUD | `Presentation.Mvc/Views/CRM/PresentAddress/Index.cshtml` |
| Statement Of Purpose | StatementOfPurposeController | Statement of purpose maintenance for applicant/application workflow | Store and update statement records linked to applicant or application processing | Reusable grid + modal CRUD | `Presentation.Mvc/Views/CRM/StatementOfPurpose/Index.cshtml` |
| Toefl Information | ToeflInformationController | TOEFL score capture for applicant evaluation | Store, edit, and review TOEFL exam result information | Grid + modal CRUD | `Presentation.Mvc/Views/CRM/ToeflInformation/Index.cshtml` |
| Visa Status | VisaStatusController | Visa Status master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/CRM/VisaStatus/Index.cshtml` |
| Work Experience | WorkExperienceController | Applicant work experience maintenance | Capture professional background, update employer details, and maintain work history | Reusable grid + modal CRUD | `Presentation.Mvc/Views/CRM/WorkExperience/Index.cshtml` |
| Year | YearController | Year master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/CRM/Year/Index.cshtml` |

## DMS

| Page | MVC Controller | Business Purpose | Primary Activities | UI Pattern | View Path |
| --- | --- | --- | --- | --- | --- |
| Document | DocumentController | Document repository, upload, metadata, tags, and version workflow | Browse documents, upload files, manage metadata, review versions, assign tags and access level | Document workflow page | `Presentation.Mvc/Views/DMS/Document/Index.cshtml` |
| Document Access Log | DocumentAccessLogController | Document access audit trail review | List access events, inspect who accessed what, filter logs, support compliance review | Review grid + modal | `Presentation.Mvc/Views/DMS/DocumentAccessLog/Index.cshtml` |
| Document Folder | DocumentFolderController | Document Folder master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/DMS/DocumentFolder/Index.cshtml` |
| Document Tag | DocumentTagController | Document Tag master/reference data maintenance | List, create, edit, activate/deactivate, and delete reference records | Reusable grid + modal CRUD | `Presentation.Mvc/Views/DMS/DocumentTag/Index.cshtml` |
| Document Type | DocumentTypeController | DMS document taxonomy maintenance | Maintain repository document types and filing categories | Reusable grid + modal CRUD | `Presentation.Mvc/Views/DMS/DocumentType/Index.cshtml` |
| File Update History | FileUpdateHistoryController | Document version and update history review | List revisions, inspect update trail, review change chronology, support rollback analysis | Review grid + modal | `Presentation.Mvc/Views/DMS/FileUpdateHistory/Index.cshtml` |

## Test

| Page | MVC Controller | Business Purpose | Primary Activities | UI Pattern | View Path |
| --- | --- | --- | --- | --- | --- |
| Session Test | TestController | Session and token behavior diagnostic page | Check session persistence, troubleshoot auth state, verify client session flow | Diagnostic utility page | `Presentation.Mvc/Views/Test/SessionTest.cshtml` |

## 7. Functional Summary by Domain

### Account
- Login is the single explicit public entry page.
- Session bootstrap and credential validation শুরু এখান থেকে।

### Home
- Dashboard একটি shell reference page, যেখানে future business widgets plug করা যাবে।
- Privacy page governance/compliance messaging-এর জন্য রাখা হয়েছে।

### Core HR
- Branch, Department, Designation, Shift master data HR structure define করে।
- Employee page complete HR profile workflow-এর কেন্দ্রবিন্দু।

### Core System Administration
- Access, approver, menu, module, group, user, workflow, settings, token, audit, query, document configuration—সব admin governance এই domain-এ।
- এটি platform control tower হিসেবে কাজ করে।

### CRM
- Applicant profile থেকে শুরু করে application submission পর্যন্ত end-to-end student/lead style workflow cover করে।
- Supporting reference data (lead stage, lead source, intake, institute, course, communication type, payment method, visa status) pipeline-কে operationalize করে।
- Additional profile pages applicant/application-এর linked sub-record structure maintain করে।

### DMS
- Document repository এবং তার surrounding metadata ecosystem (folder, tag, type, access log, update history) এখানে সংগঠিত।
- Compliance and traceability use case-ও এই domain-এ visible।

## 8. Functional Change Management Rule
যেকোনো নতুন page/module যুক্ত হলে অন্তত এই ৫টি তথ্য যোগ করতে হবে:
1. Page name
2. Business purpose
3. Primary user activities
4. UI pattern
5. Related controller/view/script path
