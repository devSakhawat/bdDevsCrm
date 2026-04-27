using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmLeadStatus data access operations.
/// </summary>
public class CrmLeadStatusRepository : RepositoryBase<CrmLeadStatus>, ICrmLeadStatusRepository
{
    public CrmLeadStatusRepository(CrmContext context) : base(context) { }

    /// <summary>Retrieves all CrmLeadStatus records asynchronously.</summary>
    public async Task<IEnumerable<CrmLeadStatus>> CrmLeadStatusesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(x => x.LeadStatusId, trackChanges, cancellationToken);
    }

    /// <summary>Retrieves a single CrmLeadStatus record by ID asynchronously.</summary>
    public async Task<CrmLeadStatus?> CrmLeadStatusAsync(int crmleadstatusid, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(
            x => x.LeadStatusId.Equals(crmleadstatusid),
            trackChanges,
            cancellationToken);
    }

    /// <summary>Retrieves CrmLeadStatus records by a collection of IDs asynchronously.</summary>
    public async Task<IEnumerable<CrmLeadStatus>> CrmLeadStatusesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByIdsAsync(x => ids.Contains(x.LeadStatusId), trackChanges, cancellationToken);
    }
}
