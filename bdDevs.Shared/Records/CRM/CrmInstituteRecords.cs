namespace bdDevs.Shared.Records.CRM;

/// <summary>
/// Record for creating a new CRM institute.
/// </summary>
public record CreateCrmInstituteRecord(
    int CountryId,
    string InstituteName,
    string? Campus,
    string? Website,
    decimal? MonthlyLivingCost,
    string? FundsRequirementforVisa,
    decimal? ApplicationFee,
    int? CurrencyId,
    bool? IsLanguageMandatory,
    string? LanguagesRequirement,
    string? InstitutionalBenefits,
    string? PartTimeWorkDetails,
    string? ScholarshipsPolicy,
    string? InstitutionStatusNotes,
    string? InstitutionLogo,
    string? InstitutionProspectus,
    int? InstituteTypeId,
    string? InstituteCode,
    string? InstituteEmail,
    string? InstituteAddress,
    string? InstitutePhoneNo,
    string? InstituteMobileNo,
    bool? Status);

/// <summary>
/// Record for updating an existing CRM institute.
/// </summary>
public record UpdateCrmInstituteRecord(
    int InstituteId,
    int CountryId,
    string InstituteName,
    string? Campus,
    string? Website,
    decimal? MonthlyLivingCost,
    string? FundsRequirementforVisa,
    decimal? ApplicationFee,
    int? CurrencyId,
    bool? IsLanguageMandatory,
    string? LanguagesRequirement,
    string? InstitutionalBenefits,
    string? PartTimeWorkDetails,
    string? ScholarshipsPolicy,
    string? InstitutionStatusNotes,
    string? InstitutionLogo,
    string? InstitutionProspectus,
    int? InstituteTypeId,
    string? InstituteCode,
    string? InstituteEmail,
    string? InstituteAddress,
    string? InstitutePhoneNo,
    string? InstituteMobileNo,
    bool? Status);

/// <summary>
/// Record for deleting a CRM institute.
/// </summary>
/// <param name="InstituteId">ID of the institute to delete.</param>
public record DeleteCrmInstituteRecord(int InstituteId);
