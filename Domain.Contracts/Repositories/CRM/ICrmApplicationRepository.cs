using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

/// <summary>
/// Repository interface for CrmApplication entity operations.
/// </summary>
public interface ICrmApplicationRepository : IRepositoryBase<CrmApplication>
{
  /// <summary>
  /// Retrieves all CrmApplication records asynchronously.
  /// </summary>
  Task<IEnumerable<CrmApplication>> CrmApplicationsAsync(bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves a single CrmApplication record by ID asynchronously.
  /// </summary>
  Task<CrmApplication?> CrmApplicationAsync(int applicationid, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmApplication records by a collection of IDs asynchronously.
  /// </summary>
  Task<IEnumerable<CrmApplication>> CrmApplicationsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmApplication records by parent ID asynchronously.
  /// </summary>
  Task<IEnumerable<CrmApplication>> CrmApplicationsByParentIdAsync(int parentId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Creates a new CrmApplication record.
	/// </summary>
	Task<CrmApplication> CreateCrmApplicationAsync(CrmApplication entity, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an existing CrmApplication record.
  /// </summary>
  void UpdateCrmApplication(CrmApplication entity);

	/// <summary>
	/// Deletes a CrmApplication record.
	/// </summary>
	Task DeleteCrmApplicationAsync(CrmApplication entity, bool trackChanges, CancellationToken cancellationToken = default);

  ///// <summary>
  ///// s all CRM applications ordered by ApplicationId.
  ///// </summary>
  //Task<IEnumerable<CrmApplicantInfo>> ApplicationsAsync(bool trackChanges, CancellationToken cancellationToken = default);

  ///// <summary>
  ///// s a single CRM application by ID.
  ///// </summary>
  //Task<CrmApplicantInfo?> ApplicationAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default);

  ///// <summary>
  ///// Creates a new CRM application.
  ///// </summary>
  //void CreateApplication(CrmApplicantInfo application);

  ///// <summary>
  ///// Updates an existing CRM application.
  ///// </summary>
  //void UpdateApplication(CrmApplicantInfo application);

  ///// <summary>
  ///// Deletes a CRM application.
  ///// </summary>
  //void DeleteApplication(CrmApplicantInfo application);
}


