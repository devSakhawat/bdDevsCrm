**Education CRM**   |   Technical Specification Document   |   v1.0   |   April 2026

**TECHNICAL SPECIFICATION DOCUMENT**

**Education CRM System**

Production-Ready Business Domain & Data Architecture


|**Attribute**|**Value**|
| :-: | :-: |
|Document Type|Technical Specification — Business Domain Layer|
|Version|1\.0 — Production Reference|
|Scope|T-01 through T-23 — Complete Domain Design|
|System|Overseas Education Consultancy CRM|
|Architecture|Single-Tenant, Multi-Branch|
|Database|SQL Server — Database-First|
|Backend|.NET Core 10 Web API|
|Frontend|.NET Core MVC + Kendo jQuery UI|
|Status|APPROVED FOR DEVELOPMENT|
|Date|April 2026|


# **Table of Contents**
**1.**      Project Overview & Architecture Decisions

**2.**      T-01: Academic Master Data ERD

**3.**      T-02: Supporting Master Data ERD

**4.**      T-03: Master Data Seed Scripts

**5.**      T-04 & T-05: Lead ERD & State Machine

**6.**      T-06: Follow-up ERD & State Machine

**7.**      T-07: Counselling Session ERD & Workflow

**8.**      T-08: Student ERD & Academic Profile

**9.**      T-09: Conversion Gate — Full Validation Sequence

**10.**     T-10: Document ERD & State Machine & Checklist Logic

**11.**     T-11: Application ERD & State Machine

**12.**     T-12: Application Conditions Workflow

**13.**     T-13: Scholarship ERD

**14.**     T-14: Visa ERD & State Machine

**15.**     T-15: Student Payment ERD & State Machine

**16.**     T-16: Commission ERD & Calculation Logic

**17.**     T-17: Refund Workflow

**18.**     T-18: Communication Log ERD

**19.**     T-19: Agent & AgentLead ERD & Commission Link

**20.**     T-20: Notification Trigger Map — All Modules

**21.**     T-21: Audit Log Design

**22.**     T-22: Branch Transfer Workflow

**23.**     T-23: Reporting Queries & KPI Definitions

App A   Complete Business Rules Catalogue

App B   Glossary of Terms


# **1. Project Overview & Architecture Decisions**
This document is the complete technical specification for the Education CRM system. It defines every database entity, state machine, business rule, workflow, and query required to build the system. It is the authoritative reference for the development team and covers T-01 through T-23 in full detail.

## **1.1 System Purpose**
The Education CRM manages the complete lifecycle of a student at an overseas education consultancy — from the first point of contact (Lead) through counselling, document preparation, university application, visa processing, pre-departure, and post-arrival enrollment confirmation. It also manages internal operations including staff roles, branch targets, agent commissions, and financial tracking.

## **1.2 Architecture Decisions (Locked)**

|**Decision**|**Choice**|**Rationale**|
| :-: | :-: | :-: |
|Tenancy|Single-Tenant|Deployed on client's own server. No multi-org isolation needed.|
|Branch Model|Multi-Branch (BranchId)|One agency, multiple branches. BranchId on all transactional tables.|
|Hosting|On-Premise|Client manages own infrastructure.|
|Master Data|Shared across branches|Countries, Universities, Programs — no per-branch duplication.|
|Delete Policy|Soft Delete Only|IsDeleted flag. No physical removal. Full audit integrity.|
|Role System|Dynamic RBAC (Boilerplate)|Roles and permissions managed by boilerplate. This doc defines WHAT needs enforcement.|
|Workflow|Dynamic (Boilerplate)|Approval workflows handled by boilerplate. This doc defines business rules.|
|Database|SQL Server — Database-First|Schema is the source of truth.|

## **1.3 Module Overview**

|**Module**|**Responsibility**|
| :-: | :-: |
|Lead Management|Entry point — all incoming prospects|
|Follow-up Management|Standalone follow-up cycle tracking|
|Counselling Management|Needs assessment & program matching|
|Student Management|Full student profile post-conversion|
|Document Management|Collection, verification, checklist|
|Application Management|University application workflow|
|Visa Management|Embassy process & outcomes|
|Agent Management|B2B agent network & commission|
|Finance Management|Payments, commissions, profit|
|Communication Management|Call/WhatsApp/email log|

## **1.4 System Flow (Conceptual)**

|Lead  -->  Follow-up (parallel)|
| :- |
|Lead  -->  Counselling Session  -->  [GATE: Conversion]  -->  Student|
|Student  -->  Document Checklist  -->  Collection  -->  Verification|
|Student  -->  [GATE: Application-Ready]  -->  Application  -->  Offer|
|Application  -->  Visa  -->  Pre-Departure  -->  Post-Arrival  -->  Enrolled|
|Application  -->  Commission (on Enrollment)|
|Student  -->  Payments|
|Agent  -->  Lead  -->  AgentLead  -->  Commission (auto-linked)|

|<p>**Critical Developer Note**</p><p>TWO SYSTEM GATES exist in the flow that are enforced at API level:</p><p>Gate 1 — Conversion Gate: Lead cannot become Student without passing all mandatory pre-requisites.</p><p>Gate 2 — Application Gate: No application can be created until Student IsApplicationReady = 1.</p><p>Both gates must be enforced server-side. UI cannot bypass them.</p>|
| :- |


# **2. T-01: Academic Master Data ERD**
The academic hierarchy is the structural foundation. Without correctly configured academic data, applications cannot be created, document checklists cannot be generated, and commissions cannot be calculated.

## **2.1 Hierarchy**

|Countries|
| :- |
|`  `└── Universities  (CountryId FK)|
|`        `└── Faculties  (UniversityId FK)|
|`        `└── Programs  (UniversityId + FacultyId + DegreeLevelId FK)|
|`              `└── Intakes  (ProgramId FK)|
|`                    `└── CourseFees  (ProgramId + IntakeId FK)|
|Countries + DegreeLevels|
|`  `└── CountryDocumentRequirements  (CountryId + DegreeLevelId + DocumentTypeId FK)|

## **2.2 Countries Table**

|CREATE TABLE Countries (|
| :- |
|`    `Id                    INT IDENTITY(1,1) PRIMARY KEY,|
|`    `Name                  NVARCHAR(100) NOT NULL,|
|`    `Code                  NVARCHAR(5)   NOT NULL UNIQUE,|
|`    `CurrencyCode          NVARCHAR(10)  NOT NULL,|
|`    `VisaProcessingNotes   NVARCHAR(MAX) NULL,|
|`    `IsActive              BIT DEFAULT 1,|
|`    `SortOrder             INT DEFAULT 0,|
|`    `CreatedBy             INT NOT NULL,|
|`    `CreatedDate           DATETIME DEFAULT GETDATE(),|
|`    `UpdatedBy             INT NULL,|
|`    `UpdatedDate           DATETIME NULL|
|);|

## **2.3 Universities Table**

|CREATE TABLE Universities (|
| :- |
|`    `Id                    INT IDENTITY(1,1) PRIMARY KEY,|
|`    `CountryId             INT NOT NULL FK --> Countries,|
|`    `Name                  NVARCHAR(200) NOT NULL,|
|`    `ShortName             NVARCHAR(50) NULL,|
|`    `AccreditationStatus   TINYINT NOT NULL, -- 1=Recognized 2=Provisional 3=Not Recognized|
|`    `WorldRanking          INT NULL,|
|`    `OfficialWebsiteURL    NVARCHAR(300) NULL,|
|`    `AdmissionPortalURL    NVARCHAR(300) NULL,|
|`    `TuitionFeeCurrency    NVARCHAR(10) NOT NULL,|
|`    `-- Partnership|
|`    `IsAuthorizedPartner   BIT DEFAULT 0,|
|`    `PartnershipStartDate  DATETIME NULL,|
|`    `PartnershipExpiryDate DATETIME NULL,|
|`    `CommissionRate        DECIMAL(5,2) NULL,|
|`    `CommissionType        TINYINT NULL, -- 1=Percentage 2=Fixed|
|`    `-- Admission Contact|
|`    `AdmissionContactName  NVARCHAR(150) NULL,|
|`    `AdmissionContactEmail NVARCHAR(150) NULL,|
|`    `AdmissionContactPhone NVARCHAR(20) NULL,|
|`    `-- Entry Requirements (university defaults)|
|`    `MinIELTSScore         DECIMAL(3,1) NULL,|
|`    `MinTOEFLScore         INT NULL,|
|`    `MinAcademicPercent    DECIMAL(5,2) NULL,|
|`    `WorkExpRequired       BIT DEFAULT 0,|
|`    `WorkExpYears          INT NULL,|
|`    `SOPRequired           BIT DEFAULT 0,|
|`    `ReferenceLettersCount INT DEFAULT 0,|
|`    `IsActive              BIT DEFAULT 1,|
|`    `IsDeleted             BIT DEFAULT 0,|
|`    `CreatedBy             INT NOT NULL,|
|`    `CreatedDate           DATETIME DEFAULT GETDATE(),|
|`    `UpdatedBy             INT NULL,|
|`    `UpdatedDate           DATETIME NULL|
|);|

|<p>**Business Rule — Partnership Expiry**</p><p>Partnership Expiry Job: Daily scheduled job checks PartnershipExpiryDate.</p><p>60 days before: Warning notification to Branch Manager.</p><p>30 days before: Critical notification to Manager + Super Admin.</p><p>0 days (expired): IsAuthorizedPartner auto-set to 0. New applications BLOCKED.</p>|
| :- |

## **2.4 Faculties Table**

|CREATE TABLE Faculties (|
| :- |
|`    `Id            INT IDENTITY(1,1) PRIMARY KEY,|
|`    `UniversityId  INT NOT NULL FK --> Universities,|
|`    `Name          NVARCHAR(200) NOT NULL,|
|`    `IsActive      BIT DEFAULT 1,|
|`    `CreatedBy     INT NOT NULL,|
|`    `CreatedDate   DATETIME DEFAULT GETDATE(),|
|`    `UpdatedBy     INT NULL,|
|`    `UpdatedDate   DATETIME NULL,|
|`    `CONSTRAINT UQ\_Faculties UNIQUE (UniversityId, Name)|
|);|

## **2.5 DegreeLevels Table (Seed Only)**

|CREATE TABLE DegreeLevels (|
| :- |
|`    `Id          INT IDENTITY(1,1) PRIMARY KEY,|
|`    `Name        NVARCHAR(100) NOT NULL UNIQUE,|
|`    `SortOrder   INT DEFAULT 0,|
|`    `IsActive    BIT DEFAULT 1|
|);|
|-- Seed: Foundation(1) Certificate(2) Diploma(3) Bachelor(4) Master(5) PhD(6) Short Course(7)|

## **2.6 Programs Table**

|CREATE TABLE Programs (|
| :- |
|`    `Id                     INT IDENTITY(1,1) PRIMARY KEY,|
|`    `UniversityId           INT NOT NULL FK --> Universities,|
|`    `FacultyId              INT NOT NULL FK --> Faculties,|
|`    `DegreeLevelId          INT NOT NULL FK --> DegreeLevels,|
|`    `Name                   NVARCHAR(200) NOT NULL,|
|`    `Duration               INT NOT NULL,|
|`    `DurationUnit           TINYINT NOT NULL, -- 1=Years 2=Months|
|`    `StudyMode              TINYINT NOT NULL, -- 1=Full-time 2=Part-time 3=Online|
|`    `LanguageOfInstruction  NVARCHAR(50) DEFAULT 'English',|
|`    `-- Program-level overrides (NULL = use University default)|
|`    `MinIELTSOverride       DECIMAL(3,1) NULL,|
|`    `MinAcademicOverride    DECIMAL(5,2) NULL,|
|`    `WorkExpOverride        BIT NULL,|
|`    `SOPOverride            BIT NULL,|
|`    `IsActive               BIT DEFAULT 1,|
|`    `IsDeleted              BIT DEFAULT 0,|
|`    `CreatedBy              INT NOT NULL,|
|`    `CreatedDate            DATETIME DEFAULT GETDATE(),|
|`    `UpdatedBy              INT NULL,|
|`    `UpdatedDate            DATETIME NULL,|
|`    `CONSTRAINT UQ\_Programs UNIQUE (UniversityId, FacultyId, DegreeLevelId, Name)|
|);|
||
|-- Effective Requirement Logic:|
|-- EffectiveIELTS = COALESCE(Program.MinIELTSOverride, University.MinIELTSScore)|
|-- Same pattern for SOP, WorkExp, MinAcademic|

## **2.7 Intakes Table**

|CREATE TABLE Intakes (|
| :- |
|`    `Id                   INT IDENTITY(1,1) PRIMARY KEY,|
|`    `ProgramId            INT NOT NULL FK --> Programs,|
|`    `Name                 NVARCHAR(100) NOT NULL, -- e.g. 'September 2026'|
|`    `ApplicationOpenDate  DATETIME NOT NULL,|
|`    `ApplicationDeadline  DATETIME NOT NULL,|
|`    `CourseStartDate      DATETIME NOT NULL,|
|`    `AvailableSeats       INT NULL,|
|`    `Status               TINYINT NOT NULL, -- 1=Upcoming 2=Open 3=Closed 4=Waitlist|
|`    `IsActive             BIT DEFAULT 1,|
|`    `CreatedBy            INT NOT NULL,|
|`    `CreatedDate          DATETIME DEFAULT GETDATE(),|
|`    `UpdatedBy            INT NULL,|
|`    `UpdatedDate          DATETIME NULL,|
|`    `CONSTRAINT UQ\_Intakes UNIQUE (ProgramId, Name)|
|);|
||
|-- Auto-Status Logic (Daily Job):|
|-- Current < OpenDate          --> Status = 1 (Upcoming)|
|-- OpenDate <= Current <= Deadline --> Status = 2 (Open)|
|-- Current > Deadline          --> Status = 3 (Closed)|
|--   Draft applications for Closed intake: flagged, Processing Officer notified|

## **2.8 CourseFees Table**

|CREATE TABLE CourseFees (|
| :- |
|`    `Id               INT IDENTITY(1,1) PRIMARY KEY,|
|`    `ProgramId        INT NOT NULL FK --> Programs,|
|`    `IntakeId         INT NOT NULL FK --> Intakes,|
|`    `FeeType          TINYINT NOT NULL,|
|`                     `-- 1=Tuition 2=Registration 3=Accommodation 4=Insurance 5=Materials|
|`    `Amount           DECIMAL(18,2) NOT NULL,|
|`    `Currency         NVARCHAR(10)  NOT NULL,|
|`    `PaymentSchedule  TINYINT NOT NULL, -- 1=Per Year 2=Per Semester 3=One-time|
|`    `Notes            NVARCHAR(500) NULL,|
|`    `CreatedBy        INT NOT NULL,|
|`    `CreatedDate      DATETIME DEFAULT GETDATE(),|
|`    `UpdatedBy        INT NULL,|
|`    `UpdatedDate      DATETIME NULL,|
|`    `CONSTRAINT UQ\_CourseFees UNIQUE (ProgramId, IntakeId, FeeType)|
|);|
||
|-- FEE SNAPSHOT RULE: At application creation, fee structure is serialized|
|-- to JSON and stored in Applications.FeeSnapshot.|
|-- Master fee changes do NOT affect existing applications.|

## **2.9 CountryDocumentRequirements Table**

|CREATE TABLE CountryDocumentRequirements (|
| :- |
|`    `Id              INT IDENTITY(1,1) PRIMARY KEY,|
|`    `CountryId       INT NOT NULL FK --> Countries,|
|`    `DegreeLevelId   INT NOT NULL FK --> DegreeLevels,|
|`    `DocumentTypeId  INT NOT NULL FK --> DocumentTypes,|
|`    `IsRequired      BIT NOT NULL DEFAULT 1,|
|`    `Notes           NVARCHAR(500) NULL,|
|`    `CreatedBy       INT NOT NULL,|
|`    `CreatedDate     DATETIME DEFAULT GETDATE(),|
|`    `CONSTRAINT UQ\_CountryDocReq UNIQUE (CountryId, DegreeLevelId, DocumentTypeId)|
|);|
||
|-- Checklist Generation: On student conversion,|
|-- SELECT DocumentTypeId, IsRequired FROM CountryDocumentRequirements|
|-- WHERE CountryId = Student.PreferredCountryId|
|-- AND   DegreeLevelId = Student.PreferredDegreeLevelId|
|-- --> INSERT into StudentDocumentChecklists|

## **2.10 T-01 Key Indexes**

|CREATE INDEX IX\_Universities\_CountryId|
| :- |
|`    `ON Universities (CountryId) WHERE IsDeleted = 0 AND IsActive = 1;|
||
|CREATE INDEX IX\_Universities\_PartnershipExpiry|
|`    `ON Universities (PartnershipExpiryDate) WHERE IsAuthorizedPartner = 1;|
||
|CREATE INDEX IX\_Programs\_UniversityId|
|`    `ON Programs (UniversityId) WHERE IsDeleted = 0 AND IsActive = 1;|
||
|CREATE INDEX IX\_Intakes\_ProgramId\_Status|
|`    `ON Intakes (ProgramId, Status) WHERE IsActive = 1;|
||
|CREATE INDEX IX\_Intakes\_Deadline|
|`    `ON Intakes (ApplicationDeadline) WHERE Status IN (1, 2);|
||
|CREATE INDEX IX\_CountryDocReq\_CountryId\_DegreeLevel|
|`    `ON CountryDocumentRequirements (CountryId, DegreeLevelId);|


