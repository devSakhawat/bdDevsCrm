namespace Domain.Entities.Entities.CRM;

public partial class CrmMonth
{
  public int MonthId { get; set; }

  public string MonthName { get; set; } = null!;

  public string? MonthCode { get; set; }

  public bool? Status { get; set; }

  //public DateTime CreatedDate { get; init; }
  //public int CreatedBy { get; init; }
  //public DateTime? UpdatedDate { get; init; }
  //public int? UpdatedBy { get; init; }
}
