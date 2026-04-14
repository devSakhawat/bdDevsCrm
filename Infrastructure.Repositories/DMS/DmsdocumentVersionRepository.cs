using bdDevCRM.Entities.Entities.DMS;
using bdDevCRM.RepositoriesContracts.DMS;
using bdDevCRM.Sql.Context;

namespace Infrastructure.Repositories.DMS;

/// <summary>
/// Repository implementation for DmsDocumentVersion entity operations.
/// </summary>
public class DmsDocumentVersionRepository : RepositoryBase<DmsDocumentVersion>, IDmsDocumentVersionRepository
{
  public DmsDocumentVersionRepository(CRMContext context) : base(context) { }

  /// <summary>
  /// s all document versions ordered by DocumentVersionId.
  /// </summary>
  public async Task<IEnumerable<DmsDocumentVersion>> DocumentVersionsAsync(bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListAsync(x => x.VersionId, trackChanges, cancellationToken);
  }

  /// <summary>
  /// s document versions by document ID.
  /// </summary>
  public async Task<IEnumerable<DmsDocumentVersion>> DocumentVersionsByDocumentAsync(int documentId, bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListByConditionAsync(x => x.DocumentId == documentId, x => x.VersionId, trackChanges, cancellationToken: cancellationToken);
  }

  /// <summary>
  /// s a single document version by ID.
  /// </summary>
  public async Task<DmsDocumentVersion?> DocumentVersionAsync(int documentVersionId, bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await FirstOrDefaultAsync(x => x.VersionId == documentVersionId, trackChanges, cancellationToken);
  }

  /// <summary>
  /// Creates a new document version.
  /// </summary>
  public async Task<DmsDocumentVersion> CreateDocumentVersionAsync(DmsDocumentVersion documentVersion, CancellationToken cancellationToken = default)
  {
    var newId = await CreateAndIdAsync(documentVersion, cancellationToken);
    documentVersion.VersionId = newId;
    return documentVersion;
  }

  /// <summary>
  /// Updates an existing document version.
  /// </summary>
  public void UpdateDocumentVersion(DmsDocumentVersion documentVersion) => UpdateByState(documentVersion);

  /// <summary>
  /// Deletes a document version.
  /// </summary>
  public async Task DeleteDocumentVersionAsync(DmsDocumentVersion documentVersion, bool trackChanges, CancellationToken cancellationToken = default)
  {
    await DeleteAsync(x => x.VersionId == documentVersion.VersionId, trackChanges, cancellationToken);
  }
}




//using bdDevCRM.Entities.Entities.DMS;
//using Domain.Contracts.Services.Core.SystemAdmin;
//using bdDevCRM.RepositoriesContracts.DMS;
//using bdDevCRM.Sql.Context;

//namespace bdDevCRM.Repositories.DMS;

//public class DmsDocumentVersionRepository : RepositoryBase<DmsDocumentVersion>, IDmsDocumentVersionRepository
//{
//  public DmsDocumentVersionRepository(CRMContext context) : base(context) { }

//  //  versions by DocumentId
//  public async Task<IEnumerable<DmsDocumentVersion>> VersionsByDocumentIdAsync(int documentId, bool trackChanges) =>
//      await ListByConditionAsync(x => x.DocumentId == documentId, x => x.VersionNumber, trackChanges);

//  // Create new version
//  public void CreateVersion(DmsDocumentVersion version) => Create(version);
//}
