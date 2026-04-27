      namespace bdDevs.Shared.DataTransferObjects.CRM;

      public record CrmDocumentTypeDto
      {
        public int DocumentTypeId { get; init; }

public string DocumentTypeName { get; init; }

public string? Code { get; init; }

public bool IsMandatoryForApplication { get; init; }

public bool IsActive { get; init; }

public DateTime CreatedDate { get; init; }

public int CreatedBy { get; init; }

public DateTime? UpdatedDate { get; init; }

public int? UpdatedBy { get; init; }
      }

      public record CrmDocumentTypeDDLDto
      {
        public int DocumentTypeId { get; init; }

public string DocumentTypeName { get; init; } = string.Empty;
      }
