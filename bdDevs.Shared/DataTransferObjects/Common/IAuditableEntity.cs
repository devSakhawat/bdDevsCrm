using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdDevs.Shared.DataTransferObjects.Common;

/// <summary>
/// Contract for audit fields. Any entity that should be auditable must implement this.
/// </summary>
public interface IAuditableEntity
{
  int? CreatedBy { get; set; }
  DateTime? CreatedDate { get; set; }
  int? UpdatedBy { get; set; }
  DateTime? UpdatedDate { get; set; }
}