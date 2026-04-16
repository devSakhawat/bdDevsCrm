namespace bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

public class BoardInstituteDto
{
    public int Id { get; set; }
    public string? InstituteName { get; set; }
    public int? IsActive { get; set; }
}

public class BoardInstituteDDLDto
{
    public int Id { get; set; }
    public string? InstituteName { get; set; }
}
