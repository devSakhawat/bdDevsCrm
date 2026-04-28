# Non-Functional Documentation

## 1. Document Purpose
এই ডকুমেন্টটি bdDevsCrm প্রজেক্টের non-functional requirement baseline, quality attribute expectation, operational guardrail, security posture, performance target, maintainability rule এবং delivery governance একসাথে সংজ্ঞায়িত করে।

## 2. Current Solution Baseline
- Solution projects: 14
- Domain entities: 99
- Service interfaces: 89
- Repository interfaces: 90
- Service implementations: 88
- Validators: 89
- MVC controllers: 81
- API controllers: 85
- MVC views: 94
- Frontend module scripts: 243
- Verified baseline: `dotnet build Presentation.Api/Presentation.Api.csproj`, `dotnet build Presentation.Mvc/Presentation.Mvc.csproj`, `dotnet test bdDevsCrm.UnitTests/bdDevsCrm.UnitTests.csproj`, `dotnet test bdDevsCrm.IntegrationTests/bdDevsCrm.IntegrationTests.csproj`

## 3. Business Context
bdDevsCrm একটি enterprise-grade CRM + HRIS style solution shell যেখানে Core HR, Core System Administration, CRM এবং DMS module একীভূতভাবে কাজ করে। এই codebase-এর architecture, UI shell, API conventions এবং shared response model এমনভাবে সাজানো যে নতুন module যোগ করলেও platform consistency নষ্ট না হয়।

## 4. Quality Attribute Matrix
| Attribute | Expected Standard | Current Codebase Evidence | Delivery Expectation |
| --- | --- | --- | --- |
| Performance | Initial page load দ্রুত, API response সাধারণ business call-এ sub-second target, grid response scalable হতে হবে | `Presentation.Api/Program.cs` response compression, distributed cache, SQL context; `Presentation.Mvc/wwwroot/js/core/app.api.js` async API usage | Large list page-এ paging/filtering ব্যবহার করতে হবে, unnecessary payload avoid করতে হবে |
| Scalability | Multi-module growth support, shared patterns reuse, high user volume readiness | Layered solution, repository/service abstractions, cache configuration hooks, reusable frontend factories | New feature add করার সময় shared component reuse করতে হবে |
| Availability | API and MVC shell predictable pipeline-এ চলবে | `Presentation.Api/Program.cs` middleware pipeline, `Presentation.Mvc/Program.cs` MVC host pipeline | Runtime failure হলে graceful response / fallback থাকতে হবে |
| Reliability | Consistent response envelope, predictable validation, traceable requests | `bdDevs.Shared/ApiResponse/ApiResponse.cs`, `Presentation.Controller/Controllers/BaseApiController.cs` | Controllers and services must return traceable, consistent outcomes |
| Security | JWT auth, password security, request-level authorization, safe routing | `Presentation.Api/Program.cs` JWT + authorization policies, `Presentation.Controller/Controllers/BaseApiController.cs` `[Authorize]`, `Infrastructure.Security` project | Every new endpoint must respect auth, validation, and least privilege |
| Maintainability | Thin presentation layer, reusable services/repositories, modular docs | Clean architecture folder separation, shared JS core, reusable CRM CRUD factory | Any feature change must keep layer boundaries intact |
| Observability | Correlation-ready response, structured log support, audit screens | Correlation ID usage in `BaseApiController`, Serilog bootstrap in `Presentation.Api/Program.cs`, audit/log pages in Core & DMS | Incidents must be diagnosable from logs + audit screens |
| Usability | Shared header/sidebar/footer, repeatable page archetypes, consistent actions | `_Layout.cshtml`, `_Header.cshtml`, `_Sidebar.cshtml`, `_PageHeader.cshtml` | New screen must follow known page archetype |
| Testability | Automated validation path must exist | Dedicated unit/integration test projects | Business and API changes should stay testable with existing projects |
| Interoperability | API contract predictable for frontend and external clients | `/bdDevs-crm` route base, `ApiResponse<T>` envelope, `app.config.js` endpoint registry | Breaking route/response changes must be versioned and documented |
| Data Integrity | Input validation, typed DTOs/records, repository boundary | `Application.Services/Validators`, `bdDevs.Shared/Records`, repository + service contracts | All create/update flows must validate before persistence |
| Compliance & Audit | Sensitive operations traceable, token and access history reviewable | Audit, transaction log, password history, token blacklist, document access log pages | Compliance-critical changes must preserve traceability |
| Accessibility Readiness | Semantic HTML, labeled controls, keyboard-friendly shell | Header/sidebar use `aria-*` attributes; forms use labels and structured sections | New screens should keep accessible labels and navigable structure |
| Operational Support | Support teams should know which module/page does what | MVC view segmentation by module and detailed technical report (this documentation set) | Module changes must update the relevant doc section |

## 5. Detailed Non-Functional Requirements

### 5.1 Performance Requirements
1. Dashboard, login, and high-frequency master data pages should open quickly with minimal blocking assets.
2. Grid-heavy modules must support paging/filtering/sorting instead of full unbounded dataset rendering.
3. API responses should remain lightweight and wrapped in the shared `ApiResponse<T>` model.
4. Repeated lookups should prefer the configured caching stack where appropriate.
5. Expensive diagnostic or audit pages should expose filtering before large result rendering.

