    namespace bdDevs.Shared.Records.CRM;

    public record CreateCrmLeadSourceRecord(
        string LeadSourceName,
int SortOrder,
bool IsActive,
DateTime CreatedDate,
int CreatedBy,
DateTime? UpdatedDate,
int? UpdatedBy);

    public record UpdateCrmLeadSourceRecord(
        int LeadSourceId,
string LeadSourceName,
int SortOrder,
bool IsActive,
DateTime CreatedDate,
int CreatedBy,
DateTime? UpdatedDate,
int? UpdatedBy);

    public record DeleteCrmLeadSourceRecord(int LeadSourceId);
