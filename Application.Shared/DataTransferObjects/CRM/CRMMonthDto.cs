using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdDevCRM.Shared.DataTransferObjects.CRM;

public class CrmMonthDto
{
  public int MonthId { get; set; }

  public string MonthName { get; set; } = null!;

  public string? MonthCode { get; set; }

  public bool? Status { get; set; }

  public DateTime CreatedDate { get; init; }
  public int CreatedBy { get; init; }
  public DateTime? UpdatedDate { get; init; }
  public int? UpdatedBy { get; init; }
}
