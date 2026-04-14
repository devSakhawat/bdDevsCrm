using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.CRM;

public partial class CrmApplication
{
    public int ApplicationId { get; set; }

    public DateTime ApplicationDate { get; set; }

    public int StateId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }
}
