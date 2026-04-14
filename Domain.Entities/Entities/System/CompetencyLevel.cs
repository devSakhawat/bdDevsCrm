using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.System;

public partial class CompetencyLevel
{
    public int LevelId { get; set; }

    public string? LevelTitle { get; set; }

    public string? Remarks { get; set; }
}
