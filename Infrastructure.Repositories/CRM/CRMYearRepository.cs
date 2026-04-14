using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using bdDevs.Shared.DataTransferObjects.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmYear data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CrmYearRepository : RepositoryBase<CrmYear>, ICRMYearRepository
{
    public CrmYearRepository(CRMContext context) : base(context) { }

    /// <summary>
    /// Retrieves all CrmYear records asynchronously.
    /// </summary>
    public async Task<IEnumerable<CrmYear>> CrmYearsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(x => x.YearId, trackChanges, cancellationToken);
    }

    /// <summary>
    /// Retrieves a single CrmYear record by ID asynchronously.
    /// </summary>
    public async Task<CrmYear?> CrmYearAsync(int crmyearid, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(
            x => x.YearId.Equals(crmyearid), 
            trackChanges, 
            cancellationToken);
    }

    /// <summary>
    /// Retrieves CrmYear records by a collection of IDs asynchronously.
    /// </summary>
    public async Task<IEnumerable<CrmYear>> CrmYearsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByIdsAsync(x => ids.Contains(x.YearId), trackChanges, cancellationToken);
    }

    /// <summary>
    /// Retrieves CrmYear records by parent ID asynchronously.
    /// </summary>
    public async Task<IEnumerable<CrmYear>> CrmYearsByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
    {
        string query = $"SELECT * FROM CrmYear WHERE ParentId = {parentId} ORDER BY CrmYear";
        return await AdoExecuteListQueryAsync<CrmYear>(query, null, cancellationToken);
    }

    /// <summary>
    /// Creates a new CrmYear record.
    /// </summary>
    public async Task<CrmYear> CreateCrmYearAsync(CrmYear entity, CancellationToken cancellationToken = default) 
    {
        var newId = await CreateAndIdAsync(entity, cancellationToken);
        entity.YearId = newId;
        return entity;
    }

    /// <summary>
    /// Updates an existing CrmYear record.
    /// </summary>
    public void UpdateCrmYear(CrmYear entity) => UpdateByState(entity);

    /// <summary>
    /// Deletes a CrmYear record.
    /// </summary>
    public async Task DeleteCrmYearAsync(CrmYear entity, bool trackChanges, CancellationToken cancellationToken = default)
    {
        await DeleteAsync(x => x.YearId == entity.YearId, trackChanges, cancellationToken);
    }
}
