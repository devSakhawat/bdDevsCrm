using Domain.Entities.Entities.DMS;
using Domain.Contracts.Repositories;

namespace Domain.Contracts.DMS;

/// <summary>
/// Repository interface for DmsDocumentFolder entity operations.
/// </summary>
public interface IDmsDocumentFolderRepository : IRepositoryBase<DmsDocumentFolder>
{
  /// <summary>
  /// s all document folders ordered by FolderId.
  /// </summary>
  Task<IEnumerable<DmsDocumentFolder>> FoldersAsync(bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// s folders by parent folder ID.
  /// </summary>
  Task<IEnumerable<DmsDocumentFolder>> FoldersByParentAsync(int? parentId, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// s a single folder by ID.
  /// </summary>
  Task<DmsDocumentFolder?> FolderAsync(int folderId, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Creates a new folder.
  /// </summary>
  Task<DmsDocumentFolder> CreateFolderAsync(DmsDocumentFolder folder, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an existing folder.
  /// </summary>
  void UpdateFolder(DmsDocumentFolder folder);

  /// <summary>
  /// Deletes a folder.
  /// </summary>
  Task DeleteFolderAsync(DmsDocumentFolder folder, bool trackChanges, CancellationToken cancellationToken = default);
}



//using Domain.Entities.Entities.DMS;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace bdDevCRM.RepositoriesContracts.DMS;

//public interface IDmsDocumentFolderRepository : IRepositoryBase<DmsDocumentFolder>
//{
//  Task<IEnumerable<DmsDocumentFolder>> FoldersByParentIdAsync(int? parentId, bool trackChanges);
//  void CreateFolder(DmsDocumentFolder folder);
//  void UpdateFolder(DmsDocumentFolder folder);
//  void DeleteFolder(DmsDocumentFolder folder);
//}