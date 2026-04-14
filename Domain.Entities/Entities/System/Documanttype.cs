using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.System;

public partial class Documanttype
{
    public int Documenttypeid { get; set; }

    public string? Documentname { get; set; }

    public DateTime? Initiationdate { get; set; }

    public string? Description { get; set; }

    /// <summary>
    /// 1=Personal Document,2=Applicant Document
    /// </summary>
    public int? UseType { get; set; }
}
