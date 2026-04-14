using Domain.Entities.Entities.CRM;
using Domain.Contracts.Repositories;

namespace Domain.Contracts.CRM;

public interface ICrmApplicantInfoRepository : IRepositoryBase<CrmApplicantInfo>
{
  /// <summary>
  /// Retrieves all CrmApplicantInfo records asynchronously.
  /// </summary>
  Task<IEnumerable<CrmApplicantInfo>> CrmApplicantInfosAsync(bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves a single CrmApplicantInfo record by ID asynchronously.
  /// </summary>
  Task<CrmApplicantInfo?> CrmApplicantInfoAsync(int crmapplicantinfoid, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmApplicantInfo records by a collection of IDs asynchronously.
  /// </summary>
  Task<IEnumerable<CrmApplicantInfo>> CrmApplicantInfosByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmApplicantInfo records by parent ID asynchronously.
  /// </summary>
  Task<IEnumerable<CrmApplicantInfo>> CrmApplicantInfosByParentIdAsync(int parentId, CancellationToken cancellationToken = default);


  /// <summary>
  /// Creates a new CrmApplicantInfo record.
  /// </summary>
  Task<CrmApplicantInfo> CreateCrmApplicantInfoAsync(CrmApplicantInfo entity, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an existing CrmApplicantInfo record.
  /// </summary>
  void UpdateCrmApplicantInfo(CrmApplicantInfo entity);

  /// <summary>
  /// Deletes a CrmApplicantInfo record.
  /// </summary>
  Task DeleteCrmApplicantInfo(CrmApplicantInfo entity, bool trackChanges, CancellationToken cancellation = default);
}

//public interface ICrmApplicantInfoRepository : IRepositoryBase<CrmApplicantInfo>
//{
//  Task<IEnumerable<CrmApplicantInfo>> ActiveApplicantInfosAsync(bool track);
//  Task<IEnumerable<CrmApplicantInfo>> ApplicantInfosAsync(bool track);
//  Task<CrmApplicantInfo?> ApplicantInfoAsync(int id, bool track);
//  Task<CrmApplicantInfo?> ApplicantInfoByApplicationIdAsync(int applicationId, bool track);
//  Task<CrmApplicantInfo?> ApplicantInfoByEmailAsync(string email, bool track);
//}