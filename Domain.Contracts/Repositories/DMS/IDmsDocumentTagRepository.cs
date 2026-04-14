using Domain.Entities.Entities.DMS;
using Domain.Contracts.Repositories;

namespace Domain.Contracts.DMS;

/// <summary>
/// Repository interface for DmsDocumentTag entity operations.
/// </summary>
public interface IDmsDocumentTagRepository : IRepositoryBase<DmsDocumentTag>
{
  /// <summary>
  /// s all document tags ordered by TagId.
  /// </summary>
  Task<IEnumerable<DmsDocumentTag>> TagsAsync(bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// s a single tag by ID.
  /// </summary>
  Task<DmsDocumentTag?> TagAsync(int tagId, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Creates a new tag.
  /// </summary>
  Task<DmsDocumentTag> CreateTagAsync(DmsDocumentTag tag, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an existing tag.
  /// </summary>
  void UpdateTag(DmsDocumentTag tag);

  /// <summary>
  /// Deletes a tag.
  /// </summary>
  Task DeleteTagAsync(DmsDocumentTag tag, bool trackChanges, CancellationToken cancellationToken = default);
}









//using Domain.Entities.Entities.DMS;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Domain.Contracts.DMS;

//public interface IDmsDocumentTagRepository : IRepositoryBase<DmsDocumentTag>
//{
//  Task<IEnumerable<DmsDocumentTag>> AllTagsAsync(bool trackChanges);
//  void CreateTag(DmsDocumentTag tag);
//  void DeleteTag(DmsDocumentTag tag);
//}
