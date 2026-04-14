using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmIELTSInformationRepository : IRepositoryBase<CrmIELTSInformation>
{
  /// <summary>
  /// Retrieves all CrmIELTSInformation records asynchronously.
  /// </summary>
  Task<IEnumerable<CrmIELTSInformation>> CrmIELTSInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves a single CrmIELTSInformation record by ID asynchronously.
  /// </summary>
  Task<CrmIELTSInformation?> CrmIELTSInformationAsync(int crmIELTSInformationId, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmIELTSInformation records by a collection of IDs asynchronously.
  /// </summary>
  Task<IEnumerable<CrmIELTSInformation>> CrmIELTSInformationsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmIELTSInformation records by parent ID asynchronously.
  /// </summary>
  Task<IEnumerable<CrmIELTSInformation>> CrmIELTSInformationsByParentIdAsync(int parentId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Creates a new CrmIELTSInformation record.
  /// </summary>
  Task<CrmIELTSInformation> CreateCrmIELTSInformationAsync(CrmIELTSInformation entity, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an existing CrmIELTSInformation record.
  /// </summary>
  void UpdateCrmIELTSInformation(CrmIELTSInformation entity);

  /// <summary>
  /// Deletes a CrmIELTSInformation record.
  /// </summary>
  Task DeleteCrmIELTSInformationAsync(CrmIELTSInformation entity, bool trackChanges, CancellationToken cancellationToken = default);


  //Task<IEnumerable<CrmIELTSInformation>> ActiveIELTSinformationsAsync(bool track);
  //Task<IEnumerable<CrmIELTSInformation>> IELTSinformationsAsync(bool track);
  //Task<CrmIELTSInformation?> IELTSinformationAsync(int id, bool track);
  //Task<IEnumerable<CrmIELTSInformation>> IELTSinformationsByApplicantIdAsync(int applicantId, bool track);
  //Task<CrmIELTSInformation?> IELTSinformationByApplicantIdAsync(int applicantId, bool track);
}