using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.CRM;

public partial class CrmEnquiry
{
    public int EnquiryId { get; set; }

    public int? LeadId { get; set; }

    public int? StudentId { get; set; }

    public int? CourseId { get; set; }

    public int? InstituteId { get; set; }

    public int? CountryId { get; set; }

    public DateTime EnquiryDate { get; set; }

    public int? ExpectedIntakeMonth { get; set; }

    public int? ExpectedIntakeYear { get; set; }

    public string? Notes { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual CrmLead? Lead { get; set; }

    public virtual CrmStudent? Student { get; set; }

    public virtual CrmCourse? Course { get; set; }

    public virtual CrmInstitute? Institute { get; set; }

    public virtual CrmCountry? Country { get; set; }
}
