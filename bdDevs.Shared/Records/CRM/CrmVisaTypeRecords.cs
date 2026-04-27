namespace bdDevs.Shared.Records.CRM;

/// <summary>Record for creating a new CRM visa type.</summary>
public record CreateCrmVisaTypeRecord(
    string VisaTypeName,
    string? VisaCode,
    string? Description,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>Record for updating an existing CRM visa type.</summary>
public record UpdateCrmVisaTypeRecord(
    int VisaTypeId,
    string VisaTypeName,
    string? VisaCode,
    string? Description,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>Record for deleting a CRM visa type.</summary>
/// <param name="VisaTypeId">ID of the visa type to delete.</param>
public record DeleteCrmVisaTypeRecord(int VisaTypeId);
