    namespace bdDevs.Shared.Records.CRM;

    public record CreateCrmVisaStatusRecord(
        string VisaStatusName,
int SequenceNo,
bool IsFinalStatus,
DateTime CreatedDate,
int CreatedBy,
DateTime? UpdatedDate,
int? UpdatedBy);

    public record UpdateCrmVisaStatusRecord(
        int VisaStatusId,
string VisaStatusName,
int SequenceNo,
bool IsFinalStatus,
DateTime CreatedDate,
int CreatedBy,
DateTime? UpdatedDate,
int? UpdatedBy);

    public record DeleteCrmVisaStatusRecord(int VisaStatusId);
