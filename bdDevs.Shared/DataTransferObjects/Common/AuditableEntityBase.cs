using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdDevs.Shared.DataTransferObjects.Common;

/// <summary>
/// Optional base class for convenience.
/// </summary>
public abstract class AuditableEntityBase : IAuditableEntity
{
  public int? CreatedBy { get; set; }
  public DateTime? CreatedDate { get; set; }
  public int? UpdatedBy { get; set; }
  public DateTime? UpdatedDate { get; set; }
}

