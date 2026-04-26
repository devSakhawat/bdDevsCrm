      namespace bdDevs.Shared.DataTransferObjects.CRM;

      public record CrmVisaStatusDto
      {
        public int VisaStatusId { get; init; }

public string VisaStatusName { get; init; }

public int SequenceNo { get; init; }

public bool IsFinalStatus { get; init; }

public DateTime CreatedDate { get; init; }

public int CreatedBy { get; init; }

public DateTime? UpdatedDate { get; init; }

public int? UpdatedBy { get; init; }
      }

      public record CrmVisaStatusDDLDto
      {
        public int VisaStatusId { get; init; }

public string VisaStatusName { get; init; } = string.Empty;
      }
