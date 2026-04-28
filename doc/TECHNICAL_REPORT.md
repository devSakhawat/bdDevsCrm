# Technical Report - bdDevsCrm

## Overview

This technical report summarizes the repository-wide backend and frontend implementation rules used in **bdDevsCrm**. It consolidates the architecture, naming, controller scope, repository usage, and form/grid behavior that are already reflected across the solution and supporting documents.

---

## Backend Section

### 1. Naming Convention

#### C# Naming Rules
- **Classes, interfaces, methods, namespaces, and properties** use `PascalCase`.
- **Interfaces** start with `I`, such as `IRepositoryManager` and `ICrmCountryService`.
- **Private fields** use `_camelCase`.
- **Parameters and local variables** use `camelCase`.
- **Async methods** must end with `Async`.
- **Record contracts** follow action-based naming such as `CreateCountryRecord`, `UpdateCountryRecord`, and `DeleteCountryRecord`.
- **DTOs** use `Dto` or `DTO` suffixes, with `Dto` preferred for consistency.

#### Backend Naming Philosophy
- Folder names and namespaces should align.
- Feature modules should keep the same name across layers.
- Route names should stay consistent with the module and operation they represent.
- Read routes are separated from create, update, and delete routes to keep API intent explicit.

### 2. Coding Design

#### Service Design
- Business logic lives in `Application.Services`.
- Services coordinate validation, repository calls, duplicate checks, mapping, logging, and transaction persistence.
- Services return DTOs instead of exposing entities directly.
- Services use dependency injection for repositories, logging, configuration, and other collaborators.
- Mapping uses shared extensions such as `MapTo<T>()` and `MapToList<T>()`.

#### API Design
- API controllers stay thin and delegate work to the service layer.
- Controllers return standardized `ApiResponse<T>` envelopes through helper methods.
- Authentication, correlation IDs, and common response helpers are centralized in `BaseApiController`.
- Validation failures and domain exceptions are handled through shared filters and middleware instead of repeated controller logic.

#### Error Handling Design
- Invalid requests, ID mismatches, null payloads, conflicts, and missing records are represented with domain-specific exceptions.
- Controllers should guard route/body mismatches and leave the rest of the error translation to the shared pipeline.

### 3. Repository Design

#### Repository Pattern
- Repository contracts live in `Domain.Contracts`.
- Repository implementations live in `Infrastructure.Repositories`.
- Shared CRUD and query behavior is centralized in `RepositoryBase<T>`.
- `RepositoryManager` acts as the composition point for feature repositories.
- Persistence is saved through the repository manager, not directly from controllers.

#### Repository Responsibilities
- Query entities from `CrmContext`.
- Expose reusable data access methods for feature services.
- Support tracked and non-tracked reads.
- Support EF-based CRUD operations and grid-oriented ADO queries.
- Avoid business rules; repositories only handle persistence concerns.

#### Repository Conventions
- Feature repositories inherit from `RepositoryBase<T>`.
- Single-record reads and collection reads are separated into distinct methods.
- Async data access is the default for web-facing operations.
- Cancellation tokens should flow through repository calls.

### 4. Coding Architecture

bdDevsCrm follows **Clean Architecture** with one-directional dependency flow:

```text
Presentation Layer     -> Presentation.Api / Presentation.Mvc / Presentation.Controller
Application Layer      -> Application.Services
Infrastructure Layer   -> Infrastructure.Repositories / Infrastructure.Sql / Infrastructure.Security / Infrastructure.Utilities
Domain Layer           -> Domain.Entities / Domain.Contracts / Domain.Exceptions
Shared Kernel          -> bdDevs.Shared
```

#### Layer Responsibilities
- **Presentation**: HTTP endpoints, MVC page routing, middleware bootstrapping, view delivery.
- **Application**: use cases, orchestration, validation, mapping, workflow rules.
- **Infrastructure**: EF Core, repositories, security helpers, caching, utility integrations.
- **Domain**: entities, contracts, and exceptions with no outward dependencies.
- **Shared Kernel**: API envelopes, constants, records, DTOs, extensions, and grid models.

#### Hard Rules
- Outer layers can depend on inner layers.
- Domain must not depend on Application, Infrastructure, or Presentation.
- Application must not depend on Infrastructure implementations.
- Controllers must not contain business logic.
- Shared contracts should be reused instead of redefining payload shapes per feature.

### 5. Form CRUD Workflow

The standard backend CRUD request flow is:

```text
Request
  -> Presentation.Controller action
  -> IServiceManager / feature service
  -> IRepositoryManager / feature repository
  -> CrmContext / SQL execution
  -> Database
```

