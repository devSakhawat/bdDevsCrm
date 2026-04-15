namespace Application.Services.Mappings;

/// <summary>
/// Mapster mapping configuration.
/// Register type adapter mappings here when property names differ between source and destination.
/// Mapster auto-maps matching property names, so only explicit mappings are needed for mismatches.
/// </summary>
public static class MappingConfig
{
  public static void RegisterMappings()
  {
    // No custom mappings needed currently.
    // The codebase uses MyMapper.JsonClone for DTO mapping.
    // Add Mapster TypeAdapterConfig mappings here when needed for performance.
  }
}