using Newtonsoft.Json;
using System.Reflection;

namespace bdDevCRM.Utilities.OthersLibrary;

/// <summary>
/// Provides object mapping, cloning, and partial update utilities using reflection and JSON serialization.
/// </summary>
public class MyMapper
{
  /// <summary>
  /// Maps matching properties from a source object to a new instance of type <typeparamref name="T"/>.
  /// Only properties with the same name are copied.
  /// </summary>
  /// <typeparam name="T">The destination type to create and populate.</typeparam>
  /// <param name="source">The source object whose property values will be copied.</param>
  /// <returns>A new instance of <typeparamref name="T"/> with matching properties populated.</returns>
  public static T Map<T>(object source)
  {
    var sourceType = source.GetType();
    var destinationType = typeof(T);
    var destinationProperties = destinationType.GetProperties();
    var sourceProperties = sourceType.GetProperties();
    var destination = Activator.CreateInstance<T>();
    foreach (var destinationProperty in destinationProperties)
    {
      var sourceProperty = sourceProperties.FirstOrDefault(x => x.Name == destinationProperty.Name);
      if (sourceProperty != null)
      {
        destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
      }
    }
    return destination;
  }

  /// <summary>
  /// Maps a source object to a destination type using JSON serialization and deserialization.
  /// Useful when the source and destination types share the same property names and structure.
  /// </summary>
  /// <typeparam name="TSource">The type of the source object.</typeparam>
  /// <typeparam name="TDestination">The type of the destination object.</typeparam>
  /// <param name="source">The source object to serialize and map.</param>
  /// <returns>A new instance of <typeparamref name="TDestination"/> populated from the serialized source.</returns>
  public static TDestination ModelMapping<TSource, TDestination>(TSource source)
  {
    string text = JsonConvert.SerializeObject((object)source);
    return JsonConvert.DeserializeObject<TDestination>(text);
  }


  /// <summary>
  /// Creates a deep clone of a source object into a different target type using JSON serialization.
  /// Throws if the source is null.
  /// </summary>
  /// <typeparam name="TSource">The type of the source object.</typeparam>
  /// <typeparam name="TTarget">The type of the target object. Must have a parameterless constructor.</typeparam>
  /// <param name="source">The source object to clone.</param>
  /// <returns>A new instance of <typeparamref name="TTarget"/> with values copied from the source.</returns>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is null.</exception>
  public static TTarget JsonClone<TSource, TTarget>(TSource source) where TTarget : new()
  {
    if (source == null) throw new ArgumentNullException(nameof(source));
    var serialized = JsonConvert.SerializeObject(source);
    var target = JsonConvert.DeserializeObject<TTarget>(serialized);
    return target;
  }

  /// <summary>
  /// Creates a deep clone of a source object into a target type using JSON serialization with safe settings.
  /// Ignores null values, missing members, and handles <see cref="DateTime"/> fields gracefully.
  /// </summary>
  /// <typeparam name="TSource">The type of the source object.</typeparam>
  /// <typeparam name="TTarget">The type of the target object. Must have a parameterless constructor.</typeparam>
  /// <param name="source">The source object to clone.</param>
  /// <returns>A new instance of <typeparamref name="TTarget"/> with values safely copied from the source.</returns>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is null.</exception>
  public static TTarget JsonCloneSafe<TSource, TTarget>(TSource source) where TTarget : new()
  {
    if (source == null) throw new ArgumentNullException(nameof(source));

    var settings = new JsonSerializerSettings
    {
      NullValueHandling = NullValueHandling.Ignore,
      MissingMemberHandling = MissingMemberHandling.Ignore,
      DateTimeZoneHandling = DateTimeZoneHandling.Utc,
      Error = (sender, args) =>
      {
        if (args.ErrorContext.Error is InvalidCastException &&
            args.ErrorContext.Path.Contains("Date"))
        {
          args.ErrorContext.Handled = true;
        }
      }
    };

    var serialized = JsonConvert.SerializeObject(source, settings);
    var target = JsonConvert.DeserializeObject<TTarget>(serialized, settings) ?? new TTarget();

    ProcessDateTimeFields(target);

    return target;
  }

