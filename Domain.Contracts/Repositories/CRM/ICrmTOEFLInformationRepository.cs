using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmTOEFLInformationRepository : IRepositoryBase<CrmTOEFLInformation>
{
  /// <summary>
  /// Retrieves all CrmTOEFLInformation records asynchronously.
  /// </summary>
  Task<IEnumerable<CrmTOEFLInformation>> CrmTOEFLInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves a single CrmTOEFLInformation record by ID asynchronously.
  /// </summary>
  Task<CrmTOEFLInformation?> CrmTOEFLInformationAsync(int crmtoeflinformationid, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmTOEFLInformation records by a collection of IDs asynchronously.
  /// </summary>
  Task<IEnumerable<CrmTOEFLInformation>> CrmTOEFLInformationsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmTOEFLInformation records by parent ID asynchronously.
  /// </summary>
  Task<IEnumerable<CrmTOEFLInformation>> CrmTOEFLInformationsByParentIdAsync(int parentId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Creates a new CrmTOEFLInformation record.
  /// </summary>
  Task<CrmTOEFLInformation> CreateCrmTOEFLInformationAsync(CrmTOEFLInformation entity, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an existing CrmTOEFLInformation record.
  /// </summary>
  void UpdateCrmTOEFLInformation(CrmTOEFLInformation entity);

  /// <summary>
  /// Deletes a CrmTOEFLInformation record.
  /// </summary>
  Task DeleteCrmTOEFLInformationAsync(CrmTOEFLInformation entity, bool trackChanges, CancellationToken cancellationToken = default);

  //Task<IEnumerable<CrmTOEFLInformation>> ActiveToeflinformationsAsync(bool track);
  //Task<IEnumerable<CrmTOEFLInformation>> ToeflinformationsAsync(bool track);
  //Task<CrmTOEFLInformation?> ToeflinformationAsync(int id, bool track);
  //Task<IEnumerable<CrmTOEFLInformation>> ToeflinformationsByApplicantIdAsync(int applicantId, bool track);
  //Task<CrmTOEFLInformation?> ToeflinformationByApplicantIdAsync(int applicantId, bool track);
}