      namespace bdDevs.Shared.DataTransferObjects.CRM;

      public record CrmLeadStageDto
      {
        public int LeadStageId { get; init; }

public string LeadStageName { get; init; }

public string? StageType { get; init; }

public bool IsClosedStage { get; init; }

public DateTime CreatedDate { get; init; }

public int CreatedBy { get; init; }

public DateTime? UpdatedDate { get; init; }

public int? UpdatedBy { get; init; }
      }

      public record CrmLeadStageDDLDto
      {
        public int LeadStageId { get; init; }

public string LeadStageName { get; init; } = string.Empty;
      }
