using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmDocumentVerificationHistoryRepository : IRepositoryBase<CrmDocumentVerificationHistory>
{
    Task<IEnumerable<CrmDocumentVerificationHistory>> DocumentVerificationHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmDocumentVerificationHistory?> DocumentVerificationHistoryAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmDocumentVerificationHistory>> DocumentVerificationHistoriesByDocumentIdAsync(int documentId, bool trackChanges, CancellationToken cancellationToken = default);
}