  /// <summary>
  /// Merges only the changed property values from a DTO object into an existing entity object
  /// and returns the updated entity.
  /// A property is considered changed when the DTO value differs from the current entity value.
  /// Only properties with matching names and compatible types between both types are evaluated.
  /// </summary>
  /// <typeparam name="TEntity">The type of the destination entity (e.g., database model).</typeparam>
  /// <typeparam name="TDto">The type of the source DTO (e.g., update request model).</typeparam>
  /// <param name="entity">The existing entity object to be updated.</param>
  /// <param name="dto">The DTO object carrying the new values from the user.</param>
  /// <returns>The same <typeparamref name="TEntity"/> instance with changed properties updated.</returns>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> or <paramref name="dto"/> is null.</exception>
  /// <example>
  /// <code>
  /// Menu menuEntity = MyMapper.MergeChangedValues&lt;Menu, MenuDto&gt;(existingMenu, modelDto);
  /// await _repository.UpdateAsync(menuEntity ,cancellationToken);
  /// </code>
  /// </example>
  public static TEntity MergeChangedValues<TEntity, TDto>(TEntity entity, TDto dto)
      where TEntity : class
      where TDto : class
  {
    if (entity == null) throw new ArgumentNullException(nameof(entity));
    if (dto == null) throw new ArgumentNullException(nameof(dto));

    var entityProperties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);
    var dtoProperties = typeof(TDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);

    foreach (var dtoProperty in dtoProperties)
    {
      // Find matching property in entity by name
      var entityProperty = entityProperties
          .FirstOrDefault(p => p.Name == dtoProperty.Name && p.CanWrite);

      if (entityProperty == null) continue;

      // Skip if types are not compatible
      if (!entityProperty.PropertyType.IsAssignableFrom(dtoProperty.PropertyType)) continue;

      var dtoValue = dtoProperty.GetValue(dto);
      var entityValue = entityProperty.GetValue(entity);

      // Only update if value has actually changed
      bool hasChanged = !Equals(dtoValue, entityValue);

      if (hasChanged)
      {
        entityProperty.SetValue(entity, dtoValue);
      }
    }
    return entity;
  }


  /// <summary>
  /// Checks all <see cref="DateTime"/> and nullable <see cref="DateTime"/> properties on the target object.
  /// If a <see cref="DateTime"/> property holds <see cref="DateTime.MinValue"/>, it is reset to null
  /// (only applicable for nullable DateTime properties).
  /// </summary>
  /// <typeparam name="T">The type of the object to inspect.</typeparam>
  /// <param name="obj">The object whose DateTime properties will be processed.</param>
  private static void ProcessDateTimeFields<T>(T obj)
  {
    if (obj == null) return;

    var type = typeof(T);
    var properties = type.GetProperties();

    foreach (var property in properties)
    {
      if (property.PropertyType == typeof(DateTime) ||
          property.PropertyType == typeof(DateTime?))
      {
        try
        {
          var value = property.GetValue(obj);

          if (value is DateTime dateTimeValue && dateTimeValue == DateTime.MinValue)
          {
            if (property.PropertyType == typeof(DateTime?))
            {
              property.SetValue(obj, null);
            }
          }
        }
        catch (Exception)
        {
          continue;
        }
      }
    }
  }


  /// <summary>
  /// Converts an <see cref="IEnumerable{TSource}"/> to a <see cref="List{TTarget}"/> using JSON serialization.
  /// Useful for bulk mapping between two collection types with matching property names.
  /// </summary>
  /// <typeparam name="TSource">The type of the source collection elements.</typeparam>
  /// <typeparam name="TTarget">The type of the target list elements.</typeparam>
  /// <param name="sourceList">The source collection to convert.</param>
  /// <returns>A <see cref="List{TTarget}"/> with elements mapped from the source collection.</returns>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="sourceList"/> is null.</exception>
  public static List<TTarget> JsonCloneIEnumerableToList<TSource, TTarget>(IEnumerable<TSource> sourceList)
  {
    if (sourceList == null) throw new ArgumentNullException(nameof(sourceList));

    var serialized = JsonConvert.SerializeObject(sourceList);
    var targetList = JsonConvert.DeserializeObject<List<TTarget>>(serialized);

    return targetList;
  }

  /// <summary>
  /// Converts a <see cref="List{TSource}"/> to an <see cref="IEnumerable{TTarget}"/> using JSON serialization.
  /// </summary>
  /// <typeparam name="TSource">The type of the source list elements.</typeparam>
  /// <typeparam name="TTarget">The type of the target enumerable elements.</typeparam>
  /// <param name="sourceList">The source list to convert.</param>
  /// <returns>An <see cref="IEnumerable{TTarget}"/> mapped from the source list.</returns>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="sourceList"/> is null.</exception>
  public static IEnumerable<TTarget> JsonCloneIEnumerableToIEnumerable<TSource, TTarget>(List<TSource> sourceList)
  {
    if (sourceList == null) throw new ArgumentNullException(nameof(sourceList));

    var serialized = JsonConvert.SerializeObject(sourceList);
    var targetList = JsonConvert.DeserializeObject<IEnumerable<TTarget>>(serialized);

    return targetList;
  }

  /// <summary>
  /// Converts a <see cref="List{TSource}"/> to a <see cref="List{TTarget}"/> using JSON serialization.
  /// </summary>
  /// <typeparam name="TSource">The type of the source list elements.</typeparam>
  /// <typeparam name="TTarget">The type of the target list elements.</typeparam>
  /// <param name="sourceList">The source list to convert.</param>
  /// <returns>A <see cref="List{TTarget}"/> mapped from the source list.</returns>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="sourceList"/> is null.</exception>
  public static List<TTarget> JsonCloneListToList<TSource, TTarget>(List<TSource> sourceList)
  {
    if (sourceList == null) throw new ArgumentNullException(nameof(sourceList));

    var serialized = JsonConvert.SerializeObject(sourceList);
    var targetList = JsonConvert.DeserializeObject<List<TTarget>>(serialized);

    return targetList;
  }

  /// <summary>
  /// Converts an <see cref="IEnumerable{TSource}"/> to an <see cref="IEnumerable{TTarget}"/> using JSON serialization.
  /// </summary>
  /// <typeparam name="TSource">The type of the source enumerable elements.</typeparam>
  /// <typeparam name="TTarget">The type of the target enumerable elements.</typeparam>
  /// <param name="sourceList">The source enumerable to convert.</param>
  /// <returns>An <see cref="IEnumerable{TTarget}"/> mapped from the source enumerable.</returns>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="sourceList"/> is null.</exception>
  public static IEnumerable<TTarget> JsonCloneIEnumerableToIEnumerable<TSource, TTarget>(IEnumerable<TSource> sourceList)
  {
    if (sourceList == null) throw new ArgumentNullException(nameof(sourceList));

    var serialized = JsonConvert.SerializeObject(sourceList);
    var targetList = JsonConvert.DeserializeObject<IEnumerable<TTarget>>(serialized);

    return targetList;
  }
}


