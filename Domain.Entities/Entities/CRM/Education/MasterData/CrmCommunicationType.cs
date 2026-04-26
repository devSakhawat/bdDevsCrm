    using System;

    namespace Domain.Entities.Entities.CRM;

    public partial class CrmCommunicationType
    {
        public int CommunicationTypeId { get; set; }

public string CommunicationTypeName { get; set; }

public bool IsDigitalChannel { get; set; }

public DateTime CreatedDate { get; set; }

public int CreatedBy { get; set; }

public DateTime? UpdatedDate { get; set; }

public int? UpdatedBy { get; set; }
    }