### 5.2 Scalability Requirements
1. New modules must plug into the existing Presentation → Application → Infrastructure → Domain structure.
2. Shared CRUD pages should reuse common frontend factories and backend service/repository conventions.
3. Configuration and master data should be centralized so that cross-module dependencies remain manageable.
4. Growth in HR, CRM, and DMS page count must not force a redesign of the shell; sidebar/header must remain extensible.
5. Route naming and API response shape must remain stable to avoid frontend cascade changes.

### 5.3 Security Requirements
1. API endpoints must stay under authenticated and authorized execution unless explicitly public.
2. All user/session sensitive flows must continue to use JWT and password security services.
3. Frontend pages must never bypass server authorization assumptions.
4. Input validation must happen before service execution and persistence.
5. Auditability must be preserved for access, password, token, transaction, and document activity.
6. Public MVC routes should remain intentionally limited (login and static assets).

### 5.4 Reliability and Error Handling Requirements
1. Errors must be reported in a consistent response structure with correlation support.
2. Validation problems must surface as actionable user/developer feedback rather than silent failures.
3. Middleware should fail predictably and not leave partial state when a request breaks.
4. CRUD pages should clearly signal load, success, warning, and failure states.
5. Authentication/session failures must redirect or instruct the user clearly.

### 5.5 Maintainability Requirements
1. Controllers remain thin and should mostly map page requests or delegate to services.
2. Business rules belong in Application/Domain-aligned code, not Razor or controller glue.
3. Shared shell changes must be isolated to shared partials and core JS/CSS.
4. Naming conventions must stay discoverable and consistent across modules.
5. Documentation must evolve with each new module/page so that onboarding cost stays low.

### 5.6 Usability and UX Consistency Requirements
1. Every business page should expose a clear title, breadcrumb, and top-right action area.
2. Common actions should stay predictable: Add/New, Refresh, Save, Cancel, Delete.
3. Complex workflows should use tabbed segmentation instead of one oversized form.
4. Master data pages should prefer a grid + modal pattern for speed and consistency.
5. Visual hierarchy should continue to distinguish dashboard, admin CRUD, workflow, and audit pages.

### 5.7 Observability and Support Requirements
1. Request tracing should keep correlation identifiers available end-to-end.
2. Logs should stay structured and environment-aware.
3. Audit-oriented pages must remain readable for operations and compliance teams.
4. Token, access, transaction, and password history data should stay explorable through dedicated screens.
5. Technical documentation must map a symptom to the right project/file/page quickly.

### 5.8 Test and Release Requirements
1. Existing build and test commands must stay green before release.
2. Module-level changes should be validated at API and MVC build level at minimum.
3. Test projects should remain runnable without feature-specific manual setup where possible.
4. Non-code documentation changes should still be checked for path, terminology, and module accuracy.

### 5.9 Operational Governance Requirements
1. Any new page must declare its module, purpose, primary activities, and UI pattern in project documentation.
2. Any new API contract change must update the technical report section on routes/response behavior.
3. Any new module activity/design change should be reflected first in functional documentation, then in technical report.
4. Architecture deviations should be documented with rationale before implementation spreads.

## 6. Daily Module Activity Documentation Rule
প্রতিদিন module-এর activity, page design, workflow, field grouping বা navigation-এ পরিবর্তন এলে নিচের update chain follow করতে হবে:
1. **Functional doc update** – page/business activity level change লিখতে হবে
2. **Technical report update** – কোন file/path/architecture layer impacted তা লিখতে হবে
3. **NFR impact note** – performance/security/usability/compliance-এ impact থাকলে এখানে যোগ করতে হবে
4. **Validation note** – build/test baseline affected কি না তা লিখতে হবে

## 7. Risk Register (Current-State Aware)
| Risk | Impact | Current Evidence | Control |
| --- | --- | --- | --- |
| Shared shell vs module-specific page style drift | UX inconsistency | Mixed legacy and new-style Razor pages | New work should align to shared shell/page archetypes |
| Duplicate controller names across namespaces | Discoverability confusion for developers | Multiple `DocumentTypeController` classes across Core/CRM/DMS | Use namespace + view path + module path in documentation and code review |
| Frontend auth middleware currently pass-through | Misunderstood security assumptions | `Presentation.Mvc/Middleware/AuthenticationCheckMiddleware.cs` | Keep API as enforcement boundary and document MVC behavior clearly |
| Kendo assets are scaffold-ready but commented in layout | Environment/setup mismatch | `Presentation.Mvc/Views/Shared/_Layout.cshtml` | Mention required asset readiness in onboarding and deployment steps |
| Large module growth can outpace docs | Onboarding slowdown | 90+ views / 80+ controllers already present | Update docs per module change as a release gate |

## 8. Acceptance Checklist for Future Work
- New page follows a known UI archetype
- New API respects `/bdDevs-crm` conventions and shared `ApiResponse<T>`
- Validation exists before persistence
- Security/auth assumptions are explicit
- Build/test baseline remains valid
- Functional + technical documentation are updated together
