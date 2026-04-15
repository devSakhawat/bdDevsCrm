namespace bdDevs.Shared.Records.CRM;

/// <summary>
/// Record for creating a new CRM permanent address.
/// </summary>
public record CreateCrmPermanentAddressRecord(
    int ApplicantId,
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
/// Record for updating an existing CRM permanent address.
/// </summary>
public record UpdateCrmPermanentAddressRecord(
    int PermanentAddressId,
    int ApplicantId,
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
/// Record for deleting a CRM permanent address.
/// </summary>
/// <param name="PermanentAddressId">ID of the permanent address to delete.</param>
public record DeleteCrmPermanentAddressRecord(int PermanentAddressId);
