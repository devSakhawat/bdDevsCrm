using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdDevs.Shared.DataTransferObjects.Core.HR;

public class BranchDto
{
  public int Branchid { get; set; }

  public string Branchname { get; set; } = null!;

  public string? Branchcode { get; set; }

  public string? Branchdescription { get; set; }

  public int? IsCostCentre { get; set; }

  public int? IsActive { get; set; }

  public int? DebitAccountHead { get; set; }

  public int? CreditAccountHead { get; set; }

  public int? ContraEntryApplicable { get; set; }

  public string? BranchAddress { get; set; }

  public int? CreateBy { get; set; }

  public DateTime? CreateDate { get; set; }

  public int? UpdateBy { get; set; }

  public DateTime? UpdateDate { get; set; }
}
