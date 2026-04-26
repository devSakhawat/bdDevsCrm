    using System;

    namespace Domain.Entities.Entities.CRM;

    public partial class CrmDocumentType
    {
        public int DocumentTypeId { get; set; }

public string DocumentTypeName { get; set; }

public string? Code { get; set; }

public bool IsMandatoryForApplication { get; set; }

public bool IsActive { get; set; }

public DateTime CreatedDate { get; set; }

public int CreatedBy { get; set; }

public DateTime? UpdatedDate { get; set; }

public int? UpdatedBy { get; set; }
    }
