using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.System;

public partial class TokenBlacklist
{
    public string Token { get; set; } = null!;

    public DateTime ExpiryDate { get; set; }

    public Guid TokenId { get; set; }
}
