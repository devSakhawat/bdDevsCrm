# Naming Conventions - bdDevsCrm

## Overview
This document defines the official naming conventions for the bdDevsCrm project. All code must follow these conventions to ensure consistency, readability, and maintainability across the codebase.

---

## C# Backend Naming Conventions

### 1. Classes and Interfaces

**Rule**: PascalCase

```csharp
// ✅ GOOD
public class UserService { }
public class CountryRepository { }
public interface IUserService { }
public interface ICountryRepository { }

// ❌ BAD
public class userService { }
public class country_repository { }
public interface IuserService { }
```

### 2. Methods and Properties

**Rule**: PascalCase, Async methods end with `Async` suffix

```csharp
// ✅ GOOD - Methods
public async Task<User> GetUserByIdAsync(int userId) { }
public void CalculateTotal() { }
public bool ValidateEmail(string email) { }

// ✅ GOOD - Properties
public string Username { get; set; }
public int UserId { get; set; }
public bool IsActive { get; set; }

// ❌ BAD
public async Task<User> getUserById(int userId) { }  // Missing Async suffix
public void calculate_total() { }  // snake_case not allowed
public string username { get; set; }  // Should be PascalCase
```

### 3. Private Fields

**Rule**: _camelCase (underscore prefix)

```csharp
// ✅ GOOD
private readonly IUserRepository _userRepository;
private readonly IMapper _mapper;
private readonly ILogger<UserService> _logger;
private string _cachedValue;

// ❌ BAD
private readonly IUserRepository userRepository;  // Missing underscore
private readonly IMapper _Mapper;  // Should be camelCase after underscore
private string CachedValue;  // Should use underscore prefix
```

### 4. Parameters and Local Variables

**Rule**: camelCase

```csharp
// ✅ GOOD
public void CreateUser(string username, int age, string emailAddress)
{
    var userId = GenerateId();
    var createdDate = DateTime.UtcNow;
    int totalCount = 0;
}

// ❌ BAD
public void CreateUser(string UserName, int Age)  // Should be camelCase
{
    var UserId = GenerateId();  // Should be camelCase
    var Created_Date = DateTime.UtcNow;  // No underscores
}
```

### 5. Constants

**Rule**: UPPER_SNAKE_CASE or PascalCase (prefer UPPER_SNAKE_CASE for clarity)

```csharp
// ✅ GOOD
public const string DEFAULT_ROLE = "User";
public const int MAX_LOGIN_ATTEMPTS = 5;
public const decimal TAX_RATE = 0.15m;

// Alternative (also acceptable)
public const string DefaultRole = "User";
public const int MaxLoginAttempts = 5;

// ❌ BAD
public const string default_role = "User";  // Should be uppercase
public const int maxLoginAttempts = 5;  // Should be uppercase or PascalCase
```

### 6. Acronyms and Abbreviations

**Rule**:
- 2 characters: ALL CAPS (ID, IO, UI)
- 3+ characters: PascalCase (Crm, Dms, Ielts, Toefl, Xml, Json, Http)

```csharp
// ✅ GOOD
public class CrmCourse { }
public class DmsDocument { }
public class IeltsInformation { }
public class ToeflInformation { }
public class HttpClient { }
public class XmlParser { }
public int UserID { get; set; }
public string APIURL { get; set; }

// ❌ BAD
public class CRMCourse { }  // Should be Crm (3+ chars = PascalCase)
public class DMSDocument { }  // Should be Dms
public class IELTSInformation { }  // Should be Ielts
public class TOEFLInformation { }  // Should be Toefl
public class XMLParser { }  // Should be Xml
public int UserId { get; set; }  // Should be UserID (2 chars)
```

### 7. Namespaces

**Rule**: PascalCase, match folder structure

```csharp
// ✅ GOOD
namespace Domain.Entities.CRM;
namespace Application.Services.Core.SystemAdmin;
namespace Infrastructure.Repositories;
namespace Presentation.Controller.Controllers.Core.HR;

// ❌ BAD
namespace domain.entities.crm;  // Should be PascalCase
namespace Application.Services.Core.System_Admin;  // No underscores
namespace Infrastructure.repositories;  // Should be PascalCase
```

### 8. Enums

**Rule**: Enum name = PascalCase (singular), Values = PascalCase

