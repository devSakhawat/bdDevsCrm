namespace bdDevs.Shared.Records.CRM;

/// <summary>
/// Record for creating a new CRM GMAT information.
/// </summary>
public record CreateCrmGmatInformationRecord(
    int ApplicantId,
    decimal? Gmatlistening,
    decimal? Gmatreading,
    decimal? Gmatwriting,
    decimal? Gmatspeaking,
    decimal? GmatoverallScore,
    DateTime? Gmatdate,
    string? GmatscannedCopyPath,
    string? GmatadditionalInformation,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for updating an existing CRM GMAT information.
/// </summary>
public record UpdateCrmGmatInformationRecord(
    int GMATInformationId,
    int ApplicantId,
    decimal? Gmatlistening,
    decimal? Gmatreading,
    decimal? Gmatwriting,
    decimal? Gmatspeaking,
    decimal? GmatoverallScore,
    DateTime? Gmatdate,
    string? GmatscannedCopyPath,
    string? GmatadditionalInformation,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for deleting a CRM GMAT information.
/// </summary>
/// <param name="GMATInformationId">ID of the GMAT information to delete.</param>
public record DeleteCrmGmatInformationRecord(int GMATInformationId);
