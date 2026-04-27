      namespace bdDevs.Shared.DataTransferObjects.CRM;

      public record CrmApplicationStatusDto
      {
        public int ApplicationStatusId { get; init; }

public string ApplicationStatusName { get; init; }

public int SequenceNo { get; init; }

public bool IsFinalStatus { get; init; }

public DateTime CreatedDate { get; init; }

public int CreatedBy { get; init; }

public DateTime? UpdatedDate { get; init; }

public int? UpdatedBy { get; init; }
      }

      public record CrmApplicationStatusDDLDto
      {
        public int ApplicationStatusId { get; init; }

public string ApplicationStatusName { get; init; } = string.Empty;
      }