# **3. T-02: Supporting Master Data ERD**

## **3.1 DocumentTypes Table**

|CREATE TABLE DocumentTypes (|
| :- |
|`    `Id                INT IDENTITY(1,1) PRIMARY KEY,|
|`    `Name              NVARCHAR(150) NOT NULL UNIQUE,|
|`    `Category          TINYINT NOT NULL,|
|`                      `-- 1=Identity 2=Academic 3=Language 4=Financial|
|`                      `-- 5=Employment 6=Supporting 7=Visa Specific|
|`    `HasExpiryDate     BIT DEFAULT 0,|
|`    `ExpiryWarningDays INT NULL,   -- Passport=90, IELTS=30|
|`    `AcceptedFormats   NVARCHAR(50) DEFAULT 'PDF',|
|`    `MaxFileSizeMB     INT DEFAULT 10,|
|`    `IsActive          BIT DEFAULT 1,|
|`    `SortOrder         INT DEFAULT 0|
|);|

## **3.2 Branches Table**

|CREATE TABLE Branches (|
| :- |
|`    `Id                     INT IDENTITY(1,1) PRIMARY KEY,|
|`    `Name                   NVARCHAR(150) NOT NULL,|
|`    `Code                   NVARCHAR(10)  NOT NULL UNIQUE, -- DHK, CTG, SYL|
|`    `Address                NVARCHAR(500) NULL,|
|`    `Phone                  NVARCHAR(20)  NULL,|
|`    `Email                  NVARCHAR(150) NULL,|
|`    `ManagerId              INT NULL FK --> Users,|
|`    `MonthlyLeadTarget      INT DEFAULT 0,|
|`    `MonthlyConversionTarget INT DEFAULT 0,|
|`    `MonthlyEnrollmentTarget INT DEFAULT 0,|
|`    `MonthlyRevenueTarget   DECIMAL(18,2) DEFAULT 0,|
|`    `IsActive               BIT DEFAULT 1,|
|`    `CreatedBy              INT NOT NULL,|
|`    `CreatedDate            DATETIME DEFAULT GETDATE(),|
|`    `UpdatedBy              INT NULL,|
|`    `UpdatedDate            DATETIME NULL|
|);|
|-- Note: ManagerId FK added after Users table creation (ALTER TABLE).|

## **3.3 Agents Table**

|CREATE TABLE Agents (|
| :- |
|`    `Id                    INT IDENTITY(1,1) PRIMARY KEY,|
|`    `BranchId              INT NOT NULL FK --> Branches,|
|`    `Name                  NVARCHAR(150) NOT NULL,|
|`    `CompanyName           NVARCHAR(200) NULL,|
|`    `Phone                 NVARCHAR(20)  NOT NULL,|
|`    `Email                 NVARCHAR(150) NULL,|
|`    `CommissionType        TINYINT NOT NULL, -- 1=Percentage 2=Fixed|
|`    `CommissionRate        DECIMAL(8,2)  NOT NULL,|
|`    `CommissionCurrency    NVARCHAR(10)  DEFAULT 'BDT',|
|`    `AgreementStartDate    DATETIME NULL,|
|`    `AgreementExpiryDate   DATETIME NULL,|
|`    `IsActive              BIT DEFAULT 1,|
|`    `IsDeleted             BIT DEFAULT 0,|
|`    `CreatedBy             INT NOT NULL,|
|`    `CreatedDate           DATETIME DEFAULT GETDATE(),|
|`    `UpdatedBy             INT NULL,|
|`    `UpdatedDate           DATETIME NULL|
|);|

## **3.4 LeadSources Table**

|CREATE TABLE LeadSources (|
| :- |
|`    `Id          INT IDENTITY(1,1) PRIMARY KEY,|
|`    `Name        NVARCHAR(100) NOT NULL UNIQUE,|
|`    `Category    TINYINT NOT NULL,|
|`                `-- 1=Digital 2=Walk-in 3=Referral 4=Agent 5=Event 6=Outbound|
|`    `IsActive    BIT DEFAULT 1,|
|`    `SortOrder   INT DEFAULT 0|
|);|

## **3.5 BranchTargets Table**

|CREATE TABLE BranchTargets (|
| :- |
|`    `Id                  INT IDENTITY(1,1) PRIMARY KEY,|
|`    `BranchId            INT NOT NULL FK --> Branches,|
|`    `Year                INT NOT NULL,|
|`    `Month               INT NOT NULL,  -- 1-12|
|`    `LeadTarget          INT DEFAULT 0,|
|`    `ConversionTarget    INT DEFAULT 0,|
|`    `EnrollmentTarget    INT DEFAULT 0,|
|`    `RevenueTarget       DECIMAL(18,2) DEFAULT 0,|
|`    `CreatedBy           INT NOT NULL,|
|`    `CreatedDate         DATETIME DEFAULT GETDATE(),|
|`    `UpdatedBy           INT NULL,|
|`    `UpdatedDate         DATETIME NULL,|
|`    `CONSTRAINT UQ\_BranchTargets UNIQUE (BranchId, Year, Month)|
|);|

## **3.6 SystemConfigurations Table**

|CREATE TABLE SystemConfigurations (|
| :- |
|`    `Id           INT IDENTITY(1,1) PRIMARY KEY,|
|`    `ConfigKey    NVARCHAR(100) NOT NULL UNIQUE,|
|`    `ConfigValue  NVARCHAR(MAX) NOT NULL,|
|`    `DataType     NVARCHAR(20),  -- int, bool, string, json|
|`    `Description  NVARCHAR(500) NULL,|
|`    `IsEditable   BIT DEFAULT 1,|
|`    `UpdatedBy    INT NULL,|
|`    `UpdatedDate  DATETIME NULL|
|);|

## **3.7 MasterDataSuggestions Table**

|CREATE TABLE MasterDataSuggestions (|
| :- |
|`    `Id                INT IDENTITY(1,1) PRIMARY KEY,|
|`    `BranchId          INT NOT NULL FK --> Branches,|
|`    `SuggestedBy       INT NOT NULL FK --> Users,|
|`    `EntityType        TINYINT NOT NULL, -- 1=University 2=Program 3=DocumentType|
|`    `SuggestedName     NVARCHAR(200) NOT NULL,|
|`    `SuggestionDetails NVARCHAR(MAX) NULL,|
|`    `Status            TINYINT NOT NULL DEFAULT 1, -- 1=Pending 2=Approved 3=Rejected|
|`    `ReviewedBy        INT NULL FK --> Users,|
|`    `ReviewedDate      DATETIME NULL,|
|`    `ReviewRemarks     NVARCHAR(500) NULL,|
|`    `CreatedDate       DATETIME DEFAULT GETDATE()|
|);|

|<p>**Master Data Suggestion Workflow**</p><p>Suggestion Workflow: Counsellor submits suggestion (Status=Pending).</p><p>Branch Manager notified --> Reviews and approves or rejects.</p><p>If Approved: Processing Officer adds full entity. Counsellor notified.</p><p>If Rejected: ReviewRemarks mandatory. Counsellor notified with reason.</p><p>Counsellors cannot directly add master data -- controlled entry only.</p>|
| :- |


# **4. T-03: Master Data Seed Scripts**
All seed scripts must run in the sequence below to satisfy foreign key dependencies. Each script uses an idempotent pattern to allow safe re-runs.

## **4.1 Execution Sequence**

|**Step**|**Entity**|**Dependency**|**Method**|
| :-: | :-: | :-: | :-: |
|1|DegreeLevels|None|IDENTITY\_INSERT + idempotent check|
|2|DocumentTypes|None|IDENTITY\_INSERT + idempotent check|
|3|LeadSources|None|IDENTITY\_INSERT + idempotent check|
|4|Countries|None|IDENTITY\_INSERT + idempotent check|
|5|CountryDocumentRequirements|Countries + DegreeLevels + DocTypes|MERGE pattern|
|6|SystemConfigurations|None|MERGE pattern|

## **4.2 DegreeLevels Seed**

|SET IDENTITY\_INSERT DegreeLevels ON;|
| :- |
|INSERT INTO DegreeLevels (Id, Name, SortOrder, IsActive) VALUES|
|`  `(1, 'Foundation', 1, 1), (2, 'Certificate', 2, 1),|
|`  `(3, 'Diploma',    3, 1), (4, 'Bachelor',    4, 1),|
|`  `(5, 'Master',     5, 1), (6, 'PhD',         6, 1),|
|`  `(7, 'Short Course', 7, 1);|
|SET IDENTITY\_INSERT DegreeLevels OFF;|

## **4.3 DocumentTypes Seed (Key Entries)**

|**Id**|**Name**|**Category**|**HasExpiry**|**WarnDays**|
| :-: | :-: | :-: | :-: | :-: |
|1|Passport|Identity (1)|**Yes**|90|
|4|Photo (Passport Size)|Identity (1)|No|-|
|5|SSC / O-Level Marksheet|Academic (2)|No|-|
|7|HSC / A-Level Marksheet|Academic (2)|No|-|
|9|University Degree Certificate|Academic (2)|No|-|
|10|University Transcript|Academic (2)|No|-|
|11|IELTS Certificate|Language (3)|**Yes**|30|
|15|Bank Statement (6 Months)|Financial (4)|**Yes**|30|
|22|Statement of Purpose (SOP)|Supporting (6)|No|-|
|24|Reference Letter 1|Supporting (6)|No|-|
|28|Police Clearance Certificate|Visa (7)|**Yes**|90|
|32|Visa Refusal Letter|Visa (7)|No|-|

## **4.4 SystemConfigurations Seed (Key Entries)**

|**ConfigKey**|**Value**|**Description**|
| :-: | :-: | :-: |
|lead.sla.response\_hours|4|Hours to contact new lead (SLA)|
|lead.sla.untouched\_hours|48|Hours before untouched lead alert|
|lead.unresponsive.missed\_count|3|Missed follow-ups before Unresponsive|
|followup.sla.overdue\_hours|24|Hours before overdue escalation to Manager|
|doc.expiry.warning\_days\_warning|90|Days before expiry for first warning|
|doc.expiry.warning\_days\_critical|30|Days before expiry for critical alert|
|doc.max\_rejection\_count|3|Rejections before Manager escalation|
|doc.max\_file\_size\_mb|10|Max upload file size (MB)|
|doc.accepted\_formats|PDF,JPG,PNG|Accepted file formats|
|partnership.expiry.warning\_days\_1|60|Days before partnership expiry warning 1|
|partnership.expiry.warning\_days\_2|30|Days before partnership expiry warning 2|
|conversion.min\_counselling\_sessions|1|Min sessions before conversion|
|application.intake\_deadline\_warning|7|Days before intake deadline alert|
|finance.commission\_dispute\_days|30|Max days to resolve commission dispute|
|post\_arrival.enrollment\_confirm\_days|14|Days after course start for enrollment confirm|

## **4.5 Idempotent Seed Pattern**

|-- Safe re-run pattern using MERGE:|
| :- |
|MERGE INTO Countries AS target|
|USING (VALUES (1,'United Kingdom','GB','GBP',1,1)) AS source|
|`  `(Id, Name, Code, CurrencyCode, IsActive, SortOrder)|
|ON target.Id = source.Id|
|WHEN NOT MATCHED THEN|
|`  `INSERT (Id, Name, Code, CurrencyCode, IsActive, SortOrder)|
|`  `VALUES (source.Id, source.Name, source.Code,|
|`          `source.CurrencyCode, source.IsActive, source.SortOrder);|
||
|-- Wrap all steps in a transaction:|
|BEGIN TRANSACTION;|
|BEGIN TRY|
|`  `-- Step 1..6 scripts here|
|`  `COMMIT TRANSACTION;|
|`  `PRINT 'All seed data committed.';|
|END TRY|
|BEGIN CATCH|
|`  `ROLLBACK TRANSACTION;|
|`  `PRINT 'Seed failed: ' + ERROR\_MESSAGE();|
|END CATCH;|


# **5. T-04 & T-05: Lead ERD & State Machine**

## **5.1 Leads Table**

|CREATE TABLE Leads (|
| :- |
|`    `Id                    INT IDENTITY(1,1) PRIMARY KEY,|
|`    `Name                  NVARCHAR(150) NOT NULL,|
|`    `Phone                 NVARCHAR(20)  NOT NULL,|
|`    `Email                 NVARCHAR(150) NULL,|
|`    `LeadSourceId          INT FK --> LeadSources,|
|`    `BranchId              INT NOT NULL FK --> Branches,|
|`    `AssignedTo            INT NULL FK --> Users,|
|`    `AssignedDate          DATETIME NULL,|
|`    `Priority              TINYINT DEFAULT 2, -- 1=Low 2=Medium 3=High|
|`    `Status                TINYINT NOT NULL DEFAULT 1,|
|`    `InterestedCountryId   INT NULL FK --> Countries,|
|`    `InterestedDegreeLevel TINYINT NULL FK --> DegreeLevels,|
|`    `Remarks               NVARCHAR(MAX) NULL,|
|`    `IsDeleted             BIT DEFAULT 0,|
|`    `DeletedBy             INT NULL,|
|`    `DeletedDate           DATETIME NULL,|
|`    `CreatedBy             INT NOT NULL,|
|`    `CreatedDate           DATETIME DEFAULT GETDATE(),|
|`    `UpdatedBy             INT NULL,|
|`    `UpdatedDate           DATETIME NULL,|
|`    `CONSTRAINT UQ\_Leads\_Phone UNIQUE (Phone) WHERE IsDeleted = 0|
|);|

## **5.2 Lead Status Values**

|**Id**|**Status Name**|**IsTerminal**|**AllowFollowUp**|**Description**|
| :-: | :-: | :-: | :-: | :-: |
|1|New|No|**Yes**|Just created, no contact yet|
|2|Contacted|No|**Yes**|First contact made|
|3|In Counselling|No|**Yes**|Active counselling sessions|
|4|Interested|No|**Yes**|Program selected, ready for conversion|
|5|Not Interested|No|No|Declined services|
|6|Converted|**Yes**|No|Lead became a Student|
|7|Lost to Competitor|**Yes**|No|Enrolled elsewhere|
|8|Unresponsive|No|No|3+ missed follow-ups, no response|
|9|Archived|**Yes**|No|Manager archived|

## **5.3 Lead State Machine**

|`                 `+-------+|
| :- |
|`                 `|  NEW  | <-- Lead Created|
|`                 `+---+---+|
|`                     `| First Contact|
|`                     `v|
|`              `+-----------+|
|`              `| CONTACTED ||
|`              `+-----+-----+|
|`                    `| Session Scheduled|
|`                    `v|
|`          `+------------------+|
|`          `|  IN COUNSELLING  ||
|`          `+--------+---------+|
|`               `+---+---+|
|`               `|       ||
|`               `v       v|
|`        `+----------+  +----------------+|
|`        `|INTERESTED|  | NOT INTERESTED | (terminal)|
|`        `+----+-----+  +----------------+|
|`             `||
|`      `+------+-------+|
|`      `|              ||
|`      `v              v|
|`  `+-----------+  +------------------+|
|`  `| CONVERTED |  | LOST TO COMPETITOR| (terminal)|
|`  `| (terminal)|  +------------------+|
|`  `+-----------+|
||
|Any Status --> UNRESPONSIVE (3 consecutive missed follow-ups)|
|Any Status --> ARCHIVED     (Manager decision)|

## **5.4 Button to Action Map**

|**Button / Action**|**From Status**|**To Status**|**Key Validations**|
| :-: | :-: | :-: | :-: |
|Create Lead|None|New|Phone unique (cross-branch). LeadSourceId required.|
|Assign Counsellor|New|New (AssignedTo set)|Counsellor must be active, same branch.|
|Mark Contacted|New / Contacted|Contacted|Min 1 CommunicationLog exists for this Lead.|
|Schedule Follow-up|Any non-terminal|Status unchanged|ScheduledDate >= today. LeadId required.|
|Complete Follow-up|Pending (FU)|Completed (FU)|Remarks required (min 10 chars). CompletedDate = now.|
|Begin Counselling|Contacted|In Counselling|CounsellingSession record created.|
|Mark Interested|In Counselling|Interested|Min 1 CounsellingSession saved.|
|Mark Not Interested|Any non-terminal|Not Interested|Reason required (Remarks field).|
|Convert to Student|Interested|Converted|Full Conversion Gate (T-09).|
|Mark Lost (Competitor)|Any non-terminal|Lost to Competitor|Reason required. Competitor name optional.|
|Archive Lead|Any|Archived|Manager/Sr.Counsellor only. Reason required.|
|Reassign Counsellor|Any non-terminal|Status unchanged|Same branch. Sr.Counsellor/Manager only.|

## **5.5 Lead Indexes**

