using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

/// <summary>Service contract for CRM note management operations.</summary>
public interface ICrmNoteService
{
    /// <summary>Creates a new note record.</summary>
    Task<CrmNoteDto> CreateAsync(CreateCrmNoteRecord record, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing note record.</summary>
    Task<CrmNoteDto> UpdateAsync(UpdateCrmNoteRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Deletes a note record.</summary>
    Task DeleteAsync(DeleteCrmNoteRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a single note record by ID.</summary>
    Task<CrmNoteDto> NoteAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves all note records.</summary>
    Task<IEnumerable<CrmNoteDto>> NotesAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a lightweight list for dropdown binding.</summary>
    Task<IEnumerable<CrmNoteDto>> NoteForDDLAsync(CancellationToken cancellationToken = default);

    /// <summary>Retrieves a paginated summary grid of notes.</summary>
    Task<GridEntity<CrmNoteDto>> NotesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
