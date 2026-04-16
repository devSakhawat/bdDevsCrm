namespace bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

public class CompetencyLevelDto
{
    public int LevelId { get; set; }
    public string? LevelTitle { get; set; }
    public string? Remarks { get; set; }
}

public class CompetencyLevelDDLDto
{
    public int LevelId { get; set; }
    public string? LevelTitle { get; set; }
}