|CREATE INDEX IX\_Leads\_BranchId\_Status ON Leads(BranchId, Status) WHERE IsDeleted=0;|
| :- |
|CREATE INDEX IX\_Leads\_AssignedTo ON Leads(AssignedTo) WHERE IsDeleted=0;|
|CREATE INDEX IX\_Leads\_Phone ON Leads(Phone) WHERE IsDeleted=0;|
|CREATE INDEX IX\_AgentLeads\_LeadId ON AgentLeads(LeadId);|

## **5.6 AgentLeads Table**

|CREATE TABLE AgentLeads (|
| :- |
|`    `Id           INT IDENTITY(1,1) PRIMARY KEY,|
|`    `AgentId      INT NOT NULL FK --> Agents,|
|`    `LeadId       INT NOT NULL FK --> Leads,|
|`    `AssignedDate DATETIME DEFAULT GETDATE(),|
|`    `Notes        NVARCHAR(500) NULL,|
|`    `CreatedBy    INT NOT NULL,|
|`    `CreatedDate  DATETIME DEFAULT GETDATE(),|
|`    `CONSTRAINT UQ\_AgentLeads\_Lead UNIQUE (LeadId)  -- One agent per lead|
|);|


# **6. T-06: Follow-up ERD & State Machine**

## **6.1 FollowUps Table**

|CREATE TABLE FollowUps (|
| :- |
|`    `Id               INT IDENTITY(1,1) PRIMARY KEY,|
|`    `LeadId           INT NOT NULL FK --> Leads,|
|`    `ScheduledDate    DATETIME NOT NULL,|
|`    `ScheduledTime    TIME NULL,|
|`    `CompletedDate    DATETIME NULL,|
|`    `Status           TINYINT NOT NULL DEFAULT 1,|
|`                     `-- 1=Pending 2=Completed 3=Missed 4=Overridden 5=Cancelled|
|`    `ContactMethod    TINYINT NULL, -- 1=Phone 2=WhatsApp 3=In-Person 4=Email|
|`    `Remarks          NVARCHAR(MAX) NULL,|
|`    `NextFollowUpDate DATETIME NULL,|
|`    `NextFollowUpNote NVARCHAR(500) NULL,|
|`    `-- Override fields|
|`    `OverriddenBy     INT NULL FK --> Users,|
|`    `OverrideReason   NVARCHAR(500) NULL,|
|`    `OverriddenDate   DATETIME NULL,|
|`    `-- Cancellation fields|
|`    `CancelledBy      INT NULL FK --> Users,|
|`    `CancelReason     NVARCHAR(500) NULL,|
|`    `CancelledDate    DATETIME NULL,|
|`    `CreatedBy        INT NOT NULL FK --> Users,|
|`    `CreatedDate      DATETIME DEFAULT GETDATE(),|
|`    `UpdatedBy        INT NULL,|
|`    `UpdatedDate      DATETIME NULL,|
|`    `CONSTRAINT CK\_FollowUps\_Date CHECK (ScheduledDate >= CAST(CreatedDate AS DATE)),|
|`    `CONSTRAINT CK\_FollowUps\_Override CHECK (|
|`        `(Status = 4 AND OverriddenBy IS NOT NULL AND OverrideReason IS NOT NULL)|
|`        `OR Status != 4),|
|`    `CONSTRAINT CK\_FollowUps\_Cancel CHECK (|
|`        `(Status = 5 AND CancelledBy IS NOT NULL AND CancelReason IS NOT NULL)|
|`        `OR Status != 5)|
|);|

## **6.2 FollowUpHistory Table**

|CREATE TABLE FollowUpHistory (|
| :- |
|`    `Id             INT IDENTITY(1,1) PRIMARY KEY,|
|`    `FollowUpId     INT NOT NULL FK --> FollowUps,|
|`    `LeadId         INT NOT NULL FK --> Leads,|
|`    `PreviousStatus TINYINT NOT NULL,|
|`    `NewStatus      TINYINT NOT NULL,|
|`    `ChangedBy      INT NOT NULL FK --> Users,|
|`    `ChangeReason   NVARCHAR(500) NULL,|
|`    `ChangedDate    DATETIME DEFAULT GETDATE()|
|);|

## **6.3 Follow-up State Machine**

|`  `+---------+|
| :- |
|`  `| PENDING | <-- Created|
|`  `+----+----+|
|`       `||
|`  `+----+--------------------+|
|`  `|                         ||
|`  `v (User completes)        v (EOD system job)|
|+-----------+          +--------+|
|| COMPLETED |          | MISSED ||
|+-----------+          +---+----+|
|`                           `| Manager Override|
|`                           `v|
|`                    `+------------+|
|`                    `| OVERRIDDEN ||
|`                    `+------------+|
||
|PENDING --> CANCELLED (Sr.Counsellor / Manager only)|

## **6.4 Scheduled Jobs**

|-- Job 1: Mark Missed (Daily 23:59)|
| :- |
|UPDATE FollowUps SET Status=3, UpdatedBy=0, UpdatedDate=GETDATE()|
|WHERE Status=1 AND CAST(ScheduledDate AS DATE) < CAST(GETDATE() AS DATE)|
|AND CompletedDate IS NULL;|
||
|-- Job 2: Unresponsive Check (After Mark Missed)|
|-- Reads threshold from SystemConfigurations: lead.unresponsive.missed\_count|
|-- If lead has >= threshold consecutive missed follow-ups with no completion:|
|-- UPDATE Leads SET Status=8 WHERE ...|
||
|-- Job 3: Due Today Reminder (Daily 08:00)|
|-- SELECT all FollowUps where Status=1 AND ScheduledDate=today|
|-- Dispatch: FOLLOWUP\_DUE\_TODAY | Channel: In-App + SMS|
||
|-- Job 4: Overdue 24h Manager Alert (Hourly)|
|-- SELECT missed follow-ups where DATEDIFF(HOUR, ScheduledDate, GETDATE()) >= 24|
|-- Dispatch: FOLLOWUP\_OVERDUE\_MANAGER | Channel: In-App + Email|

## **6.5 Follow-up Business Rules**

|**Rule ID**|**Rule**|
| :-: | :-: |
|BR-F01|ScheduledDate must be >= today at creation. Enforced by CHECK constraint.|
|BR-F02|Completing a follow-up requires Remarks. Minimum 10 characters at API level.|
|BR-F03|Only creator OR Senior Counsellor / Manager can complete a follow-up.|
|BR-F04|MISSED status is set ONLY by the system EOD job. No user can manually set Status=MISSED.|
|BR-F05|Override requires OverrideReason (min 20 chars). Role >= SeniorCounsellor.|
|BR-F06|After 3 consecutive missed follow-ups with no completion: Lead.Status -> Unresponsive.|
|BR-F07|A lead with terminal status cannot have a new follow-up scheduled.|
|BR-F08|Every status change is written to FollowUpHistory -- no exceptions.|
|BR-F09|Reschedule updates ScheduledDate only. Old and new dates logged in history.|


# **7. T-07: Counselling Session ERD & Workflow**

## **7.1 CounsellingSessions Table**

|CREATE TABLE CounsellingSessions (|
| :- |
|`    `Id                     INT IDENTITY(1,1) PRIMARY KEY,|
|`    `LeadId                 INT NOT NULL FK --> Leads,|
|`    `CounsellorId           INT NOT NULL FK --> Users,|
|`    `SessionDate            DATETIME DEFAULT GETDATE(),|
|`    `SessionType            TINYINT NOT NULL,|
|`                           `-- 1=Initial Assessment 2=Program Discussion|
|`                           `-- 3=Decision Session 4=Follow-up 5=Re-engagement|
|`    `-- Needs Assessment|
|`    `Budget                 DECIMAL(18,2) NULL,|
|`    `BudgetCurrency         NVARCHAR(10) DEFAULT 'BDT',|
|`    `PreferredCountryId     INT NULL FK --> Countries,|
|`    `PreferredDegreeLevelId INT NULL FK --> DegreeLevels,|
|`    `IELTSStatus            TINYINT NULL, -- 1=Given 2=Registered 3=Planning 4=NotReq 5=NotGiven|
|`    `IELTSScore             DECIMAL(3,1) NULL,|
|`    `IELTSExamDate          DATETIME NULL,|
|`    `DesiredIntake          NVARCHAR(100) NULL,|
|`    `-- Background|
|`    `AcademicBackground     NVARCHAR(MAX) NULL,|
|`    `WorkExperienceYears    INT NULL,|
|`    `HasGapYear             BIT NULL,|
|`    `GapYearReason          NVARCHAR(500) NULL,|
|`    `HasVisaRefusal         BIT NULL,|
|`    `VisaRefusalDetails     NVARCHAR(MAX) NULL,|
|`    `-- Output|
|`    `Notes                  NVARCHAR(MAX) NULL,|
|`    `NextAction             NVARCHAR(500) NULL,|
|`    `NextActionDate         DATETIME NULL,|
|`    `SessionOutcome         TINYINT NULL,|
|`                           `-- 1=Needs More Time 2=Program Shortlisted|
|`                           `-- 3=Decision Made 4=Not Interested 5=Deferred|
|`    `CreatedBy              INT NOT NULL,|
|`    `CreatedDate            DATETIME DEFAULT GETDATE(),|
|`    `UpdatedBy              INT NULL,|
|`    `UpdatedDate            DATETIME NULL,|
|`    `CONSTRAINT CK\_Session\_IELTS CHECK ((IELTSStatus=1 AND IELTSScore IS NOT NULL) OR IELTSStatus!=1),|
|`    `CONSTRAINT CK\_Session\_Gap   CHECK ((HasGapYear=1 AND GapYearReason IS NOT NULL) OR HasGapYear!=1),|
|`    `CONSTRAINT CK\_Session\_Visa  CHECK ((HasVisaRefusal=1 AND VisaRefusalDetails IS NOT NULL) OR HasVisaRefusal!=1)|
|);|

## **7.2 SessionProgramShortlist Table**

|CREATE TABLE SessionProgramShortlist (|
| :- |
|`    `Id            INT IDENTITY(1,1) PRIMARY KEY,|
|`    `SessionId     INT NOT NULL FK --> CounsellingSessions,|
|`    `LeadId        INT NOT NULL FK --> Leads,|
|`    `UniversityId  INT NOT NULL FK --> Universities,|
|`    `ProgramId     INT NOT NULL FK --> Programs,|
|`    `IntakeId      INT NULL FK --> Intakes,|
|`    `Priority      TINYINT DEFAULT 1, -- 1=First Choice 2=Second 3=Backup|
|`    `IsSelected    BIT DEFAULT 0,|
|`    `SelectionNote NVARCHAR(500) NULL,|
|`    `CreatedDate   DATETIME DEFAULT GETDATE(),|
|`    `CONSTRAINT UQ\_Shortlist UNIQUE (SessionId, ProgramId)|
|);|

## **7.3 Session Outcome to Lead Status Map**

|**SessionOutcome**|**Lead Status Change**|**Additional Action**|
| :-: | :-: | :-: |
|1 = Needs More Time|No change|NextActionDate required. Follow-up auto-created.|
|2 = Program Shortlisted|Interested|Shortlist must exist (min 1 program).|
|3 = Decision Made|Interested|IsSelected=1 must exist. Conversion gate unlocked.|
|4 = Not Interested|Not Interested|Reason in Notes required. Lead becomes non-active.|
|5 = Deferred|No change|NextActionDate required. Follow-up auto-created.|

## **7.4 Program Eligibility Filter Query**

|SELECT p.Id, p.Name, u.Name AS University,|
| :- |
|`       `i.Name AS Intake, i.ApplicationDeadline, cf.Amount AS TuitionFee|
|FROM Programs p|
|JOIN Universities u  ON u.Id = p.UniversityId|
|JOIN Intakes i       ON i.ProgramId = p.Id|
|JOIN CourseFees cf   ON cf.ProgramId = p.Id AND cf.IntakeId = i.Id AND cf.FeeType = 1|
|WHERE u.CountryId            = @PreferredCountryId|
|AND   p.DegreeLevelId        = @PreferredDegreeLevelId|
|AND   u.IsAuthorizedPartner  = 1 AND u.IsActive = 1 AND p.IsActive = 1|
|AND   i.Status               = 2  -- Open|
|AND   i.ApplicationDeadline  > GETDATE()|
|AND   cf.Amount              <= @Budget|
|AND   COALESCE(p.MinIELTSOverride, u.MinIELTSScore, 0) <= ISNULL(@IELTSScore, 0)|
|AND   (COALESCE(p.WorkExpOverride, u.WorkExpRequired, 0) = 0|
|`       `OR @WorkExperienceYears >= ISNULL(u.WorkExpYears, 0))|
|ORDER BY u.WorldRanking ASC, cf.Amount ASC;|

## **7.5 Session Business Rules**

|**Rule ID**|**Rule**|
| :-: | :-: |
|BR-C01|Session can only be created for a Lead whose status is not terminal (not 6,7,8,9).|
|BR-C02|SessionType is mandatory at creation.|
|BR-C03|If IELTSStatus=1 (Given), IELTSScore must be provided. API-level enforcement.|
|BR-C04|If HasGapYear=1, GapYearReason must be provided.|
|BR-C05|If HasVisaRefusal=1, VisaRefusalDetails must be provided.|
|BR-C06|Maximum 5 programs per session shortlist.|
|BR-C07|Only one program can have IsSelected=1 per Lead (across all sessions).|
|BR-C08|SessionOutcome=3 requires IsSelected=1 in shortlist.|
|BR-C09|SessionOutcome=1 or 5 requires NextActionDate. Auto follow-up created on save.|
|BR-C10|StudentAcademicProfiles is UPSERT on every session save.|


# **8. T-08: Student ERD & Academic Profile**

## **8.1 Students Table**

|CREATE TABLE Students (|
| :- |
|`    `Id                       INT IDENTITY(1,1) PRIMARY KEY,|
|`    `LeadId                   INT NULL UNIQUE FK --> Leads,|
|`    `BranchId                 INT NOT NULL FK --> Branches,|
|`    `ProcessingOfficerId      INT NULL FK --> Users,|
|`    `FullName                 NVARCHAR(150) NOT NULL,|
|`    `Phone                    NVARCHAR(20)  NOT NULL,|
|`    `Email                    NVARCHAR(150) NULL,|
|`    `DateOfBirth              DATE NULL,|
|`    `Gender                   TINYINT NULL, -- 1=Male 2=Female 3=Other|
|`    `NationalityCountryId     INT NULL FK --> Countries,|
|`    `PermanentAddress         NVARCHAR(500) NULL,|
|`    `EmergencyContactName     NVARCHAR(150) NULL,|
|`    `EmergencyContactPhone    NVARCHAR(20)  NULL,|
|`    `EmergencyContactRelation NVARCHAR(50)  NULL,|
|`    `-- Passport|
|`    `PassportNumber           NVARCHAR(50)  NULL,|
|`    `PassportExpiryDate       DATE NULL,|
|`    `PassportIssueDate        DATE NULL,|
|`    `PassportIssueCountry     NVARCHAR(100) NULL,|
|`    `-- Academic Preference|
|`    `PreferredCountryId       INT NULL FK --> Countries,|
|`    `PreferredDegreeLevelId   INT NULL FK --> DegreeLevels,|
|`    `DesiredIntake            NVARCHAR(100) NULL,|
|`    `-- English Proficiency|
|`    `IELTSStatus              TINYINT NULL,|
|`    `IELTSScore               DECIMAL(3,1) NULL,|
|`    `IELTSExamDate            DATE NULL,|
|`    `-- Status|
|`    `Status                   TINYINT NOT NULL DEFAULT 1,|
|`    `-- 1=Active 2=Application Ready 3=Applied 4=Offer Received|
|`    `-- 5=Visa In Process 6=Visa Approved 7=Visa Refused|
|`    `-- 8=Pre-Departure 9=Enrolled 10=Deferred 11=Withdrawn 12=Archived|
|`    `IsApplicationReady       BIT DEFAULT 0,|
|`    `ApplicationReadyDate     DATETIME NULL,|
|`    `ApplicationReadySetBy    INT NULL FK --> Users,|
|`    `-- Consent|
|`    `ConsentDataProcess       BIT DEFAULT 0,|
|`    `ConsentDataProcessDate   DATETIME NULL,|
|`    `ConsentSMS               BIT DEFAULT 0,|
|`    `ConsentEmail             BIT DEFAULT 0,|
|`    `ConsentWhatsApp          BIT DEFAULT 0,|
|`    `ConsentUniversityShare   BIT DEFAULT 0,|
|`    `IsDeleted                BIT DEFAULT 0,|
|`    `DeletedBy                INT NULL,|
|`    `DeletedDate              DATETIME NULL,|
|`    `CreatedBy                INT NOT NULL,|
|`    `CreatedDate              DATETIME DEFAULT GETDATE(),|
|`    `UpdatedBy                INT NULL,|
|`    `UpdatedDate              DATETIME NULL,|
|`    `CONSTRAINT UQ\_Students\_Phone UNIQUE (Phone) WHERE IsDeleted=0|
|);|

## **8.2 StudentAcademicProfiles Table (1:1 with Student)**

