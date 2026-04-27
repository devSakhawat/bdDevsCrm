namespace bdDevs.Shared.Records.CRM;

/// <summary>Record for creating a new CRM note.</summary>
public record CreateCrmNoteRecord(
    string EntityType,
    int EntityId,
    string NoteText,
    DateTime NoteDate,
    bool IsPrivate,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>Record for updating an existing CRM note.</summary>
public record UpdateCrmNoteRecord(
    int NoteId,
    string EntityType,
    int EntityId,
    string NoteText,
    DateTime NoteDate,
    bool IsPrivate,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>Record for deleting a CRM note.</summary>
/// <param name="NoteId">ID of the note to delete.</param>
public record DeleteCrmNoteRecord(int NoteId);
