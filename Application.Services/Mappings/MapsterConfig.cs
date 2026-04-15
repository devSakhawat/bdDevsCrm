using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services.Mappings;

/// <summary>
/// Mapster mapping configuration.
/// Register type adapter mappings here when property names differ between source and destination.
/// Mapster auto-maps matching property names, so only explicit mappings are needed for mismatches.
/// </summary>
public static class MappingConfig
{
    /// <summary>
    /// Registers Mapster mappings and configuration.
    /// Call this method in Program.cs during service registration.
    /// </summary>
    /// <param name="services">The service collection</param>
    public static IServiceCollection AddMapsterConfiguration(this IServiceCollection services)
    {
        // Get global TypeAdapterConfig
        var config = TypeAdapterConfig.GlobalSettings;

        // Configure global settings
        config.Default.IgnoreNullValues(true);
        config.Default.PreserveReference(true);

        // Scan assembly for IRegister implementations
        config.Scan(typeof(MappingConfig).Assembly);

        // Register IMapper service
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }

    /// <summary>
    /// Registers custom mappings that don't follow convention.
    /// Add explicit type mappings here when needed.
    /// </summary>
    public static void RegisterCustomMappings()
    {
        // Example: Custom mapping when property names differ
        // TypeAdapterConfig<SourceType, DestinationType>
        //     .NewConfig()
        //     .Map(dest => dest.PropertyName, src => src.DifferentPropertyName);

        // No custom mappings needed currently.
        // The codebase uses Mapster's convention-based mapping.
    }
}