using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.CRM;

public partial class CrmIntakeYear
{
    public int IntakeYearId { get; set; }

    public string YearName { get; set; } = null!;

    public string? YearCode { get; set; }

    public int YearValue { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }
}