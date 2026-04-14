namespace bdDevCRM.Shared.DataTransferObjects.CRM;

public record CrmIntakeYearDto
{
  public int IntakeYearId { get; init; }
  public string YearName { get; init; } = string.Empty;
  public string? YearCode { get; init; }
  public int YearValue { get; init; }
  public string? Description { get; init; }
  public bool IsActive { get; init; }
  public DateTime CreatedDate { get; init; }
  public int CreatedBy { get; init; }
  public DateTime? UpdatedDate { get; init; }
  public int? UpdatedBy { get; init; }
}

public record CrmIntakeYearDDL
{
  public int IntakeYearId { get; init; }
  public string YearName { get; init; } = string.Empty;
  public string? YearCode { get; init; }
  public int YearValue { get; init; }
}