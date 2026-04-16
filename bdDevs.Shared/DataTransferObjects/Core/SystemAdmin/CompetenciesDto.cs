namespace bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

public class CompetenciesDto
{
    public int Id { get; set; }
    public string? CompetencyName { get; set; }
    public int? CompetencyType { get; set; }
    public int? IsDepartmentHead { get; set; }
    public int? IsActive { get; set; }
}

public class CompetenciesDDLDto
{
    public int Id { get; set; }
    public string? CompetencyName { get; set; }
}
