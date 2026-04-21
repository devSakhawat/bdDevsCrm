# API Endpoints Documentation
**bdDevsCrm - Enterprise CRM System**

> **Generated:** 2026-04-21
> **Base Route:** `bdDevs-crm`
> **API Version:** v1.0

---

## Table of Contents

1. [Authentication](#authentication)
2. [Core - System Admin](#core---system-admin)
   - [Module Management](#module-management)
   - [Menu Management](#menu-management)
   - [Country Management](#country-management)
   - [Currency Management](#currency-management)
   - [Group Management](#group-management)
   - [User Management](#user-management)
   - [Access Control](#access-control)
   - [Workflow](#workflow)
   - [Company Management](#company-management)
   - [Holiday Management](#holiday-management)
   - [Timesheet Management](#timesheet-management)
   - [Currency Rate Management](#currency-rate-management)
   - [Thana Management](#thana-management)
   - [Document Management](#document-management)
   - [Audit & Logging](#audit--logging)
   - [System Settings](#system-settings)
3. [Core - HR](#core---hr)
4. [CRM Module](#crm-module)
5. [DMS Module](#dms-module)
6. [Workflow Management](#workflow-management)

---

## Authentication

### Base Route
`/bdDevs-crm`

### Endpoints

#### 1. Login
- **Route:** `POST /login`
- **Controller:** `AuthenticationController`
- **Method:** `Authenticate([FromBody] UserForAuthenticationDto user)`
- **Authorization:** `[AllowAnonymous]`
- **Description:** Authenticates user and returns JWT access token and refresh token
- **Request Body:**
  ```json
  {
    "loginId": "string",
    "password": "string"
  }
  ```
- **Response:**
  ```json
  {
    "correlationId": "string",
    "statusCode": 200,
    "success": true,
    "message": "Login successful",
    "data": {
      "accessToken": "string",
      "refreshToken": "string (HTTP-only cookie)",
      "accessTokenExpiry": "datetime",
      "refreshTokenExpiry": "datetime",
      "tokenType": "Bearer",
      "expiresIn": 1800,
      "userSession": { ... }
    }
  }
  ```

#### 2. Refresh Token
- **Route:** `POST /refresh-token`
- **Controller:** `AuthenticationController`
- **Method:** `RefreshToken()`
- **Authorization:** `[AllowAnonymous]`
- **Description:** Refreshes expired access token using refresh token from HTTP-only cookie
- **Response:** New access token and refresh token

#### 3. Revoke Token
- **Route:** `POST /revoke-token`
- **Controller:** `AuthenticationController`
- **Method:** `RevokeToken()`
- **Authorization:** `[AllowAnonymous]`
- **Description:** Revokes refresh token to invalidate user session
- **Response:** Success message

#### 4. Get User Info
- **Route:** `GET /user-info`
- **Controller:** `AuthenticationController`
- **Method:** `UserInfo()`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Retrieves current authenticated user's information
- **Response:** User DTO with profile details (password cleared)

#### 5. Logout
- **Route:** `POST /logout`
- **Controller:** `AuthenticationController`
- **Method:** `Logout()`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Logs out user, revokes all tokens, clears cache
- **Response:** Success message

---

## Core - System Admin

### Module Management

**Base Route:** `/bdDevs-crm`

#### CUD Operations

##### 1. Create Module
- **Route:** `POST /module`
- **Controller:** `ModuleController`
- **Method:** `CreateModuleAsync([FromBody] ModuleDto modelDto, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Creates a new module record
- **Request Body:** `ModuleDto`
- **Response:** Created module with ID

##### 2. Update Module
- **Route:** `PUT /module/{key}`
- **Controller:** `ModuleController`
- **Method:** `UpdateModuleAsync([FromRoute] int key, [FromBody] ModuleDto modelDto, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Updates an existing module record
- **Path Parameter:** `key` (int) - Module ID
- **Request Body:** `ModuleDto`
- **Response:** Updated module

##### 3. Delete Module
- **Route:** `DELETE /module/{key}`
- **Controller:** `ModuleController`
- **Method:** `DeleteModuleAsync([FromRoute] int key, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Deletes a module by ID
- **Path Parameter:** `key` (int) - Module ID
- **Response:** Success message

#### Read Operations

##### 4. Module Summary (Grid/Paginated)
- **Route:** `POST /module-summary`
- **Controller:** `ModuleController`
- **Method:** `ModuleSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Retrieves paginated summary grid of modules
- **Request Body:** `GridOptions` (page, pageSize, sort, filter)
- **Response:** `GridEntity<ModuleDto>` with pagination metadata

##### 5. Get All Modules
- **Route:** `GET /modules`
- **Controller:** `ModuleController`
- **Method:** `ModulesAsync(CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Cache:** 60 seconds
- **Description:** Retrieves all modules (list view)
- **Response:** `IEnumerable<ModuleDto>`

##### 6. Get Module by ID
- **Route:** `GET /module/{id:int}`
- **Controller:** `ModuleController`
- **Method:** `ModuleAsync([FromRoute] int id, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Retrieves a single module by ID
- **Path Parameter:** `id` (int) - Module ID
- **Response:** `ModuleDto`

##### 7. Modules for Dropdown
- **Route:** `GET /modules-ddl`
- **Controller:** `ModuleController`
- **Method:** `ModulesForDDLAsync(CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Retrieves modules for dropdown list (ID + Name only)
- **Response:** `IEnumerable<ModuleForDDLDto>`

---

### Menu Management

**Base Route:** `/bdDevs-crm`

#### CUD Operations

##### 1. Create Menu
- **Route:** `POST /menu`
- **Controller:** `MenuController`
- **Method:** `CreateMenu([FromBody] MenuDto modelDto, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Creates a new menu item
- **Request Body:** `MenuDto`
- **Response:** Created menu with ID

##### 2. Update Menu
- **Route:** `PUT /menu/{key}`
- **Controller:** `MenuController`
- **Method:** `UpdateMenuAsync([FromRoute] int key, [FromBody] MenuDto modelDto, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Updates an existing menu item
- **Path Parameter:** `key` (int) - Menu ID
- **Request Body:** `MenuDto`
- **Response:** Updated menu

##### 3. Delete Menu
- **Route:** `DELETE /menu/{key}`
- **Controller:** `MenuController`
- **Method:** `DeleteMenuAsync([FromRoute] int key, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Deletes a menu by ID
- **Path Parameter:** `key` (int) - Menu ID
- **Response:** Success message

#### Read Operations

##### 4. Menu Summary (Grid/Paginated with HATEOAS)
- **Route:** `POST /menu-summary`
- **Controller:** `MenuController`
- **Method:** `MenuSummary([FromBody] GridOptions options, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Retrieves paginated summary grid of menus with HATEOAS links
- **Request Body:** `GridOptions`
- **Response:** `GridEntity<LinkedResource<MenuDto>>` with resource links

##### 5. Get All Menus
- **Route:** `GET /menus`
- **Controller:** `MenuController`
- **Method:** `ReadMenus(CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Cache:** 60 seconds
- **Description:** Retrieves all menus
- **Response:** `IEnumerable<MenuDto>`

##### 6. Menus for Dropdown
- **Route:** `GET /menus-ddl`
- **Controller:** `MenuController`
- **Method:** `MenusDDL(CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Retrieves menus for dropdown list
- **Response:** `IEnumerable<MenuForDDLDto>`

##### 7. Menus by User Permission
- **Route:** `GET /menus-user-permission`
- **Controller:** `MenuController`
- **Method:** `MenusByUserPermission(CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Cache:** 300 seconds (L1 Memory Cache)
- **Description:** Retrieves menus accessible by current authenticated user based on permissions
- **Response:** `IEnumerable<MenuDto>`
- **Note:** Uses caching to prevent redundant permission checks

##### 8. Menus by Module ID
- **Route:** `GET /menus-moduleId/{moduleId:int}`
- **Controller:** `MenuController`
- **Method:** `MenusByModuleId([FromRoute] int moduleId, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Cache:** 60 seconds
- **Description:** Retrieves all menus belonging to a specific module
- **Path Parameter:** `moduleId` (int)
- **Response:** `IEnumerable<MenuDto>`

##### 9. Parent Menus by Menu ID
- **Route:** `GET /parent-by-menu/{parentMenuId:int}`
- **Controller:** `MenuController`
- **Method:** `ParentMenuByMenu(int menuId, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Retrieves parent menu hierarchy for a given menu
- **Path Parameter:** `parentMenuId` (int)
- **Response:** `IEnumerable<MenuDto>`

##### 10. Get Menu by ID
- **Route:** `GET /menu/{menuId:int}`
- **Controller:** `MenuController`
- **Method:** `ReadMenu(int menuId, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Cache:** 60 seconds
- **Description:** Retrieves a single menu by ID
- **Path Parameter:** `menuId` (int)
- **Response:** `MenuDto`

---

### Country Management

**Base Route:** `/bdDevs-crm`

#### CUD Operations

##### 1. Create Country
- **Route:** `POST /country`
- **Controller:** `CountryController`
- **Method:** `CreateCountryAsync([FromBody] CreateCountryRecord record, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Creates a new country record using CRUD Record pattern
- **Request Body:** `CreateCountryRecord`
- **Response:** Created `CrmCountryDto`

##### 2. Update Country
- **Route:** `PUT /country/{key}`
- **Controller:** `CountryController`
- **Method:** `UpdateCountryAsync([FromRoute] int key, [FromBody] UpdateCountryRecord record, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Updates an existing country record using CRUD Record pattern
- **Path Parameter:** `key` (int) - Country ID
- **Request Body:** `UpdateCountryRecord`
- **Response:** Updated `CrmCountryDto`

##### 3. Delete Country
- **Route:** `DELETE /country/{key}`
- **Controller:** `CountryController`
- **Method:** `DeleteCountryAsync([FromRoute] int key, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Deletes a country by ID using CRUD Record pattern
- **Path Parameter:** `key` (int) - Country ID
- **Response:** Success message

#### Read Operations

##### 4. Country Summary (Grid/Paginated)
- **Route:** `POST /country-summary`
- **Controller:** `CountryController`
- **Method:** `CountrySummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Retrieves paginated summary grid of countries
- **Request Body:** `GridOptions`
- **Response:** `GridEntity<CrmCountryDto>`

##### 5. Get All Countries
- **Route:** `GET /countries`
- **Controller:** `CountryController`
- **Method:** `CountryAsync([FromRoute] int countryId, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Retrieves all countries
- **Path Parameter:** `countryId` (int)
- **Response:** `CrmCountryDto`

##### 6. Country Dropdown List
- **Route:** `GET /countryddl`
- **Controller:** `CountryController`
- **Method:** `CountriesForDDLAsync(CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Cache:** 300 seconds
- **Description:** Retrieves countries for dropdown list
- **Response:** `IEnumerable<CrmCountryDDL>`

##### 7. Get Country by ID
- **Route:** `GET /country/{countryId:int}`
- **Controller:** `CountryController`
- **Method:** `CountryAsync([FromRoute] int countryId, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Retrieves a single country by ID
- **Path Parameter:** `countryId` (int)
- **Response:** `CrmCountryDto`

---

### Currency Management

**Base Route:** `/bdDevs-crm`

#### CUD Operations

##### 1. Create Currency
- **Route:** `POST /currency`
- **Controller:** `CurrencyController`
- **Method:** `CreateCurrencyAsync([FromBody] CurrencyDto modelDto, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Creates a new currency record
- **Request Body:** `CurrencyDto`
- **Response:** Created `CurrencyDto`

##### 2. Update Currency
- **Route:** `PUT /currency/{key}`
- **Controller:** `CurrencyController`
- **Method:** `UpdateCurrencyAsync([FromRoute] int key, [FromBody] CurrencyDto modelDto, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Updates an existing currency record
- **Path Parameter:** `key` (int) - Currency ID
- **Request Body:** `CurrencyDto`
- **Response:** Updated `CurrencyDto`

##### 3. Delete Currency
- **Route:** `DELETE /currency/{key}`
- **Controller:** `CurrencyController`
- **Method:** `DeleteCurrencyAsync([FromRoute] int key, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Deletes a currency by ID
- **Path Parameter:** `key` (int) - Currency ID
- **Response:** Success message

#### Read Operations

##### 4. Currency Summary (Grid/Paginated)
- **Route:** `POST /currency-summary`
- **Controller:** `CurrencyController`
- **Method:** `CurrencySummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Retrieves paginated summary grid of currencies
- **Request Body:** `GridOptions`
- **Response:** `GridEntity<CurrencyDto>`

##### 5. Get All Currencies
- **Route:** `GET /currencies`
- **Controller:** `CurrencyController`
- **Method:** `ReadCurrenciesAsync(CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Cache:** 60 seconds
- **Description:** Retrieves all currencies
- **Response:** `IEnumerable<CurrencyDto>`

##### 6. Currency Dropdown List
- **Route:** `GET /currencies-ddl`
- **Controller:** `CurrencyController`
- **Method:** `CurrenciesForDDLAsync(CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Cache:** 300 seconds
- **Description:** Retrieves currencies for dropdown list
- **Response:** `IEnumerable<CurrencyDDLDto>`

##### 7. Get Currency by ID
- **Route:** `GET /currency/{id:int}`
- **Controller:** `CurrencyController`
- **Method:** `CurrencyAsync([FromRoute] int id, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Cache:** 60 seconds
- **Description:** Retrieves a single currency by ID
- **Path Parameter:** `id` (int) - Currency ID
- **Response:** `CurrencyDto`

---

### Group Management

**Base Route:** `/bdDevs-crm`

#### CUD Operations

##### 1. Create Group
- **Route:** `POST /group`
- **Controller:** `GroupController`
- **Method:** `CreateGroupAsync([FromBody] GroupDto modelDto, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Creates a new group
- **Request Body:** `GroupDto`
- **Response:** Created `GroupDto`

##### 2. Update Group
- **Route:** `PUT /group/{key}`
- **Controller:** `GroupController`
- **Method:** `UpdateGroupAsync([FromRoute] int key, [FromBody] GroupDto modelDto, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Updates an existing group
- **Path Parameter:** `key` (int) - Group ID
- **Request Body:** `GroupDto`
- **Response:** Updated `GroupDto`

##### 3. Delete Group
- **Route:** `DELETE /group/{key}`
- **Controller:** `GroupController`
- **Method:** `DeleteGroupAsync([FromRoute] int key, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Deletes a group by ID
- **Path Parameter:** `key` (int) - Group ID
- **Response:** Success message

#### Read Operations

##### 4. Group Summary (Grid/Paginated)
- **Route:** `POST /group-summary`
- **Controller:** `GroupController`
- **Method:** `GroupSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Retrieves paginated summary grid of groups
- **Request Body:** `GridOptions`
- **Response:** `GridEntity<GroupSummaryDto>`

##### 5. Group Permissions by Group ID
- **Route:** `GET /group-permissions/{groupId:int}`
- **Controller:** `GroupController`
- **Method:** `GroupPermissionsByGroupIdAsync([FromRoute] int groupId, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Retrieves all permissions for a specific group
- **Path Parameter:** `groupId` (int)
- **Response:** `IEnumerable<GroupPermissionDto>`

##### 6. Access Controls
- **Route:** `GET /access-controls`
- **Controller:** `GroupController`
- **Method:** `AccessControlsAsync(CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Cache:** 300 seconds
- **Description:** Retrieves all available access controls (reference data)
- **Response:** `IEnumerable<AccessControlDto>`

##### 7. Groups for Dropdown
- **Route:** `GET /groups-ddl`
- **Controller:** `GroupController`
- **Method:** `GroupsForDDLAsync(CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Cache:** 300 seconds
- **Description:** Retrieves groups for dropdown list
- **Response:** `IEnumerable<GroupDDLDto>`

##### 8. Get Group by ID
- **Route:** `GET /group/{groupId:int}`
- **Controller:** `GroupController`
- **Method:** `GroupAsync([FromRoute] int groupId, CancellationToken cancellationToken)`
- **Authorization:** `[AuthorizeUser]`
- **Cache:** 60 seconds
- **Description:** Retrieves a single group by ID
- **Path Parameter:** `groupId` (int)
- **Response:** `GroupDto`

---

### User Management

**Base Route:** `/bdDevs-crm`

#### Endpoints

##### 1. Create User
- **Route:** `POST /user`
- **Route Constant:** `RouteConstants.CreateUser`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Creates a new user account

##### 2. Update User
- **Route:** `PUT /user/{key}`
- **Route Constant:** `RouteConstants.UpdateUser`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Updates an existing user account

##### 3. Delete User
- **Route:** `DELETE /user/{key}`
- **Route Constant:** `RouteConstants.DeleteUser`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Deletes a user account

##### 4. User Summary (Grid/Paginated)
- **Route:** `POST /user-summary`
- **Route Constant:** `RouteConstants.UserSummary`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Retrieves paginated summary grid of users

##### 5. Get All Users
- **Route:** `GET /users`
- **Route Constant:** `RouteConstants.ReadUsers`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Retrieves all users

##### 6. Users for Dropdown
- **Route:** `GET /users-ddl`
- **Route Constant:** `RouteConstants.UserForDDL`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Retrieves users for dropdown list

##### 7. Get User by ID
- **Route:** `GET /user/{id:int}`
- **Route Constant:** `RouteConstants.ReadUser`
- **Authorization:** `[AuthorizeUser]`
- **Description:** Retrieves a single user by ID

---

### Company Management

**Base Route:** `/bdDevs-crm`

#### Endpoints

##### 1. Create Company
- **Route:** `POST /company`
- **Route Constant:** `RouteConstants.CreateCompany`

##### 2. Update Company
- **Route:** `PUT /company/{key}`
- **Route Constant:** `RouteConstants.UpdateCompany`

##### 3. Delete Company
- **Route:** `DELETE /company/{key}`
- **Route Constant:** `RouteConstants.DeleteCompany`

##### 4. Get All Companies
- **Route:** `GET /companies`
- **Route Constant:** `RouteConstants.Companies`

##### 5. Companies for Dropdown
- **Route:** `GET /companies-ddl`
- **Route Constant:** `RouteConstants.CompaniesDDL`

##### 6. Get Mother Companies
- **Route:** `GET /mother-companies`
- **Route Constant:** `RouteConstants.MotherCompany`

##### 7. Get Company by Key
- **Route:** `GET /company/key/{key}`
- **Route Constant:** `RouteConstants.ReadCompany`

---

### Holiday Management

**Base Route:** `/bdDevs-crm`

#### Endpoints

##### 1. Create Holiday
- **Route:** `POST /holiday`
- **Route Constant:** `RouteConstants.CreateHoliday`

##### 2. Update Holiday
- **Route:** `PUT /holiday/{key}`
- **Route Constant:** `RouteConstants.UpdateHoliday`

##### 3. Delete Holiday
- **Route:** `DELETE /holiday/{key}`
- **Route Constant:** `RouteConstants.DeleteHoliday`

##### 4. Holiday Summary
- **Route:** `POST /holiday-summary`
- **Route Constant:** `RouteConstants.HolidaySummary`

##### 5. Get All Holidays
- **Route:** `GET /holidays`
- **Route Constant:** `RouteConstants.ReadHolidays`

##### 6. Holidays Dropdown
- **Route:** `GET /holidays-ddl`
- **Route Constant:** `RouteConstants.HolidayDDL`

##### 7. Get Holiday by ID
- **Route:** `GET /holiday/{id:int}`
- **Route Constant:** `RouteConstants.ReadHoliday`

---

### Timesheet Management

**Base Route:** `/bdDevs-crm`

#### Endpoints

##### 1. Create Timesheet
- **Route:** `POST /timesheet`
- **Route Constant:** `RouteConstants.CreateTimesheet`

##### 2. Update Timesheet
- **Route:** `PUT /timesheet/{key}`
- **Route Constant:** `RouteConstants.UpdateTimesheet`

##### 3. Delete Timesheet
- **Route:** `DELETE /timesheet/{key}`
- **Route Constant:** `RouteConstants.DeleteTimesheet`

##### 4. Timesheet Summary
- **Route:** `POST /timesheet-summary`
- **Route Constant:** `RouteConstants.TimesheetSummary`

##### 5. Get All Timesheets
- **Route:** `GET /timesheets`
- **Route Constant:** `RouteConstants.ReadTimesheets`

##### 6. Timesheets Dropdown
- **Route:** `GET /timesheets-ddl`
- **Route Constant:** `RouteConstants.TimesheetDDL`

##### 7. Get Timesheet by ID
- **Route:** `GET /timesheet/{id:int}`
- **Route Constant:** `RouteConstants.ReadTimesheet`

##### 8. Timesheets by Employee
- **Route:** `GET /timesheets-by-employee/{hrRecordId:int}`
- **Route Constant:** `RouteConstants.TimesheetsByEmployee`

---

### Currency Rate Management

**Base Route:** `/bdDevs-crm`

#### Endpoints

##### 1. Create Currency Rate
- **Route:** `POST /currency-rate`
- **Route Constant:** `RouteConstants.CreateCurrencyRate`

##### 2. Update Currency Rate
- **Route:** `PUT /currency-rate/{key}`
- **Route Constant:** `RouteConstants.UpdateCurrencyRate`

##### 3. Delete Currency Rate
- **Route:** `DELETE /currency-rate/{key}`
- **Route Constant:** `RouteConstants.DeleteCurrencyRate`

##### 4. Currency Rate Summary
- **Route:** `POST /currency-rate-summary`
- **Route Constant:** `RouteConstants.CurrencyRateSummary`

##### 5. Get All Currency Rates
- **Route:** `GET /currency-rates`
- **Route Constant:** `RouteConstants.ReadCurrencyRates`

##### 6. Currency Rates Dropdown
- **Route:** `GET /currency-rates-ddl`
- **Route Constant:** `RouteConstants.CurrencyRateDDL`

##### 7. Get Currency Rate by ID
- **Route:** `GET /currency-rate/{id:int}`
- **Route Constant:** `RouteConstants.ReadCurrencyRate`

##### 8. Currency Rates by Currency
- **Route:** `GET /currency-rates-by-currency/{currencyId:int}`
- **Route Constant:** `RouteConstants.CurrencyRatesByCurrency`

---

### Thana Management

**Base Route:** `/bdDevs-crm`

#### Endpoints

##### 1. Create Thana
- **Route:** `POST /thana`
- **Route Constant:** `RouteConstants.CreateThana`

##### 2. Update Thana
- **Route:** `PUT /thana/{key}`
- **Route Constant:** `RouteConstants.UpdateThana`

##### 3. Delete Thana
- **Route:** `DELETE /thana/{key}`
- **Route Constant:** `RouteConstants.DeleteThana`

##### 4. Thana Summary
- **Route:** `POST /thana-summary`
- **Route Constant:** `RouteConstants.ThanaSummary`

##### 5. Get All Thanas
- **Route:** `GET /thanas`
- **Route Constant:** `RouteConstants.ReadThanas`

##### 6. Thanas Dropdown
- **Route:** `GET /thanas-ddl`
- **Route Constant:** `RouteConstants.ThanaDDL`

##### 7. Get Thana by ID
- **Route:** `GET /thana/{id:int}`
- **Route Constant:** `RouteConstants.ReadThana`

##### 8. Thanas by District
- **Route:** `GET /thanas-by-district/{districtId:int}`
- **Route Constant:** `RouteConstants.ThanasByDistrict`

---

### Access Control

**Base Route:** `/bdDevs-crm`

#### Endpoints

##### 1. Create Access Control
- **Route:** `POST /access-control`
- **Route Constant:** `RouteConstants.CreateAccessControl`

##### 2. Update Access Control
- **Route:** `PUT /access-control/{key}`
- **Route Constant:** `RouteConstants.UpdateAccessControl`

##### 3. Delete Access Control
- **Route:** `DELETE /access-control/{key}`
- **Route Constant:** `RouteConstants.DeleteAccessControl`

##### 4. Access Control Summary
- **Route:** `POST /access-control-summary`
- **Route Constant:** `RouteConstants.AccessControlSummary`

##### 5. Get All Access Controls
- **Route:** `GET /access-controls`
- **Route Constant:** `RouteConstants.ReadAccessControls`

##### 6. Get Access Control by Key
- **Route:** `GET /access-control/key/{key:int}`
- **Route Constant:** `RouteConstants.ReadAccessControl`

---

### Workflow

**Base Route:** `/bdDevs-crm`

#### Workflow State Endpoints

##### 1. Create Workflow State
- **Route:** `POST /workflow-state`
- **Route Constant:** `RouteConstants.CreateWorkflowState`

##### 2. Update Workflow State
- **Route:** `PUT /workflow-state/{key}`
- **Route Constant:** `RouteConstants.UpdateWorkflowState`

##### 3. Delete Workflow State
- **Route:** `DELETE /workflow-state/{key}`
- **Route Constant:** `RouteConstants.DeleteWorkflowState`

##### 4. Workflow Summary
- **Route:** `POST /workflow-summary`
- **Route Constant:** `RouteConstants.WorkflowSummary`

##### 5. Statuses by Menu ID
- **Route:** `GET /statuses-by-menu/{menuId:int}`
- **Route Constant:** `RouteConstants.StatusByMenuId`

##### 6. Status by Menu Name
- **Route:** `GET /status/menu/{key}`
- **Route Constant:** `RouteConstants.StatusByMenuName`

##### 7. Status by Menu ID & User ID
- **Route:** `GET /Status-MenuId-UserId`
- **Route Constant:** `RouteConstants.StatusByMenuNUserId`

#### Workflow Action Endpoints

##### 8. Create Workflow Action
- **Route:** `POST /workflow-action`
- **Route Constant:** `RouteConstants.CreateWorkflowAction`

##### 9. Update Workflow Action
- **Route:** `PUT /workflow-action/{key}`
- **Route Constant:** `RouteConstants.UpdateWorkflowAction`

##### 10. Delete Workflow Action
- **Route:** `DELETE /workflow-action/{key}`
- **Route Constant:** `RouteConstants.DeleteWorkflowAction`

##### 11. Actions by Status ID for Group
- **Route:** `GET /actions-by-status/{statusId:int}`
- **Route Constant:** `RouteConstants.ActionsByStatusIdForGroup`

---

### Document Management

**Base Route:** `/bdDevs-crm`

#### Document Template

##### 1. Create Document Template
- **Route:** `POST /document-template`
- **Route Constant:** `RouteConstants.CreateDocumentTemplate`

##### 2. Update Document Template
- **Route:** `PUT /document-template/{key}`
- **Route Constant:** `RouteConstants.UpdateDocumentTemplate`

##### 3. Delete Document Template
- **Route:** `DELETE /document-template/{key}`
- **Route Constant:** `RouteConstants.DeleteDocumentTemplate`

##### 4. Document Template Summary
- **Route:** `POST /document-template-summary`
- **Route Constant:** `RouteConstants.DocumentTemplateSummary`

##### 5. Get All Document Templates
- **Route:** `GET /document-templates`
- **Route Constant:** `RouteConstants.ReadDocumentTemplates`

##### 6. Document Templates Dropdown
- **Route:** `GET /document-templates-ddl`
- **Route Constant:** `RouteConstants.DocumentTemplateDDL`

##### 7. Get Document Template by ID
- **Route:** `GET /document-template/{id:int}`
- **Route Constant:** `RouteConstants.ReadDocumentTemplate`

#### Document Type

##### 8. Create Document Type
- **Route:** `POST /document-type`
- **Route Constant:** `RouteConstants.CreateDocumentType`

##### 9. Update Document Type
- **Route:** `PUT /document-type/{key}`
- **Route Constant:** `RouteConstants.UpdateDocumentType`

##### 10. Delete Document Type
- **Route:** `DELETE /document-type/{key}`
- **Route Constant:** `RouteConstants.DeleteDocumentType`

##### 11. Document Type Summary
- **Route:** `POST /document-type-summary`
- **Route Constant:** `RouteConstants.DocumentTypeSummary`

##### 12. Get All Document Types
- **Route:** `GET /document-types`
- **Route Constant:** `RouteConstants.ReadDocumentTypes`

##### 13. Document Types Dropdown
- **Route:** `GET /document-types-ddl`
- **Route Constant:** `RouteConstants.DocumentTypeDDL`

##### 14. Get Document Type by ID
- **Route:** `GET /document-type/{id:int}`
- **Route Constant:** `RouteConstants.ReadDocumentType`

#### Document Parameter

##### 15. Create Document Parameter
- **Route:** `POST /document-parameter`
- **Route Constant:** `RouteConstants.CreateDocumentParameter`

##### 16. Update Document Parameter
- **Route:** `PUT /document-parameter/{key}`
- **Route Constant:** `RouteConstants.UpdateDocumentParameter`

##### 17. Delete Document Parameter
- **Route:** `DELETE /document-parameter/{key}`
- **Route Constant:** `RouteConstants.DeleteDocumentParameter`

##### 18. Document Parameter Summary
- **Route:** `POST /document-parameter-summary`
- **Route Constant:** `RouteConstants.DocumentParameterSummary`

##### 19. Get All Document Parameters
- **Route:** `GET /document-parameters`
- **Route Constant:** `RouteConstants.ReadDocumentParameters`

##### 20. Document Parameters Dropdown
- **Route:** `GET /document-parameters-ddl`
- **Route Constant:** `RouteConstants.DocumentParameterDDL`

##### 21. Get Document Parameter by ID
- **Route:** `GET /document-parameter/{id:int}`
- **Route Constant:** `RouteConstants.ReadDocumentParameter`

---

### Audit & Logging

**Base Route:** `/bdDevs-crm`

#### Audit Log

##### 1. Create Audit Log
- **Route:** `POST /audit-log`
- **Route Constant:** `RouteConstants.CreateAuditLog`

##### 2. Update Audit Log
- **Route:** `PUT /audit-log/{key}`
- **Route Constant:** `RouteConstants.UpdateAuditLog`

##### 3. Delete Audit Log
- **Route:** `DELETE /audit-log/{key}`
- **Route Constant:** `RouteConstants.DeleteAuditLog`

##### 4. Audit Log Summary
- **Route:** `POST /audit-log-summary`
- **Route Constant:** `RouteConstants.AuditLogSummary`

##### 5. Get All Audit Logs
- **Route:** `GET /audit-logs`
- **Route Constant:** `RouteConstants.ReadAuditLogs`

##### 6. Audit Logs Dropdown
- **Route:** `GET /audit-logs-ddl`
- **Route Constant:** `RouteConstants.AuditLogDDL`

##### 7. Get Audit Log by ID
- **Route:** `GET /audit-log/{id:long}`
- **Route Constant:** `RouteConstants.ReadAuditLog`
- **Note:** ID parameter is `long` type

#### Audit Trail

##### 8. Create Audit Trail
- **Route:** `POST /audit-trail`
- **Route Constant:** `RouteConstants.CreateAuditTrail`

##### 9. Update Audit Trail
- **Route:** `PUT /audit-trail/{key}`
- **Route Constant:** `RouteConstants.UpdateAuditTrail`

##### 10. Delete Audit Trail
- **Route:** `DELETE /audit-trail/{key}`
- **Route Constant:** `RouteConstants.DeleteAuditTrail`

##### 11. Audit Trail Summary
- **Route:** `POST /audit-trail-summary`
- **Route Constant:** `RouteConstants.AuditTrailSummary`

##### 12. Get All Audit Trails
- **Route:** `GET /audit-trails`
- **Route Constant:** `RouteConstants.ReadAuditTrails`

##### 13. Audit Trails Dropdown
- **Route:** `GET /audit-trails-ddl`
- **Route Constant:** `RouteConstants.AuditTrailDDL`

##### 14. Get Audit Trail by ID
- **Route:** `GET /audit-trail/{id:int}`
- **Route Constant:** `RouteConstants.ReadAuditTrail`

---

### System Settings

**Base Route:** `/bdDevs-crm`

#### Endpoints

##### 1. Get System Settings
- **Route:** `GET /system-settings`
- **Route Constant:** `RouteConstants.ReadSystemSettings`

##### 2. Update System Settings
- **Route:** `PUT /system-settings`
- **Route Constant:** `RouteConstants.UpdateSystemSettings`

##### 3. Get System Settings by Company ID
- **Route:** `GET /system-settings/company/{companyId:int}`
- **Route Constant:** `RouteConstants.SystemSettingsByCompanyId`

##### 4. Get Assembly Info
- **Route:** `GET /assembly-info`
- **Route Constant:** `RouteConstants.AssemblyInfo`

---

## Core - HR

**Base Route:** `/bdDevs-crm`

### Employee Management

#### 1. Get Employee Types
- **Route:** `GET /employeetypes`
- **Route Constant:** `RouteConstants.EmployeeTypes`

#### 2. Get Employees by Identities
- **Route:** `GET /employees-by-indentities`
- **Route Constant:** `RouteConstants.EmployeesByCompanyIdAndBranchIdAndDepartmentId`
- **Query Parameters:** `companyId`, `branchId`, `departmentId`

### Branch Management

#### 3. Get Branches by Company ID (Combo)
- **Route:** `GET /branches/{companyId:int}`
- **Route Constant:** `RouteConstants.BranchesByCompanyIdCombo`

### Department Management

#### 4. Get Departments by Company ID (Combo)
- **Route:** `GET /departments-by-compnayId/companyId/`
- **Route Constant:** `RouteConstants.DepartmentByCompanyIdForCombo`

---

## CRM Module

**Base Route:** `/bdDevs-crm`

### Institute Type Management

#### CUD Operations

##### 1. Create Institute Type
- **Route:** `POST /crm-institutetype`
- **Route Constant:** `RouteConstants.CreateInstituteType`

##### 2. Update Institute Type
- **Route:** `PUT /crm-institutetype/{key:int}`
- **Route Constant:** `RouteConstants.UpdateInstituteType`

##### 3. Delete Institute Type
- **Route:** `DELETE /crm-institutetype/{key:int}`
- **Route Constant:** `RouteConstants.DeleteInstituteType`

##### 4. Create or Update Institute Type (Upsert)
- **Route:** `POST /crm-institutetype-saveORupdate/{key:int}`
- **Route Constant:** `RouteConstants.CreateOrUpdateInstituteType`

#### Read Operations

##### 5. Institute Type Summary
- **Route:** `POST /crm-institutetype-summary`
- **Route Constant:** `RouteConstants.InstituteTypeSummary`

##### 6. Institute Types Dropdown
- **Route:** `GET /crm-institutetype-ddl`
- **Route Constant:** `RouteConstants.InstituteTypeDDL`

---

### Institute Management

#### CUD Operations

##### 1. Create Institute
- **Route:** `POST /crm-institute`
- **Route Constant:** `RouteConstants.CreateInstitute`

##### 2. Update Institute
- **Route:** `PUT /crm-institute/{key:int}`
- **Route Constant:** `RouteConstants.UpdateInstitute`

##### 3. Delete Institute
- **Route:** `DELETE /crm-institute/{key:int}`
- **Route Constant:** `RouteConstants.DeleteInstitute`

##### 4. Create or Update Institute (Upsert)
- **Route:** `POST /institute-saveORupdate/{key:int}`
- **Route Constant:** `RouteConstants.CreateOrUpdateInstitute`

#### Read Operations

##### 5. Institute Summary
- **Route:** `POST /crm-institute-summary`
- **Route Constant:** `RouteConstants.InstituteSummary`

##### 6. Institutes Dropdown
- **Route:** `GET /crm-institute-ddl`
- **Route Constant:** `RouteConstants.InstituteDDL`

##### 7. Institutes by Country ID (Dropdown)
- **Route:** `GET /crm-institut-by-countryid-ddl/{countryId:int}`
- **Route Constant:** `RouteConstants.InstituteDDLByCountryId`

---

### Course Management

#### CUD Operations

##### 1. Create Course
- **Route:** `POST /crm-course`
- **Route Constant:** `RouteConstants.CreateCourse`

##### 2. Update Course
- **Route:** `PUT /crm-course/{key:int}`
- **Route Constant:** `RouteConstants.UpdateCourse`

##### 3. Delete Course
- **Route:** `DELETE /crm-course/{key:int}`
- **Route Constant:** `RouteConstants.DeleteCourse`

##### 4. Create or Update Course (Upsert)
- **Route:** `POST /course-saveORupdate/{key:int}`
- **Route Constant:** `RouteConstants.CreateOrUpdateCourse`

#### Read Operations

##### 5. Course Summary
- **Route:** `POST /crm-course-summary`
- **Route Constant:** `RouteConstants.CourseSummary`

##### 6. Courses Dropdown
- **Route:** `GET /crm-course-ddl`
- **Route Constant:** `RouteConstants.CourseDDL`

##### 7. Courses by Institute ID (Dropdown)
- **Route:** `GET /crm-course-by-instituteid-ddl/{instituteId:int}`
- **Route Constant:** `RouteConstants.CourseByInstituteIdDDL`

---

### Application Management

#### CUD Operations

##### 1. Create CRM Application
- **Route:** `POST /crm-application`
- **Route Constant:** `RouteConstants.CRMApplicationCreate`

##### 2. Update CRM Application
- **Route:** `PUT /crm-Application/{key:int}`
- **Route Constant:** `RouteConstants.CRMApplicationUpdate`

##### 3. Delete CRM Application
- **Route:** `DELETE /crm-Application/{key}`
- **Route Constant:** `RouteConstants.CRMApplicationDelete`

#### Read Operations

##### 4. CRM Application Summary
- **Route:** `POST /crm-application-summary/{statusId:int}`
- **Route Constant:** `RouteConstants.CRMApplicationSummary`

##### 5. Get CRM Application by Key
- **Route:** `GET /crm-application/key/{key}`
- **Route Constant:** `RouteConstants.CRMApplicationByKey`

##### 6. Get CRM Application by Application ID
- **Route:** `GET /crm-application-by-applicationId/{applicationId:int}`
- **Route Constant:** `RouteConstants.CRMApplicationByApplicationId`

##### 7. Get CRM Application Status
- **Route:** `GET /crm-application-status`
- **Route Constant:** `RouteConstants.CRMApplicationStatus`

---

### Applicant Course Management

#### CUD Operations

##### 1. Create Applicant Course
- **Route:** `POST /applicant-course`
- **Route Constant:** `RouteConstants.CreateApplicantCourse`

##### 2. Update Applicant Course
- **Route:** `PUT /applicant-course/{key:int}`
- **Route Constant:** `RouteConstants.UpdateApplicantCourse`

##### 3. Delete Applicant Course
- **Route:** `DELETE /applicant-course/{key:int}`
- **Route Constant:** `RouteConstants.DeleteApplicantCourse`

#### Read Operations

##### 4. Applicant Course Summary
- **Route:** `POST /applicant-course-summary`
- **Route Constant:** `RouteConstants.ApplicantCourseSummary`

##### 5. Applicant Courses Dropdown
- **Route:** `GET /applicant-course-ddl`
- **Route Constant:** `RouteConstants.ApplicantCourseDDL`

##### 6. Applicant Courses by Applicant ID
- **Route:** `GET /applicant-courses-by-applicantid/{applicantId:int}`
- **Route Constant:** `RouteConstants.ApplicantCoursesByApplicantId`

---

### Additional CRM Entities

#### Applicant Info, Education History, Work Experience, etc.

For brevity, similar CRUD patterns apply to:
- **Applicant Info** (`/applicant-info` endpoints)
- **Education History** (`/education-history` endpoints)
- **Work Experience** (`/work-experience` endpoints)
- **Permanent Address** (`/permanent-address` endpoints)
- **Present Address** (`/present-address` endpoints)
- **Applicant Reference** (`/applicant-reference` endpoints)
- **IELTS Information** (`/ielts-information` endpoints)
- **TOEFL Information** (`/toefl-information` endpoints)
- **PTE Information** (`/pte-information` endpoints)
- **GMAT Information** (`/gmat-information` endpoints)
- **Others Information** (`/others-information` endpoints)
- **Statement of Purpose** (`/statement-of-purpose` endpoints)
- **Additional Info** (`/additional-info` endpoints)

Each follows the standard pattern:
- `POST` - Create
- `PUT /{key:int}` - Update
- `DELETE /{key:int}` - Delete
- `POST /{entity}-summary` - Summary grid
- `GET /{entity}-ddl` - Dropdown list
- `GET /{entity}-by-applicantid/{applicantId:int}` - Get by applicant

---

### Intake Month Management

#### CUD Operations

##### 1. Create Intake Month
- **Route:** `POST /intake-month`
- **Route Constant:** `RouteConstants.CreateIntakeMonth`

##### 2. Update Intake Month
- **Route:** `PUT /intake-month/{key:int}`
- **Route Constant:** `RouteConstants.UpdateIntakeMonth`

##### 3. Delete Intake Month
- **Route:** `DELETE /intake-month/{key:int}`
- **Route Constant:** `RouteConstants.DeleteIntakeMonth`

##### 4. Create or Update Intake Month (Upsert)
- **Route:** `POST /intake-month-saveORupdate/{key:int}`
- **Route Constant:** `RouteConstants.CreateOrUpdateIntakeMonth`

#### Read Operations

##### 5. Intake Month Summary
- **Route:** `POST /intake-month-summary`
- **Route Constant:** `RouteConstants.IntakeMonthSummary`

##### 6. Intake Months Dropdown
- **Route:** `GET /intake-month-ddl`
- **Route Constant:** `RouteConstants.IntakeMonthDDL`

##### 7. Get Intake Month by Key
- **Route:** `GET /intake-month/key`
- **Route Constant:** `RouteConstants.IntakeMonthByKey`

---

### Intake Year Management

#### CUD Operations

##### 1. Create Intake Year
- **Route:** `POST /intake-year`
- **Route Constant:** `RouteConstants.CreateIntakeYear`

##### 2. Update Intake Year
- **Route:** `PUT /intake-year/{key:int}`
- **Route Constant:** `RouteConstants.UpdateIntakeYear`

##### 3. Delete Intake Year
- **Route:** `DELETE /intake-year/{key:int}`
- **Route Constant:** `RouteConstants.DeleteIntakeYear`

##### 4. Create or Update Intake Year (Upsert)
- **Route:** `POST /intake-year-saveORupdate/{key:int}`
- **Route Constant:** `RouteConstants.CreateOrUpdateIntakeYear`

#### Read Operations

##### 5. Intake Year Summary
- **Route:** `POST /intake-year-summary`
- **Route Constant:** `RouteConstants.IntakeYearSummary`

##### 6. Intake Years Dropdown
- **Route:** `GET /intake-year-ddl`
- **Route Constant:** `RouteConstants.IntakeYearDDL`

##### 7. Get Intake Year by Key
- **Route:** `GET /intake-year/key`
- **Route Constant:** `RouteConstants.IntakeYearByKey`

---

### Payment Method Management

#### CUD Operations

##### 1. Create Payment Method
- **Route:** `POST /payment-method`
- **Route Constant:** `RouteConstants.CreatePaymentMethod`

##### 2. Update Payment Method
- **Route:** `PUT /payment-method/{key:int}`
- **Route Constant:** `RouteConstants.UpdatePaymentMethod`

##### 3. Delete Payment Method
- **Route:** `DELETE /payment-method/{key:int}`
- **Route Constant:** `RouteConstants.DeletePaymentMethod`

##### 4. Create or Update Payment Method (Upsert)
- **Route:** `POST /payment-method-saveORupdate/{key:int}`
- **Route Constant:** `RouteConstants.CreateOrUpdatePaymentMethod`

#### Read Operations

##### 5. Payment Method Summary
- **Route:** `POST /payment-method-summary`
- **Route Constant:** `RouteConstants.PaymentMethodSummary`

##### 6. Payment Methods Dropdown
- **Route:** `GET /payment-method-ddl`
- **Route Constant:** `RouteConstants.PaymentMethodDDL`

##### 7. Online Payment Methods Dropdown
- **Route:** `GET /online-payment-method-ddl`
- **Route Constant:** `RouteConstants.OnlinePaymentMethodDDL`

##### 8. Get Payment Method by Key
- **Route:** `GET /payment-method/key`
- **Route Constant:** `RouteConstants.PaymentMethodByKey`

---

## DMS Module

**Base Route:** `/bdDevs-crm`

### File Update History

#### CUD Operations

##### 1. Create DMS File Update History
- **Route:** `POST /dms-file-update-history`
- **Route Constant:** `RouteConstants.CreateDmsFileUpdateHistory`

##### 2. Update DMS File Update History
- **Route:** `PUT /dms-file-update-history/{key}`
- **Route Constant:** `RouteConstants.UpdateDmsFileUpdateHistory`

##### 3. Delete DMS File Update History
- **Route:** `DELETE /dms-file-update-history/{key}`
- **Route Constant:** `RouteConstants.DeleteDmsFileUpdateHistory`

#### Read Operations

##### 4. DMS File Update History Summary
- **Route:** `POST /dms-file-update-history-summary`
- **Route Constant:** `RouteConstants.DmsFileUpdateHistorySummary`

##### 5. Get All DMS File Update Histories
- **Route:** `GET /dms-file-update-histories`
- **Route Constant:** `RouteConstants.ReadDmsFileUpdateHistories`

##### 6. DMS File Update Histories Dropdown
- **Route:** `GET /dms-file-update-histories-ddl`
- **Route Constant:** `RouteConstants.DmsFileUpdateHistoryDDL`

##### 7. Get DMS File Update History by ID
- **Route:** `GET /dms-file-update-history/{id:int}`
- **Route Constant:** `RouteConstants.ReadDmsFileUpdateHistory`

##### 8. DMS File Update Histories by Entity
- **Route:** `GET /dms-file-update-histories-by-entity/{entityId}`
- **Route Constant:** `RouteConstants.DmsFileUpdateHistoriesByEntity`

---

## Token Blacklist Management

**Base Route:** `/bdDevs-crm`

### Endpoints

#### 1. Create Token Blacklist
- **Route:** `POST /token-blacklist`
- **Route Constant:** `RouteConstants.CreateTokenBlacklist`

#### 2. Update Token Blacklist
- **Route:** `PUT /token-blacklist/{key:guid}`
- **Route Constant:** `RouteConstants.UpdateTokenBlacklist`
- **Note:** Key parameter is GUID type

#### 3. Delete Token Blacklist
- **Route:** `DELETE /token-blacklist/{key:guid}`
- **Route Constant:** `RouteConstants.DeleteTokenBlacklist`
- **Note:** Key parameter is GUID type

#### 4. Token Blacklist Summary
- **Route:** `POST /token-blacklist-summary`
- **Route Constant:** `RouteConstants.TokenBlacklistSummary`

#### 5. Get All Token Blacklists
- **Route:** `GET /token-blacklists`
- **Route Constant:** `RouteConstants.ReadTokenBlacklists`

#### 6. Get Token Blacklist by ID
- **Route:** `GET /token-blacklist/{id:guid}`
- **Route Constant:** `RouteConstants.ReadTokenBlacklist`
- **Note:** ID parameter is GUID type

#### 7. Check if Token is Blacklisted
- **Route:** `GET /token-blacklist/check`
- **Route Constant:** `RouteConstants.IsTokenBlacklisted`

#### 8. Blacklist Token
- **Route:** `POST /token-blacklist/blacklist`
- **Route Constant:** `RouteConstants.BlacklistToken`

#### 9. Remove Expired Tokens (Cleanup)
- **Route:** `POST /token-blacklist/cleanup`
- **Route Constant:** `RouteConstants.RemoveExpiredTokens`

---

## Approver Management

**Base Route:** `/bdDevs-crm`

### Approver Details

#### CUD Operations

##### 1. Create Approver Details
- **Route:** `POST /approver-details`
- **Route Constant:** `RouteConstants.CreateApproverDetails`

##### 2. Update Approver Details
- **Route:** `PUT /approver-details/{key}`
- **Route Constant:** `RouteConstants.UpdateApproverDetails`

##### 3. Delete Approver Details
- **Route:** `DELETE /approver-details/{key}`
- **Route Constant:** `RouteConstants.DeleteApproverDetails`

#### Read Operations

##### 4. Approver Details Summary
- **Route:** `POST /approver-details-summary`
- **Route Constant:** `RouteConstants.ApproverDetailsSummary`

##### 5. Get All Approver Details
- **Route:** `GET /approver-details`
- **Route Constant:** `RouteConstants.ReadApproverDetails`

##### 6. Approver Details Dropdown
- **Route:** `GET /approver-details-ddl`
- **Route Constant:** `RouteConstants.ApproverDetailsDDL`

##### 7. Get Approver Detail by ID
- **Route:** `GET /approver-details/{id:int}`
- **Route Constant:** `RouteConstants.ReadApproverDetail`

---

### Approver History

#### CUD Operations

##### 1. Create Approver History
- **Route:** `POST /approver-history`
- **Route Constant:** `RouteConstants.CreateApproverHistory`

##### 2. Update Approver History
- **Route:** `PUT /approver-history/{key}`
- **Route Constant:** `RouteConstants.UpdateApproverHistory`

##### 3. Delete Approver History
- **Route:** `DELETE /approver-history/{key}`
- **Route Constant:** `RouteConstants.DeleteApproverHistory`

#### Read Operations

##### 4. Approver History Summary
- **Route:** `POST /approver-history-summary`
- **Route Constant:** `RouteConstants.ApproverHistorySummary`

##### 5. Get All Approver Histories
- **Route:** `GET /approver-histories`
- **Route Constant:** `RouteConstants.ReadApproverHistories`

##### 6. Approver Histories Dropdown
- **Route:** `GET /approver-histories-ddl`
- **Route Constant:** `RouteConstants.ApproverHistoryDDL`

##### 7. Get Approver History by ID
- **Route:** `GET /approver-histories/{id:int}`
- **Route Constant:** `RouteConstants.ReadApproverHistory`

---

## Standard CRUD Pattern Reference

Most endpoints in the system follow this standard pattern:

### CUD Operations (Create, Update, Delete)
1. **Create:** `POST /{entity}` - Creates a new record
2. **Update:** `PUT /{entity}/{key}` - Updates existing record by key
3. **Delete:** `DELETE /{entity}/{key}` - Deletes record by key

### Read Operations (High to Low Data Volume)
4. **Summary Grid (Paginated):** `POST /{entity}-summary` - Returns `GridEntity<DTO>` with pagination
5. **List All:** `GET /{entities}` - Returns all records as `IEnumerable<DTO>`
6. **Dropdown List:** `GET /{entities}-ddl` - Returns minimal data for dropdowns (ID + Name)
7. **Get by ID:** `GET /{entity}/{id:int}` - Returns single record by ID

### Additional Specialized Queries
- **By Parent ID:** `GET /{entities}-by-{parent}/{parentId:int}` - Filter by parent entity
- **By Status:** Often included as query parameter in summary endpoints
- **By User Permission:** Special authorization-filtered endpoints

---

## Common HTTP Status Codes

### Success Responses
- **200 OK** - Request succeeded, data returned
- **201 Created** - Resource created successfully
- **204 No Content** - Request succeeded, no data returned (usually delete operations)

### Client Error Responses
- **400 Bad Request** - Invalid request data or validation failed
- **401 Unauthorized** - Authentication required or token expired
- **403 Forbidden** - User lacks permissions for this resource
- **404 Not Found** - Requested resource doesn't exist

### Server Error Responses
- **500 Internal Server Error** - Unhandled server exception

---

## API Response Format

All API responses follow the unified `ApiResponse<T>` structure:

```json
{
  "correlationId": "string",      // Request tracking ID
  "statusCode": 200,               // HTTP status code
  "success": true,                 // Success flag
  "message": "string",             // User-friendly message
  "data": { },                     // Response data (generic type T)
  "errors": [],                    // Error details (if any)
  "timestamp": "2026-04-21T10:30:00Z"  // Response timestamp
}
```

### Grid Response Format

Paginated grid responses return `GridEntity<T>`:

```json
{
  "correlationId": "string",
  "statusCode": 200,
  "success": true,
  "message": "Data retrieved successfully",
  "data": {
    "items": [ ],                // Array of data items
    "total": 150,                // Total count of all records
    "page": 1,                   // Current page
    "pageSize": 20,              // Items per page
    "totalPages": 8              // Total number of pages
  },
  "timestamp": "2026-04-21T10:30:00Z"
}
```

---

## Authentication & Authorization

### JWT Bearer Token

All authenticated endpoints require a Bearer token in the Authorization header:

```
Authorization: Bearer <access_token>
```

### Token Management

1. **Login** → Receive access token (30 minutes) + refresh token (HTTP-only cookie, 7 days)
2. **Access Token Expires** → Call `/refresh-token` to get new access token
3. **Refresh Token Expires** → User must re-login
4. **Logout** → Revokes all tokens and clears cache

### Authorization Levels

- **[AllowAnonymous]** - Public endpoint (Login, Refresh Token)
- **[AuthorizeUser]** - Requires authenticated user
- **Role-based** - Additional role/permission checks via Group/Menu permissions

---

## Caching Strategy

### L1: Memory Cache (IMemoryCache)
- **Duration:** 60-300 seconds
- **Use Cases:** User sessions, frequently accessed reference data (DDLs, menus)
- **Examples:**
  - User info: 5 hours
  - Menu permissions: 5 minutes
  - Dropdown lists: 5 minutes

### L2: Redis Cache (Distributed)
- **Duration:** 30-60 minutes
- **Use Cases:** Cross-server shared cache
- **Fallback:** PostgreSQL cache layer

### L3: PostgreSQL Cache (Persistent)
- **Duration:** Until manually invalidated
- **Use Cases:** Long-lived reference data

---

## Grid Options (Pagination, Sorting, Filtering)

All summary grid endpoints accept a `GridOptions` object:

```json
{
  "page": 1,                     // Page number (1-indexed)
  "pageSize": 20,                // Items per page (default: 20, max: 100)
  "sort": [                      // Optional sorting
    {
      "field": "countryName",
      "dir": "asc"               // "asc" or "desc"
    }
  ],
  "filter": {                    // Optional filtering
    "logic": "and",              // "and" or "or"
    "filters": [
      {
        "field": "isActive",
        "operator": "eq",        // eq, neq, lt, lte, gt, gte, contains, startswith, endswith
        "value": true
      }
    ]
  }
}
```

---

## Error Handling

All exceptions are handled by `StandardExceptionMiddleware` and return a consistent error response:

```json
{
  "correlationId": "abc-123",
  "statusCode": 400,
  "success": false,
  "message": "Validation failed",
  "errors": [
    "Country name is required",
    "Country code must be 2-3 characters"
  ],
  "timestamp": "2026-04-21T10:30:00Z"
}
```

### Common Custom Exceptions

- `NullModelBadRequestException` - Request body is null
- `IdMismatchBadRequestException` - Route ID doesn't match DTO ID
- `IdParametersBadRequestException` - Invalid or missing ID parameter
- `InvalidCreateOperationException` - Create operation failed
- `InvalidUpdateOperationException` - Update operation failed
- `GenericUnauthorizedException` - User not authenticated
- `UserNotFoundException` - User account not found
- `AccountLockedException` - Account locked due to failed login attempts

---

## Additional Notes

### CRUD Record Pattern

Some controllers (e.g., CountryController) use the **CRUD Record pattern**:
- `CreateCountryRecord` - Input for create operations
- `UpdateCountryRecord` - Input for update operations
- `DeleteCountryRecord` - Input for delete operations (contains only ID)

This approach enforces strong typing and validation at the record level.

### HATEOAS Support

Some controllers (e.g., MenuController) implement **HATEOAS** (Hypermedia as the Engine of Application State):
- Each grid row includes `_links` with available actions (self, edit, delete)
- Resource-level links for collection operations (create, list)
- Implemented via `ILinkFactory<T>` and `LinkedResource<T>`

### Action Filters

- `[EmptyObjectFilterAttribute]` - Validates that request body is not empty/null
- `[ServiceFilter(typeof(EmptyObjectFilterAttribute))]` - Applied to Create/Update endpoints

### Response Cache

Many GET endpoints use `[ResponseCache(Duration = seconds)]` attribute for browser-side caching:
- DDL endpoints: 300 seconds (5 minutes)
- List endpoints: 60 seconds (1 minute)

---

## Test Endpoints

**Base Route:** `/bdDevs-crm`

### Development/Testing Endpoints

##### 1. Test DDL
- **Route:** `GET /test-ddl`
- **Route Constant:** `RouteConstants.TestDDL`

##### 2. Get Tests
- **Route:** `GET /tests`
- **Route Constant:** `RouteConstants.Tests`

##### 3. Get Test
- **Route:** `GET /get-test`
- **Route Constant:** `RouteConstants.Test`

##### 4. Read Test by Key
- **Route:** `GET /test/key/{key}`
- **Route Constant:** `RouteConstants.ReadTest`

##### 5. Create Test
- **Route:** `POST /test`
- **Route Constant:** `RouteConstants.TestCreate`

##### 6. Update Test
- **Route:** `PUT /test/{key:int}`
- **Route Constant:** `RouteConstants.TestUpdate`

##### 7. Delete Test
- **Route:** `DELETE /test/{key:int}`
- **Route Constant:** `RouteConstants.TestDelete`

##### 8. Search Test
- **Route:** `GET /test/{key:int}`
- **Route Constant:** `RouteConstants.TestSearch`

---

## Query Analyzer

**Base Route:** `/bdDevs-crm`

### Endpoints

##### 1. Get Query Analyzers
- **Route:** `GET /query-analyzers`
- **Route Constant:** `RouteConstants.QueryAnalyzers`

##### 2. Get Customized Report Info
- **Route:** `GET /customized-report`
- **Route Constant:** `RouteConstants.CustomizedReportInfo`

---

## Summary Statistics

**Total Endpoint Groups:** 70+

**Entities with Full CRUD:**
- Core System Admin: 25+ entities
- CRM Module: 30+ entities
- DMS Module: 5+ entities
- Workflow: 10+ entities
- Approver Management: 6+ entities

**Common Patterns:**
- Standard CRUD (Create, Read, Update, Delete)
- Grid Summary with Pagination
- Dropdown List (DDL) endpoints
- Specialized queries (by parent, by user permission, etc.)

---

## Frontend Integration

**Fetch API Pattern:**

```javascript
// GET request
const response = await window.ApiClient.get('/bdDevs-crm/countries-ddl');

// POST request (Grid Summary)
const gridOptions = {
  page: 1,
  pageSize: 20,
  sort: [{ field: 'countryName', dir: 'asc' }],
  filter: null
};
const response = await window.ApiClient.post('/bdDevs-crm/country-summary', gridOptions);

// PUT request (Update)
const country = { countryId: 1, countryName: 'Bangladesh', ... };
const response = await window.ApiClient.put('/bdDevs-crm/country/1', country);

// DELETE request
const response = await window.ApiClient.delete('/bdDevs-crm/country/1');
```

**Session Management:**
- Access token stored in `localStorage` (in-memory via `window.ApiClient`)
- Refresh token stored in HTTP-only cookie
- Session timeout: 30 minutes (configurable)
- Idle timeout: 15 minutes (configurable)
- Auto token refresh: 25 minutes (5 minutes before expiry)

---

## Revision History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2026-04-21 | Claude Sonnet 4.5 | Initial documentation - extracted from RouteConstants.cs and controller implementations |

---

**End of Documentation**
