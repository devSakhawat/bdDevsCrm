using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.System;

public partial class CompanyLocationMap
{
    public int SbuLocationMapId { get; set; }

    public int CompanyId { get; set; }

    public int BranchId { get; set; }
}
