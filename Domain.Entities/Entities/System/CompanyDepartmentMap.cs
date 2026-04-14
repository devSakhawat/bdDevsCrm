using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.System;

public partial class CompanyDepartmentMap
{
    public int SbuDepartmentMapId { get; set; }

    public int CompanyId { get; set; }

    public int DepartmentId { get; set; }
}