/// <summary>
/// Provides safe JSON deserialization with tolerant settings,
/// suppressing null values, missing members, and parsing errors.
/// </summary>
public static class JsonSafeDeserializer
{
  /// <summary>
  /// Deserializes a JSON string into an instance of type <typeparamref name="T"/> using safe settings.
  /// Null values and missing members are ignored. Errors during deserialization are logged and suppressed.
  /// Returns a default instance of <typeparamref name="T"/> if deserialization fails.
  /// </summary>
  /// <typeparam name="T">The target type to deserialize into. Must have a parameterless constructor.</typeparam>
  /// <param name="json">The JSON string to deserialize.</param>
  /// <returns>A populated instance of <typeparamref name="T"/>, or a new default instance if deserialization fails.</returns>
  public static T SafeDeserialize<T>(string json) where T : new()
  {
    var settings = new JsonSerializerSettings
    {
      NullValueHandling = NullValueHandling.Ignore,
      MissingMemberHandling = MissingMemberHandling.Ignore,
      Error = (sender, args) =>
      {
        Console.WriteLine("JSON Error at: " + args.ErrorContext.Path);
        args.ErrorContext.Handled = true;
      }
    };

    return JsonConvert.DeserializeObject<T>(json, settings) ?? new T();
  }
}




//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Application.Services.Mappings;

//public class MyMapper
//{
//  public static T Map<T>(object source)
//  {
//    var sourceType = source.GetType();
//    var destinationType = typeof(T);
//    var destinationProperties = destinationType.GetProperties();
//    var sourceProperties = sourceType.GetProperties();
//    var destination = Activator.CreateInstance<T>();
//    foreach (var destinationProperty in destinationProperties)
//    {
//      var sourceProperty = sourceProperties.FirstOrDefault(x => x.Name == destinationProperty.Name);
//      if (sourceProperty != null)
//      {
//        destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
//      }
//    }
//    return destination;
//  }

//  // how to use
//  // var source = new Source { Name = "Name", Age = 10 };
//  // var destination = MyMapper.Map<Destination>(source);
//  // example : var clonedObjectJson = MyClone.ModelMapping<Module, ModuleHistory>(oldData);
//  public static TDestination ModelMapping<TSource, TDestination>(TSource source)
//  {
//    string text = JsonConvert.SerializeObject((object)source);
//    return JsonConvert.DeserializeObject<TDestination>(text);
//  }


//  public static TTarget JsonClone<TSource, TTarget>(TSource source) where TTarget : new()
//  {
//    if (source == null) throw new ArgumentNullException(nameof(source));
//    var serialized = JsonConvert.SerializeObject(source);
//    var target = JsonConvert.DeserializeObject<TTarget>(serialized);

