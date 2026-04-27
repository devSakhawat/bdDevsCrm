namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmNoteDto
{
    public int NoteId { get; init; }
    public string EntityType { get; init; } = string.Empty;
    public int EntityId { get; init; }
    public string NoteText { get; init; } = string.Empty;
    public DateTime NoteDate { get; init; }
    public bool IsPrivate { get; init; }
    public DateTime CreatedDate { get; init; }
    public int CreatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public int? UpdatedBy { get; init; }
}
