using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.System;

public partial class GroupMember
{
    public int GroupId { get; set; }

    public int UserId { get; set; }

    public string? GroupOption { get; set; }
}
