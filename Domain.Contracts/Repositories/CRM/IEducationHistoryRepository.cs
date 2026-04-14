using Domain.Entities.Entities.CRM;
using Domain.Contracts.Repositories;

namespace Domain.Contracts.CRM;

public interface ICrmEducationHistoryRepository : IRepositoryBase<CrmEducationHistory>
{


  /// <summary>
  /// Retrieves all CrmEducationHistory records by ApplicantId asynchronously.
  /// </summary>
  Task<IEnumerable<CrmEducationHistory>> CrmEducationHistorysByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves all CrmEducationHistory records asynchronously.
	/// </summary>
	Task<IEnumerable<CrmEducationHistory>> CrmEducationHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves a single CrmEducationHistory record by ID asynchronously.
  /// </summary>
  Task<CrmEducationHistory?> CrmEducationHistoryAsync(int crmeducationhistoryid, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmEducationHistory records by a collection of IDs asynchronously.
  /// </summary>
  Task<IEnumerable<CrmEducationHistory>> CrmEducationHistorysByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmEducationHistory records by parent ID asynchronously.
  /// </summary>
  Task<IEnumerable<CrmEducationHistory>> CrmEducationHistorysByParentIdAsync(int parentId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Creates a new CrmEducationHistory record.
  /// </summary>
  Task<CrmEducationHistory> CreateCrmEducationHistoryAsync(CrmEducationHistory entity, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an existing CrmEducationHistory record.
  /// </summary>
  void UpdateCrmEducationHistory(CrmEducationHistory entity);

  /// <summary>
  /// Deletes a CrmEducationHistory record.
  /// </summary>
  Task DeleteCrmEducationHistoryAsync(CrmEducationHistory entity, bool trackChanges, CancellationToken cancellationToken = default);

  //Task<IEnumerable<CrmEducationHistory>> ActiveEducationHistoriesAsync(bool track);
  //Task<IEnumerable<CrmEducationHistory>> EducationHistoriesAsync(bool track);
  //Task<CrmEducationHistory?> EducationHistoryAsync(int id, bool track);
  //Task<IEnumerable<CrmEducationHistory>> EducationHistoriesByApplicantIdAsync(int applicantId, bool track);
  //Task<CrmEducationHistory?> EducationHistoryByInstitutionAsync(string institution, bool track);
  //Task<IEnumerable<EducationHistory>> EducationHistoryByApplicantId(int applicantId);
}