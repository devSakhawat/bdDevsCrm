      namespace bdDevs.Shared.DataTransferObjects.CRM;

      public record CrmLeadSourceDto
      {
        public int LeadSourceId { get; init; }

public string LeadSourceName { get; init; }

public int SortOrder { get; init; }

public bool IsActive { get; init; }

public DateTime CreatedDate { get; init; }

public int CreatedBy { get; init; }

public DateTime? UpdatedDate { get; init; }

public int? UpdatedBy { get; init; }
      }

      public record CrmLeadSourceDDLDto
      {
        public int LeadSourceId { get; init; }

public string LeadSourceName { get; init; } = string.Empty;
      }
