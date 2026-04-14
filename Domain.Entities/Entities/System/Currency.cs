using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.System;

public partial class Currency
{
    public int CurrencyId { get; set; }

    public string CurrencyName { get; set; } = null!;

    public string CurrencyCode { get; set; } = null!;
}
