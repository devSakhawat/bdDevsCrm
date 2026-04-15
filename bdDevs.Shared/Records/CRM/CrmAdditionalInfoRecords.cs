namespace bdDevs.Shared.Records.CRM;

/// <summary>
/// Record for creating a new CRM additional info.
/// </summary>
public record CreateCrmAdditionalInfoRecord(
    int ApplicantId,
    bool? RequireAccommodation,
    bool? HealthNmedicalNeeds,
    string? HealthNmedicalNeedsRemarks,
    string? AdditionalInformationRemarks,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for updating an existing CRM additional info.
/// </summary>
public record UpdateCrmAdditionalInfoRecord(
    int AdditionalInfoId,
    int ApplicantId,
    bool? RequireAccommodation,
    bool? HealthNmedicalNeeds,
    string? HealthNmedicalNeedsRemarks,
    string? AdditionalInformationRemarks,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for deleting a CRM additional info.
/// </summary>
/// <param name="AdditionalInfoId">ID of the additional info to delete.</param>
public record DeleteCrmAdditionalInfoRecord(int AdditionalInfoId);
