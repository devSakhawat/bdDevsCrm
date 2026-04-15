namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new company-location mapping.
/// </summary>
public record CreateCompanyLocationMapRecord(
    int CompanyId,
    int BranchId);

/// <summary>
/// Record for updating an existing company-location mapping.
/// </summary>
public record UpdateCompanyLocationMapRecord(
    int SbuLocationMapId,
    int CompanyId,
    int BranchId);

/// <summary>
/// Record for deleting a company-location mapping.
/// </summary>
public record DeleteCompanyLocationMapRecord(int SbuLocationMapId);
