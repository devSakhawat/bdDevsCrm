namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating new approver details.
/// </summary>
public record CreateApproverDetailsRecord(
    int MenuId,
    int ApplicationId,
    bool IsOpen,
    string? Comments,
    DateTime? ApprovedDate,
    int? ApplicantHrRecordId,
    int? ApprovedBy,
    int? ApproverType,
    int? Sequence,
    int? ModuleId,
    int? CompanyId,
    int? BranchId,
    int? DivisionId,
    int? DepartmentId,
    int? FacilityId,
    int? SectionId,
    int? DesignationId,
    int? GradeId,
    int? EmployeeTypeId,
    int? FuncId,
    int? AssignApproverId);

/// <summary>
/// Record for updating existing approver details.
/// </summary>
public record UpdateApproverDetailsRecord(
    int RemarksId,
    int MenuId,
    int ApplicationId,
    bool IsOpen,
    string? Comments,
    DateTime? ApprovedDate,
    int? ApplicantHrRecordId,
    int? ApprovedBy,
    int? ApproverType,
    int? Sequence,
    int? ModuleId,
    int? CompanyId,
    int? BranchId,
    int? DivisionId,
    int? DepartmentId,
    int? FacilityId,
    int? SectionId,
    int? DesignationId,
    int? GradeId,
    int? EmployeeTypeId,
    int? FuncId,
    int? AssignApproverId);

/// <summary>
/// Record for deleting approver details.
/// </summary>
public record DeleteApproverDetailsRecord(int RemarksId);
