namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new company.
/// </summary>
/// <param name="CompanyName">Name of the company.</param>
/// <param name="CompanyCode">Unique company code.</param>
/// <param name="Address">Company address.</param>
/// <param name="Phone">Company phone number.</param>
/// <param name="Email">Company email address.</param>
/// <param name="Flag">Company flag identifier.</param>
/// <param name="FiscalYearStart">Fiscal year start month.</param>
/// <param name="MotherId">Parent company ID.</param>
/// <param name="IsActive">Active status (1 = active, 0 = inactive).</param>
public record CreateCompanyRecord(
    string CompanyName,
    string? CompanyCode,
    string? Address,
    string? Phone,
    string? Email,
    int Flag,
    int FiscalYearStart,
    int? MotherId,
    int? IsActive);

/// <summary>
/// Record for updating an existing company.
/// </summary>
/// <param name="CompanyId">ID of the company to update.</param>
/// <param name="CompanyName">Updated company name.</param>
/// <param name="CompanyCode">Updated company code.</param>
/// <param name="Address">Updated company address.</param>
/// <param name="Phone">Updated phone number.</param>
/// <param name="Email">Updated email address.</param>
/// <param name="Flag">Updated flag identifier.</param>
/// <param name="FiscalYearStart">Updated fiscal year start month.</param>
/// <param name="MotherId">Updated parent company ID.</param>
/// <param name="IsActive">Updated active status.</param>
public record UpdateCompanyRecord(
    int CompanyId,
    string CompanyName,
    string? CompanyCode,
    string? Address,
    string? Phone,
    string? Email,
    int Flag,
    int FiscalYearStart,
    int? MotherId,
    int? IsActive);

/// <summary>
/// Record for deleting a company.
/// </summary>
/// <param name="CompanyId">ID of the company to delete.</param>
public record DeleteCompanyRecord(int CompanyId);
