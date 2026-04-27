using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.CRM;

public partial class CrmCounselor
{
    public int CounselorId { get; set; }

    public string CounselorName { get; set; } = null!;

    public string? CounselorCode { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public int? OfficeId { get; set; }

    public int? UserId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual CrmOffice? Office { get; set; }
}