```csharp
// ✅ GOOD
public enum UserStatus
{
    Active,
    Inactive,
    Suspended,
    Deleted
}

public enum PaymentMethod
{
    Cash,
    CreditCard,
    BankTransfer
}

// ❌ BAD
public enum UserStatuses { }  // Should be singular
public enum paymentMethod { }  // Should be PascalCase
public enum Status
{
    active,  // Should be PascalCase
    in_active  // No underscores
}
```

### 9. Records (CRUD Pattern)

**Rule**: PascalCase + `Record` suffix, specific action prefix

```csharp
// ✅ GOOD
public record CreateCountryRecord(string CountryName, string CountryCode);
public record UpdateCountryRecord(int CountryId, string CountryName, string CountryCode);
public record DeleteCountryRecord(int CountryId, string Reason);

// ❌ BAD
public record CountryCreateRecord { }  // Wrong order
public record country_record { }  // Should be PascalCase
public record CreateCountry { }  // Missing Record suffix
```

### 10. Module Prefix Conventions

**Rule**: Every artifact that belongs to a business module must carry the module prefix so that its domain origin is immediately clear.

| Module | Prefix | Scope |
|---|---|---|
| CRM (Customer Relationship) | `Crm` | All entities, DTOs, Records, Repos, Services, Controllers inside `CRM/` |
| DMS (Document Management) | `Dms` | All artifacts inside `DMS/` |
| System / Core | *(no prefix)* | Shared system artifacts (Company, Branch, Users, etc.) |

```csharp
// ✅ GOOD – CRM module artifacts all carry the Crm prefix
public class CrmLead { }
public class CrmLeadDto { }
public record CreateCrmLeadRecord(...);
public interface ICrmLeadRepository { }
public class CrmLeadService { }
public class CrmLeadController { }

// ❌ BAD – prefix missing or wrong capitalisation
public class Lead { }          // Ambiguous – could belong to any module
public class CRMLead { }       // CRM is 3+ chars → must be PascalCase: Crm
public class crmlead { }       // Violates PascalCase rule

// ✅ GOOD – DMS module
public class DmsDocument { }
public class DmsDocumentDto { }

// ❌ BAD
public class DMSDocument { }   // Should be Dms
```

**Cross-reference:** The full per-layer pattern for CRM artifacts is locked in
`doc/EDUCATION_CRM_V1_IMPLEMENTATION_PACKAGE.md` § 5 "Naming Convention Lock".

---

### 11. DTOs (Data Transfer Objects)

**Rule**: PascalCase + `DTO` or `Dto` suffix (prefer `Dto` for consistency with acronym rules)

```csharp
// ✅ GOOD (Preferred - follows acronym rule)
public class UserDto { }
public class CountryDto { }
public class CreateUserDto { }

// ✅ ACCEPTABLE (Less preferred but common)
public class UserDTO { }
public class CountryDTO { }

// ❌ BAD
public class User_DTO { }  // No underscores
public class Userdto { }  // Should be UserDto or UserDTO
```

---

## JavaScript/Frontend Naming Conventions

### 1. Variables and Functions

**Rule**: camelCase

```javascript
// ✅ GOOD
const userName = "John Doe";
let userId = 123;
const isActive = true;

function getUserById(userId) { }
async function loadCountries() { }
const calculateTotal = (items) => { };

// ❌ BAD
const UserName = "John";  // Should be camelCase
let user_id = 123;  // No underscores
function GetUserById(userId) { }  // Should be camelCase
```

### 2. Classes and Constructors

**Rule**: PascalCase

```javascript
// ✅ GOOD
class UserManager { }
class ApiClient { }
class GridHelper { }

// ❌ BAD
class userManager { }  // Should be PascalCase
class api_client { }  // No underscores
```

### 3. Constants

**Rule**: UPPER_SNAKE_CASE

```javascript
// ✅ GOOD
const API_BASE_URL = "https://api.example.com";
const MAX_RETRY_ATTEMPTS = 3;
const DEFAULT_PAGE_SIZE = 20;
const AUTH_TOKEN_KEY = "authToken";

// ❌ BAD
const apiBaseUrl = "...";  // Should be uppercase
const Max_Retry_Attempts = 3;  // Should be fully uppercase
const default-page-size = 20;  // No hyphens, use underscores
```

### 4. Private Properties (Convention)

**Rule**: Prefix with underscore `_camelCase`

```javascript
// ✅ GOOD
class User {
    _privateField = null;
    _internalCache = {};

    get name() {
        return this._privateField;
    }
}

// Note: This is a convention only, not enforced by JavaScript
```

