using bdDevs.Shared.DataTransferObjects.CRM;
using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

/// <summary>
/// Repository interface for CrmAdditionalDocument entity operations.
/// </summary>
public interface ICrmAdditionalDocumentRepository : IRepositoryBase<CrmAdditionalDocument>
{


  /// <summary>
  /// Retrieves all CrmAdditionalDocument records asynchronously.
  /// </summary>
  Task<IEnumerable<CrmAdditionalDocument>> CrmAdditionalDocumentsAsync(bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves a single CrmAdditionalDocument record by ID asynchronously.
  /// </summary>
  Task<CrmAdditionalDocument?> CrmAdditionalDocumentAsync(int crmadditionaldocumentid, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmAdditionalDocument records by a collection of IDs asynchronously.
  /// </summary>
  Task<IEnumerable<CrmAdditionalDocument>> CrmAdditionalDocumentsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);


  /// <summary>
  /// Retrieves CrmAdditionalDocument records by a applicantid asynchronously.
  /// </summary>
  Task<IEnumerable<CrmAdditionalDocument>> CrmAdditionalDocumentsByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmAdditionalDocument records by parent ID asynchronously.
  /// </summary>
  Task<IEnumerable<AdditionalDocumentDto>> CrmAdditionalDocumentsByParentIdAsync(int parentId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Creates a new CrmAdditionalDocument record.
  /// </summary>
  Task<CrmAdditionalDocument> CreateCrmAdditionalDocumentAsync(CrmAdditionalDocument entity, CancellationToken cancellation = default);

  /// <summary>
  /// Updates an existing CrmAdditionalDocument record.
  /// </summary>
  void UpdateCrmAdditionalDocument(CrmAdditionalDocument entity);

  /// <summary>
  /// Deletes a CrmAdditionalDocument record.
  /// </summary>
  Task DeleteCrmAdditionalDocumentAsync(CrmAdditionalDocument entity, bool trackChanges, CancellationToken cancellationToken = default);
}


//using Domain.Entities.Entities.CRM;
//using bdDevCRM.s.CRM;

//namespace bdDevCRM.RepositoriesContracts.CRM;

//public interface ICrmAdditionalDocumentRepository : IRepositoryBase<CrmAdditionalDocument>
//{
//  Task<IEnumerable<CrmAdditionalDocument>> ActiveAdditionalDocumentsAsync(bool track);
//  Task<IEnumerable<CrmAdditionalDocument>> AdditionalDocumentsAsync(bool track);
//  Task<CrmAdditionalDocument?> AdditionalDocumentAsync(int id, bool track);
//  Task<IEnumerable<CrmAdditionalDocument>> AdditionalDocumentsByApplicantIdAsync(int applicantId, bool track);

//  Task<IEnumerable<AdditionalDocument>> AdditionalDocumentsByApplicantId(int applicantId);
//}