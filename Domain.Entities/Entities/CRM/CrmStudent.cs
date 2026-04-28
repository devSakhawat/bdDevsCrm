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
    public int? BranchId { get; set; }
    public int? ProcessingOfficerId { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public byte? Gender { get; set; }
    public string? PassportNumber { get; set; }
    public DateTime? PassportExpiryDate { get; set; }
    public DateTime? PassportIssueDate { get; set; }
    public int? PassportIssueCountryId { get; set; }
    public int? VisaTypeId { get; set; }
    public string? Nationality { get; set; }
    public int? NationalityCountryId { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? EmergencyContactRelation { get; set; }
    public int? PreferredCountryId { get; set; }
    public int? PreferredDegreeLevelId { get; set; }
    public string? DesiredIntake { get; set; }
    public byte? IeltsStatus { get; set; }
    public decimal? IeltsScore { get; set; }
    public DateTime? IeltsExamDate { get; set; }
    public bool IsApplicationReady { get; set; }
    public DateTime? ApplicationReadyDate { get; set; }
    public int? ApplicationReadySetBy { get; set; }
    public bool ConsentPersonalData { get; set; }
    public bool ConsentMarketing { get; set; }
    public bool ConsentDocumentProcessing { get; set; }
    public bool ConsentInternationalSharing { get; set; }
    public bool ConsentTermsAccepted { get; set; }
    public bool IsDeleted { get; set; }
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
