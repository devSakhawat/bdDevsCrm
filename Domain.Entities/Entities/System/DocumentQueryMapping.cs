using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.System;

public partial class DocumentQueryMapping
{
    public int DocumentQueryId { get; set; }

    public int ReportHeaderId { get; set; }

    public int DocumentTypeId { get; set; }

    public string? ParameterDefination { get; set; }
}
