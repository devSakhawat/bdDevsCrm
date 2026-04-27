using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.CRM;

public partial class CrmNote
{
    public int NoteId { get; set; }

    public string EntityType { get; set; } = null!;

    public int EntityId { get; set; }

    public string NoteText { get; set; } = null!;

    public DateTime NoteDate { get; set; }

    public bool IsPrivate { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }
}
