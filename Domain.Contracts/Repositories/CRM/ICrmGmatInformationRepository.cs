using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmGmatInformationRepository : IRepositoryBase<CrmGmatInformation>
{

  /// <summary>
  /// Retrieves all CrmGmatInformation records asynchronously.
  /// </summary>
  Task<IEnumerable<CrmGmatInformation>> CrmGmatInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves a single CrmGmatInformation record by ID asynchronously.
  /// </summary>
  Task<CrmGmatInformation?> CrmGmatInformationAsync(int crmgmatinformationid, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmGmatInformation records by a collection of IDs asynchronously.
  /// </summary>
  Task<IEnumerable<CrmGmatInformation>> CrmGmatInformationsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmGmatInformation records by parent ID asynchronously.
  /// </summary>
  Task<IEnumerable<CrmGmatInformation>> CrmGmatInformationsByParentIdAsync(int parentId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Creates a new CrmGmatInformation record.
  /// </summary>
  Task<CrmGmatInformation> CreateCrmGmatInformationAsync(CrmGmatInformation entity, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an existing CrmGmatInformation record.
  /// </summary>
  void UpdateCrmGmatInformation(CrmGmatInformation entity);

  /// <summary>
  /// Deletes a CrmGmatInformation record.
  /// </summary>
  Task DeleteCrmGmatInformationAsync(CrmGmatInformation entity, bool trackChanges, CancellationToken cancellationToken = default);

  //Task<IEnumerable<CrmGmatInformation>> ActiveGmatinformationsAsync(bool track);
  //Task<IEnumerable<CrmGmatInformation>> GmatinformationsAsync(bool track);
  //Task<CrmGmatInformation?> GmatinformationAsync(int id, bool track);
  //Task<IEnumerable<CrmGmatInformation>> GmatinformationsByApplicantIdAsync(int applicantId, bool track);
  //Task<CrmGmatInformation?> GmatinformationByApplicantIdAsync(int applicantId, bool track);
}