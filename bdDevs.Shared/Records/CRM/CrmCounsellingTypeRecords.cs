namespace bdDevs.Shared.Records.CRM;

public record CreateCrmCounsellingTypeRecord(
    string CounsellingTypeName,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

public record UpdateCrmCounsellingTypeRecord(
    int CounsellingTypeId,
    string CounsellingTypeName,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

public record DeleteCrmCounsellingTypeRecord(int CounsellingTypeId);
