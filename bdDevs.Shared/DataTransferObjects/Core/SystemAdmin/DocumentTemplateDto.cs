namespace bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

public class DocumentTemplateDto
{
    public int DocumentId { get; set; }
    public string DocumentTitle { get; set; } = null!;
    public string? DocumentText { get; set; }
    public string TemplateName { get; set; } = null!;
    public int? DocumentTypeId { get; set; }
}

public class DocumentTemplateDDLDto
{
    public int DocumentId { get; set; }
    public string TemplateName { get; set; } = null!;
    public string DocumentTitle { get; set; } = null!;
}
