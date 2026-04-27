using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>Repository for CrmFollowUp data access operations.</summary>
public class CrmFollowUpRepository : RepositoryBase<CrmFollowUp>, ICrmFollowUpRepository
{
    public CrmFollowUpRepository(CrmContext context) : base(context) { }

    /// <summary>Retrieves all CrmFollowUp records asynchronously.</summary>
    public async Task<IEnumerable<CrmFollowUp>> CrmFollowUpsAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.FollowUpId, trackChanges, cancellationToken);

    /// <summary>Retrieves a single CrmFollowUp record by ID asynchronously.</summary>
    public async Task<CrmFollowUp?> CrmFollowUpAsync(int followUpId, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.FollowUpId.Equals(followUpId), trackChanges, cancellationToken);

    /// <summary>Retrieves CrmFollowUp records by a collection of IDs asynchronously.</summary>
    public async Task<IEnumerable<CrmFollowUp>> CrmFollowUpsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByIdsAsync(x => ids.Contains(x.FollowUpId), trackChanges, cancellationToken);
}
