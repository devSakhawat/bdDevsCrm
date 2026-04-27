namespace bdDevs.Shared.Records.CRM;

/// <summary>Record for creating a new CRM enquiry.</summary>
public record CreateCrmEnquiryRecord(
    int? LeadId,
    int? StudentId,
    int? CourseId,
    int? InstituteId,
    int? CountryId,
    DateTime EnquiryDate,
    int? ExpectedIntakeMonth,
    int? ExpectedIntakeYear,
    string? Notes,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>Record for updating an existing CRM enquiry.</summary>
public record UpdateCrmEnquiryRecord(
    int EnquiryId,
    int? LeadId,
    int? StudentId,
    int? CourseId,
    int? InstituteId,
    int? CountryId,
    DateTime EnquiryDate,
    int? ExpectedIntakeMonth,
    int? ExpectedIntakeYear,
    string? Notes,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>Record for deleting a CRM enquiry.</summary>
/// <param name="EnquiryId">ID of the enquiry to delete.</param>
public record DeleteCrmEnquiryRecord(int EnquiryId);
