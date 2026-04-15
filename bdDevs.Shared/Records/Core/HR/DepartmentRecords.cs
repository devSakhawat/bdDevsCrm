namespace bdDevs.Shared.Records.Core.HR;

/// <summary>
/// Record for creating a new department.
/// </summary>
/// <param name="DepartmentName">Name of the department.</param>
/// <param name="DepartmentCode">Department code.</param>
/// <param name="IsCostCentre">Whether the department is a cost centre.</param>
/// <param name="IsActive">Active status (1 = active, 0 = inactive).</param>
public record CreateDepartmentRecord(
    string? DepartmentName,
    string? DepartmentCode,
    int? IsCostCentre,
    int? IsActive);

/// <summary>
/// Record for updating an existing department.
/// </summary>
/// <param name="DepartmentId">ID of the department to update.</param>
/// <param name="DepartmentName">Updated department name.</param>
/// <param name="DepartmentCode">Updated department code.</param>
/// <param name="IsCostCentre">Updated cost centre flag.</param>
/// <param name="IsActive">Updated active status.</param>
public record UpdateDepartmentRecord(
    int DepartmentId,
    string? DepartmentName,
    string? DepartmentCode,
    int? IsCostCentre,
    int? IsActive);

/// <summary>
/// Record for deleting a department.
/// </summary>
/// <param name="DepartmentId">ID of the department to delete.</param>
public record DeleteDepartmentRecord(int DepartmentId);
