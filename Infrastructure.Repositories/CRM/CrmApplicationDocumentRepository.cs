using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmApplicationDocumentRepository : RepositoryBase<CrmApplicationDocument>, ICrmApplicationDocumentRepository
{
    public CrmApplicationDocumentRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmApplicationDocument>> ApplicationDocumentsAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.ApplicationDocumentId, trackChanges, cancellationToken);

    public async Task<CrmApplicationDocument?> ApplicationDocumentAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.ApplicationDocumentId == id, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmApplicationDocument>> ApplicationDocumentsByApplicationIdAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByConditionAsync(x => x.ApplicationId == applicationId, x => x.ApplicationDocumentId, trackChanges, false, cancellationToken);
}
