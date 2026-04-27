using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmDocumentVerificationHistoryRepository : RepositoryBase<CrmDocumentVerificationHistory>, ICrmDocumentVerificationHistoryRepository
{
    public CrmDocumentVerificationHistoryRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmDocumentVerificationHistory>> DocumentVerificationHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.DocumentVerificationHistoryId, trackChanges, cancellationToken);

    public async Task<CrmDocumentVerificationHistory?> DocumentVerificationHistoryAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.DocumentVerificationHistoryId == id, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmDocumentVerificationHistory>> DocumentVerificationHistoriesByDocumentIdAsync(int documentId, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByConditionAsync(x => x.DocumentId == documentId, x => x.DocumentVerificationHistoryId, trackChanges, false, cancellationToken);
}
