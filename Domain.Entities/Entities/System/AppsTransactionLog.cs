using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.System;

public partial class AppsTransactionLog
{
    public int TransactionLogId { get; set; }

    public DateTime TransactionDate { get; set; }

    public string TransactionType { get; set; } = null!;

    public int? ResponseCode { get; set; }

    public string? Request { get; set; }

    public string? Response { get; set; }

    public string? Remarks { get; set; }

    public string? AppsUserId { get; set; }

    public string? EmployeeId { get; set; }
}
