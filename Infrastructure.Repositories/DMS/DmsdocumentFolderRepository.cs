using Domain.Entities.Entities.DMS;
using Domain.Contracts.DMS;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.DMS;

/// <summary>
/// Repository implementation for DmsDocumentFolder entity operations.
/// </summary>
public class DmsDocumentFolderRepository : RepositoryBase<DmsDocumentFolder>, IDmsDocumentFolderRepository
{
  public DmsDocumentFolderRepository(CRMContext context) : base(context) { }

  /// <summary>
  /// s all document folders ordered by FolderId.
  /// </summary>
  public async Task<IEnumerable<DmsDocumentFolder>> FoldersAsync(bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListAsync(x => x.FolderId, trackChanges, cancellationToken);
  }

  /// <summary>
  /// s folders by parent folder ID.
  /// </summary>
  public async Task<IEnumerable<DmsDocumentFolder>> FoldersByParentAsync(int? parentId, bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListByConditionAsync(x => x.ParentFolderId == parentId, x => x.FolderId, trackChanges, cancellationToken: cancellationToken);
  }

  /// <summary>
  /// s a single folder by ID.
  /// </summary>
  public async Task<DmsDocumentFolder?> FolderAsync(int folderId, bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await FirstOrDefaultAsync(x => x.FolderId == folderId, trackChanges, cancellationToken);
  }

  /// <summary>
  /// Creates a new folder.
  /// </summary>
  public async Task<DmsDocumentFolder> CreateFolderAsync(DmsDocumentFolder folder, CancellationToken cancellationToken = default)
  {
    var newId = await CreateAndIdAsync(folder, cancellationToken);
    folder.FolderId = newId;
    return folder;
  }

  /// <summary>
  /// Updates an existing folder.
  /// </summary>
  public void UpdateFolder(DmsDocumentFolder folder) => UpdateByState(folder);

  /// <summary>
  /// Deletes a folder.
  /// </summary>
  public async Task DeleteFolderAsync(DmsDocumentFolder folder, bool trackChanges, CancellationToken cancellationToken = default)
  {
    await DeleteAsync(x => x.FolderId == folder.FolderId, trackChanges, cancellationToken);
  }
}


//using Domain.Entities.Entities.DMS;
//using Domain.Contracts.DMS;
//using Infrastructure.Sql.Context;

//namespace Infrastructure.Repositories.DMS;

//public class DmsDocumentFolderRepository : RepositoryBase<DmsDocumentFolder>, IDmsDocumentFolderRepository
//{
//  public DmsDocumentFolderRepository(CRMContext context) : base(context) { }

//  //  folders by ParentFolderId
//  public async Task<IEnumerable<DmsDocumentFolder>> FoldersByParentIdAsync(int? parentId, bool trackChanges) =>
//      await ListByConditionAsync(x => x.ParentFolderId == parentId, x => x.FolderId, trackChanges);

//  // Create new folder
//  public void CreateFolder(DmsDocumentFolder folder) => Create(folder);

//  // Update folder
//  public void UpdateFolder(DmsDocumentFolder folder) => UpdateByState(folder);

//  // Delete folder
//  public void DeleteFolder(DmsDocumentFolder folder) => Delete(folder);
//}
