using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmIntakeMonthRepository : IRepositoryBase<CrmIntakeMonth>
{
  /// <summary>
  /// Retrieves all CrmIntakeMonth records asynchronously.
  /// </summary>
  Task<IEnumerable<CrmIntakeMonth>> CrmIntakeMonthsAsync(bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves a single CrmIntakeMonth record by ID asynchronously.
  /// </summary>
  Task<CrmIntakeMonth?> CrmIntakeMonthAsync(int crmintakemonthid, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmIntakeMonth records by a collection of IDs asynchronously.
  /// </summary>
  Task<IEnumerable<CrmIntakeMonth>> CrmIntakeMonthsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmIntakeMonth records by parent ID asynchronously.
  /// </summary>
  Task<IEnumerable<CrmIntakeMonth>> CrmIntakeMonthsByParentIdAsync(int parentId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Creates a new CrmIntakeMonth record.
	/// </summary>
	Task<CrmIntakeMonth> CreateCrmIntakeMonthAsync(CrmIntakeMonth entity, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an existing CrmIntakeMonth record.
  /// </summary>
  void UpdateCrmIntakeMonth(CrmIntakeMonth entity);

  /// <summary>
  /// Deletes a CrmIntakeMonth record.
  /// </summary>
  Task DeleteCrmIntakeMonthAsync(CrmIntakeMonth entity, bool trackChanges, CancellationToken cancellationToken = default);

  //Task<IEnumerable<CrmIntakeMonth>> IntakeMonthsAsync(bool trackChanges, CancellationToken cancellationToken = default);
  //Task<IEnumerable<CrmIntakeMonth>> ActiveIntakeMonthsAsync(bool trackChanges, CancellationToken cancellationToken = default);
  //Task<CrmIntakeMonth?> IntakeMonthAsync(int intakeMonthId, bool trackChanges, CancellationToken cancellationToken = default);
  //void CreateIntakeMonth(CrmIntakeMonth intakeMonth);
  //void UpdateIntakeMonth(CrmIntakeMonth intakeMonth);
  //void DeleteIntakeMonth(CrmIntakeMonth intakeMonth);
}





//using Domain.Entities.Entities.CRM;

//namespace bdDevCRM.RepositoriesContracts.Core.SystemAdmin;

//public interface ICrmIntakeMonthRepository : IRepositoryBase<CrmIntakeMonth>
//{
//  Task<IEnumerable<CrmIntakeMonth>> ActiveIntakeMonthsAsync(bool trackChanges);
//  Task<CrmIntakeMonth?> IntakeMonthByIdAsync(int intakeMonthId, bool trackChanges);
//  void CreateIntakeMonth(CrmIntakeMonth intakeMonth);
//  void UpdateIntakeMonth(CrmIntakeMonth intakeMonth);
//  void DeleteIntakeMonth(CrmIntakeMonth intakeMonth);
//}