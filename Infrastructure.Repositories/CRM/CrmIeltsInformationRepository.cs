using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmIeltsInformation data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CrmIeltsInformationRepository : RepositoryBase<CrmIeltsInformation>, ICrmIeltsInformationRepository
{
  public CrmIeltsInformationRepository(CrmContext context) : base(context) { }

  /// <summary>
  /// Retrieves all CrmIeltsInformation records asynchronously.
  /// </summary>
  public async Task<IEnumerable<CrmIeltsInformation>> CrmIeltsInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListAsync(x => x.IELTSInformationId, trackChanges, cancellationToken);
  }

  /// <summary>
  /// Retrieves a single CrmIeltsInformation record by ID asynchronously.
  /// </summary>
  public async Task<CrmIeltsInformation?> CrmIeltsInformationAsync(int crmIELTSInformationId, bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await FirstOrDefaultAsync(
        x => x.IELTSInformationId.Equals(crmIELTSInformationId),
        trackChanges,
        cancellationToken);
  }

  /// <summary>
  /// Retrieves CrmIeltsInformation records by a collection of IDs asynchronously.
  /// </summary>
  public async Task<IEnumerable<CrmIeltsInformation>> CrmIeltsInformationsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListByIdsAsync(x => ids.Contains(x.IELTSInformationId), trackChanges, cancellationToken);
  }

  /// <summary>
  /// Retrieves CrmIeltsInformation records by parent ID asynchronously.
  /// </summary>
  public async Task<IEnumerable<CrmIeltsInformation>> CrmIeltsInformationsByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
  {
    string query = $"SELECT * FROM CrmIeltsInformation WHERE ParentId = {parentId} ORDER BY IELTSInformationId";
    return await AdoExecuteListQueryAsync<CrmIeltsInformation>(query, null, cancellationToken);
  }

  /// <summary>
  /// Creates a new CrmIeltsInformation record.
  /// </summary>
  public async Task<CrmIeltsInformation> CreateCrmIELTSInformationAsync(CrmIeltsInformation entity, CancellationToken cancellationToken = default)
  { 
    var newId = await CreateAndIdAsync(entity, cancellationToken);
    entity.IELTSInformationId = newId;
    return entity;
}

  /// <summary>
  /// Updates an existing CrmIeltsInformation record.
  /// </summary>
  public void UpdateCrmIELTSInformation(CrmIeltsInformation entity) => UpdateByState(entity);

  /// <summary>
  /// Deletes a CrmIeltsInformation record.
  /// </summary>
  public async Task DeleteCrmIELTSInformationAsync(CrmIeltsInformation entity, bool trackChanges, CancellationToken cancellationToken = default) 
    => await DeleteAsync(x => x.IELTSInformationId.Equals(entity.IELTSInformationId) ,trackChanges, cancellationToken);
}
