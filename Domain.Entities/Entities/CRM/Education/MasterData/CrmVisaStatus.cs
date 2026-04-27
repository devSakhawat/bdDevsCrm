    using System;

    namespace Domain.Entities.Entities.CRM;

    public partial class CrmVisaStatus
    {
        public int VisaStatusId { get; set; }

public string VisaStatusName { get; set; }

public int SequenceNo { get; set; }

public bool IsFinalStatus { get; set; }

public DateTime CreatedDate { get; set; }

public int CreatedBy { get; set; }

public DateTime? UpdatedDate { get; set; }

public int? UpdatedBy { get; set; }
    }
