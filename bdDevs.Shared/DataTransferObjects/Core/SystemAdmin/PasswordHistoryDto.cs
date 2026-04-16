using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

public class PasswordHistoryDto
{
  public int HistoryId { get; set; }
  public int? UserId { get; set; }
  public string? OldPassword { get; set; }
  public DateTime? PasswordChangeDate { get; set; }
}

public class PasswordHistoryDDLDto
{
  public int HistoryId { get; set; }
  public int? UserId { get; set; }
  public DateTime? PasswordChangeDate { get; set; }
}
