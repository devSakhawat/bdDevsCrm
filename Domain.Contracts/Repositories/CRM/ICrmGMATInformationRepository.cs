using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmGMATInformationRepository : IRepositoryBase<CrmGMATInformation>
{

  /// <summary>
  /// Retrieves all CrmGMATInformation records asynchronously.
  /// </summary>
  Task<IEnumerable<CrmGMATInformation>> CrmGmatInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves a single CrmGMATInformation record by ID asynchronously.
  /// </summary>
  Task<CrmGMATInformation?> CrmGmatInformationAsync(int crmgmatinformationid, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmGMATInformation records by a collection of IDs asynchronously.
  /// </summary>
  Task<IEnumerable<CrmGMATInformation>> CrmGmatInformationsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmGMATInformation records by parent ID asynchronously.
  /// </summary>
  Task<IEnumerable<CrmGMATInformation>> CrmGmatInformationsByParentIdAsync(int parentId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Creates a new CrmGMATInformation record.
  /// </summary>
  Task<CrmGMATInformation> CreateCrmGmatInformationAsync(CrmGMATInformation entity, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an existing CrmGMATInformation record.
  /// </summary>
  void UpdateCrmGmatInformation(CrmGMATInformation entity);

  /// <summary>
  /// Deletes a CrmGMATInformation record.
  /// </summary>
  Task DeleteCrmGmatInformationAsync(CrmGMATInformation entity, bool trackChanges, CancellationToken cancellationToken = default);

  //Task<IEnumerable<CrmGMATInformation>> ActiveGmatinformationsAsync(bool track);
  //Task<IEnumerable<CrmGMATInformation>> GmatinformationsAsync(bool track);
  //Task<CrmGMATInformation?> GmatinformationAsync(int id, bool track);
  //Task<IEnumerable<CrmGMATInformation>> GmatinformationsByApplicantIdAsync(int applicantId, bool track);
  //Task<CrmGMATInformation?> GmatinformationByApplicantIdAsync(int applicantId, bool track);
}