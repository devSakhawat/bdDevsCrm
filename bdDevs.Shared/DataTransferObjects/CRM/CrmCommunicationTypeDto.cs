      namespace bdDevs.Shared.DataTransferObjects.CRM;

      public record CrmCommunicationTypeDto
      {
        public int CommunicationTypeId { get; init; }

public string CommunicationTypeName { get; init; }

public bool IsDigitalChannel { get; init; }

public DateTime CreatedDate { get; init; }

public int CreatedBy { get; init; }

public DateTime? UpdatedDate { get; init; }

public int? UpdatedBy { get; init; }
      }

      public record CrmCommunicationTypeDDLDto
      {
        public int CommunicationTypeId { get; init; }

public string CommunicationTypeName { get; init; } = string.Empty;
      }
