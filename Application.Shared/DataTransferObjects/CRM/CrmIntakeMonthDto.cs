namespace bdDevCRM.Shared.DataTransferObjects.CRM;

public record CrmIntakeMonthDto
{
  public int IntakeMonthId { get; init; }
  public string MonthName { get; init; } = string.Empty;
  public string? MonthCode { get; init; }
  public int MonthNumber { get; init; }
  public string? Description { get; init; }
  public bool IsActive { get; init; }
  public DateTime CreatedDate { get; init; }
  public int CreatedBy { get; init; }
  public DateTime? UpdatedDate { get; init; }
  public int? UpdatedBy { get; init; }
}

public record CrmIntakeMonthDDL
{
  public int IntakeMonthId { get; init; }
  public string MonthName { get; init; } = string.Empty;
  public string? MonthCode { get; init; }
  public int MonthNumber { get; init; }
}