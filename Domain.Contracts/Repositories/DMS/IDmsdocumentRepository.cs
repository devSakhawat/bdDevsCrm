using Domain.Entities.Entities.DMS;
using Domain.Contracts.Repositories;

namespace Domain.Contracts.DMS;

/// <summary>
/// Repository interface for DmsDocument entity operations.
/// </summary>
public interface IDmsDocumentRepository : IRepositoryBase<DmsDocument>
{
  /// <summary>
  /// s all documents ordered by DocumentId.
  /// </summary>
  Task<IEnumerable<DmsDocument>> DocumentsAsync(bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// s a single document by ID.
  /// </summary>
  Task<DmsDocument?> DocumentAsync(int documentId, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Creates a new document.
  /// </summary>
  Task<DmsDocument> CreateDocumentAsync(DmsDocument document, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an existing document.
  /// </summary>
  void UpdateDocument(DmsDocument document);

  /// <summary>
  /// Deletes a document.
  /// </summary>
  Task DeleteDocumentAsync(DmsDocument document, bool trackChanges, CancellationToken cancellationToken = default);
}