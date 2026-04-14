using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.Mappings;


public static class MappingConfig
{
  public static void RegisterMappings()
  {
    // Mapping: MenuProjection → MenuDto
    TypeAdapterConfig<MenuProjection, MenuDto>
        .NewConfig()
        .Map(dest => dest.MenuId, src => src.MenuId)
        .Map(dest => dest.MenuName, src => src.MenuName);

    // Only map properties explicitly when the names are different
    // If property names are the same, Mapster will map them automatically

    // Add other mappings here
    // Example:
    // EmployeeProjection → EmployeeDto
    // CrmApplicationProjection → CrmApplicationDto
  }
}