### 5. jQuery Selectors (Cached)

**Rule**: Prefix with `$`, camelCase

```javascript
// ✅ GOOD
const $grid = $("#userGrid");
const $form = $("#createUserForm");
const $submitButton = $("#btnSubmit");

// ❌ BAD
const grid = $("#userGrid");  // Missing $ prefix for jQuery object
const $submit_button = $("#btnSubmit");  // Should be camelCase
```

### 6. Event Handlers

**Rule**: Prefix with `on` or `handle`, camelCase

```javascript
// ✅ GOOD
function onUserClick(event) { }
function handleFormSubmit(e) { }
const onSaveButtonClick = () => { };

// ❌ BAD
function userClick(event) { }  // Should have on/handle prefix
function FormSubmit(e) { }  // Should be camelCase
```

### 7. Async Functions

**Rule**: No special suffix needed in JavaScript (unlike C#)

```javascript
// ✅ GOOD
async function getUser(userId) { }
async function loadData() { }
const fetchCountries = async () => { };

// ❌ BAD - DON'T copy C# convention
async function getUserAsync(userId) { }  // Async suffix not needed in JS
```

---

## File and Folder Naming Conventions

### 1. C# Files

**Rule**: PascalCase, match class name exactly, `.cs` extension

```
✅ GOOD
UserService.cs
CountryRepository.cs
IUserService.cs
CreateCountryRecord.cs

❌ BAD
userService.cs  // Should match class name (PascalCase)
User_Service.cs  // No underscores
user-service.cs  // No hyphens
UserService.CS  // Extension should be lowercase .cs
```

### 2. JavaScript Files

**Rule**: camelCase for utility files, PascalCase for modules/components

```
✅ GOOD
apiClient.js
validation.js
notification.js
countrySettings.js  // 3-file pattern
countryDetails.js
countrySummary.js

❌ BAD
ApiClient.js  // Should be camelCase for utilities
country_settings.js  // No underscores
country-details.js  // No hyphens
```

### 3. Folders/Directories

**Rule**: PascalCase, no spaces, no underscores, no special characters

```
✅ GOOD
/Domain/Entities/CRM/
/Application/Services/Core/SystemAdmin/
/Infrastructure/Repositories/
/wwwroot/js/modules/core/country/

❌ BAD
/domain/entities/  // Should be PascalCase
/Application/Services/Core/System_Admin/  // No underscores
/Infrastructure/Repositories/User Repos/  // No spaces
/wwwroot/js/modules/core/country-mgmt/  // No hyphens
```

### 4. Configuration Files

**Rule**: lowercase, hyphen-separated (kebab-case) for JSON/config files

```
✅ GOOD
appsettings.json
appsettings.Development.json
.editorconfig
.gitignore

❌ BAD
AppSettings.json  // Should be lowercase
app_settings.json  // Use dot notation, not underscores
```

---

## Database Naming Conventions

### 1. Table Names

**Rule**: PascalCase, singular or plural based on existing schema (prefer singular for new tables)

```sql
-- ✅ GOOD (Existing schema uses mix)
Users
Company
Branch
CrmCourse

-- ❌ BAD
user_table  -- No underscores
crm_courses  -- No underscores
```

### 2. Column Names

**Rule**: PascalCase

```sql
-- ✅ GOOD
UserId
UserName
CreatedDate
IsActive

-- ❌ BAD
user_id  -- No underscores
USERNAME  -- Should be PascalCase
is_active  -- No underscores
```

### 3. Foreign Keys

**Rule**: Referenced table name + `Id` suffix

```sql
-- ✅ GOOD
UserId  -- References Users.UserId
CompanyId  -- References Company.CompanyId
BranchId

-- ❌ BAD
User_Id  -- No underscores
UserID  -- ID should be Id (not all caps)
FK_UserId  -- No FK_ prefix needed
```

---

## API Endpoint Naming Conventions

### 1. Route Naming

**Rule**: kebab-case, lowercase, plural nouns for collections

```csharp
// ✅ GOOD
[Route("api/users")]
[Route("api/countries")]
[Route("api/crm/courses")]
[Route("api/system-admin/users")]

// ❌ BAD
[Route("api/Users")]  // Should be lowercase
[Route("api/user")]  // Should be plural
[Route("api/CRM/Courses")]  // Should be lowercase
[Route("api/system_admin/users")]  // Use hyphens, not underscores
```

### 2. Action Methods

**Rule**: PascalCase, HTTP verb prefix optional

```csharp
// ✅ GOOD
[HttpGet]
public async Task<IActionResult> GetAll() { }

[HttpGet("{id}")]
public async Task<IActionResult> GetById(int id) { }

[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateUserRecord record) { }

// ❌ BAD
[HttpGet]
public async Task<IActionResult> get_all() { }  // Should be PascalCase
```

---

## Special Patterns

### 1. 3-File JavaScript Pattern

Each menu/module has 3 files with specific suffixes:

```
✅ GOOD Structure
/wwwroot/js/modules/core/country/
    countrySettings.js   // Document ready, initialization
    countryDetails.js    // CRUD operations, form handling
    countrySummary.js    // Grid, data fetching, display

❌ BAD
/wwwroot/js/modules/core/country/
    country.js  // Missing pattern specificity
    countryInit.js  // Wrong suffix
    countryGrid.js  // Should be countrySummary.js
```

### 2. Repository Pattern

```csharp
// ✅ GOOD
public interface IUserRepository : IRepositoryBase<User> { }
public class UserRepository : RepositoryBase<User>, IUserRepository { }

public interface ICountryRepository : IRepositoryBase<Country> { }
public class CountryRepository : RepositoryBase<Country>, ICountryRepository { }

// ❌ BAD
public interface IUserRepo { }  // Use full word Repository
public class UserDataAccess { }  // Should follow Repository naming
```

---

## Validation and Enforcement

### Code Review Checklist

Before committing code, verify:

- [ ] All C# classes, methods, properties use PascalCase
- [ ] All private fields use `_camelCase`
- [ ] All parameters and local variables use camelCase
- [ ] Acronyms follow the 2-char/3+-char rule (`Crm`, `Dms`, not `CRM`, `DMS`)
- [ ] CRM module artifacts carry the `Crm` prefix on every layer (entity → controller)
- [ ] DMS module artifacts carry the `Dms` prefix on every layer
- [ ] JavaScript variables and functions use camelCase
- [ ] JavaScript constants use UPPER_SNAKE_CASE
- [ ] File names match class names exactly
- [ ] Folder names use PascalCase with no spaces/underscores
- [ ] No inconsistent naming within same scope

### Automated Tools

Consider using:
- **EditorConfig**: Enforce naming conventions
- **StyleCop**: C# code style analyzer
- **ESLint**: JavaScript linting
- **ReSharper/Rider**: IDE-based naming convention checks

---

## Examples Summary

### C# Complete Example

```csharp
namespace Application.Services.Core.SystemAdmin;

/// <summary>
/// Service for managing user operations
/// </summary>
public class UserService : IUserService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    private const string DEFAULT_ROLE = "User";
    private const int MAX_LOGIN_ATTEMPTS = 5;

    public UserService(
        IRepositoryManager repository,
        IMapper mapper,
        ILogger<UserService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UserDto> GetUserByIdAsync(int userId)
    {
        var user = await _repository.Users.GetByIdAsync(userId);
        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> CreateUserAsync(CreateUserRecord record)
    {
        // Implementation
    }
}
```

### JavaScript Complete Example

```javascript
// Constants
const API_BASE_URL = "/api/users";
const DEFAULT_PAGE_SIZE = 20;

// Class
class UserManager {
    constructor() {
        this._cache = {};
        this._initialized = false;
    }

    // Methods
    async loadUsers() {
        const response = await ApiClient.get(API_BASE_URL);
        return response.data;
    }

    handleUserClick(event) {
        const userId = event.target.dataset.userId;
        this.editUser(userId);
    }
}

// Functions
async function getUserById(userId) {
    const response = await ApiClient.get(`${API_BASE_URL}/${userId}`);
    return response.data;
}

function validateEmail(email) {
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailPattern.test(email);
}

// Event handlers
function onFormSubmit(event) {
    event.preventDefault();
    const formData = new FormData(event.target);
    saveUser(formData);
}
```

---

## References

- Microsoft .NET Naming Guidelines: https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/naming-guidelines
- Google JavaScript Style Guide: https://google.github.io/styleguide/jsguide.html
- Clean Code by Robert C. Martin

---

**Last Updated**: 2026-04-15
**Version**: 1.0.0
**Status**: ✅ Official Naming Convention Standard
