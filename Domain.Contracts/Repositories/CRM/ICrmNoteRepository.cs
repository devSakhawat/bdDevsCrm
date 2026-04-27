using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmNoteRepository : IRepositoryBase<CrmNote>
{
    /// <summary>Retrieves all CrmNote records asynchronously.</summary>
    Task<IEnumerable<CrmNote>> CrmNotesAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a single CrmNote record by ID asynchronously.</summary>
    Task<CrmNote?> CrmNoteAsync(int noteId, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves CrmNote records by a collection of IDs asynchronously.</summary>
    Task<IEnumerable<CrmNote>> CrmNotesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}
