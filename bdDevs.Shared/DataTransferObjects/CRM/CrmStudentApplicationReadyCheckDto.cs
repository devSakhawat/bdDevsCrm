namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmStudentApplicationReadyCheckDto
{
    public int StudentId { get; init; }
    public bool IsReady { get; init; }
    public IEnumerable<string> MissingRequirements { get; init; } = Enumerable.Empty<string>();
}
