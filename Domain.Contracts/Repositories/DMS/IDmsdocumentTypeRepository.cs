using Domain.Entities.Entities.DMS;
using Domain.Contracts.Repositories;

namespace Domain.Contracts.DMS;

/// <summary>
/// Repository interface for DmsDocumentType entity operations.
/// </summary>
public interface IDmsDocumentTypeRepository : IRepositoryBase<DmsDocumentType>
{
  /// <summary>
  /// s all document types ordered by DocumentTypeId.
  /// </summary>
  Task<IEnumerable<DmsDocumentType>> DocumentTypesAsync(bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// s a single document type by ID.
  /// </summary>
  Task<DmsDocumentType?> DocumentTypeAsync(int documentTypeId, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Creates a new document type.
  /// </summary>
  Task<DmsDocumentType> CreateDocumentTypeAsync(DmsDocumentType documentType, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an existing document type.
  /// </summary>
  void UpdateDocumentType(DmsDocumentType documentType);

  /// <summary>
  /// Deletes a document type.
  /// </summary>
  Task DeleteDocumentTypeAsync(DmsDocumentType documentType, bool trackChanges, CancellationToken cancellationToken = default);
}


//using Domain.Entities.Entities.DMS;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace bdDevCRM.RepositoriesContracts.DMS;

//public interface IDmsDocumentTypeRepository : IRepositoryBase<DmsDocumentType>
//{
//  Task<IEnumerable<DmsDocumentType>> AllDocumentTypesAsync(bool trackChanges);
//  void CreateDocumentType(DmsDocumentType type);
//  void UpdateDocumentType(DmsDocumentType type);
//  void DeleteDocumentType(DmsDocumentType type);
//}