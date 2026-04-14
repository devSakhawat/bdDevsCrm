using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.System;

public partial class DocumentTemplate
{
    public int DocumentId { get; set; }

    public string DocumentTitle { get; set; } = null!;

    public string? DocumentText { get; set; }

    public string TemplateName { get; set; } = null!;

    public int? DocumentTypeId { get; set; }
}
