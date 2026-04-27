    namespace bdDevs.Shared.Records.CRM;

    public record CreateCrmCommunicationTypeRecord(
        string CommunicationTypeName,
bool IsDigitalChannel,
DateTime CreatedDate,
int CreatedBy,
DateTime? UpdatedDate,
int? UpdatedBy);

    public record UpdateCrmCommunicationTypeRecord(
        int CommunicationTypeId,
string CommunicationTypeName,
bool IsDigitalChannel,
DateTime CreatedDate,
int CreatedBy,
DateTime? UpdatedDate,
int? UpdatedBy);

    public record DeleteCrmCommunicationTypeRecord(int CommunicationTypeId);
