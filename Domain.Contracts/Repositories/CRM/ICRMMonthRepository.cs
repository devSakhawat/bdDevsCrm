using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmMonthRepository : IRepositoryBase<CrmMonth>
{
  /// <summary>
  /// Retrieves all CrmMonth records asynchronously.
  /// </summary>
  Task<IEnumerable<CrmMonth>> CrmMonthsAsync(bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves a single CrmMonth record by ID asynchronously.
  /// </summary>
  Task<CrmMonth?> CrmMonthAsync(int crmmonthid, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmMonth records by a collection of IDs asynchronously.
  /// </summary>
  Task<IEnumerable<CrmMonth>> CrmMonthsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmMonth records by parent ID asynchronously.
  /// </summary>
  Task<IEnumerable<CrmMonth>> CrmMonthsByParentIdAsync(int parentId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Creates a new CrmMonth record.
  /// </summary>
  Task<CrmMonth> CreateCrmMonthAsync(CrmMonth entity, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an existing CrmMonth record.
  /// </summary>
  void UpdateCrmMonth(CrmMonth entity);

  /// <summary>
  /// Deletes a CrmMonth record.
  /// </summary>
  Task DeleteCrmMonthAsync(CrmMonth entity, bool trackChanges, CancellationToken cancellationToken = default);

  //Task<IEnumerable<CrmMonth>> MonthsAsync(bool trackChanges, CancellationToken cancellationToken = default);
  //Task<IEnumerable<CrmMonth>> ActiveMonthsAsync(bool trackChanges, CancellationToken cancellationToken = default);
  //Task<CrmMonth?> MonthAsync(int monthId, bool trackChanges, CancellationToken cancellationToken = default);
  //void CreateMonth(CrmMonth month);
  //void UpdateMonth(CrmMonth month);
  //void DeleteMonth(CrmMonth month);
}



//using Domain.Entities.Entities.CRM;

//namespace bdDevCRM.RepositoriesContracts.Core.SystemAdmin;

//public interface ICrmMonthRepository : IRepositoryBase<CrmMonth>
//{
//  Task<IEnumerable<CrmMonth>> ActiveMonthAsync(bool trackChanges);
//}
