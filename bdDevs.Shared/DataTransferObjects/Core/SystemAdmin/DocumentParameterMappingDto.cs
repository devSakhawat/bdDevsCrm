namespace bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

public class DocumentParameterMappingDto
{
    public int MappingId { get; set; }
    public int? DocumentTypeId { get; set; }
    public int? ParameterId { get; set; }
    public bool? IsVisible { get; set; }
}

public class DocumentParameterMappingDDLDto
{
    public int MappingId { get; set; }
    public int? DocumentTypeId { get; set; }
    public int? ParameterId { get; set; }
}
