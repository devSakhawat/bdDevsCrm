    using System;

    namespace Domain.Entities.Entities.CRM;

    public partial class CrmApplicationStatus
    {
        public int ApplicationStatusId { get; set; }

public string ApplicationStatusName { get; set; }

public int SequenceNo { get; set; }

public bool IsFinalStatus { get; set; }

public DateTime CreatedDate { get; set; }

public int CreatedBy { get; set; }

public DateTime? UpdatedDate { get; set; }

public int? UpdatedBy { get; set; }
    }
