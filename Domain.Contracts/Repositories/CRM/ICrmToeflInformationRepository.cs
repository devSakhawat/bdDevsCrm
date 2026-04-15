using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmToeflInformationRepository : IRepositoryBase<CrmToeflInformation>
{
  /// <summary>
  /// Retrieves all CrmToeflInformation records asynchronously.
  /// </summary>
  Task<IEnumerable<CrmToeflInformation>> CrmToeflInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves a single CrmToeflInformation record by ID asynchronously.
  /// </summary>
  Task<CrmToeflInformation?> CrmToeflInformationAsync(int crmtoeflinformationid, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmToeflInformation records by a collection of IDs asynchronously.
  /// </summary>
  Task<IEnumerable<CrmToeflInformation>> CrmToeflInformationsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmToeflInformation records by parent ID asynchronously.
  /// </summary>
  Task<IEnumerable<CrmToeflInformation>> CrmToeflInformationsByParentIdAsync(int parentId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Creates a new CrmToeflInformation record.
  /// </summary>
  Task<CrmToeflInformation> CreateCrmTOEFLInformationAsync(CrmToeflInformation entity, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an existing CrmToeflInformation record.
  /// </summary>
  void UpdateCrmTOEFLInformation(CrmToeflInformation entity);

  /// <summary>
  /// Deletes a CrmToeflInformation record.
  /// </summary>
  Task DeleteCrmTOEFLInformationAsync(CrmToeflInformation entity, bool trackChanges, CancellationToken cancellationToken = default);

  //Task<IEnumerable<CrmToeflInformation>> ActiveToeflinformationsAsync(bool track);
  //Task<IEnumerable<CrmToeflInformation>> ToeflinformationsAsync(bool track);
  //Task<CrmToeflInformation?> ToeflinformationAsync(int id, bool track);
  //Task<IEnumerable<CrmToeflInformation>> ToeflinformationsByApplicantIdAsync(int applicantId, bool track);
  //Task<CrmToeflInformation?> ToeflinformationByApplicantIdAsync(int applicantId, bool track);
}