|CREATE TABLE StudentAcademicProfiles (|
| :- |
|`    `Id                  INT IDENTITY(1,1) PRIMARY KEY,|
|`    `StudentId           INT NOT NULL UNIQUE FK --> Students,|
|`    `-- SSC / O-Level|
|`    `SSCResult           NVARCHAR(20) NULL,|
|`    `SSCPassingYear      INT NULL,|
|`    `SSCBoard            NVARCHAR(100) NULL,|
|`    `SSCInstitution      NVARCHAR(200) NULL,|
|`    `SSCSubjectGroup     NVARCHAR(100) NULL,|
|`    `-- HSC / A-Level|
|`    `HSCResult           NVARCHAR(20) NULL,|
|`    `HSCPassingYear      INT NULL,|
|`    `HSCBoard            NVARCHAR(100) NULL,|
|`    `HSCInstitution      NVARCHAR(200) NULL,|
|`    `-- Undergraduate|
|`    `UGUniversityName    NVARCHAR(200) NULL,|
|`    `UGSubject           NVARCHAR(200) NULL,|
|`    `UGCGPA              DECIMAL(4,2)  NULL,|
|`    `UGPassingYear       INT NULL,|
|`    `UGCountry           NVARCHAR(100) NULL,|
|`    `-- Postgraduate (PhD applicants)|
|`    `PGUniversityName    NVARCHAR(200) NULL,|
|`    `PGCGPA              DECIMAL(4,2)  NULL,|
|`    `PGPassingYear       INT NULL,|
|`    `-- Work Experience|
|`    `WorkExperienceYears INT NULL,|
|`    `WorkExperienceField NVARCHAR(200) NULL,|
|`    `CurrentEmployer     NVARCHAR(200) NULL,|
|`    `-- Gap Year|
|`    `HasGapYear          BIT DEFAULT 0,|
|`    `GapYearFrom         INT NULL,|
|`    `GapYearTo           INT NULL,|
|`    `GapYearReason       NVARCHAR(500) NULL,|
|`    `-- Visa History|
|`    `HasPreviousVisa     BIT DEFAULT 0,|
|`    `PreviousVisaDetails NVARCHAR(MAX) NULL,|
|`    `HasVisaRefusal      BIT DEFAULT 0,|
|`    `VisaRefusalDetails  NVARCHAR(MAX) NULL,  -- PERMANENT, never deleted|
|`    `CreatedBy           INT NOT NULL,|
|`    `CreatedDate         DATETIME DEFAULT GETDATE(),|
|`    `UpdatedBy           INT NULL,|
|`    `UpdatedDate         DATETIME NULL|
|);|

## **8.3 StudentStatusHistory Table**

|CREATE TABLE StudentStatusHistory (|
| :- |
|`    `Id             INT IDENTITY(1,1) PRIMARY KEY,|
|`    `StudentId      INT NOT NULL FK --> Students,|
|`    `PreviousStatus TINYINT NOT NULL,|
|`    `NewStatus      TINYINT NOT NULL,|
|`    `ChangedBy      INT NOT NULL FK --> Users,  -- 0=System|
|`    `ChangeReason   NVARCHAR(500) NULL,|
|`    `ChangedDate    DATETIME DEFAULT GETDATE(),|
|`    `TriggerSource  TINYINT DEFAULT 1|
|`    `-- 1=Manual 2=System 3=Conversion 4=DocVerification 5=AppStatus 6=VisaOutcome|
|);|

## **8.4 Student Status State Machine**

|`  `ACTIVE(1) --> APPLICATION READY(2) [All mandatory docs Verified]|
| :- |
|`  `APPLICATION READY(2) --> APPLIED(3) [Application created]|
|`  `APPLIED(3) --> OFFER RECEIVED(4) [Any application gets offer]|
|`  `OFFER RECEIVED(4) --> VISA IN PROCESS(5) [Visa application created]|
|`  `VISA IN PROCESS(5) --> VISA APPROVED(6) [Visa outcome = Approved]|
|`  `VISA IN PROCESS(5) --> VISA REFUSED(7) [Visa outcome = Refused]|
|`  `VISA APPROVED(6) --> PRE-DEPARTURE(8) [Departure date confirmed]|
|`  `PRE-DEPARTURE(8) --> ENROLLED(9) [Arrival + enrollment confirmed]|
|`  `VISA REFUSED(7) --> ACTIVE(1) [Re-engagement decision made]|
|`  `Any non-terminal --> WITHDRAWN(11) [Manager / Sr.Counsellor]|
|`  `Any --> ARCHIVED(12) [Manager only]|
||
|`  `Student.Status always reflects BEST status across all applications.|

## **8.5 Student Business Rules**

|**Rule ID**|**Rule**|
| :-: | :-: |
|BR-S01|Phone must be unique across all non-deleted Students. Filtered unique index.|
|BR-S02|LeadId must be unique in Students. One Lead produces only one Student.|
|BR-S03|StudentAcademicProfiles created automatically at conversion. 1:1, cannot be deleted.|
|BR-S04|StudentDocumentChecklists auto-generated at conversion using CountryDocumentRequirements.|
|BR-S05|IsApplicationReady set automatically when all required checklist items = Verified or Waived.|
|BR-S06|Document waiver requires Manager role. WaivedReason mandatory.|
|BR-S07|Student personal info locked for editing at Status >= 5 without Manager approval.|
|BR-S08|Visa refusal history is PERMANENT. Cannot be deleted or archived separately.|
|BR-S09|Branch transfer requires approval from BOTH branch managers. Finance stays in origin branch.|
|BR-S10|Withdrawn student's active applications are automatically set to Withdrawn status.|
|BR-S11|Visa Refused student can be re-engaged. Status -> Active. New counselling session required.|
|BR-S12|Every Student status change recorded in StudentStatusHistory -- no exceptions.|


# **9. T-09: Conversion Gate — Full Validation Sequence**
The Conversion Gate runs server-side in 7 layers when 'Convert to Student' is submitted. Every layer runs at API level. The UI cannot bypass any layer.

## **9.1 API Contract**

|POST /api/leads/{leadId}/convert|
| :- |
|Body: {|
|`  `confirmedWarnings: [],   // Warning codes acknowledged by user|
|`  `consentDataProcess: true,|
|`  `consentSMS: true,|
|`  `consentEmail: true,|
|`  `consentWhatsApp: true,|
|`  `consentUniversityShare: true|
|}|
||
|Responses:|
|`  `HTTP 400 --> Pre-flight failure|
|`  `HTTP 422 --> Hard block (HARD\_BLOCK type) or unconfirmed warnings|
|`  `HTTP 200 type=SOFT\_WARNING  --> Warnings shown, user must re-submit with confirmedWarnings|
|`  `HTTP 200 type=RETURNING\_STUDENT --> Existing student found, redirect|
|`  `HTTP 200 type=CONVERTED --> Success, studentId returned|

## **9.2 Layer 1 — Pre-flight Checks (HTTP 400 on failure)**

|**Code**|**Check**|**Failure Message**|
| :-: | :-: | :-: |
|PF-1|LeadId exists in database|Lead not found.|
|PF-2|Lead.IsDeleted = 0|Lead has been deleted.|
|PF-3|Lead.BranchId = currentUser.BranchId|Access denied. Lead belongs to another branch.|
|PF-4|Current user has Convert permission|Insufficient permissions.|
|PF-5|Lead.Status NOT IN (6,7,8,9) — not terminal|Lead is already converted or closed.|
|PF-6|Lead.Status = 4 (Interested)|Lead must be Interested to convert.|
|PF-7|ConsentDataProcess = true in request body|Student consent to data processing is required.|

## **9.3 Layer 2 — Hard Block Validations (HTTP 422, all failures collected)**

|**Code**|**Check**|**Query Pattern**|
| :-: | :-: | :-: |
|HB-01|Phone not null|Lead.Phone IS NOT NULL AND LEN(TRIM(Phone)) > 0|
|HB-02|Min 1 counselling session|COUNT(\*) FROM CounsellingSessions WHERE LeadId=X >= 1|
|HB-03|Preferred country selected|Lead.InterestedCountryId IS NOT NULL OR Session.PreferredCountryId IS NOT NULL|
|HB-04|Preferred degree level selected|Lead.InterestedDegreeLevel IS NOT NULL OR Session.PreferredDegreeLevelId IS NOT NULL|
|HB-05|Budget captured in at least one session|SELECT TOP 1 Budget FROM CounsellingSessions WHERE LeadId=X AND Budget IS NOT NULL|
|HB-06|IELTS/English status captured|SELECT TOP 1 IELTSStatus FROM CounsellingSessions WHERE LeadId=X AND IELTSStatus IS NOT NULL|
|HB-07|Academic background entered (min SSC result)|SELECT Id FROM StudentAcademicProfiles WHERE LeadId=X AND SSCResult IS NOT NULL|
|HB-08|Min 1 program shortlisted|COUNT(\*) FROM SessionProgramShortlist JOIN CounsellingSessions WHERE LeadId=X >= 1|
|HB-09|Session with Decision Made (Outcome=3) exists|COUNT(\*) FROM CounsellingSessions WHERE LeadId=X AND SessionOutcome=3 >= 1|

## **9.4 Layer 3 — Soft Warning Evaluations (HTTP 200 SOFT\_WARNING)**

|**Code**|**Check**|**Warning Message**|
| :-: | :-: | :-: |
|SW-01|Passport availability not captured|Passport not confirmed. Checklist may be incomplete.|
|SW-02|Visa history not captured (HasVisaRefusal IS NULL)|Visa refusal history not recorded. Affects visa strategy.|
|SW-03|Student photo not collected|Student photo not uploaded. Required for some applications.|
|SW-04|No completed follow-ups exist|No follow-up has been marked Completed for this lead.|
|SW-05|Budget vs fee mismatch (Fee > Budget \* 1.2)|Student budget may not cover selected program tuition fee.|
|SW-06|IELTS score below program minimum|IELTS score below minimum required by selected university.|
|SW-07|Desired intake deadline within 30 days|Intake deadline within 30 days. Document collection urgency.|

## **9.5 Layer 4 — Duplicate Student Check**

|SELECT Id, FullName, Status FROM Students|
| :- |
|WHERE Phone = @LeadPhone AND IsDeleted = 0|
||
|CASE A: No record --> Proceed to Layer 5|
|CASE B: Record found, Status=ARCHIVED(12) --> Return ARCHIVED\_STUDENT\_EXISTS|
|`        `UI prompts Manager to reactivate existing profile|
|CASE C: Record found, Status != ARCHIVED --> Return RETURNING\_STUDENT|
|`        `Redirect to existing student profile. Do NOT create duplicate.|

## **9.6 Layer 5 — Already Converted Check**

|SELECT Id FROM Students WHERE LeadId=@LeadId AND IsDeleted=0|
| :- |
|IF record found --> HTTP 409 ALREADY\_CONVERTED|
|-- Data integrity guard. Should not occur if Layer 1 status check passed.|

## **9.7 Layer 6 — Transaction Execution**

|BEGIN TRANSACTION|
| :- |
|`  `Step 1: Read latest CounsellingSession data|
|`  `Step 2: INSERT Students (from Lead + Session data)|
|`  `Step 3: INSERT StudentAcademicProfiles (from AcademicProfiles where LeadId=X)|
|`  `Step 4: INSERT StudentDocumentChecklists|
|`          `SELECT DocumentTypeId, IsRequired FROM CountryDocumentRequirements|
|`          `WHERE CountryId=@CountryId AND DegreeLevelId=@DegreeLevelId|
|`  `Step 5: ADD SOP if University.SOPRequired=1 (not already in checklist)|
|`  `Step 6: ADD Reference Letters if University.ReferenceLettersCount >= 1 or 2|
|`  `Step 7: UPDATE Leads SET Status=6 (Converted)|
|`  `Step 8: INSERT StudentStatusHistory (PreviousStatus=0, NewStatus=1, TriggerSource=3)|
|`  `Step 9: Log confirmed warnings to AuditLogs (if any SW-xx acknowledged)|
|COMMIT TRANSACTION|
|-- If any step fails: ROLLBACK TRANSACTION|

## **9.8 Layer 7 — Post-Conversion Triggers (fire AFTER commit)**

|**#**|**Trigger**|**Recipient**|**Channel**|
| :-: | :-: | :-: | :-: |
|1|Lead converted notification|Branch Manager|In-App|
|2|New student created notification|Sr. Counsellor|In-App|
|3|Assign Processing Officer (action req)|Branch Manager|In-App + Email|
|4|Create audit log entry (CONVERSION)|System -- AuditLogs table|DB|
|5|Update lead pipeline metrics|System -- background job|DB|
|6|Tag Student with AgentId if AgentLead exists|System -- automatic|DB|

|<p>**Layer 7 Failure Policy**</p><p>CRITICAL: Triggers in Layer 7 fire AFTER the transaction commits.</p><p>Their failure does NOT rollback the conversion.</p><p>Failed triggers are logged for retry.</p><p>The conversion is considered successful once Layer 6 commits.</p>|
| :- |

## **9.9 Conversion Gate Business Rules**

|**Rule ID**|**Rule**|
| :-: | :-: |
|BR-G01|All Layer 1 and Layer 2 checks run at API level. Direct API calls cannot bypass them.|
|BR-G02|Soft warnings must be explicitly confirmed in request body. Confirmed codes stored in AuditLogs.|
|BR-G03|Duplicate phone detection runs against Students table, not just Leads table.|
|BR-G04|Entire conversion is one database transaction. Any failure triggers full rollback.|
|BR-G05|Post-conversion triggers fire after commit. Failure does not reverse conversion.|
|BR-G06|Agent lead is auto-tagged to Student at conversion for commission linking.|
|BR-G07|Warning acknowledgment codes permanently stored in AuditLogs against new StudentId.|


# **10. T-10: Document ERD & State Machine & Checklist Logic**

## **10.1 Documents Table**

|CREATE TABLE Documents (|
| :- |
|`    `Id                  INT IDENTITY(1,1) PRIMARY KEY,|
|`    `StudentId           INT NOT NULL FK --> Students,|
|`    `DocumentTypeId      INT NOT NULL FK --> DocumentTypes,|
|`    `ChecklistItemId     INT NULL FK --> StudentDocumentChecklists,|
|`    `-- File Info|
|`    `OriginalFileName    NVARCHAR(300) NOT NULL,|
|`    `StoredFileName      NVARCHAR(300) NOT NULL,  -- GUID-based|
|`    `FilePath            NVARCHAR(500) NOT NULL,|
|`    `FileExtension       NVARCHAR(10)  NOT NULL,|
|`    `FileSizeMB          DECIMAL(6,2)  NOT NULL,|
|`    `MimeType            NVARCHAR(100) NOT NULL,|
|`    `ExpiryDate          DATE NULL,|
|`    `DocumentDate        DATE NULL,|
|`    `IssuingAuthority    NVARCHAR(200) NULL,|
|`    `-- Status|
|`    `Status              TINYINT NOT NULL DEFAULT 1,|
|`                        `-- 1=Submitted 2=Under Review 3=Verified|
|`                        `-- 4=Rejected 5=Expired 6=Superseded|
|`    `UploadedBy          INT NOT NULL FK --> Users,|
|`    `VerifiedBy          INT NULL FK --> Users,|
|`    `VerifiedDate        DATETIME NULL,|
|`    `RejectionCount      INT DEFAULT 0,|
|`    `RejectionReason     NVARCHAR(MAX) NULL,|
|`    `RejectionCode       NVARCHAR(50)  NULL,|
|`    `-- Version Control|
|`    `Version             INT DEFAULT 1,|
|`    `PreviousDocumentId  INT NULL FK --> Documents,  -- self-reference|
|`    `IsDeleted           BIT DEFAULT 0,|
|`    `DeletedBy           INT NULL,|
|`    `DeletedDate         DATETIME NULL,|
|`    `CreatedDate         DATETIME DEFAULT GETDATE(),|
|`    `UpdatedBy           INT NULL,|
|`    `UpdatedDate         DATETIME NULL,|
|`    `-- Separation of Duty: uploader cannot be verifier|
|`    `CONSTRAINT CK\_Docs\_SeparationOfDuty CHECK (VerifiedBy IS NULL OR VerifiedBy != UploadedBy),|
|`    `CONSTRAINT CK\_Docs\_Rejection CHECK ((Status=4 AND RejectionReason IS NOT NULL) OR Status!=4),|
|`    `CONSTRAINT CK\_Docs\_Verified CHECK (|
|`        `(Status=3 AND VerifiedBy IS NOT NULL AND VerifiedDate IS NOT NULL) OR Status!=3)|
|);|

## **10.2 DocumentVerificationHistory Table**

|CREATE TABLE DocumentVerificationHistory (|
| :- |
|`    `Id             INT IDENTITY(1,1) PRIMARY KEY,|
|`    `DocumentId     INT NOT NULL FK --> Documents,|
|`    `StudentId      INT NOT NULL FK --> Students,|
|`    `ActionType     TINYINT NOT NULL,|
|`                   `-- 1=Submitted 2=Review Started 3=Verified 4=Rejected|
|`                   `-- 5=Expired 6=Superseded 7=Deleted 8=Separation Override|
|`    `PreviousStatus TINYINT NOT NULL,|
|`    `NewStatus      TINYINT NOT NULL,|
|`    `ActorId        INT NOT NULL, -- 0=System|
|`    `Notes          NVARCHAR(MAX) NULL,|
|`    `RejectionCode  NVARCHAR(50) NULL,|
|`    `ActionDate     DATETIME DEFAULT GETDATE()|
|);|

