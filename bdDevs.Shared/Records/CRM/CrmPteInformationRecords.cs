namespace bdDevs.Shared.Records.CRM;

/// <summary>
/// Record for creating a new CRM PTE information.
/// </summary>
public record CreateCrmPteInformationRecord(
    int ApplicantId,
    decimal? Ptelistening,
    decimal? Ptereading,
    decimal? Ptewriting,
    decimal? Ptespeaking,
    decimal? PteoverallScore,
    DateTime? Ptedate,
    string? PtescannedCopyPath,
    string? PteadditionalInformation,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for updating an existing CRM PTE information.
/// </summary>
public record UpdateCrmPteInformationRecord(
    int PTEInformationId,
    int ApplicantId,
    decimal? Ptelistening,
    decimal? Ptereading,
    decimal? Ptewriting,
    decimal? Ptespeaking,
    decimal? PteoverallScore,
    DateTime? Ptedate,
    string? PtescannedCopyPath,
    string? PteadditionalInformation,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for deleting a CRM PTE information.
/// </summary>
/// <param name="PTEInformationId">ID of the PTE information to delete.</param>
public record DeleteCrmPteInformationRecord(int PTEInformationId);
