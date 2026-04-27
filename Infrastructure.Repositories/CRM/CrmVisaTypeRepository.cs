using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmVisaType data access operations.
/// </summary>
public class CrmVisaTypeRepository : RepositoryBase<CrmVisaType>, ICrmVisaTypeRepository
{
    public CrmVisaTypeRepository(CrmContext context) : base(context) { }

    /// <summary>Retrieves all CrmVisaType records asynchronously.</summary>
    public async Task<IEnumerable<CrmVisaType>> CrmVisaTypesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(x => x.VisaTypeId, trackChanges, cancellationToken);
    }

    /// <summary>Retrieves a single CrmVisaType record by ID asynchronously.</summary>
    public async Task<CrmVisaType?> CrmVisaTypeAsync(int crmvisatypeid, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(
            x => x.VisaTypeId.Equals(crmvisatypeid),
            trackChanges,
            cancellationToken);
    }

    /// <summary>Retrieves CrmVisaType records by a collection of IDs asynchronously.</summary>
    public async Task<IEnumerable<CrmVisaType>> CrmVisaTypesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByIdsAsync(x => ids.Contains(x.VisaTypeId), trackChanges, cancellationToken);
    }
}