## **10.3 StudentDocumentChecklists Table**

|CREATE TABLE StudentDocumentChecklists (|
| :- |
|`    `Id             INT IDENTITY(1,1) PRIMARY KEY,|
|`    `StudentId      INT NOT NULL FK --> Students,|
|`    `DocumentTypeId INT NOT NULL FK --> DocumentTypes,|
|`    `IsRequired     BIT DEFAULT 1,|
|`    `Status         TINYINT DEFAULT 1,|
|`                   `-- 1=Missing 2=Submitted 3=Verified 4=Rejected 5=Expired 6=Waived|
|`    `WaivedBy       INT NULL FK --> Users,|
|`    `WaivedReason   NVARCHAR(500) NULL,|
|`    `WaivedDate     DATETIME NULL,|
|`    `Notes          NVARCHAR(500) NULL,|
|`    `CreatedDate    DATETIME DEFAULT GETDATE(),|
|`    `UpdatedDate    DATETIME NULL,|
|`    `CONSTRAINT UQ\_DocChecklist UNIQUE (StudentId, DocumentTypeId),|
|`    `CONSTRAINT CK\_DocChecklist\_Waived CHECK (|
|`        `(Status=6 AND WaivedBy IS NOT NULL AND WaivedReason IS NOT NULL) OR Status!=6)|
|);|

## **10.4 Document State Machine**

|`  `SUBMITTED(1)|
| :- |
|`    `--> UNDER REVIEW(2)  [Processing Officer opens document]|
|`    `--> REJECTED(4)      [Processing Officer rejects directly]|
|`  `UNDER REVIEW(2)|
|`    `--> VERIFIED(3)      [Processing Officer verifies. VerifiedBy != UploadedBy enforced.]|
|`    `--> REJECTED(4)      [Processing Officer rejects. RejectionCode + Reason mandatory.]|
|`  `REJECTED(4)|
|`    `--> (Student re-uploads) --> new SUBMITTED row, previous row --> SUPERSEDED(6)|
|`  `VERIFIED(3)|
|`    `--> EXPIRED(5)       [System job: ExpiryDate < today]|
|`    `--> SUPERSEDED(6)    [Manager override: new version uploaded]|

## **10.5 Upload Validation Sequence**

|Step 1: Student check -- IsDeleted=0, Status not in (11,12)|
| :- |
|Step 2: DocumentType check -- IsActive=1, get AcceptedFormats, MaxFileSizeMB|
|Step 3: File validation -- extension in AcceptedFormats, size <= max|
|Step 4: Expiry check -- if HasExpiryDate=1: ExpiryDate required, must be future|
|Step 5: Existing document check -- if Status=Verified exists, show warning|
|Step 6: BEGIN TRANSACTION|
|`          `If previous doc exists: UPDATE previous SET Status=Superseded|
|`          `INSERT Documents (Version=previous.Version+1, PreviousDocumentId=previous.Id)|
|`          `UPDATE StudentDocumentChecklists SET Status=Submitted|
|`          `INSERT DocumentVerificationHistory (ActionType=1, Submitted)|
|`        `COMMIT|

## **10.6 Application-Ready Check (runs after every doc status change)**

|SELECT @Total=COUNT(\*), @Satisfied=SUM(CASE WHEN Status IN (3,6) THEN 1 ELSE 0 END)|
| :- |
|FROM StudentDocumentChecklists|
|WHERE StudentId=@StudentId AND IsRequired=1;|
||
|IF @Total > 0 AND @Total = @Satisfied|
|BEGIN|
|`    `UPDATE Students SET IsApplicationReady=1, Status=2,|
|`           `ApplicationReadyDate=GETDATE(), ApplicationReadySetBy=0|
|`    `WHERE Id=@StudentId AND IsApplicationReady=0;|
|`    `INSERT StudentStatusHistory (...TriggerSource=4);|
|`    `-- Notify Processing Officer + Counsellor|
|END|
|ELSE IF IsApplicationReady was 1 but now condition fails|
|BEGIN|
|`    `UPDATE Students SET IsApplicationReady=0, Status=1;|
|`    `INSERT StudentStatusHistory (...'Doc expired or removed');|
|END|

## **10.7 Document Rejection Escalation Job**

|-- Runs after every rejection INSERT|
| :- |
|-- Reads threshold from SystemConfigurations: doc.max\_rejection\_count (default 3)|
|SELECT d.Id, d.StudentId, d.RejectionCount, s.BranchId|
|FROM Documents d JOIN Students s ON s.Id=d.StudentId|
|WHERE d.RejectionCount >= @Threshold AND d.Status=4 AND d.IsDeleted=0;|
|-- Dispatch: DOCUMENT\_REJECTION\_ESCALATION | Channel: In-App + Email | Recipient: Manager|

## **10.8 Document Business Rules**

|**Rule ID**|**Rule**|
| :-: | :-: |
|BR-D01|UploadedBy and VerifiedBy must be different users. DB CHECK constraint + API enforcement.|
|BR-D02|Manager can override separation of duty. Override reason mandatory. Logged in AuditLogs.|
|BR-D03|If DocumentType.HasExpiryDate=1, ExpiryDate is required at upload. Past dates blocked.|
|BR-D04|Re-uploading supersedes previous version. Previous row -> Superseded. Version incremented.|
|BR-D05|RejectionCode is mandatory on rejection (from DocumentRejectionCodes table).|
|BR-D06|After 3 rejections of same document, automatic escalation to Branch Manager.|
|BR-D07|Verified document that expires is auto-set to Expired by system job. Checklist synced.|
|BR-D08|Application-Ready check runs after every document status change.|
|BR-D09|If verified doc expires after Application-Ready was set, IsApplicationReady is reset to 0.|
|BR-D10|Document delete is soft delete only. Manager role required.|
|BR-D11|Waived document counts as satisfied in Application-Ready check. Manager + reason required.|
|BR-D12|Every document status change recorded in DocumentVerificationHistory. ActorId=0 for system.|


# **11. T-11: Application ERD & State Machine**

## **11.1 Applications Table**

|CREATE TABLE Applications (|
| :- |
|`    `Id                      INT IDENTITY(1,1) PRIMARY KEY,|
|`    `StudentId               INT NOT NULL FK --> Students,|
|`    `BranchId                INT NOT NULL FK --> Branches,|
|`    `ProcessingOfficerId     INT NOT NULL FK --> Users,|
|`    `CountryId               INT NOT NULL FK --> Countries,|
|`    `UniversityId            INT NOT NULL FK --> Universities,|
|`    `ProgramId               INT NOT NULL FK --> Programs,|
|`    `IntakeId                INT NOT NULL FK --> Intakes,|
|`    `-- Snapshots (locked at creation time)|
|`    `UniversityNameSnapshot  NVARCHAR(200) NOT NULL,|
|`    `ProgramNameSnapshot     NVARCHAR(200) NOT NULL,|
|`    `IntakeNameSnapshot      NVARCHAR(100) NOT NULL,|
|`    `DegreeLevelSnapshot     NVARCHAR(100) NOT NULL,|
|`    `FeeSnapshot             NVARCHAR(MAX) NULL,  -- JSON array of fee objects|
|`    `CommissionRateSnapshot  DECIMAL(5,2)  NULL,|
|`    `-- Reference Numbers|
|`    `InternalRefNo           NVARCHAR(50) NOT NULL,  -- APP-DHK-2026-00047|
|`    `UniversityRefNo         NVARCHAR(100) NULL,|
|`    `-- Status|
|`    `Status                  TINYINT NOT NULL DEFAULT 1,|
|`    `-- 1=Draft 2=Internal Review 3=Submitted 4=Under Review by Uni|
|`    `-- 5=Conditional Offer 6=Unconditional Offer 7=Enrolled|
|`    `-- 8=Rejected 9=Withdrawn 10=Waitlisted 11=Deferred|
|`    `Priority                TINYINT DEFAULT 2, -- 1=First 2=Second 3=Backup|
|`    `-- Key Dates|
|`    `AppliedDate             DATETIME NULL,|
|`    `OfferReceivedDate       DATETIME NULL,|
|`    `EnrollmentDate          DATETIME NULL,|
|`    `WithdrawnDate           DATETIME NULL,|
|`    `-- Offer Details|
|`    `OfferLetterPath         NVARCHAR(500) NULL,|
|`    `OfferLetterType         TINYINT NULL, -- 1=Conditional 2=Unconditional|
|`    `OfferExpiryDate         DATE NULL,|
|`    `-- Withdrawal / Rejection|
|`    `WithdrawalReason        NVARCHAR(MAX) NULL,|
|`    `WithdrawalInitiatedBy   TINYINT NULL, -- 1=Student 2=Agency 3=University|
|`    `RejectionReason         NVARCHAR(MAX) NULL,|
|`    `-- Portal Credentials (encrypted)|
|`    `PortalUsername          NVARCHAR(150) NULL,|
|`    `PortalPassword          NVARCHAR(150) NULL,|
|`    `InternalNotes           NVARCHAR(MAX) NULL,|
|`    `IsDeleted               BIT DEFAULT 0,|
|`    `DeletedBy               INT NULL,|
|`    `DeletedDate             DATETIME NULL,|
|`    `CreatedBy               INT NOT NULL,|
|`    `CreatedDate             DATETIME DEFAULT GETDATE(),|
|`    `UpdatedBy               INT NULL,|
|`    `UpdatedDate             DATETIME NULL,|
|`    `CONSTRAINT UQ\_App\_Student\_Program\_Intake|
|`        `UNIQUE (StudentId, ProgramId, IntakeId) WHERE IsDeleted=0,|
|`    `CONSTRAINT CK\_App\_Withdrawal CHECK (|
|`        `(Status=9 AND WithdrawalReason IS NOT NULL AND WithdrawnDate IS NOT NULL) OR Status!=9),|
|`    `CONSTRAINT CK\_App\_Rejection CHECK (|
|`        `(Status=8 AND RejectionReason IS NOT NULL) OR Status!=8)|
|);|

## **11.2 ApplicationConditions Table**

|CREATE TABLE ApplicationConditions (|
| :- |
|`    `Id              INT IDENTITY(1,1) PRIMARY KEY,|
|`    `ApplicationId   INT NOT NULL FK --> Applications,|
|`    `ConditionText   NVARCHAR(MAX) NOT NULL,|
|`    `ConditionType   TINYINT NOT NULL,|
|`                    `-- 1=IELTS Score 2=Additional Document|
|`                    `-- 3=Academic 4=Financial 5=Other|
|`    `Status          TINYINT DEFAULT 1, -- 1=Pending 2=Met 3=Waived by University|
|`    `DueDate         DATE NULL,|
|`    `ResolvedDate    DATETIME NULL,|
|`    `ResolvedBy      INT NULL FK --> Users,|
|`    `ResolvedNotes   NVARCHAR(500) NULL,|
|`    `CreatedBy       INT NOT NULL,|
|`    `CreatedDate     DATETIME DEFAULT GETDATE(),|
|`    `UpdatedBy       INT NULL,|
|`    `UpdatedDate     DATETIME NULL|
|);|

## **11.3 ApplicationDocuments Table**

|CREATE TABLE ApplicationDocuments (|
| :- |
|`    `Id              INT IDENTITY(1,1) PRIMARY KEY,|
|`    `ApplicationId   INT NOT NULL FK --> Applications,|
|`    `DocumentId      INT NOT NULL FK --> Documents,|
|`    `DocumentTypeId  INT NOT NULL FK --> DocumentTypes,|
|`    `AddedBy         INT NOT NULL FK --> Users,|
|`    `AddedDate       DATETIME DEFAULT GETDATE(),|
|`    `IsRequired      BIT DEFAULT 1,|
|`    `Notes           NVARCHAR(300) NULL,|
|`    `CONSTRAINT UQ\_AppDoc\_DocType UNIQUE (ApplicationId, DocumentTypeId)|
|);|

## **11.4 Application State Machine**

|`  `DRAFT(1) --> INTERNAL REVIEW(2) --> SUBMITTED TO UNIVERSITY(3)|
| :- |
|`  `DRAFT(1) --> SUBMITTED TO UNIVERSITY(3)  [direct, skip internal review]|
|`  `SUBMITTED(3) --> UNDER REVIEW BY UNI(4)|
|`  `UNDER REVIEW(4) --> CONDITIONAL OFFER(5)   [offer letter + conditions entered]|
|`  `UNDER REVIEW(4) --> UNCONDITIONAL OFFER(6)  [direct unconditional]|
|`  `UNDER REVIEW(4) --> WAITLISTED(10)|
|`  `UNDER REVIEW(4) --> REJECTED(8)             [terminal - negative]|
|`  `CONDITIONAL OFFER(5) --> UNCONDITIONAL OFFER(6) [all conditions Met/Waived]|
|`  `UNCONDITIONAL OFFER(6) --> ENROLLED(7)      [terminal - positive]|
|`  `UNCONDITIONAL OFFER(6) --> DEFERRED(11)|
|`  `Any non-terminal --> WITHDRAWN(9)           [terminal - negative]|
||
|`  `Student.Status always = BEST status across all active applications.|

## **11.5 Application Creation Validation Sequence**

|**Step**|**Check**|**Failure Action**|
| :-: | :-: | :-: |
|1|Student.IsApplicationReady=1 AND Status not in (11,12) AND IsDeleted=0|HTTP 422 -- Student not application ready|
|2|Student.ConsentUniversityShare=1|HTTP 422 -- Consent required|
|3|Intake.Status=Open(2) AND Deadline > GETDATE()|HTTP 422 -- Intake closed or deadline passed|
|4|University.IsAuthorizedPartner=1 AND PartnershipExpiryDate > today|HTTP 422 -- Partnership expired|
|5|Program.IsActive=1 AND Program.UniversityId=@UniversityId|HTTP 422 -- Program invalid|
|6|No duplicate (StudentId+ProgramId+IntakeId, Status not Rejected/Withdrawn)|HTTP 422 -- Duplicate application|
|7|Snapshot fee structure: JSON serialize CourseFees for ProgramId+IntakeId|Store in FeeSnapshot|
|8|Snapshot commission rate from University.CommissionRate|Store in CommissionRateSnapshot|
|9|Generate InternalRefNo: APP-{BranchCode}-{YYYY}-{Seq:00000}|Auto-generated|

## **11.6 Conditional to Unconditional Upgrade Flow**

|Step 1: Conditional offer received --> Status=5, conditions entered in ApplicationConditions|
| :- |
|Step 2: Per condition: Processing Officer marks Status=Met or University Waives=3|
|Step 3: After each condition update:|
|`        `SELECT @Total=COUNT(\*), @Resolved=SUM(CASE WHEN Status IN (2,3) THEN 1 ELSE 0 END)|
|`        `FROM ApplicationConditions WHERE ApplicationId=@AppId|
|`        `IF @Total=@Resolved --> Unlock 'Upgrade to Unconditional' button|
|Step 4: Processing Officer uploads unconditional offer letter|
|`        `Status --> Unconditional Offer (6)|
|`        `Notify: Counsellor + Manager + Accounts|

## **11.7 Application Business Rules**

|**Rule ID**|**Rule**|
| :-: | :-: |
|BR-A01|Application only created if Student.IsApplicationReady=1. API enforced.|
|BR-A02|Duplicate application (same Student+Program+Intake) blocked. Filtered unique index.|
|BR-A03|University must be active authorized partner at application creation time.|
|BR-A04|Intake must be Open (Status=2) and ApplicationDeadline must be in the future.|
|BR-A05|Fee structure and commission rate snapshotted at creation. Master changes don't affect existing apps.|
|BR-A06|Student.ConsentUniversityShare=1 required before submitting to university.|
|BR-A07|Conditional->Unconditional upgrade requires ALL conditions to be Met or Waived.|
|BR-A08|Enrollment requires: EnrollmentDate + enrollment letter + tuition deposit payment recorded.|
|BR-A09|On Withdrawal: if last active app, Student.Status reset to Active(1).|
|BR-A10|Application rejection triggers suggestion to schedule new counselling session.|
|BR-A11|Every application status change recorded in ApplicationStatusHistory -- no exceptions.|
|BR-A12|PortalPassword stored encrypted. Never returned as plain text. Audit log on every access.|
|BR-A13|Commission record auto-created when Application.Status -> Enrolled.|


# **12. T-12: Application Conditions Workflow**

## **12.1 Condition State Machine**

|`  `PENDING(1)|
| :- |
|`    `--> MET(2)    [Student fulfills condition. Processing Officer marks resolved.]|
|`    `--> WAIVED(3) [University waives condition. Waiver evidence upload recommended.]|
||
|`  `Note: There is no REJECTED status on conditions.|
|`  `If condition cannot be met: Application --> Withdrawn by student.|

## **12.2 All Conditions Resolved Check**

