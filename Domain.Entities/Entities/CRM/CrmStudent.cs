using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.CRM;

public partial class CrmStudent
{
    public int StudentId { get; set; }

    public string StudentName { get; set; } = null!;

    public string? StudentCode { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public int? LeadId { get; set; }

    public int? StudentStatusId { get; set; }

    public int? AgentId { get; set; }

    public int? CounselorId { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? PassportNumber { get; set; }

    public int? VisaTypeId { get; set; }

    public string? Nationality { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual CrmLead? Lead { get; set; }

    public virtual CrmStudentStatus? StudentStatus { get; set; }

    public virtual CrmAgent? Agent { get; set; }

    public virtual CrmCounselor? Counselor { get; set; }

    public virtual CrmVisaType? VisaType { get; set; }
}
