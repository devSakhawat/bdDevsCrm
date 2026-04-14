namespace bdDevs.Shared;

/// <summary>
/// HATEOAS wrapper for any DTO.
/// Keeps DTO pure — links are separate concern.
/// Usage: LinkedResource<MenuDto>, LinkedResource<ModuleDto>, etc.
/// </summary>
public class LinkedResource<T>
{
  /// <summary>
  /// The actual DTO data — pure, unchanged
  /// </summary>
  public T Data { get; set; }

  /// <summary>
  /// Row-level HATEOAS action links
  /// e.g. self, edit, delete
  /// </summary>
  public List<ResourceLink> Links { get; set; } = new();

  public LinkedResource(T data, List<ResourceLink> links)
  {
    Data = data;
    Links = links;
  }
}