    namespace bdDevs.Shared.Records.CRM;

    public record CreateCrmApplicationStatusRecord(
        string ApplicationStatusName,
int SequenceNo,
bool IsFinalStatus,
DateTime CreatedDate,
int CreatedBy,
DateTime? UpdatedDate,
int? UpdatedBy);

    public record UpdateCrmApplicationStatusRecord(
        int ApplicationStatusId,
string ApplicationStatusName,
int SequenceNo,
bool IsFinalStatus,
DateTime CreatedDate,
int CreatedBy,
DateTime? UpdatedDate,
int? UpdatedBy);

    public record DeleteCrmApplicationStatusRecord(int ApplicationStatusId);
