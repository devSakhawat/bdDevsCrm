using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using bdDevs.Shared.DataTransferObjects.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmGMATInformation data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CrmGMATInformationRepository : RepositoryBase<CrmGMATInformation>, ICrmGMATInformationRepository
{
    public CrmGMATInformationRepository(CRMContext context) : base(context) { }

    /// <summary>
    /// Retrieves all CrmGMATInformation records asynchronously.
    /// </summary>
    public async Task<IEnumerable<CrmGMATInformation>> CrmGmatInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(x => x.GMATInformationId, trackChanges, cancellationToken);
    }

    /// <summary>
    /// Retrieves a single CrmGMATInformation record by ID asynchronously.
    /// </summary>
    public async Task<CrmGMATInformation?> CrmGmatInformationAsync(int crmgmatinformationid, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(
            x => x.GMATInformationId.Equals(crmgmatinformationid), 
            trackChanges, 
            cancellationToken);
    }

    /// <summary>
    /// Retrieves CrmGMATInformation records by a collection of IDs asynchronously.
    /// </summary>
    public async Task<IEnumerable<CrmGMATInformation>> CrmGmatInformationsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByIdsAsync(x => ids.Contains(x.GMATInformationId), trackChanges, cancellationToken);
    }

    /// <summary>
    /// Retrieves CrmGMATInformation records by parent ID asynchronously.
    /// </summary>
    public async Task<IEnumerable<CrmGMATInformation>> CrmGmatInformationsByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
    {
        string query = $"SELECT * FROM CrmGMATInformation WHERE ParentId = {parentId} ORDER BY GMATInformationId";
        return await AdoExecuteListQueryAsync<CrmGMATInformation>(query, null, cancellationToken);
    }

    /// <summary>
    /// Creates a new CrmGMATInformation record.
    /// </summary>
    public async Task<CrmGMATInformation> CreateCrmGmatInformationAsync(CrmGMATInformation entity ,CancellationToken cancellationToken = default)
{
    var newId = await CreateAndIdAsync(entity, cancellationToken);
    entity.GMATInformationId = newId;
    return entity;
}

    /// <summary>
    /// Updates an existing CrmGMATInformation record.
    /// </summary>
    public void UpdateCrmGmatInformation(CrmGMATInformation entity) => UpdateByState(entity);

    /// <summary>
    /// Deletes a CrmGMATInformation record.
    /// </summary>
    public async Task DeleteCrmGmatInformationAsync(CrmGMATInformation entity ,bool trackChanges ,CancellationToken cancellationToken = default) 
    => await DeleteAsync(x => x.GMATInformationId.Equals(entity.GMATInformationId) ,trackChanges, cancellationToken);
}