//    return target;
//  }

//  public static TTarget JsonCloneSafe<TSource, TTarget>(Domain.Entities.Entities.System.Company existingEntity, TSource source) where TTarget : new()
//  {
//    if (source == null) throw new ArgumentNullException(nameof(source));

//    var settings = new JsonSerializerSettings
//    {
//      NullValueHandling = NullValueHandling.Ignore,
//      MissingMemberHandling = MissingMemberHandling.Ignore,
//      DateTimeZoneHandling = DateTimeZoneHandling.Utc,
//      Error = (sender, args) =>
//      {
//        // Handle DateTime conversion errors
//        if (args.ErrorContext.Error is InvalidCastException &&
//            args.ErrorContext.Path.Contains("Date"))
//        {
//          // Set default DateTime for null values
//          args.ErrorContext.Handled = true;
//        }
//      }
//    };

//    var serialized = JsonConvert.SerializeObject(source, settings);
//    var target = JsonConvert.DeserializeObject<TTarget>(serialized, settings) ?? new TTarget();

//    ProcessDateTimeFields(target);

//    return target;
//  }

//  /// <summary>
//  /// Checke DateTime properties. If there are  DateTime.MinValue then set null.
//  /// </summary>
//  /// <typeparam name="T">Target object type</typeparam>
//  /// <param name="obj">check Object DateTime properties</param>
//  private static void ProcessDateTimeFields<T>(T obj)
//  {
//    if (obj == null) return;

//    var type = typeof(T);
//    var properties = type.GetProperties();

//    foreach (var property in properties)
//    {
//      if (property.PropertyType == typeof(DateTime) ||
//          property.PropertyType == typeof(DateTime?))
//      {
//        try
//        {
//          var value = property.GetValue(obj);

//          if (value is DateTime dateTimeValue && dateTimeValue == DateTime.MinValue)
//          {
//            if (property.PropertyType == typeof(DateTime?))
//            {
//              property.SetValue(obj, null);
//            }
//          }
//        }
//        catch (Exception)
//        {
//          continue;
//        }
//      }
//    }
//  }


//  // IEnumerable<Country> countries = await _repository.Countries.CountriesAsync(trackChanges);
//  //List<CountryDto> countryDtos = MyMapper.JsonCloneIEnumerableToList<Country, CountryDto>(countries);
//  public static List<TTarget> JsonCloneIEnumerableToList<TSource, TTarget>(IEnumerable<TSource> sourceList)
//  {
//    if (sourceList == null) throw new ArgumentNullException(nameof(sourceList));

//    var serialized = JsonConvert.SerializeObject(sourceList);
//    var targetList = JsonConvert.DeserializeObject<List<TTarget>>(serialized);

//    return targetList;
//  }

//  public static IEnumerable<TTarget> JsonCloneIEnumerableToIEnumerable<TSource, TTarget>(List<TSource> sourceList)
//  {
//    if (sourceList == null) throw new ArgumentNullException(nameof(sourceList));

//    var serialized = JsonConvert.SerializeObject(sourceList);
//    var targetList = JsonConvert.DeserializeObject<IEnumerable<TTarget>>(serialized);

//    return targetList;
//  }

//  public static List<TTarget> JsonCloneListToList<TSource, TTarget>(List<TSource> sourceList)
//  {
//    if (sourceList == null) throw new ArgumentNullException(nameof(sourceList));

//    var serialized = JsonConvert.SerializeObject(sourceList);
//    var targetList = JsonConvert.DeserializeObject<List<TTarget>>(serialized);

//    return targetList;
//  }

//  public static IEnumerable<TTarget> JsonCloneIEnumerableToIEnumerable<TSource, TTarget>(IEnumerable<TSource> sourceList)
//  {
//    if (sourceList == null) throw new ArgumentNullException(nameof(sourceList));

//    var serialized = JsonConvert.SerializeObject(sourceList);
//    var targetList = JsonConvert.DeserializeObject<IEnumerable<TTarget>>(serialized);

//    return targetList;
//  }
//}


//public static class JsonSafeDeserializer
//{
//  public static T SafeDeserialize<T>(string json) where T : new()
//  {
//    var settings = new JsonSerializerSettings
//    {
//      NullValueHandling = NullValueHandling.Ignore,
//      MissingMemberHandling = MissingMemberHandling.Ignore,
//      Error = (sender, args) =>
//      {
//        Console.WriteLine("JSON Error at: " + args.ErrorContext.Path);
//        args.ErrorContext.Handled = true;
//      }
//    };

//    return JsonConvert.DeserializeObject<T>(json, settings) ?? new T();
//  }
//}



