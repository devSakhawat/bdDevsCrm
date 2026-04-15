namespace bdDevs.Shared.Records.CRM;

/// <summary>
/// Record for creating a new CRM IELTS information.
/// </summary>
public record CreateCrmIeltsInformationRecord(
    int ApplicantId,
    decimal? Ieltslistening,
    decimal? Ieltsreading,
    decimal? Ieltswriting,
    decimal? Ieltsspeaking,
    decimal? IeltsoverallScore,
    DateTime? Ieltsdate,
    string? IeltsscannedCopyPath,
    string? IeltsadditionalInformation,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for updating an existing CRM IELTS information.
/// </summary>
public record UpdateCrmIeltsInformationRecord(
    int IELTSInformationId,
    int ApplicantId,
    decimal? Ieltslistening,
    decimal? Ieltsreading,
    decimal? Ieltswriting,
    decimal? Ieltsspeaking,
    decimal? IeltsoverallScore,
    DateTime? Ieltsdate,
    string? IeltsscannedCopyPath,
    string? IeltsadditionalInformation,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for deleting a CRM IELTS information.
/// </summary>
/// <param name="IELTSInformationId">ID of the IELTS information to delete.</param>
public record DeleteCrmIeltsInformationRecord(int IELTSInformationId);
