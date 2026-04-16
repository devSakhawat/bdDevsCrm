namespace bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

public class DocumentQueryMappingDto
{
    public int DocumentQueryId { get; set; }
    public int? ReportHeaderId { get; set; }
    public int? DocumentTypeId { get; set; }
    public string? ParameterDefination { get; set; }
}

public class DocumentQueryMappingDDLDto
{
    public int DocumentQueryId { get; set; }
    public int? DocumentTypeId { get; set; }
}
