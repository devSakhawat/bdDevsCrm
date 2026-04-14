using Domain.Entities.Entities.DMS;

namespace Domain.Contracts.DMS;
using Domain.Contracts.Repositories;

/// <summary>
/// Repository interface for DmsDocumentTagMap entity operations.
/// </summary>
public interface IDmsDocumentTagMapRepository : IRepositoryBase<DmsDocumentTagMap>
{
  /// <summary>
  /// s all document tag mappings ordered by DocumentTagMapId.
  /// </summary>
  Task<IEnumerable<DmsDocumentTagMap>> TagMapsAsync(bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// s tag mappings by document ID.
  /// </summary>
  Task<IEnumerable<DmsDocumentTagMap>> TagMapsByDocumentAsync(int documentId, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// s tag mappings by tag ID.
  /// </summary>
  Task<IEnumerable<DmsDocumentTagMap>> TagMapsByTagAsync(int tagId, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// s a single tag mapping by ID.
  /// </summary>
  Task<DmsDocumentTagMap?> TagMapAsync(int documentTagMapId, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Creates a new tag mapping.
  /// </summary>
  Task<DmsDocumentTagMap> CreateTagMapAsync(DmsDocumentTagMap tagMap, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an existing tag mapping.
  /// </summary>
  void UpdateTagMap(DmsDocumentTagMap tagMap);

  /// <summary>
  /// Deletes a tag mapping.
  /// </summary>
  Task DeleteTagMapAsync(DmsDocumentTagMap tagMap, bool trackChanges, CancellationToken cancellationToken = default);
}





//using Domain.Entities.Entities.DMS;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Domain.Contracts.DMS;

//public interface IDmsDocumentTagMapRepository : IRepositoryBase<DmsDocumentTagMap>
//{
//  //Task<IEnumerable<DmsDocumentTagMap>> DocumentTagsByDocumentIdAsync(int documentId, bool trackChanges = false);
//  //Task<IEnumerable<DmsDocumentTagMap>> TagsByDocumentIdsAsync(IEnumerable<int> documentIds, bool trackChanges = false);
//  //Task<bool> AddTagToDocumentAsync(int documentId, int tagId);
//  //Task<bool> RemoveTagFromDocumentAsync(int documentId, int tagId);
//}