|PROCEDURE CheckAllConditionsResolved @ApplicationId INT|
| :- |
|BEGIN|
|`  `DECLARE @Total INT, @Resolved INT;|
|`  `SELECT @Total=COUNT(\*),|
|`         `@Resolved=SUM(CASE WHEN Status IN (2,3) THEN 1 ELSE 0 END)|
|`  `FROM ApplicationConditions WHERE ApplicationId=@ApplicationId;|
||
|`  `IF @Total > 0 AND @Total = @Resolved|
|`  `BEGIN|
|`    `-- Unlock 'Upgrade to Unconditional' in UI|
|`    `-- Notify: Processing Officer (In-App: CONDITIONS\_ALL\_RESOLVED)|
|`  `END|
|END|

## **12.3 Condition Business Rules**

|**Rule ID**|**Rule**|
| :-: | :-: |
|BR-C01|Conditions created only when Application.Status=Conditional Offer(5).|
|BR-C02|Minimum 1 condition must exist when status is Conditional Offer. Empty list is invalid.|
|BR-C03|ALL conditions must be Met or Waived before upgrade to Unconditional. System enforced.|
|BR-C04|Condition DueDate is optional but recommended. System alerts Processing Officer 7 days before.|
|BR-C05|Waived conditions recommend evidence upload. Not a hard block but tracked.|


# **13. T-13: Scholarship ERD**

## **13.1 ScholarshipApplications Table**

|CREATE TABLE ScholarshipApplications (|
| :- |
|`    `Id              INT IDENTITY(1,1) PRIMARY KEY,|
|`    `ApplicationId   INT NOT NULL FK --> Applications,|
|`    `ScholarshipName NVARCHAR(200) NOT NULL,|
|`    `DiscountType    TINYINT NOT NULL, -- 1=Percentage 2=Fixed Amount|
|`    `DiscountValue   DECIMAL(10,2) NOT NULL,|
|`    `Currency        NVARCHAR(10) NULL,|
|`    `Status          TINYINT DEFAULT 1, -- 1=Applied 2=Granted 3=Rejected|
|`    `AwardedDate     DATE NULL,|
|`    `ExpiryDate      DATE NULL,|
|`    `LetterPath      NVARCHAR(500) NULL,|
|`    `Remarks         NVARCHAR(MAX) NULL,|
|`    `CreatedBy       INT NOT NULL,|
|`    `CreatedDate     DATETIME DEFAULT GETDATE(),|
|`    `UpdatedBy       INT NULL,|
|`    `UpdatedDate     DATETIME NULL|
|);|

## **13.2 Scholarship Commission Impact**

|When ScholarshipApplications.Status -> Granted (2):|
| :- |
||
|NetTuitionFee =|
|`  `IF DiscountType=1 (Percentage):|
|`    `FeeSnapshot.TuitionAmount \* (1 - DiscountValue / 100)|
|`  `IF DiscountType=2 (Fixed):|
|`    `FeeSnapshot.TuitionAmount - DiscountValue|
||
|Commission.UniversityAmount = NetTuitionFee \* CommissionRateSnapshot / 100|
|Commission.AdjustedBaseAmount = NetTuitionFee|
|Commission.ScholarshipApplied = 1|
|Commission.ScholarshipAmount  = DiscountValue|

## **13.3 Scholarship Business Rules**

|**Rule ID**|**Rule**|
| :-: | :-: |
|BR-SCH01|Only one scholarship record per Application at a time. Multiple require Manager approval.|
|BR-SCH02|Granted scholarship automatically triggers recalculation of Commission.UniversityAmount.|
|BR-SCH03|Scholarship letter upload is mandatory when Status -> Granted.|


# **14. T-14: Visa ERD & State Machine**

## **14.1 VisaApplications Table**

|CREATE TABLE VisaApplications (|
| :- |
|`    `Id                       INT IDENTITY(1,1) PRIMARY KEY,|
|`    `ApplicationId            INT NOT NULL UNIQUE FK --> Applications,|
|`    `StudentId                INT NOT NULL FK --> Students,|
|`    `CountryId                INT NOT NULL FK --> Countries,|
|`    `ProcessingOfficerId      INT NOT NULL FK --> Users,|
|`    `-- Visa Reference|
|`    `VisaRefNo                NVARCHAR(100) NULL,|
|`    `EmbassyPortalUsername    NVARCHAR(150) NULL,|
|`    `EmbassyPortalPassword    NVARCHAR(150) NULL,  -- Encrypted|
|`    `-- Status|
|`    `Status                   TINYINT NOT NULL DEFAULT 1,|
|`    `-- 1=Preparation 2=Submitted to Embassy 3=Under Review|
|`    `-- 4=Additional Docs Requested 5=Interview Scheduled|
|`    `-- 6=Approved 7=Refused 8=Withdrawn 9=Deferred|
|`    `-- Key Dates|
|`    `SubmittedDate            DATETIME NULL,|
|`    `InterviewDate            DATETIME NULL,|
|`    `InterviewLocation        NVARCHAR(300) NULL,|
|`    `DecisionDate             DATETIME NULL,|
|`    `VisaExpiryDate           DATE NULL,|
|`    `VisaValidFrom            DATE NULL,|
|`    `-- Approval|
|`    `VisaVignettePath         NVARCHAR(500) NULL,|
|`    `VisaType                 NVARCHAR(100) NULL,|
|`    `-- Refusal (PERMANENT RECORD)|
|`    `RefusalReasonText        NVARCHAR(MAX) NULL,|
|`    `RefusalCode              NVARCHAR(100) NULL,|
|`    `RefusalLetterPath        NVARCHAR(500) NULL,|
|`    `-- Additional Docs|
|`    `AdditionalDocsRequested  NVARCHAR(MAX) NULL,|
|`    `AdditionalDocsDeadline   DATE NULL,|
|`    `AdditionalDocsSubmitted  BIT NULL,|
|`    `-- Next Step (after refusal)|
|`    `NextStepDecision         TINYINT NULL,|
|`    `-- 1=Appeal 2=Reapply Same 3=Different Country 4=Next Intake 5=Close|
|`    `NextStepDecisionDate     DATETIME NULL,|
|`    `NextStepDecisionBy       INT NULL FK --> Users,|
|`    `StudentConsentRecorded   BIT DEFAULT 0,|
|`    `IsDeleted                BIT DEFAULT 0,|
|`    `CreatedBy                INT NOT NULL,|
|`    `CreatedDate              DATETIME DEFAULT GETDATE(),|
|`    `UpdatedBy                INT NULL,|
|`    `UpdatedDate              DATETIME NULL,|
|`    `CONSTRAINT CK\_Visa\_Refusal CHECK (|
|`        `(Status=7 AND RefusalReasonText IS NOT NULL AND RefusalLetterPath IS NOT NULL)|
|`        `OR Status!=7),|
|`    `CONSTRAINT CK\_Visa\_Interview CHECK (|
|`        `(Status=5 AND InterviewDate IS NOT NULL) OR Status!=5)|
|);|

## **14.2 VisaStatusHistory Table**

|CREATE TABLE VisaStatusHistory (|
| :- |
|`    `Id                INT IDENTITY(1,1) PRIMARY KEY,|
|`    `VisaApplicationId INT NOT NULL FK --> VisaApplications,|
|`    `StudentId         INT NOT NULL FK --> Students,|
|`    `PreviousStatus    TINYINT NOT NULL,|
|`    `NewStatus         TINYINT NOT NULL,|
|`    `ChangedBy         INT NOT NULL, -- 0=System|
|`    `ChangeReason      NVARCHAR(500) NULL,|
|`    `ChangedDate       DATETIME DEFAULT GETDATE()|
|);|

## **14.3 Visa State Machine**

|`  `PREPARATION(1) --> SUBMITTED TO EMBASSY(2)  [All visa docs verified]|
| :- |
|`  `SUBMITTED(2) --> UNDER REVIEW(3)             [Embassy acknowledges]|
|`  `UNDER REVIEW(3) --> ADDITIONAL DOCS(4)       [Embassy requests more docs]|
|`  `UNDER REVIEW(3) --> INTERVIEW SCHEDULED(5)   [Interview date set]|
|`  `UNDER REVIEW(3) --> APPROVED(6)              [Visa granted -- terminal positive]|
|`  `UNDER REVIEW(3) --> REFUSED(7)               [Visa refused -- permanent record]|
|`  `ADDITIONAL DOCS(4) --> UNDER REVIEW(3)       [Docs submitted]|
|`  `INTERVIEW SCHEDULED(5) --> APPROVED(6) or REFUSED(7)|
|`  `REFUSED(7) --> [Re-engagement: NextStepDecision set + StudentConsentRecorded=1]|

## **14.4 Pre-Visa Creation Validation**

|Step 1: Application.Status = Unconditional Offer (6)|
| :- |
|Step 2: No existing VisaApplication for this ApplicationId (UNIQUE constraint)|
|Step 3: Tuition deposit payment confirmed:|
|`        `COUNT(\*) FROM StudentPayments|
|`        `WHERE StudentId=@StudentId AND PaymentType=6 AND Status=2 >= 1|
|Step 4: CAS/Enrollment letter uploaded and verified in ApplicationDocuments|
|Step 5: All visa-specific documents verified in StudentDocumentChecklists|

## **14.5 Visa Outcome Handling**

|**Outcome**|**Mandatory Data**|**System Actions**|**Next Steps**|
| :-: | :-: | :-: | :-: |
|APPROVED(6)|VisaVignettePath, DecisionDate, VisaExpiryDate|Student.Status->Visa Approved(6). Pre-departure activated. Notify Counsellor+Manager.|Pre-departure checklist begins.|
|REFUSED(7)|RefusalReasonText, RefusalLetterPath, DecisionDate (all MANDATORY)|Student.Status->Visa Refused(7). IMMEDIATE notification to Counsellor+Manager.|NextStepDecision + StudentConsentRecorded required before new application.|
|ADDITIONAL DOCS(4)|AdditionalDocsRequested, AdditionalDocsDeadline|Processing Officer assigned task. Deadline tracked.|Gather docs and resubmit.|
|INTERVIEW(5)|InterviewDate, InterviewLocation|Reminders set: 7 days + 1 day before. Notify Counsellor+Student.|Prepare student for interview.|

## **14.6 Visa Business Rules**

|**Rule ID**|**Rule**|
| :-: | :-: |
|BR-V01|VisaApplication only created when Application.Status=Unconditional Offer AND tuition deposit confirmed.|
|BR-V02|One VisaApplication per Application. UNIQUE constraint on ApplicationId.|
|BR-V03|Visa refusal record is PERMANENT. No soft delete, no separate archive. Stays on student forever.|
|BR-V04|After refusal, NextStepDecision AND StudentConsentRecorded must be set before new application.|
|BR-V05|Visa approval auto-activates Pre-departure module.|
|BR-V06|Every visa status change recorded in VisaStatusHistory -- no exceptions.|
|BR-V07|EmbassyPortalPassword stored encrypted. Never returned in plain text. Audit log on access.|


# **15. T-15: Student Payment ERD & State Machine**

## **15.1 StudentPayments Table**

|CREATE TABLE StudentPayments (|
| :- |
|`    `Id              INT IDENTITY(1,1) PRIMARY KEY,|
|`    `StudentId       INT NOT NULL FK --> Students,|
|`    `BranchId        INT NOT NULL FK --> Branches,|
|`    `ApplicationId   INT NULL FK --> Applications,|
|`    `PaymentType     TINYINT NOT NULL,|
|`    `-- 1=Service Fee 2=Document Fee 3=Application Fee 4=Visa Fee|
|`    `-- 5=Courier Fee 6=Tuition Deposit 7=Accommodation 8=Insurance 9=Other|
|`    `Amount          DECIMAL(18,2) NOT NULL,|
|`    `Currency        NVARCHAR(10) DEFAULT 'BDT',|
|`    `ExchangeRate    DECIMAL(10,4) NULL,|
|`    `AmountBDT       DECIMAL(18,2) NULL,  -- Amount \* ExchangeRate|
|`    `PaymentMethod   TINYINT NOT NULL,|
|`    `-- 1=Cash 2=Bank Transfer 3=Mobile Banking 4=Cheque 5=Online|
|`    `TransactionRef  NVARCHAR(200) NULL,|
|`    `PaymentDate     DATETIME DEFAULT GETDATE(),|
|`    `ReceiptNo       NVARCHAR(100) NOT NULL,  -- Auto-generated|
|`    `ReceiptPath     NVARCHAR(500) NULL,|
|`    `Status          TINYINT DEFAULT 1,|
|`    `-- 1=Pending Confirmation 2=Confirmed 3=Refunded 4=Partially Refunded 5=Cancelled|
|`    `ConfirmedBy     INT NULL FK --> Users,|
|`    `ConfirmedDate   DATETIME NULL,|
|`    `Notes           NVARCHAR(MAX) NULL,|
|`    `IsDeleted       BIT DEFAULT 0,|
|`    `DeletedBy       INT NULL,|
|`    `DeletedDate     DATETIME NULL,|
|`    `CreatedBy       INT NOT NULL,|
|`    `CreatedDate     DATETIME DEFAULT GETDATE(),|
|`    `UpdatedBy       INT NULL,|
|`    `UpdatedDate     DATETIME NULL,|
|`    `CONSTRAINT CK\_Payment\_Amount CHECK (Amount > 0),|
|`    `CONSTRAINT CK\_Payment\_Confirmed CHECK (|
|`        `(Status=2 AND ConfirmedBy IS NOT NULL AND ConfirmedDate IS NOT NULL) OR Status!=2)|
|);|

## **15.2 PaymentRefunds Table**

|CREATE TABLE PaymentRefunds (|
| :- |
|`    `Id              INT IDENTITY(1,1) PRIMARY KEY,|
|`    `PaymentId       INT NOT NULL FK --> StudentPayments,|
|`    `StudentId       INT NOT NULL FK --> Students,|
|`    `BranchId        INT NOT NULL FK --> Branches,|
|`    `RefundAmount    DECIMAL(18,2) NOT NULL,|
|`    `RefundType      TINYINT NOT NULL, -- 1=Full 2=Partial|
|`    `RefundReason    NVARCHAR(MAX) NOT NULL,|
|`    `RefundMethod    TINYINT NOT NULL,|
|`    `TransactionRef  NVARCHAR(200) NULL,|
|`    `Status          TINYINT DEFAULT 1, -- 1=Requested 2=Approved 3=Processed 4=Rejected|
|`    `RequestedBy     INT NOT NULL FK --> Users,|
|`    `ApprovedBy      INT NULL FK --> Users,|
|`    `ApprovedDate    DATETIME NULL,|
|`    `ProcessedDate   DATETIME NULL,|
|`    `RejectionReason NVARCHAR(500) NULL,|
|`    `CreatedDate     DATETIME DEFAULT GETDATE(),|
|`    `UpdatedBy       INT NULL,|
|`    `UpdatedDate     DATETIME NULL|
|);|

## **15.3 Payment State Machine**

|`  `PENDING CONFIRMATION(1) --> CONFIRMED(2)  [Accounts Officer confirms. RECORD IMMUTABLE after this.]|
| :- |
|`  `CONFIRMED(2) --> REFUNDED(3)              [Full refund processed after Manager approval]|
|`  `CONFIRMED(2) --> PARTIALLY REFUNDED(4)    [Partial refund processed]|
|`  `PENDING(1)   --> CANCELLED(5)             [Before confirmation only. Accounts role required.]|

## **15.4 ReceiptNo Generation**

|Format: RCP-{BranchCode}-{YYYY}-{Seq:00000}|
| :- |
|Example: RCP-DHK-2026-00234|
||
|DECLARE @Seq INT;|
|SELECT @Seq = COUNT(\*) + 1 FROM StudentPayments|
|WHERE BranchId=@BranchId AND YEAR(CreatedDate)=YEAR(GETDATE());|
||
|SET @ReceiptNo = 'RCP-' + @BranchCode + '-'|
|`               `+ CAST(YEAR(GETDATE()) AS NVARCHAR) + '-'|
|`               `+ RIGHT('00000' + CAST(@Seq AS NVARCHAR), 5);|

## **15.5 Refund Workflow**

|**Step**|**Action**|**System Behavior**|
| :-: | :-: | :-: |
|1|Accounts Officer submits refund request|INSERT PaymentRefunds, Status=Requested(1). Manager notified.|
|2|Manager reviews|Views original payment + refund details.|
|3a|Manager approves|Status=Approved(2). ApprovedBy+Date set. Accounts notified.|
|3b|Manager rejects|Status=Rejected(4). RejectionReason mandatory. Accounts notified.|
|4|Accounts processes refund|Status=Processed(3). TransactionRef entered. ProcessedDate=now.|
|5|System reconciles|Full refund: Payment.Status=Refunded(3). Partial: Partially Refunded(4).|

## **15.6 Payment Business Rules**

|**Rule ID**|**Rule**|
| :-: | :-: |
|BR-P01|Confirmed payment is IMMUTABLE. Only Super Admin can edit with mandatory audit log entry.|
|BR-P02|Refund requires Branch Manager approval. Cannot process without ApprovedBy set.|
|BR-P03|RefundAmount cannot exceed original payment amount. API-level validation.|
|BR-P04|ReceiptNo is auto-generated and unique per branch per year. Cannot be manually entered.|
|BR-P05|PaymentType=Tuition Deposit(6) confirmed payment required before VisaApplication creation.|
|BR-P06|Every payment must have BranchId. Finance data is always branch-scoped.|


