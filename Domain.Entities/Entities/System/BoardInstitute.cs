using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.System;

public partial class BoardInstitute
{
    public int BoardInstituteId { get; set; }

    public string? BoardInstituteName { get; set; }

    public int? IsActive { get; set; }
}
