using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

public class CurrencyDto
{
  public int CurrencyId { get; set; }

  public string? CurrencyName { get; set; }

  public int? IsDefault { get; set; }

  public int? IsActive { get; set; }

  public int? CreateBy { get; set; }

  public DateTime? CreateDate { get; set; }

  public int? UpdateBy { get; set; }

  public DateTime? UpdateDate { get; set; }
}
