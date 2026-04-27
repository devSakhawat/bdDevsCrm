using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmLeadSource data access operations.
/// </summary>
public class CrmLeadSourceRepository : RepositoryBase<CrmLeadSource>, ICrmLeadSourceRepository
{
    public CrmLeadSourceRepository(CrmContext context) : base(context) { }

    /// <summary>Retrieves all CrmLeadSource records asynchronously.</summary>
    public async Task<IEnumerable<CrmLeadSource>> CrmLeadSourcesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(x => x.LeadSourceId, trackChanges, cancellationToken);
    }

    /// <summary>Retrieves a single CrmLeadSource record by ID asynchronously.</summary>
    public async Task<CrmLeadSource?> CrmLeadSourceAsync(int crmleadsourceid, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(
            x => x.LeadSourceId.Equals(crmleadsourceid),
            trackChanges,
            cancellationToken);
    }

    /// <summary>Retrieves CrmLeadSource records by a collection of IDs asynchronously.</summary>
    public async Task<IEnumerable<CrmLeadSource>> CrmLeadSourcesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByIdsAsync(x => ids.Contains(x.LeadSourceId), trackChanges, cancellationToken);
    }
}
