using Domain.Entities.Entities.DMS;
using Domain.Contracts.Repositories;

namespace Domain.Contracts.DMS;

/// <summary>
/// Repository interface for DmsDocumentVersion entity operations.
/// </summary>
public interface IDmsDocumentVersionRepository : IRepositoryBase<DmsDocumentVersion>
{
  /// <summary>
  /// s all document versions ordered by DocumentVersionId.
  /// </summary>
  Task<IEnumerable<DmsDocumentVersion>> DocumentVersionsAsync(bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// s document versions by document ID.
  /// </summary>
  Task<IEnumerable<DmsDocumentVersion>> DocumentVersionsByDocumentAsync(int documentId, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// s a single document version by ID.
  /// </summary>
  Task<DmsDocumentVersion?> DocumentVersionAsync(int documentVersionId, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Creates a new document version.
  /// </summary>
  Task<DmsDocumentVersion> CreateDocumentVersionAsync(DmsDocumentVersion documentVersion, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an existing document version.
  /// </summary>
  void UpdateDocumentVersion(DmsDocumentVersion documentVersion);

  /// <summary>
  /// Deletes a document version.
  /// </summary>
  Task DeleteDocumentVersionAsync(DmsDocumentVersion documentVersion, bool trackChanges, CancellationToken cancellationToken = default);
}






//using Domain.Entities.Entities.DMS;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Domain.Contracts.DMS;

//public interface IDmsDocumentVersionRepository : IRepositoryBase<DmsDocumentVersion>
//{
//  Task<IEnumerable<DmsDocumentVersion>> VersionsByDocumentIdAsync(int documentId, bool trackChanges);
//  void CreateVersion(DmsDocumentVersion version);
//}
