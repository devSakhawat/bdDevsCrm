using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>Repository for CrmLead data access operations.</summary>
public class CrmLeadRepository : RepositoryBase<CrmLead>, ICrmLeadRepository
{
    public CrmLeadRepository(CrmContext context) : base(context) { }

    /// <summary>Retrieves all CrmLead records asynchronously.</summary>
    public async Task<IEnumerable<CrmLead>> CrmLeadsAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.LeadId, trackChanges, cancellationToken);

    /// <summary>Retrieves a single CrmLead record by ID asynchronously.</summary>
    public async Task<CrmLead?> CrmLeadAsync(int leadId, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.LeadId.Equals(leadId), trackChanges, cancellationToken);

    /// <summary>Retrieves CrmLead records by a collection of IDs asynchronously.</summary>
    public async Task<IEnumerable<CrmLead>> CrmLeadsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByIdsAsync(x => ids.Contains(x.LeadId), trackChanges, cancellationToken);
}
