using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmAdditionalInfoRepository : IRepositoryBase<CrmAdditionalInfo>
{

  /// <summary>
  /// Retrieves all CrmAdditionalInfo records asynchronously.
  /// </summary>
  Task<IEnumerable<CrmAdditionalInfo>> CrmAdditionalInfosAsync(bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves a single CrmAdditionalInfo record by ID asynchronously.
  /// </summary>
  Task<CrmAdditionalInfo?> CrmAdditionalInfoAsync(int crmadditionalinfoid, bool trackChanges, CancellationToken cancellationToken = default);
  /// <summary>
  /// Retrieves CrmAdditionalInfo records by a collection of IDs asynchronously.
  /// </summary>
  Task<IEnumerable<CrmAdditionalInfo>> CrmAdditionalInfosByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmAdditionalInfo records by applicantId asynchronously.
  /// </summary>
  Task<IEnumerable<CrmAdditionalInfo>> CrmAdditionalInfosByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves CrmAdditionalInfo records by parent ID asynchronously.
	/// </summary>
	Task<IEnumerable<CrmAdditionalInfo>> CrmAdditionalInfosByParentIdAsync(int parentId, CancellationToken cancellationToken = default);
	/// <summary>
	/// Creates a new CrmAdditionalInfo record.
	/// </summary>
	Task<CrmAdditionalInfo> CreateCrmAdditionalInfo(CrmAdditionalInfo entity, CancellationToken cancellation = default);

  /// <summary>
  /// Updates an existing CrmAdditionalInfo record.
  /// </summary>
  void UpdateCrmAdditionalInfo(CrmAdditionalInfo entity);

  /// <summary>
  /// Deletes a CrmAdditionalInfo record.
  /// </summary>
  Task DeleteCrmAdditionalInfo(CrmAdditionalInfo entity, bool trackChanges, CancellationToken cancellationToken = default);
}

//public interface ICrmAdditionalInfoRepository : IRepositoryBase<CrmAdditionalInfo>
//{
//  Task<IEnumerable<CrmAdditionalInfo>> ActiveAdditionalInfosAsync(bool track);
//  Task<IEnumerable<CrmAdditionalInfo>> AdditionalInfosAsync(bool track);
//  Task<CrmAdditionalInfo?> AdditionalInfoAsync(int id, bool track);
//  Task<IEnumerable<CrmAdditionalInfo>> AdditionalInfosByApplicantIdAsync(int applicantId, bool track);
//}