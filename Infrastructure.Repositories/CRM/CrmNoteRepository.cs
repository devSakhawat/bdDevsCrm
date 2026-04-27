using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>Repository for CrmNote data access operations.</summary>
public class CrmNoteRepository : RepositoryBase<CrmNote>, ICrmNoteRepository
{
    public CrmNoteRepository(CrmContext context) : base(context) { }

    /// <summary>Retrieves all CrmNote records asynchronously.</summary>
    public async Task<IEnumerable<CrmNote>> CrmNotesAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.NoteId, trackChanges, cancellationToken);

    /// <summary>Retrieves a single CrmNote record by ID asynchronously.</summary>
    public async Task<CrmNote?> CrmNoteAsync(int noteId, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.NoteId.Equals(noteId), trackChanges, cancellationToken);

    /// <summary>Retrieves CrmNote records by a collection of IDs asynchronously.</summary>
    public async Task<IEnumerable<CrmNote>> CrmNotesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByIdsAsync(x => ids.Contains(x.NoteId), trackChanges, cancellationToken);
}
