namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmCommunicationTemplateDto
{
    public int CommunicationTemplateId { get; init; }

    public int CommunicationTypeId { get; init; }

    public string? CommunicationTypeName { get; init; }

    public string TemplateName { get; init; } = string.Empty;

    public string? Subject { get; init; }

    public string TemplateBody { get; init; } = string.Empty;

    public bool IsActive { get; init; }

    public DateTime CreatedDate { get; init; }

    public int CreatedBy { get; init; }

    public DateTime? UpdatedDate { get; init; }

    public int? UpdatedBy { get; init; }
}

public record CrmCommunicationTemplateDDLDto
{
    public int CommunicationTemplateId { get; init; }

    public string TemplateName { get; init; } = string.Empty;
}
