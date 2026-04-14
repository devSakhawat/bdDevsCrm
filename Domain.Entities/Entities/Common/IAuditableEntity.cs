using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Entities.Common;

/// <summary>
/// Implement on entities that need audit fields.
/// Place this file in your core/entities project (not Shared).
/// </summary>
public interface IAuditableEntity
{
  int? CreatedBy { get; set; }
  DateTime? CreatedDate { get; set; }
  int? UpdatedBy { get; set; }
  DateTime? UpdatedDate { get; set; }
}
