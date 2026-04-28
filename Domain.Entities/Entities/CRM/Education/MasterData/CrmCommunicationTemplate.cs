using System;

namespace Domain.Entities.Entities.CRM;

public partial class CrmCommunicationTemplate
{
    public int CommunicationTemplateId { get; set; }

    public int CommunicationTypeId { get; set; }

    public string TemplateName { get; set; } = string.Empty;

    public string? Subject { get; set; }

    public string TemplateBody { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }
}
