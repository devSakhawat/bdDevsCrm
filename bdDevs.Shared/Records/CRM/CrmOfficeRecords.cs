namespace bdDevs.Shared.Records.CRM;

/// <summary>Record for creating a new CRM office.</summary>
public record CreateCrmOfficeRecord(
    string OfficeName,
    string? OfficeCode,
    string? Address,
    string? City,
    string? Phone,
    string? Email,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>Record for updating an existing CRM office.</summary>
public record UpdateCrmOfficeRecord(
    int OfficeId,
    string OfficeName,
    string? OfficeCode,
    string? Address,
    string? City,
    string? Phone,
    string? Email,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>Record for deleting a CRM office.</summary>
/// <param name="OfficeId">ID of the office to delete.</param>
public record DeleteCrmOfficeRecord(int OfficeId);
