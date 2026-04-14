using bdDevCRM.Entities.Entities.CRM;
using bdDevCRM.RepositoriesContracts.CRM;
using bdDevCRM.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmIELTSInformation data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CrmIELTSInformationRepository : RepositoryBase<CrmIELTSInformation>, ICrmIELTSInformationRepository
{
  public CrmIELTSInformationRepository(CRMContext context) : base(context) { }

  /// <summary>
  /// Retrieves all CrmIELTSInformation records asynchronously.
  /// </summary>
  public async Task<IEnumerable<CrmIELTSInformation>> CrmIELTSInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListAsync(x => x.IELTSInformationId, trackChanges, cancellationToken);
  }

  /// <summary>
  /// Retrieves a single CrmIELTSInformation record by ID asynchronously.
  /// </summary>
  public async Task<CrmIELTSInformation?> CrmIELTSInformationAsync(int crmIELTSInformationId, bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await FirstOrDefaultAsync(
        x => x.IELTSInformationId.Equals(crmIELTSInformationId),
        trackChanges,
        cancellationToken);
  }

  /// <summary>
  /// Retrieves CrmIELTSInformation records by a collection of IDs asynchronously.
  /// </summary>
  public async Task<IEnumerable<CrmIELTSInformation>> CrmIELTSInformationsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListByIdsAsync(x => ids.Contains(x.IELTSInformationId), trackChanges, cancellationToken);
  }

  /// <summary>
  /// Retrieves CrmIELTSInformation records by parent ID asynchronously.
  /// </summary>
  public async Task<IEnumerable<CrmIELTSInformation>> CrmIELTSInformationsByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
  {
    string query = $"SELECT * FROM CrmIELTSInformation WHERE ParentId = {parentId} ORDER BY IELTSInformationId";
    return await AdoExecuteListQueryAsync<CrmIELTSInformation>(query, null, cancellationToken);
  }

  /// <summary>
  /// Creates a new CrmIELTSInformation record.
  /// </summary>
  public async Task<CrmIELTSInformation> CreateCrmIELTSInformationAsync(CrmIELTSInformation entity, CancellationToken cancellationToken = default)
  { 
    var newId = await CreateAndIdAsync(entity, cancellationToken);
    entity.IELTSInformationId = newId;
    return entity;
}

  /// <summary>
  /// Updates an existing CrmIELTSInformation record.
  /// </summary>
  public void UpdateCrmIELTSInformation(CrmIELTSInformation entity) => UpdateByState(entity);

  /// <summary>
  /// Deletes a CrmIELTSInformation record.
  /// </summary>
  public async Task DeleteCrmIELTSInformationAsync(CrmIELTSInformation entity, bool trackChanges, CancellationToken cancellationToken = default) 
    => await DeleteAsync(x => x.IELTSInformationId.Equals(entity.IELTSInformationId) ,trackChanges, cancellationToken);
}
