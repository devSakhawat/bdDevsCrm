using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.System;

public partial class CurrencyRate
{
    public int CurencyRateId { get; set; }

    public int CurrencyId { get; set; }

    public decimal? CurrencyRateRation { get; set; }

    public DateTime? CurrencyMonth { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }
}
