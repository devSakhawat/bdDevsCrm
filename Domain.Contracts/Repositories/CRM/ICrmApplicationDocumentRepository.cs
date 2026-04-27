using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmApplicationDocumentRepository : IRepositoryBase<CrmApplicationDocument>
{
    Task<IEnumerable<CrmApplicationDocument>> ApplicationDocumentsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmApplicationDocument?> ApplicationDocumentAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmApplicationDocument>> ApplicationDocumentsByApplicationIdAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default);
}
