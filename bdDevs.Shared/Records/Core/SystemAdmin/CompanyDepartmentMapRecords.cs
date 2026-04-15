namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new company-department mapping.
/// </summary>
public record CreateCompanyDepartmentMapRecord(
    int CompanyId,
    int DepartmentId);

/// <summary>
/// Record for updating an existing company-department mapping.
/// </summary>
public record UpdateCompanyDepartmentMapRecord(
    int SbuDepartmentMapId,
    int CompanyId,
    int DepartmentId);

/// <summary>
/// Record for deleting a company-department mapping.
/// </summary>
public record DeleteCompanyDepartmentMapRecord(int SbuDepartmentMapId);
