using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmIeltsInformationRepository : IRepositoryBase<CrmIeltsInformation>
{
  /// <summary>
  /// Retrieves all CrmIeltsInformation records asynchronously.
  /// </summary>
  Task<IEnumerable<CrmIeltsInformation>> CrmIeltsInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves a single CrmIeltsInformation record by ID asynchronously.
  /// </summary>
  Task<CrmIeltsInformation?> CrmIeltsInformationAsync(int crmIELTSInformationId, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmIeltsInformation records by a collection of IDs asynchronously.
  /// </summary>
  Task<IEnumerable<CrmIeltsInformation>> CrmIeltsInformationsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmIeltsInformation records by parent ID asynchronously.
  /// </summary>
  Task<IEnumerable<CrmIeltsInformation>> CrmIeltsInformationsByParentIdAsync(int parentId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Creates a new CrmIeltsInformation record.
  /// </summary>
  Task<CrmIeltsInformation> CreateCrmIELTSInformationAsync(CrmIeltsInformation entity, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an existing CrmIeltsInformation record.
  /// </summary>
  void UpdateCrmIELTSInformation(CrmIeltsInformation entity);

  /// <summary>
  /// Deletes a CrmIeltsInformation record.
  /// </summary>
  Task DeleteCrmIELTSInformationAsync(CrmIeltsInformation entity, bool trackChanges, CancellationToken cancellationToken = default);


  //Task<IEnumerable<CrmIeltsInformation>> ActiveIELTSinformationsAsync(bool track);
  //Task<IEnumerable<CrmIeltsInformation>> IELTSinformationsAsync(bool track);
  //Task<CrmIeltsInformation?> IELTSinformationAsync(int id, bool track);
  //Task<IEnumerable<CrmIeltsInformation>> IELTSinformationsByApplicantIdAsync(int applicantId, bool track);
  //Task<CrmIeltsInformation?> IELTSinformationByApplicantIdAsync(int applicantId, bool track);
}