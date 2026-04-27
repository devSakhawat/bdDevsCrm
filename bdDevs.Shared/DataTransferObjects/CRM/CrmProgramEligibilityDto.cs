namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmProgramEligibilityDto
{
    public int ProgramId { get; init; }
    public string ProgramName { get; init; } = string.Empty;
    public int InstituteId { get; init; }
    public string InstituteName { get; init; } = string.Empty;
    public bool IsEligible { get; init; }
    public string MatchedCriteria { get; init; } = string.Empty;
    public string MissingCriteria { get; init; } = string.Empty;
}