# **16. T-16: Commission ERD & Calculation Logic**

## **16.1 Commissions Table**

|CREATE TABLE Commissions (|
| :- |
|`    `Id                   INT IDENTITY(1,1) PRIMARY KEY,|
|`    `ApplicationId        INT NOT NULL UNIQUE FK --> Applications,|
|`    `StudentId            INT NOT NULL FK --> Students,|
|`    `BranchId             INT NOT NULL FK --> Branches,|
|`    `AgentId              INT NULL FK --> Agents,|
|`    `-- University Commission|
|`    `UniversityAmount     DECIMAL(18,2) NOT NULL,|
|`    `Currency             NVARCHAR(10) DEFAULT 'GBP',|
|`    `ExchangeRate         DECIMAL(10,4) NULL,|
|`    `UniversityAmountBDT  DECIMAL(18,2) NULL,|
|`    `-- Agent Commission (deduction)|
|`    `AgentAmount          DECIMAL(18,2) DEFAULT 0,|
|`    `AgentAmountBDT       DECIMAL(18,2) NULL,|
|`    `-- Scholarship|
|`    `ScholarshipApplied   BIT DEFAULT 0,|
|`    `ScholarshipAmount    DECIMAL(18,2) NULL,|
|`    `AdjustedBaseAmount   DECIMAL(18,2) NULL,|
|`    `-- Computed net profit|
|`    `NetProfit            AS (ISNULL(UniversityAmountBDT,0) - ISNULL(AgentAmountBDT,0)) PERSISTED,|
|`    `-- Basis|
|`    `CommissionRateUsed   DECIMAL(5,2) NOT NULL,  -- Snapshot|
|`    `CommissionBasisAmount DECIMAL(18,2) NOT NULL,|
|`    `-- Status|
|`    `Status               TINYINT DEFAULT 1,|
|`    `-- 1=Expected 2=Invoiced 3=Partially Received 4=Received 5=Disputed 6=Written Off|
|`    `InvoiceNo            NVARCHAR(100) NULL,|
|`    `InvoiceDate          DATE NULL,|
|`    `InvoicePath          NVARCHAR(500) NULL,|
|`    `ReceivedAmount       DECIMAL(18,2) NULL,|
|`    `ReceivedDate         DATETIME NULL,|
|`    `ReceivedBy           INT NULL FK --> Users,|
|`    `BankTransactionRef   NVARCHAR(200) NULL,|
|`    `DisputeReason        NVARCHAR(MAX) NULL,|
|`    `DisputeDate          DATETIME NULL,|
|`    `DisputeResolvedDate  DATETIME NULL,|
|`    `DisputeResolution    NVARCHAR(MAX) NULL,|
|`    `WriteOffReason       NVARCHAR(MAX) NULL,|
|`    `WriteOffDate         DATETIME NULL,|
|`    `WriteOffBy           INT NULL FK --> Users,|
|`    `CreatedBy            INT NOT NULL,|
|`    `CreatedDate          DATETIME DEFAULT GETDATE(),|
|`    `UpdatedBy            INT NULL,|
|`    `UpdatedDate          DATETIME NULL|
|);|

## **16.2 Commission Calculation (Auto-triggered on Enrollment)**

|Step 1: Get FeeSnapshot from Applications.FeeSnapshot (JSON)|
| :- |
|`        `TuitionAmount = FeeSnapshot where FeeType=1|
||
|Step 2: Apply scholarship if ScholarshipApplications.Status=Granted(2)|
|`        `IF DiscountType=1: AdjustedBase = TuitionAmount \* (1 - DiscountValue/100)|
|`        `IF DiscountType=2: AdjustedBase = TuitionAmount - DiscountValue|
|`        `ELSE: AdjustedBase = TuitionAmount|
||
|Step 3: CommissionRateUsed = Applications.CommissionRateSnapshot|
|`        `UniversityAmount = AdjustedBase \* CommissionRateUsed / 100|
||
|Step 4: AgentAmount = 0 (default)|
|`        `IF AgentId IS NOT NULL:|
|`          `IF Agent.CommissionType=1: AgentAmount = UniversityAmount \* AgentRate/100|
|`          `IF Agent.CommissionType=2: AgentAmount = Agent.CommissionRate (fixed)|
||
|Step 5: INSERT Commissions|
|`        `Status = Expected (1)|
|`        `NetProfit = PERSISTED computed column (auto-calculated)|

## **16.3 Commission State Machine**

|`  `EXPECTED(1) --> INVOICED(2)            [Invoice sent to university]|
| :- |
|`  `INVOICED(2) --> PARTIALLY RECEIVED(3) [Partial payment received]|
|`  `INVOICED(2) --> RECEIVED(4)            [Full payment received -- terminal positive]|
|`  `PARTIALLY RECEIVED(3) --> RECEIVED(4)  [Remainder received]|
|`  `Any --> DISPUTED(5)                    [Amount mismatch. DisputeReason mandatory.]|
|`  `DISPUTED(5) --> RECEIVED(4)            [Dispute resolved]|
|`  `Any --> WRITTEN OFF(6)                 [Unrecoverable. Manager+SuperAdmin both required.]|

## **16.4 Commission Business Rules**

|**Rule ID**|**Rule**|
| :-: | :-: |
|BR-COM01|Commission auto-created ONLY when Application.Status -> Enrolled. No manual creation before.|
|BR-COM02|NetProfit is a PERSISTED computed column. Cannot be manually set.|
|BR-COM03|Commission dispute must be resolved within days set in SystemConfigurations (default 30).|
|BR-COM04|Write-off requires both Branch Manager and Super Admin acknowledgment.|
|BR-COM05|Agent commission auto-calculated if AgentId exists on Student at enrollment time.|


# **17. T-17: Refund Workflow**
Refund workflow is defined in T-15 (PaymentRefunds table and state machine). Key additional rules:

|**Rule**|**Detail**|
| :-: | :-: |
|Approval mandatory|Branch Manager approval required. No exceptions. ApprovedBy must be set.|
|Amount ceiling|RefundAmount <= original Payment.Amount. API validates.|
|Finance reconciliation|Accounts Officer prepares reconciliation: original vs refund vs services rendered.|
|Outcome options|Full refund / Partial refund / Declined (all outcomes permanently recorded).|
|Commission impact|If refund relates to a commissioned application, Commission record flagged for review.|


# **18. T-18: Communication Log ERD**

## **18.1 CommunicationLogs Table**

|CREATE TABLE CommunicationLogs (|
| :- |
|`    `Id           INT IDENTITY(1,1) PRIMARY KEY,|
|`    `BranchId     INT NOT NULL FK --> Branches,|
|`    `UserId       INT NOT NULL FK --> Users,|
|`    `-- ONE of these must be set (not both, not neither)|
|`    `LeadId       INT NULL FK --> Leads,|
|`    `StudentId    INT NULL FK --> Students,|
|`    `-- Communication Details|
|`    `Type         TINYINT NOT NULL,|
|`    `-- 1=Inbound Call 2=Outbound Call 3=WhatsApp Sent|
|`    `-- 4=WhatsApp Received 5=Email Sent 6=Email Received 7=In-Person Meeting 8=SMS Sent|
|`    `Direction    TINYINT NOT NULL, -- 1=Inbound 2=Outbound|
|`    `Duration     INT NULL,         -- Seconds (for calls)|
|`    `Summary      NVARCHAR(MAX) NOT NULL,|
|`    `Outcome      TINYINT NULL,|
|`    `-- 1=Positive 2=Neutral 3=Negative 4=No Answer 5=Callback Requested|
|`    `CallbackDate DATETIME NULL,|
|`    `CreatedDate  DATETIME DEFAULT GETDATE(),|
|`    `CONSTRAINT CK\_CommLog\_Entity CHECK (|
|`        `(LeadId IS NOT NULL AND StudentId IS NULL)|
|`        `OR (LeadId IS NULL AND StudentId IS NOT NULL)),|
|`    `CONSTRAINT CK\_CommLog\_Callback CHECK (|
|`        `(Outcome=5 AND CallbackDate IS NOT NULL) OR Outcome!=5 OR Outcome IS NULL)|
|);|
||
|CREATE INDEX IX\_CommLogs\_LeadId\_Date ON CommunicationLogs(LeadId, CreatedDate) WHERE LeadId IS NOT NULL;|
|CREATE INDEX IX\_CommLogs\_StudentId\_Date ON CommunicationLogs(StudentId, CreatedDate) WHERE StudentId IS NOT NULL;|

## **18.2 Communication Business Rules**

|**Rule ID**|**Rule**|
| :-: | :-: |
|BR-CL01|Every log must be linked to EITHER LeadId OR StudentId. Not both, not neither. DB CHECK constraint.|
|BR-CL02|Summary is mandatory. Minimum 10 characters enforced at API.|
|BR-CL03|If Outcome=Callback Requested(5), CallbackDate is mandatory. Follow-up reminder created.|
|BR-CL04|Communication logs are NEVER deleted. No soft delete. Permanent audit trail.|
|BR-CL05|After student conversion, Lead CommunicationLogs accessible from Student timeline via Student.LeadId.|


# **19. T-19: Agent & AgentLead ERD & Commission Link**

## **19.1 Agent Commission Link Flow**

|Lead created with Agent source:|
| :- |
|`  `INSERT AgentLeads (AgentId, LeadId)|
|`  `CONSTRAINT: UNIQUE (LeadId) -- one agent per lead|
||
|Lead converts to Student:|
|`  `Student tagged via AgentLeads.LeadId chain|
||
|Student Application -> Enrolled:|
|`  `Commission.AgentId populated from:|
|`    `SELECT al.AgentId FROM AgentLeads al|
|`    `JOIN Leads l ON l.Id = al.LeadId|
|`    `WHERE l.Id = Student.LeadId|
||
|Commission calculated:|
|`  `AgentAmount = UniversityAmount \* Agent.CommissionRate / 100|
|`  `(or fixed amount based on CommissionType)|

## **19.2 Agent Performance Query**

|SELECT ag.Id, ag.Name, ag.CommissionRate,|
| :- |
|`       `COUNT(DISTINCT al.LeadId)           AS TotalLeads,|
|`       `COUNT(DISTINCT s.Id)                AS TotalStudents,|
|`       `COUNT(DISTINCT CASE WHEN a.Status=7 THEN a.Id END) AS TotalEnrolled,|
|`       `SUM(ISNULL(c.AgentAmount, 0))       AS TotalCommissionExpected,|
|`       `SUM(CASE WHEN c.Status=4 THEN c.AgentAmount ELSE 0 END) AS TotalPaid|
|FROM Agents ag|
|LEFT JOIN AgentLeads al ON al.AgentId = ag.Id|
|LEFT JOIN Leads l       ON l.Id = al.LeadId|
|LEFT JOIN Students s    ON s.LeadId = l.Id|
|LEFT JOIN Applications a ON a.StudentId = s.Id|
|LEFT JOIN Commissions c  ON c.ApplicationId = a.Id AND c.AgentId = ag.Id|
|WHERE ag.BranchId = @BranchId AND ag.IsDeleted = 0|
|GROUP BY ag.Id, ag.Name, ag.CommissionRate;|


# **20. T-20: Notification Trigger Map — All Modules**

|**Module**|**Event**|**Recipient(s)**|**Channel**|**Priority**|
| :-: | :-: | :-: | :-: | :-: |
|Lead|New lead created|Assigned Counsellor|In-App + SMS|HIGH|
|Lead|Lead assigned / reassigned|Old + New Counsellor|In-App|MEDIUM|
|Lead|Lead untouched 48h (SLA breach)|Counsellor + Manager|In-App + Email|HIGH|
|Lead|Lead converted to Student|Branch Manager|In-App|INFO|
|Lead|Lead Unresponsive (3 missed FU)|Manager|In-App + Email|HIGH|
|Follow-up|Follow-up due today (08:00)|Counsellor|In-App + SMS|HIGH|
|Follow-up|Follow-up due in 1 hour|Counsellor|In-App|HIGH|
|Follow-up|Follow-up missed (EOD)|Counsellor + Manager|In-App + Email|HIGH|
|Follow-up|Follow-up overdue 24h|Branch Manager|Email|CRITICAL|
|Student|New student created|Branch Manager|In-App|INFO|
|Student|Processing Officer not assigned (24h)|Branch Manager|In-App + Email|HIGH|
|Student|Application Ready status set|Processing + Counsellor|In-App|INFO|
|Student|Passport expiry 90 days|Processing + Counsellor|In-App|MEDIUM|
|Student|Passport expiry 30 days|Processing + Manager|In-App + Email|HIGH|
|Student|Branch transfer requested|Both Managers|In-App + Email|HIGH|
|Document|Document uploaded|Processing Officer|In-App|MEDIUM|
|Document|Document verified|Counsellor + Student (SMS)|In-App + SMS|MEDIUM|
|Document|Document rejected|Counsellor + Student|In-App + Email|HIGH|
|Document|Document rejected 3rd time|Branch Manager|In-App + Email|HIGH|
|Document|Document expired (system)|Processing + Counsellor|In-App|HIGH|
|App|Application created|Branch Manager|In-App|INFO|
|App|Conditional offer received|Counsellor + Manager|In-App + SMS|HIGH|
|App|Unconditional offer received|Counsellor+Manager+Accounts|In-App + SMS|HIGH|
|App|Application rejected by university|Counsellor + Manager|In-App + Email|HIGH|
|App|Offer expiry in 7 days|Processing + Counsellor|In-App + Email|HIGH|
|App|Application stale 10 days|Processing + Manager|In-App|MEDIUM|
|App|All conditions resolved (upgrade ready)|Processing Officer|In-App|HIGH|
|Visa|Visa application created|Branch Manager|In-App|INFO|
|Visa|Interview scheduled|Counsellor + Student|In-App + SMS|HIGH|
|Visa|Interview in 7 days|Counsellor + Student|In-App + SMS|HIGH|
|Visa|Interview in 1 day|Counsellor + Student|In-App + SMS|CRITICAL|
|Visa|Visa approved|Counsellor + Manager|In-App + SMS|HIGH|
|Visa|Visa refused|Counsellor + Manager|In-App + Email|CRITICAL|
|Finance|Payment recorded|Accounts + Manager|In-App|INFO|
|Finance|Refund requested|Branch Manager|In-App + Email|HIGH|
|Finance|Commission created (enrollment)|Accounts|In-App|INFO|
|Finance|Commission disputed|Accounts + Manager|In-App + Email|HIGH|
|Finance|Commission received|Manager + Accounts|In-App + Email|INFO|
|Master|Partnership expiry 60 days|Manager|In-App + Email|MEDIUM|
|Master|Partnership expiry 30 days|Manager + Super Admin|In-App + Email|HIGH|
|Master|Partnership expired -- auto-block|Manager + Super Admin|In-App + Email|CRITICAL|
|Perf|Lead response SLA breach (4h)|Branch Manager|In-App|HIGH|
|Perf|Follow-up backlog >= 10|Branch Manager|In-App + Email|HIGH|
|Perf|Monthly target < 50% by 20th|Branch Manager|In-App + Email|HIGH|
|Perf|Monthly target < 30% by 25th|Manager + Super Admin|In-App + Email|CRITICAL|


# **21. T-21: Audit Log Design**

## **21.1 AuditLogs Table**

|CREATE TABLE AuditLogs (|
| :- |
|`    `Id            BIGINT IDENTITY(1,1) PRIMARY KEY,|
|`    `TableName     NVARCHAR(100) NOT NULL,|
|`    `RecordId      INT NOT NULL,|
|`    `Action        NVARCHAR(50) NOT NULL,|
|`    `-- CREATE, UPDATE, DELETE, VIEW\_SENSITIVE, EXPORT,|
|`    `-- STATUS\_CHANGE, OVERRIDE, CONVERSION, LOGIN, LOGOUT|
|`    `OldValue      NVARCHAR(MAX) NULL,  -- JSON before|
|`    `NewValue      NVARCHAR(MAX) NULL,  -- JSON after|
|`    `ChangedFields NVARCHAR(MAX) NULL,  -- JSON array of changed field names|
|`    `UserId        INT NOT NULL,        -- 0=System|
|`    `UserName      NVARCHAR(150) NOT NULL,  -- Snapshot at time of action|
|`    `IPAddress     NVARCHAR(50) NULL,|
|`    `UserAgent     NVARCHAR(500) NULL,|
|`    `BranchId      INT NULL,|
|`    `Reason        NVARCHAR(500) NULL,|
|`    `Timestamp     DATETIME DEFAULT GETDATE()|
|);|
||
|-- AuditLogs: NO DELETE endpoint exists. NO soft delete column.|
|-- Partition by year recommended for performance at scale.|
||
|CREATE TABLE SensitiveFieldAccess (|
|`    `Id         BIGINT IDENTITY(1,1) PRIMARY KEY,|
|`    `UserId     INT NOT NULL,|
|`    `TableName  NVARCHAR(100) NOT NULL,|
|`    `RecordId   INT NOT NULL,|
|`    `FieldName  NVARCHAR(100) NOT NULL,  -- PassportNumber, etc.|
|`    `AccessType NVARCHAR(20) NOT NULL,   -- VIEW, EXPORT, PRINT|
|`    `IPAddress  NVARCHAR(50) NULL,|
|`    `Timestamp  DATETIME DEFAULT GETDATE()|
|);|

