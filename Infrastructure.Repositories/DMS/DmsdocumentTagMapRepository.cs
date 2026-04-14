using bdDevCRM.Entities.Entities.DMS;
using bdDevCRM.RepositoriesContracts.DMS;
using bdDevCRM.Sql.Context;

namespace Infrastructure.Repositories.DMS;

/// <summary>
/// Repository implementation for DmsDocumentTagMap entity operations.
/// </summary>
public class DmsDocumentTagMapRepository : RepositoryBase<DmsDocumentTagMap>, IDmsDocumentTagMapRepository
{
  public DmsDocumentTagMapRepository(CRMContext context) : base(context) { }

  /// <summary>
  /// s all document tag mappings ordered by DocumentTagMapId.
  /// </summary>
  public async Task<IEnumerable<DmsDocumentTagMap>> TagMapsAsync(bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListAsync(x => x.TagMapId, trackChanges, cancellationToken);
  }

  /// <summary>
  /// s tag mappings by document ID.
  /// </summary>
  public async Task<IEnumerable<DmsDocumentTagMap>> TagMapsByDocumentAsync(int documentId, bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListByConditionAsync(x => x.DocumentId == documentId, x => x.TagMapId, trackChanges, cancellationToken: cancellationToken);
  }

  /// <summary>
  /// s tag mappings by tag ID.
  /// </summary>
  public async Task<IEnumerable<DmsDocumentTagMap>> TagMapsByTagAsync(int tagId, bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListByConditionAsync(x => x.TagId == tagId, x => x.TagMapId, trackChanges, cancellationToken:  cancellationToken);
  }

  /// <summary>
  /// s a single tag mapping by ID.
  /// </summary>
  public async Task<DmsDocumentTagMap?> TagMapAsync(int documentTagMapId, bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await FirstOrDefaultAsync(x => x.TagMapId == documentTagMapId, trackChanges, cancellationToken);
  }

  /// <summary>
  /// Creates a new tag mapping.
  /// </summary>
  public async Task<DmsDocumentTagMap> CreateTagMapAsync(DmsDocumentTagMap tagMap, CancellationToken cancellationToken = default)
  {
    var newId = await CreateAndIdAsync(tagMap, cancellationToken);
    tagMap.TagMapId = newId;
    return tagMap;
  }

  /// <summary>
  /// Updates an existing tag mapping.
  /// </summary>
  public void UpdateTagMap(DmsDocumentTagMap tagMap) => UpdateByState(tagMap);

  /// <summary>
  /// Deletes a tag mapping.
  /// </summary>
  public async Task DeleteTagMapAsync(DmsDocumentTagMap tagMap, bool trackChanges, CancellationToken cancellationToken = default)
  {
    await DeleteAsync(x => x.TagMapId == tagMap.TagMapId, trackChanges, cancellationToken);
  }
}





//using bdDevCRM.Entities.Entities.DMS;
//using bdDevCRM.RepositoriesContracts.DMS;
//using bdDevCRM.Sql.Context;

//namespace bdDevCRM.Repositories.DMS;


//public class DmsDocumentTagMapRepository : RepositoryBase<DmsDocumentTagMap>, IDmsDocumentTagMapRepository
//{
//  public DmsDocumentTagMapRepository(CRMContext context) : base(context) { }


//}
