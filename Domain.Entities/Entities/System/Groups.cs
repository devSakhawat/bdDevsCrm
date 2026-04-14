using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.System;

public partial class Groups
{
    public int GroupId { get; set; }

    public string? GroupName { get; set; }

    public int? IsDefault { get; set; }
}
