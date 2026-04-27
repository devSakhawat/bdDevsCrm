    namespace bdDevs.Shared.Records.CRM;

    public record CreateCrmLeadStageRecord(
        string LeadStageName,
string? StageType,
bool IsClosedStage,
DateTime CreatedDate,
int CreatedBy,
DateTime? UpdatedDate,
int? UpdatedBy);

    public record UpdateCrmLeadStageRecord(
        int LeadStageId,
string LeadStageName,
string? StageType,
bool IsClosedStage,
DateTime CreatedDate,
int CreatedBy,
DateTime? UpdatedDate,
int? UpdatedBy);

    public record DeleteCrmLeadStageRecord(int LeadStageId);
