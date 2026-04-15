namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new employee type.
/// </summary>
public record CreateEmployeetypeRecord(
    string Employeetypename,
    string? EmployeeTypeCode,
    int? IsActive,
    bool? IsContract,
    bool? IsNotAccess,
    int? IsPfApplicable,
    int? IsTrainee,
    int? IsRegular,
    int? IsUnion,
    int? IsProbationary,
    int? IsUnionProbationary,
    int? EmpTypeSortOrder,
    int? IsEwfApplicable);

/// <summary>
/// Record for updating an existing employee type.
/// </summary>
public record UpdateEmployeetypeRecord(
    int Employeetypeid,
    string Employeetypename,
    string? EmployeeTypeCode,
    int? IsActive,
    bool? IsContract,
    bool? IsNotAccess,
    int? IsPfApplicable,
    int? IsTrainee,
    int? IsRegular,
    int? IsUnion,
    int? IsProbationary,
    int? IsUnionProbationary,
    int? EmpTypeSortOrder,
    int? IsEwfApplicable);

/// <summary>
/// Record for deleting an employee type.
/// </summary>
public record DeleteEmployeetypeRecord(int Employeetypeid);
