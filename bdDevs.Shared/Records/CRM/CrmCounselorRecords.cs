namespace bdDevs.Shared.Records.CRM;

/// <summary>Record for creating a new CRM counselor.</summary>
public record CreateCrmCounselorRecord(
    string CounselorName,
    string? CounselorCode,
    string? Email,
    string? Phone,
    int? OfficeId,
    int? UserId,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>Record for updating an existing CRM counselor.</summary>
public record UpdateCrmCounselorRecord(
    int CounselorId,
    string CounselorName,
    string? CounselorCode,
    string? Email,
    string? Phone,
    int? OfficeId,
    int? UserId,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>Record for deleting a CRM counselor.</summary>
/// <param name="CounselorId">ID of the counselor to delete.</param>
public record DeleteCrmCounselorRecord(int CounselorId);
