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
    // TODO: Add mapping configurations when projection types are created.
    // Example:
    // TypeAdapterConfig<MenuProjection, MenuDto>
    //     .NewConfig()
    //     .Map(dest => dest.MenuId, src => src.MenuId)
    //     .Map(dest => dest.MenuName, src => src.MenuName);
  }
}