## **21.2 What Must Be Logged**

|**Category**|**Events to Log**|**Table Used**|
| :-: | :-: | :-: |
|Always (all tables)|INSERT, UPDATE, soft DELETE, status changes, role/permission changes|AuditLogs|
|Always (auth)|Login, logout, failed login attempts|AuditLogs|
|Always (finance)|Payment confirmation, refund approval, commission write-off|AuditLogs|
|Always (override)|Separation of duty override, Manager payment edit, forced approval|AuditLogs|
|Sensitive field access|PassportNumber unmask, NID view, bank statement access, portal credentials access|SensitiveFieldAccess|
|Export events|Data export requests, report generation with student PII|AuditLogs|

## **21.3 Audit Log Indexes**

|CREATE INDEX IX\_AuditLogs\_TableName\_RecordId ON AuditLogs(TableName, RecordId, Timestamp);|
| :- |
|CREATE INDEX IX\_AuditLogs\_UserId\_Timestamp ON AuditLogs(UserId, Timestamp);|
|CREATE INDEX IX\_AuditLogs\_Action\_Timestamp ON AuditLogs(Action, Timestamp);|
|CREATE INDEX IX\_AuditLogs\_BranchId\_Timestamp ON AuditLogs(BranchId, Timestamp) WHERE BranchId IS NOT NULL;|


# **22. T-22: Branch Transfer Workflow**

## **22.1 BranchTransfers Table**

|CREATE TABLE BranchTransfers (|
| :- |
|`    `Id                  INT IDENTITY(1,1) PRIMARY KEY,|
|`    `StudentId           INT NOT NULL FK --> Students,|
|`    `FromBranchId        INT NOT NULL FK --> Branches,|
|`    `ToBranchId          INT NOT NULL FK --> Branches,|
|`    `RequestedBy         INT NOT NULL FK --> Users,|
|`    `RequestReason       NVARCHAR(500) NOT NULL,|
|`    `Status              TINYINT DEFAULT 1, -- 1=Pending 2=Approved 3=Rejected|
|`    `ApprovedByFrom      INT NULL FK --> Users,|
|`    `ApprovedByFromDate  DATETIME NULL,|
|`    `ApprovedByTo        INT NULL FK --> Users,|
|`    `ApprovedByToDate    DATETIME NULL,|
|`    `SuperAdminNotified  BIT DEFAULT 0,|
|`    `RejectedBy          INT NULL FK --> Users,|
|`    `RejectionReason     NVARCHAR(500) NULL,|
|`    `TransferDate        DATETIME NULL,|
|`    `CreatedDate         DATETIME DEFAULT GETDATE(),|
|`    `CONSTRAINT CK\_BranchTransfer\_Diff CHECK (FromBranchId != ToBranchId)|
|);|

## **22.2 Transfer Execution Flow**

|**Step**|**Action**|**System Behavior**|
| :-: | :-: | :-: |
|1|Manager A initiates request|INSERT BranchTransfers Status=Pending. Manager B notified. Super Admin INFO.|
|2|Manager B reviews|Views student profile and transfer reason.|
|3a|Manager B approves|ApprovedByTo set. ApprovedByToDate=now. Manager A notified.|
|3b|Manager B rejects|Status=Rejected. RejectionReason mandatory. Manager A notified. Transfer ends.|
|4|Manager A confirms|ApprovedByFrom set. Both approvals now complete.|
|5|System executes transfer|UPDATE Students.BranchId=ToBranch. Status=Approved. TransferDate=now. INSERT AuditLog.|
|6|Finance records NOT transferred|StudentPayments retain FromBranchId. Future payments use ToBranchId.|

## **22.3 Branch Transfer Business Rules**

|**Rule ID**|**Rule**|
| :-: | :-: |
|BR-BT01|Transfer requires BOTH branch manager approvals. Auto-executes when both ApprovedBy fields set.|
|BR-BT02|FromBranchId != ToBranchId. Enforced by CHECK constraint.|
|BR-BT03|Finance records stay with originating branch. Only future transactions go to new branch.|
|BR-BT04|Full student history moves with student logically. Historical records retain original BranchId.|
|BR-BT05|Super Admin can override either manager approval. Override reason mandatory. Audit log created.|


# **23. T-23: Reporting Queries & KPI Definitions**

## **23.1 Lead Pipeline Report (Counsellor Performance)**

|SELECT u.FullName AS Counsellor,|
| :- |
|`       `COUNT(l.Id) AS TotalLeads,|
|`       `SUM(CASE WHEN l.Status=6 THEN 1 ELSE 0 END) AS Converted,|
|`       `SUM(CASE WHEN l.Status IN (7,8,9) THEN 1 ELSE 0 END) AS Lost,|
|`       `CAST(SUM(CASE WHEN l.Status=6 THEN 1.0 ELSE 0 END)|
|`            `/ NULLIF(COUNT(l.Id),0) \* 100 AS DECIMAL(5,2)) AS ConversionRate|
|FROM Leads l JOIN Users u ON u.Id=l.AssignedTo|
|WHERE l.BranchId=@BranchId AND l.IsDeleted=0|
|AND l.CreatedDate BETWEEN @FromDate AND @ToDate|
|GROUP BY u.Id, u.FullName ORDER BY ConversionRate DESC;|

## **23.2 Lead Response Time KPI**

|SELECT l.Id, l.Name, l.CreatedDate AS LeadCreatedAt,|
| :- |
|`       `MIN(cl.CreatedDate) AS FirstContactAt,|
|`       `DATEDIFF(MINUTE, l.CreatedDate, MIN(cl.CreatedDate)) AS ResponseMinutes,|
|`       `CASE WHEN DATEDIFF(HOUR, l.CreatedDate, MIN(cl.CreatedDate)) <= 4|
|`            `THEN 'Within SLA' ELSE 'SLA Breached' END AS SLAStatus|
|FROM Leads l LEFT JOIN CommunicationLogs cl ON cl.LeadId=l.Id|
|WHERE l.BranchId=@BranchId AND l.IsDeleted=0|
|AND l.CreatedDate BETWEEN @FromDate AND @ToDate|
|GROUP BY l.Id, l.Name, l.CreatedDate ORDER BY ResponseMinutes DESC;|

## **23.3 Application Success Rate by University**

|SELECT u.Name AS University,|
| :- |
|`       `COUNT(a.Id) AS TotalApplications,|
|`       `SUM(CASE WHEN a.Status IN (5,6,7) THEN 1 ELSE 0 END) AS OffersReceived,|
|`       `SUM(CASE WHEN a.Status=7 THEN 1 ELSE 0 END) AS Enrolled,|
|`       `CAST(SUM(CASE WHEN a.Status IN (5,6,7) THEN 1.0 ELSE 0 END)|
|`            `/ NULLIF(COUNT(a.Id),0) \* 100 AS DECIMAL(5,2)) AS SuccessRate|
|FROM Applications a JOIN Universities u ON u.Id=a.UniversityId|
|WHERE a.BranchId=@BranchId AND a.IsDeleted=0|
|AND a.CreatedDate BETWEEN @FromDate AND @ToDate|
|GROUP BY u.Id, u.Name ORDER BY SuccessRate DESC;|

## **23.4 Monthly Revenue Report**

|SELECT YEAR(sp.PaymentDate) AS Year, MONTH(sp.PaymentDate) AS Month,|
| :- |
|`       `b.Name AS Branch,|
|`       `SUM(CASE WHEN sp.Currency='BDT' THEN sp.Amount ELSE sp.AmountBDT END) AS TotalRevenueBDT,|
|`       `COUNT(DISTINCT sp.StudentId) AS UniqueStudents|
|FROM StudentPayments sp JOIN Branches b ON b.Id=sp.BranchId|
|WHERE sp.Status=2 AND sp.IsDeleted=0|
|AND (@BranchId IS NULL OR sp.BranchId=@BranchId)|
|AND sp.PaymentDate BETWEEN @FromDate AND @ToDate|
|GROUP BY YEAR(sp.PaymentDate), MONTH(sp.PaymentDate), b.Id, b.Name|
|ORDER BY Year DESC, Month DESC;|

## **23.5 Commission Aging Report**

|SELECT c.Id, s.FullName AS Student, a.UniversityNameSnapshot AS University,|
| :- |
|`       `c.UniversityAmountBDT AS Expected, ISNULL(c.ReceivedAmount,0) AS Received,|
|`       `c.UniversityAmountBDT - ISNULL(c.ReceivedAmount,0) AS Outstanding,|
|`       `DATEDIFF(DAY, c.CreatedDate, GETDATE()) AS AgeDays,|
|`       `CASE WHEN DATEDIFF(DAY, c.CreatedDate, GETDATE()) <= 30 THEN '0-30 days'|
|`            `WHEN DATEDIFF(DAY, c.CreatedDate, GETDATE()) <= 60 THEN '31-60 days'|
|`            `WHEN DATEDIFF(DAY, c.CreatedDate, GETDATE()) <= 90 THEN '61-90 days'|
|`            `ELSE '90+ days' END AS AgeBucket|
|FROM Commissions c JOIN Students s ON s.Id=c.StudentId|
|JOIN Applications a ON a.Id=c.ApplicationId|
|WHERE c.Status NOT IN (4,6) AND (@BranchId IS NULL OR c.BranchId=@BranchId)|
|ORDER BY AgeDays DESC;|

## **23.6 Branch Target vs Achievement**

|SELECT bt.Year, bt.Month, b.Name AS Branch,|
| :- |
|`       `bt.LeadTarget, COUNT(DISTINCT l.Id) AS ActualLeads,|
|`       `bt.EnrollmentTarget,|
|`       `COUNT(DISTINCT CASE WHEN a.Status=7 THEN a.Id END) AS ActualEnrollments,|
|`       `bt.RevenueTarget,|
|`       `SUM(CASE WHEN sp.Status=2 THEN ISNULL(sp.AmountBDT, sp.Amount) ELSE 0 END) AS ActualRevenue|
|FROM BranchTargets bt JOIN Branches b ON b.Id=bt.BranchId|
|LEFT JOIN Leads l ON l.BranchId=bt.BranchId|
|`  `AND YEAR(l.CreatedDate)=bt.Year AND MONTH(l.CreatedDate)=bt.Month AND l.IsDeleted=0|
|LEFT JOIN Applications a ON a.BranchId=bt.BranchId|
|`  `AND YEAR(a.EnrollmentDate)=bt.Year AND MONTH(a.EnrollmentDate)=bt.Month AND a.IsDeleted=0|
|LEFT JOIN StudentPayments sp ON sp.BranchId=bt.BranchId|
|`  `AND YEAR(sp.PaymentDate)=bt.Year AND MONTH(sp.PaymentDate)=bt.Month AND sp.Status=2|
|WHERE (@BranchId IS NULL OR bt.BranchId=@BranchId)|
|AND bt.Year=@Year AND bt.Month=@Month|
|GROUP BY bt.Year, bt.Month, b.Id, b.Name,|
|`         `bt.LeadTarget, bt.EnrollmentTarget, bt.RevenueTarget;|

## **23.7 Student 360 Timeline Query**

|-- Combines: Lead creation, Follow-ups, Sessions, Status changes,|
| :- |
|--           Documents, Applications, Visa, Payments, Communication logs|
|-- Using UNION ALL across all history tables.|
|-- Full query: SELECT EventDate, EventType, EventDescription, ActorName, Module|
|-- FROM (union of all event sources) AS Timeline|
|-- WHERE LeadId=@LeadId OR StudentId=@StudentId|
|-- ORDER BY EventDate DESC;|
||
|-- Sources included:|
|-- 1. Leads table (creation event)|
|-- 2. FollowUps (all status events)|
|-- 3. CounsellingSessions (all sessions)|
|-- 4. StudentStatusHistory|
|-- 5. DocumentVerificationHistory|
|-- 6. ApplicationStatusHistory|
|-- 7. VisaStatusHistory|
|-- 8. StudentPayments|
|-- 9. CommunicationLogs|

## **23.8 KPI Reference Table**

|**KPI**|**Formula**|**Target**|**Owner**|
| :-: | :-: | :-: | :-: |
|Lead Response Time|Minutes from lead creation to first CommunicationLog|< 240 min (4h)|Branch Manager|
|Lead-to-Counselling Rate|Leads with 1+ session / total leads %|> 70%|Branch Manager|
|Conversion Rate|Students created / active leads %|> 35%|Branch Manager|
|Application Success Rate|Offers / Applications submitted %|> 80%|Processing|
|Visa Success Rate|Approved / Visa applications submitted %|By country|Branch Manager|
|Doc Completion Time|Days from student creation to Application-Ready|< 14 days|Processing|
|Commission Collection|Commission received / expected %|> 90%|Accounts|
|Follow-up SLA Compliance|Completed on time / total follow-ups %|> 95%|Branch Manager|
|Monthly Target Achievement|Actual enrollments / target %|> 100%|Branch Manager|


# **Appendix A: Complete Business Rules Catalogue**

|**Rule ID**|**Rule Statement**|**Enforced At**|
| :-: | :-: | :-: |
|BR-F01-09|Follow-up rules: date, remarks, role, system-only missed, override, unresponsive, terminal block, history, reschedule|API + DB|
|BR-C01-10|Counselling rules: terminal check, type required, IELTS/gap/visa constraints, max shortlist, single selection, outcome requirements, UPSERT academic|API + DB|
|BR-S01-12|Student rules: phone unique, LeadId unique, auto-profile, auto-checklist, app-ready flag, waiver, edit lock, visa refusal permanent, transfer, withdrawal cascade, re-engagement, status history|API + DB|
|BR-G01-07|Conversion gate rules: API-level all layers, warning acknowledgment logged, cross-table duplicate check, full transaction, post-commit triggers, agent tag, audit log|API|
|BR-D01-12|Document rules: separation of duty, manager override, expiry required, version supersede, rejection code, escalation at 3, expired auto-set, app-ready trigger, app-ready reset, soft delete, waiver policy, history always|API + DB|
|BR-A01-13|Application rules: app-ready gate, duplicate block, partner check, intake open, fee snapshot, consent required, conditions all resolved, enrollment triple check, withdrawal cascade, rejection counselling, status history, portal encrypted, commission auto-create|API + DB|
|BR-V01-07|Visa rules: unconditional+deposit gate, one per application, refusal permanent, next step required, pre-departure auto, status history, portal encrypted|API + DB|
|BR-P01-06|Payment rules: confirmed immutable, refund approval, amount ceiling, receipt auto-generate, tuition deposit gate, branch-scoped|API + DB|
|BR-COM01-05|Commission rules: enrollment only, computed net profit, dispute timeline, write-off authority, agent auto-calculate|API + DB|
|BR-CL01-05|Communication rules: entity link constraint, summary required, callback date, no delete, timeline via LeadId chain|API + DB|
|BR-BT01-05|Branch transfer rules: both approvals, different branches, finance stays, history follows, super admin override|API + DB|


# **Appendix B: Glossary of Terms**

|**Term**|**Definition**|
| :-: | :-: |
|Lead|A prospective student who has expressed interest but not yet passed the conversion gate.|
|Student|A Lead formally converted after all mandatory pre-requisites are met.|
|Intake|A specific enrollment window for a program (e.g., September 2026).|
|Conversion Gate|Server-side multi-layer validation running when Convert to Student is submitted.|
|Application-Ready|System flag on Student set automatically when all required checklist docs are Verified or Waived.|
|CAS|Confirmation of Acceptance for Studies. UK university document needed for Tier 4 visa.|
|Conditional Offer|University offer subject to stated conditions. Not final until all conditions Met/Waived.|
|Unconditional Offer|Firm university offer with no outstanding conditions. Enables visa application.|
|Soft Delete|IsDeleted=1. Record not physically removed. Remains visible to Super Admin.|
|Separation of Duty|Uploader of a document cannot be the verifier of the same document.|
|RBAC|Role-Based Access Control. Permissions assigned to roles, users assigned to roles.|
|SLA|Service Level Agreement. Maximum acceptable time for completing an action.|
|Commission|Revenue received from a university for successfully enrolling a student.|
|Fee Snapshot|JSON copy of fee structure taken at application creation. Immutable after creation.|
|Grandfather Clause|Existing records not affected by new policy that would have blocked their creation.|
|NetProfit|Persisted computed column: UniversityAmountBDT minus AgentAmountBDT.|
|InternalRefNo|Auto-generated application reference: APP-{BranchCode}-{YYYY}-{Seq:00000}.|
|ReceiptNo|Auto-generated payment receipt number: RCP-{BranchCode}-{YYYY}-{Seq:00000}.|
|TriggerSource|Field in history tables identifying what caused a status change (Manual/System/Conversion etc.).|
|IsApplicationReady|Boolean flag on Student. Set by system. Required before application can be created.|



**END OF DOCUMENT**

Education CRM Technical Specification  |  Version 1.0  |  April 2026
Page  of 
