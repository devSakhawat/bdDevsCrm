namespace bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

public class CompanyDto
{
  public int CompanyId { get; set; }

  public string? CompanyCode { get; set; }

  public string CompanyName { get; set; } = null!;

  public string? Address { get; set; }

  public string? Phone { get; set; }

  public string? Fax { get; set; }

  public string? Email { get; set; }

  public string? FullLogoPath { get; set; }

  public string? PrimaryContact { get; set; }

  public int Flag { get; set; }

  public int FiscalYearStart { get; set; }

  public int? MotherId { get; set; }

  public int? IsCostCentre { get; set; }

  public int? IsActive { get; set; }

  public DateOnly? GratuityStartDate { get; set; }

  public string? FullLogoPathForReport { get; set; }

  public string? CompanyTin { get; set; }

  public bool? IsPfApplicable { get; set; }

  public bool? IsEwfApplicable { get; set; }

  public int? IsPfApplicabe { get; set; }

  public int? IsEwfApplicabe { get; set; }

  public string? CompanyAlias { get; set; }

  public string? CompanyZone { get; set; }

  public string? CompanyCircle { get; set; }

  public int? IsCompanyContributionDisable { get; set; }

  public int? CreateBy { get; set; }

  public DateTime? CreateDate { get; set; }

  public int? UpdateBy { get; set; }

  public DateTime? UpdateDate { get; set; }

  public int? IsElautoAddedForCurrentYear { get; set; }

  public int? IsRosterAutoCarryForward { get; set; }

  public string? LetterHeader { get; set; }

  public string? LetterFooter { get; set; }

  public string? CompanyRegisterNo { get; set; }

  public int? CompanySortOrder { get; set; }

  public int? CompanyAccessGroupNo { get; set; }

  public int? IsSentGreetingsOrWishNotification { get; set; }

  public int? IsNotifyForNextMonIncEligible { get; set; }
}
