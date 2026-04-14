using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.System;

public partial class PasswordHistory
{
    public int HistoryId { get; set; }

    public int? UserId { get; set; }

    public string? OldPassword { get; set; }

    public DateTime? PasswordChangeDate { get; set; }
}
