namespace bdDevs.Shared.Records.CRM;

/// <summary>Record for creating a new CRM student.</summary>
public record CreateCrmStudentRecord(
    string StudentName,
    string? StudentCode,
    string? Email,
    string? Phone,
    int? LeadId,
    int? StudentStatusId,
    int? AgentId,
    int? CounselorId,
    DateTime? DateOfBirth,
    string? PassportNumber,
    int? VisaTypeId,
    string? Nationality,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>Record for updating an existing CRM student.</summary>
public record UpdateCrmStudentRecord(
    int StudentId,
    string StudentName,
    string? StudentCode,
    string? Email,
    string? Phone,
    int? LeadId,
    int? StudentStatusId,
    int? AgentId,
    int? CounselorId,
    DateTime? DateOfBirth,
    string? PassportNumber,
    int? VisaTypeId,
    string? Nationality,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>Record for deleting a CRM student.</summary>
/// <param name="StudentId">ID of the student to delete.</param>
public record DeleteCrmStudentRecord(int StudentId);
