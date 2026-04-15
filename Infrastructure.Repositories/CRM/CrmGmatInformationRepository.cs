using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using bdDevs.Shared.DataTransferObjects.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmGmatInformation data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CrmGmatInformationRepository : RepositoryBase<CrmGmatInformation>, ICrmGmatInformationRepository
{
    public CrmGmatInformationRepository(CrmContext context) : base(context) { }

    /// <summary>
    /// Retrieves all CrmGmatInformation records asynchronously.
    /// </summary>
    public async Task<IEnumerable<CrmGmatInformation>> CrmGmatInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(x => x.GMATInformationId, trackChanges, cancellationToken);
    }

    /// <summary>
    /// Retrieves a single CrmGmatInformation record by ID asynchronously.
    /// </summary>
    public async Task<CrmGmatInformation?> CrmGmatInformationAsync(int crmgmatinformationid, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(
            x => x.GMATInformationId.Equals(crmgmatinformationid), 
            trackChanges, 
            cancellationToken);
    }

    /// <summary>
    /// Retrieves CrmGmatInformation records by a collection of IDs asynchronously.
    /// </summary>
    public async Task<IEnumerable<CrmGmatInformation>> CrmGmatInformationsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByIdsAsync(x => ids.Contains(x.GMATInformationId), trackChanges, cancellationToken);
    }

    /// <summary>
    /// Retrieves CrmGmatInformation records by parent ID asynchronously.
    /// </summary>
    public async Task<IEnumerable<CrmGmatInformation>> CrmGmatInformationsByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
    {
        string query = $"SELECT * FROM CrmGmatInformation WHERE ParentId = {parentId} ORDER BY GMATInformationId";
        return await AdoExecuteListQueryAsync<CrmGmatInformation>(query, null, cancellationToken);
    }

    /// <summary>
    /// Creates a new CrmGmatInformation record.
    /// </summary>
    public async Task<CrmGmatInformation> CreateCrmGmatInformationAsync(CrmGmatInformation entity ,CancellationToken cancellationToken = default)
{
    var newId = await CreateAndIdAsync(entity, cancellationToken);
    entity.GMATInformationId = newId;
    return entity;
}

    /// <summary>
    /// Updates an existing CrmGmatInformation record.
    /// </summary>
    public void UpdateCrmGmatInformation(CrmGmatInformation entity) => UpdateByState(entity);

    /// <summary>
    /// Deletes a CrmGmatInformation record.
    /// </summary>
    public async Task DeleteCrmGmatInformationAsync(CrmGmatInformation entity ,bool trackChanges ,CancellationToken cancellationToken = default) 
    => await DeleteAsync(x => x.GMATInformationId.Equals(entity.GMATInformationId) ,trackChanges, cancellationToken);
}
