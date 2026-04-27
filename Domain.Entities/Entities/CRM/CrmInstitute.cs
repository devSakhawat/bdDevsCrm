using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.CRM;

public partial class CrmInstitute
{
    public int InstituteId { get; set; }

    public int CountryId { get; set; }

    public string InstituteName { get; set; } = null!;

    public string? Campus { get; set; }

    public string? Website { get; set; }

    public decimal? MonthlyLivingCost { get; set; }

    public string? FundsRequirementforVisa { get; set; }

    public decimal? ApplicationFee { get; set; }

    public int? CurrencyId { get; set; }

    public bool? IsLanguageMandatory { get; set; }

    public string? LanguagesRequirement { get; set; }

    public string? InstitutionalBenefits { get; set; }

    public string? PartTimeWorkDetails { get; set; }

    public string? ScholarshipsPolicy { get; set; }

    public string? InstitutionStatusNotes { get; set; }

    public string? InstitutionLogo { get; set; }

    public string? InstitutionProspectus { get; set; }

    public int? InstituteTypeId { get; set; }

    public string? InstituteCode { get; set; }

    public string? InstituteEmail { get; set; }

    public string? InstituteAddress { get; set; }

    public string? InstitutePhoneNo { get; set; }

    public string? InstituteMobileNo { get; set; }
    public bool? Status { get; set; }

    // Phase 1 upgrade — partnership & ranking
    public string? AccreditationStatus { get; set; }
    public int? WorldRanking { get; set; }
    public DateTime? PartnershipStartDate { get; set; }
    public DateTime? PartnershipExpiryDate { get; set; }
    public decimal? CommissionRate { get; set; }
    /// <summary>CommissionType: 1=Percentage, 2=Fixed</summary>
    public byte? CommissionType { get; set; }
    public string? AdmissionContactName { get; set; }
    public string? AdmissionContactEmail { get; set; }
    public string? AdmissionContactPhone { get; set; }

    // Phase 1 upgrade — entry requirements
    public decimal? MinIeltsScore { get; set; }
    public decimal? MinToeflScore { get; set; }
    public decimal? MinAcademicScore { get; set; }
    public int? MinWorkExpYears { get; set; }
    public bool RequiresSop { get; set; }
    public int? RequiredRefLetters { get; set; }
}
