using Mapster;

namespace bdDevs.Shared.Extensions;

/// <summary>
/// Extension methods for Mapster mapping configuration.
/// Provides reusable mapping utilities across the application.
/// </summary>
public static class MapsterExtensions
{
    /// <summary>
    /// Maps a source object to a destination type using Mapster.
    /// </summary>
    /// <typeparam name="TDestination">The destination type</typeparam>
    /// <param name="source">The source object</param>
    /// <returns>Mapped destination object</returns>
    public static TDestination MapTo<TDestination>(this object source)
    {
        return source.Adapt<TDestination>();
    }

    /// <summary>
    /// Maps a source object to an existing destination object using Mapster.
    /// </summary>
    /// <typeparam name="TSource">The source type</typeparam>
    /// <typeparam name="TDestination">The destination type</typeparam>
    /// <param name="source">The source object</param>
    /// <param name="destination">The destination object to map to</param>
    /// <returns>Updated destination object</returns>
    public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
    {
        return source.Adapt(destination);
    }

    /// <summary>
    /// Maps a collection of source objects to a list of destination type using Mapster.
    /// </summary>
    /// <typeparam name="TDestination">The destination type</typeparam>
    /// <param name="source">The source collection</param>
    /// <returns>List of mapped destination objects</returns>
    public static List<TDestination> MapToList<TDestination>(this IEnumerable<object> source)
    {
        return source.Adapt<List<TDestination>>();
    }

    /// <summary>
    /// Configures Mapster to ignore null values during mapping.
    /// </summary>
    public static TypeAdapterConfig ConfigureIgnoreNullValues(this TypeAdapterConfig config)
    {
        config.Default.IgnoreNullValues(true);
        return config;
    }

    /// <summary>
    /// Configures Mapster to map enums by name instead of value.
    /// </summary>
    public static TypeAdapterConfig ConfigureMapEnumsByName(this TypeAdapterConfig config)
    {
        config.Default.MapToConstructor(true);
        return config;
    }
}
