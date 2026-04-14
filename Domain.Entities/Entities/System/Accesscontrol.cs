using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.System;

public partial class AccessControl
{
    public int AccessId { get; set; }

    public string AccessName { get; set; } = null!;
}
