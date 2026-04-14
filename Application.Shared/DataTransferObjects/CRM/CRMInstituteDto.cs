using Microsoft.AspNetCore.Http;

namespace bdDevCRM.Shared.DataTransferObjects.CRM;

public class CrmInstituteDto
{
  // --- PK & FK ---
  public int InstituteId { get; set; }
  public int CountryId { get; set; }
  public int? CurrencyId { get; set; }
  public int? InstituteTypeId { get; set; }

  // --- Basic Info ---
  public string InstituteName { get; set; } = null!;
  public string? InstituteCode { get; set; }
  public string? InstituteEmail { get; set; }
  public string? InstituteAddress { get; set; }
  public string? InstitutePhoneNo { get; set; }
  public string? InstituteMobileNo { get; set; }
  public string? Campus { get; set; }
  public string? Website { get; set; }

  // --- Financial / Visa ---
  public decimal? MonthlyLivingCost { get; set; }
  //public string? FundsRequirementforVisa { get; set; }
  public decimal? FundsRequirementforVisa { get; set; }
  public decimal? ApplicationFee { get; set; }

  // --- Language & Academic ---
  public bool? IsLanguageMandatory { get; set; }
  public string? LanguagesRequirement { get; set; }

  // --- Descriptive Info ---
  public string? InstitutionalBenefits { get; set; }
  public string? PartTimeWorkDetails { get; set; }
  public string? ScholarshipsPolicy { get; set; }
  public string? InstitutionStatusNotes { get; set; }

  // --- File Path (DB) ---
  public string? InstitutionLogo { get; set; }
  public string? InstitutionProspectus { get; set; }

  // ---  New file field ---
  public IFormFile? InstitutionLogoFile { get; set; }
  public IFormFile? InstitutionProspectusFile { get; set; }

  // --- Status ---
  public bool? Status { get; set; }


  // --- PK & FK ---
  public string? CountryName { get; set; }
  public string? CurrencyName { get; set; }
  public string? InstituteTypeName { get; set; }
}


