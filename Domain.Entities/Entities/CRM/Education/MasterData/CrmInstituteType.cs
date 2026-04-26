using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.CRM;

public partial class CrmInstituteType
{
    public int InstituteTypeId { get; set; }

    public string InstituteTypeName { get; set; } = null!;
}
