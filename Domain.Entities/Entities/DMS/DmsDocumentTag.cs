using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.DMS;

public partial class DmsDocumentTag
{
    public int TagId { get; set; }

    public string DocumentTagName { get; set; } = null!;

    public virtual ICollection<DmsDocumentTagMap> DmsDocumentTagMap { get; set; } = new List<DmsDocumentTagMap>();
}
