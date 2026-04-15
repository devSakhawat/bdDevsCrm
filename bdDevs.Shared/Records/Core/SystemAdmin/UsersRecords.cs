namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new user.
/// </summary>
/// <param name="CompanyId">Company the user belongs to.</param>
/// <param name="LoginId">Login identifier.</param>
/// <param name="UserName">Display name of the user.</param>
/// <param name="Password">User password.</param>
/// <param name="EmployeeId">Linked HR employee record ID.</param>
/// <param name="IsActive">Whether the user account is active.</param>
/// <param name="Theme">UI theme preference.</param>
/// <param name="AccessParentCompany">Parent company access ID.</param>
/// <param name="DefaultDashboard">Default dashboard ID.</param>
/// <param name="IsSystemUser">Whether this is a system user.</param>
public record CreateUsersRecord(
    int? CompanyId,
    string? LoginId,
    string? UserName,
    string? Password,
    int? EmployeeId,
    bool? IsActive,
    string? Theme,
    int? AccessParentCompany,
    int? DefaultDashboard,
    bool? IsSystemUser);

/// <summary>
/// Record for updating an existing user.
/// </summary>
/// <param name="UserId">ID of the user to update.</param>
/// <param name="CompanyId">Updated company ID.</param>
/// <param name="LoginId">Updated login identifier.</param>
/// <param name="UserName">Updated display name.</param>
/// <param name="Password">Updated password.</param>
/// <param name="EmployeeId">Updated employee record ID.</param>
/// <param name="IsActive">Updated active status.</param>
/// <param name="IsExpired">Whether the account is expired.</param>
/// <param name="Theme">Updated UI theme preference.</param>
/// <param name="AccessParentCompany">Updated parent company access ID.</param>
/// <param name="DefaultDashboard">Updated default dashboard ID.</param>
/// <param name="IsSystemUser">Updated system user flag.</param>
public record UpdateUsersRecord(
    int UserId,
    int? CompanyId,
    string? LoginId,
    string? UserName,
    string? Password,
    int? EmployeeId,
    bool? IsActive,
    bool? IsExpired,
    string? Theme,
    int? AccessParentCompany,
    int? DefaultDashboard,
    bool? IsSystemUser);

/// <summary>
/// Record for deleting a user.
/// </summary>
/// <param name="UserId">ID of the user to delete.</param>
public record DeleteUsersRecord(int UserId);
