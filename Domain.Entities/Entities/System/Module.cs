using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.System;

public partial class Module
{
    public int ModuleId { get; set; }

    public string ModuleName { get; set; } = null!;
}
