namespace bdDevs.Shared.Records.CRM;

/// <summary>
/// Record for creating a new CRM present address.
/// </summary>
public record CreateCrmPresentAddressRecord(
    int ApplicantId,
    bool SameAsPermanentAddress,
    string? Address,
    string? City,
    string? State,
    int CountryId,
    string? PostalCode,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for updating an existing CRM present address.
/// </summary>
public record UpdateCrmPresentAddressRecord(
    int PresentAddressId,
    int ApplicantId,
    bool SameAsPermanentAddress,
    string? Address,
    string? City,
    string? State,
    int CountryId,
    string? PostalCode,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for deleting a CRM present address.
/// </summary>
/// <param name="PresentAddressId">ID of the present address to delete.</param>
public record DeleteCrmPresentAddressRecord(int PresentAddressId);