#### CRUD Work Sequence
1. Define or confirm the DB-backed entity.
2. Define CRUD records in `bdDevs.Shared/Records`.
3. Define DTOs in `bdDevs.Shared/DataTransferObjects`.
4. Add repository interfaces in `Domain.Contracts/Repositories`.
5. Add service interfaces in `Domain.Contracts/Services`.
6. Implement repository methods in `Infrastructure.Repositories`.
7. Implement service logic in `Application.Services`.
8. Expose endpoints from `Presentation.Controller`.
9. Return unified API responses for reads, summaries, create, update, and delete actions.

#### Summary Endpoint Philosophy
- Grid pages use dedicated summary endpoints for server-side paging, sorting, and filtering.
- Single-record reads use explicit read endpoints.
- Create, update, and delete each keep separate routes and contracts.

### 6. Per Controller Responsibilities

#### Base API Controller
- Apply the shared base route.
- Enforce authentication and authorization attributes.
- Expose current user and correlation ID helpers.
- Provide consistent success, validation, and error response helpers.
- Support HATEOAS-style linked grid responses when needed.

#### Feature API Controllers
- Accept HTTP input and route parameters.
- Validate route-to-body key alignment.
- Call the appropriate service method.
- Return standardized response payloads.
- Avoid repository calls and domain logic directly.

#### MVC Controllers
- Return the correct Razor view for the module.
- Keep UI routing separate from backend business processing.
- Let JavaScript modules own client-side fetch, grid, and form behavior.

---

## Frontend Section

### 1. Naming Convention

#### JavaScript Naming Rules
- **Functions and variables** use `camelCase`.
- **Classes** use `PascalCase`.
- **Constants** use `UPPER_SNAKE_CASE`.
- Module globals follow feature-oriented names such as `PaymentMethodModule`.
- DOM selectors are grouped under a `dom` configuration object.

#### Frontend Naming Philosophy
- File names should map to feature purpose, such as `paymentMethodSettings.js`, `paymentMethodSummary.js`, and `paymentMethodDetails.js`.
- A module should keep one vocabulary across controller, view, JavaScript, and API endpoint names.
- Grid IDs, button IDs, and form IDs should remain feature-specific to avoid collisions.

### 2. Coding Design

#### UI Module Design
- MVC views define the shell: header, actions, grid host, popup window, and script references.
- JavaScript modules own configuration, API endpoints, field metadata, and Kendo initialization.
- Shared CRUD behavior is centralized in `crmSimpleCrudFactory.js`.
- API communication uses `fetch()` through `window.ApiClient`, not `jQuery.ajax()`.
- jQuery is used for DOM wiring and Kendo widget setup only.

#### Frontend Interaction Design
- Summary modules own the grid experience.
- Details modules own modal form rendering, validation, record loading, and save handling.
- Settings modules declare the schema for both grid columns and form fields.
- Authentication-aware client helpers redirect expired sessions to login.

### 3. Coding Architecture

The frontend architecture separates page delivery from interaction logic:

```text
Presentation.Mvc Controller
  -> Razor View
  -> Shared CRM JavaScript factory
  -> Feature settings / summary / details modules
  -> API endpoints under /bdDevs-crm
```

#### Frontend Layers in Practice
- **MVC Controller**: returns the page.
- **Razor View**: renders page structure and references scripts.
- **Feature JS Config**: defines IDs, endpoints, columns, fields, and payload builders.
- **Shared CRUD Factory**: provides reusable grid, modal, validation, and CRUD orchestration.
- **API Client**: wraps fetch, headers, token use, and error handling.

### 4. Per Controller Responsibilities

#### MVC Controllers
- Expose page entry points such as `Index()`.
- Return the feature view path only.
- Avoid embedding data-loading logic or HTTP integration details.

#### API Controllers Used by Frontend
- Serve the actual CRUD and summary endpoints.
- Return response envelopes the JavaScript client can interpret consistently.
- Keep endpoint behavior aligned with grid and modal expectations.

### 5. Form Philosophy and Grid Philosophy

#### Form Philosophy
- Forms are configuration-driven.
- Popup forms are rendered from field metadata instead of handwritten markup per field.
- Validation happens before submission and should fail fast with user-friendly feedback.
- Create and edit use the same form shell and change behavior by mode.
- Payload builders normalize types and nullable values before sending requests.

#### Grid Philosophy
- Grids are the default entry point for list management screens.
- Grid summaries favor server-side paging, sorting, and filtering for scalability.
- Grid actions should be limited to predictable operations such as add, refresh, edit, and delete.
- Empty states should clearly tell the user that no records were found.
- Grid configuration should remain declarative so features can share the same rendering pipeline.

#### Shared UX Principles
- Use a consistent header, action bar, grid shell, popup window, and save/cancel flow.
- Keep forms focused on record editing while grids focus on discovery and management.
- Reuse the same CRUD factory across CRM, DMS, and compatible System Admin pages whenever possible.

---

## Conclusion

This report defines the implementation baseline for documentation, onboarding, and future feature work in **bdDevsCrm**. New backend and frontend modules should follow these conventions so the repository remains consistent, scalable, and maintainable.
