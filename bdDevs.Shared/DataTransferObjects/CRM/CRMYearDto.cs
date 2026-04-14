using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdDevs.Shared.DataTransferObjects.CRM;

public class CrmYearDto
{
  public int YearId { get; set; }

  public string YearName { get; set; } = null!;

  public string? YearCode { get; set; }

  public bool? Status { get; set; }
}
