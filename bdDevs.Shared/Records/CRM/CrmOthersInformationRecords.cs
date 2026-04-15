namespace bdDevs.Shared.Records.CRM;

/// <summary>
/// Record for creating a new CRM others information.
/// </summary>
public record CreateCrmOthersInformationRecord(
    int ApplicantId,
    string? AdditionalInformation,
    string? OthersScannedCopyPath,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for updating an existing CRM others information.
/// </summary>
public record UpdateCrmOthersInformationRecord(
    int OthersInformationId,
    int ApplicantId,
    string? AdditionalInformation,
    string? OthersScannedCopyPath,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for deleting a CRM others information.
/// </summary>
/// <param name="OthersInformationId">ID of the others information to delete.</param>
public record DeleteCrmOthersInformationRecord(int OthersInformationId);
