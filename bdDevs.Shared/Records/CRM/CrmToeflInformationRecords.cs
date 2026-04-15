namespace bdDevs.Shared.Records.CRM;

/// <summary>
/// Record for creating a new CRM TOEFL information.
/// </summary>
public record CreateCrmToeflInformationRecord(
    int ApplicantId,
    decimal? Toefllistening,
    decimal? Toeflreading,
    decimal? Toeflwriting,
    decimal? Toeflspeaking,
    decimal? ToefloverallScore,
    DateTime? Toefldate,
    string? ToeflscannedCopyPath,
    string? ToefladditionalInformation,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for updating an existing CRM TOEFL information.
/// </summary>
public record UpdateCrmToeflInformationRecord(
    int TOEFLInformationId,
    int ApplicantId,
    decimal? Toefllistening,
    decimal? Toeflreading,
    decimal? Toeflwriting,
    decimal? Toeflspeaking,
    decimal? ToefloverallScore,
    DateTime? Toefldate,
    string? ToeflscannedCopyPath,
    string? ToefladditionalInformation,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for deleting a CRM TOEFL information.
/// </summary>
/// <param name="TOEFLInformationId">ID of the TOEFL information to delete.</param>
public record DeleteCrmToeflInformationRecord(int TOEFLInformationId);
