using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.CRM;

public partial class CrmFollowUp
{
    public int FollowUpId { get; set; }

    public int? LeadId { get; set; }

    public int? EnquiryId { get; set; }

    public DateTime FollowUpDate { get; set; }

    public string? FollowUpType { get; set; }

    public string? Notes { get; set; }

    public DateTime? NextFollowUpDate { get; set; }

    public bool IsCompleted { get; set; }

    public int? CounselorId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual CrmLead? Lead { get; set; }

    public virtual CrmEnquiry? Enquiry { get; set; }

    public virtual CrmCounselor? Counselor { get; set; }
}
