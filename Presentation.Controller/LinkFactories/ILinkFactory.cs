using bdDevs.Shared;

namespace Presentation.LinkFactories;

/// <summary>
/// Contract for generating HATEOAS links per entity.
/// Row-level links: self, edit, delete (per record)
/// Resource-level links: self, create (for the collection)
/// </summary>
public interface ILinkFactory<T>
{
  /// <summary>
  /// Generates action links for a single row/record
  /// Called per-item when building grid response
  /// </summary>
  List<ResourceLink> GenerateRowLinks(int key);

  /// <summary>
  /// Generates navigation links for the resource collection
  /// Goes into ApiResponse.Links (top-level)
  /// </summary>
  List<ResourceLink> GenerateResourceLinks();
}