using bdDevCRM.Entities.Entities.CRM;
using bdDevCRM.RepositoriesContracts.CRM;
using bdDevCRM.s.CRM;
using bdDevCRM.Sql.Context;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmMonth data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CrmMonthRepository : RepositoryBase<CrmMonth>, ICrmMonthRepository
{
    public CrmMonthRepository(CRMContext context) : base(context) { }

    /// <summary>
    /// Retrieves all CrmMonth records asynchronously.
    /// </summary>
    public async Task<IEnumerable<CrmMonth>> CrmMonthsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(x => x.MonthId, trackChanges, cancellationToken);
    }

    /// <summary>
    /// Retrieves a single CrmMonth record by ID asynchronously.
    /// </summary>
    public async Task<CrmMonth?> CrmMonthAsync(int crmmonthid, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(
            x => x.MonthId.Equals(crmmonthid), 
            trackChanges, 
            cancellationToken);
    }

    /// <summary>
    /// Retrieves CrmMonth records by a collection of IDs asynchronously.
    /// </summary>
    public async Task<IEnumerable<CrmMonth>> CrmMonthsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByIdsAsync(x => ids.Contains(x.MonthId), trackChanges, cancellationToken);
    }

    /// <summary>
    /// Retrieves CrmMonth records by parent ID asynchronously.
    /// </summary>
    public async Task<IEnumerable<CrmMonth>> CrmMonthsByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
    {
        string query = $"SELECT * FROM CrmMonth WHERE ParentId = {parentId} ORDER BY CrmMonthId";
        return await AdoExecuteListQueryAsync<CrmMonth>(query, null, cancellationToken);
    }

  /// <summary>
  /// Creates a new CrmMonth record.
  /// </summary>
  public async Task<CrmMonth> CreateCrmMonthAsync(CrmMonth entity, CancellationToken cancellationToken = default)
  { 
    var newId = await CreateAndIdAsync(entity, cancellationToken);
    entity.MonthId = newId;
    return entity;
  }

    /// <summary>
    /// Updates an existing CrmMonth record.
    /// </summary>
    public void UpdateCrmMonth(CrmMonth entity) => UpdateByState(entity);

    /// <summary>
    /// Deletes a CrmMonth record.
    /// </summary>
    public async Task DeleteCrmMonthAsync(CrmMonth entity ,bool trackChanges ,CancellationToken cancellationToken = default) 
    => await DeleteAsync(x => x.MonthId.Equals(entity.MonthId) ,trackChanges ,cancellationToken);
}
