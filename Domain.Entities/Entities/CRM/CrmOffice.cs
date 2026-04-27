using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.CRM;

public partial class CrmOffice
{
    public int OfficeId { get; set; }

    public string OfficeName { get; set; } = null!;

    public string? OfficeCode { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }
}
