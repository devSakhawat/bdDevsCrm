using bdDevCRM.Entities.Entities.DMS;
using bdDevCRM.RepositoriesContracts.DMS;
using bdDevCRM.Sql.Context;

namespace Infrastructure.Repositories.DMS;

/// <summary>
/// Repository implementation for DmsDocumentTag entity operations.
/// </summary>
public class DmsDocumentTagRepository : RepositoryBase<DmsDocumentTag>, IDmsDocumentTagRepository
{
  public DmsDocumentTagRepository(CRMContext context) : base(context) { }

  /// <summary>
  /// s all document tags ordered by TagId.
  /// </summary>
  public async Task<IEnumerable<DmsDocumentTag>> TagsAsync(bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListAsync(x => x.TagId, trackChanges, cancellationToken);
  }

  /// <summary>
  /// s a single tag by ID.
  /// </summary>
  public async Task<DmsDocumentTag?> TagAsync(int tagId, bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await FirstOrDefaultAsync(x => x.TagId == tagId, trackChanges, cancellationToken);
  }

  /// <summary>
  /// Creates a new tag.
  /// </summary>
  public async Task<DmsDocumentTag> CreateTagAsync(DmsDocumentTag tag, CancellationToken cancellationToken = default)
  {
    var newId = await CreateAndIdAsync(tag, cancellationToken);
    tag.TagId = newId;
    return tag;
  }

  /// <summary>
  /// Updates an existing tag.
  /// </summary>
  public void UpdateTag(DmsDocumentTag tag) => UpdateByState(tag);

  /// <summary>
  /// Deletes a tag.
  /// </summary>
  public async Task DeleteTagAsync(DmsDocumentTag tag, bool trackChanges, CancellationToken cancellationToken = default)
  {
    await DeleteAsync(x => x.TagId == tag.TagId, trackChanges, cancellationToken);
  }
}






//using bdDevCRM.Entities.Entities.DMS;
//using bdDevCRM.RepositoriesContracts.DMS;
//using bdDevCRM.Sql.Context;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace bdDevCRM.Repositories.DMS;

//// DmsDocumentTag Repository Implementation
//public class DmsDocumentTagRepository : RepositoryBase<DmsDocumentTag>, IDmsDocumentTagRepository
//{
//  public DmsDocumentTagRepository(CRMContext context) : base(context) { }

//  //  all tags
//  public async Task<IEnumerable<DmsDocumentTag>> AllTagsAsync(bool trackChanges) =>
//      await ListAsync(x => x.TagId, trackChanges);

//  // Create new tag
//  public void CreateTag(DmsDocumentTag tag) => Create(tag);

//  // Delete tag
//  public void DeleteTag(DmsDocumentTag tag) => Delete(tag);
//}