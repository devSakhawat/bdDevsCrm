using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmOffice data access operations.
/// </summary>
public class CrmOfficeRepository : RepositoryBase<CrmOffice>, ICrmOfficeRepository
{
    public CrmOfficeRepository(CrmContext context) : base(context) { }

    /// <summary>Retrieves all CrmOffice records asynchronously.</summary>
    public async Task<IEnumerable<CrmOffice>> CrmOfficesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(x => x.OfficeId, trackChanges, cancellationToken);
    }

    /// <summary>Retrieves a single CrmOffice record by ID asynchronously.</summary>
    public async Task<CrmOffice?> CrmOfficeAsync(int crmofficeid, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(
            x => x.OfficeId.Equals(crmofficeid),
            trackChanges,
            cancellationToken);
    }

    /// <summary>Retrieves CrmOffice records by a collection of IDs asynchronously.</summary>
    public async Task<IEnumerable<CrmOffice>> CrmOfficesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByIdsAsync(x => ids.Contains(x.OfficeId), trackChanges, cancellationToken);
    }
}
