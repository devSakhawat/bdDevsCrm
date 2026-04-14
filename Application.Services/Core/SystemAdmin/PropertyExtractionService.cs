using Domain.Contracts.Services.Core.SystemAdmin;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Application.Services.Core.SystemAdmin;

internal sealed class PropertyInspectorService : IPropertyInspectorService
{
  private readonly Dictionary<string, Type> _typeCache;
  private readonly ILogger<PropertyInspectorService> _logger;
  private readonly IConfiguration _config;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public PropertyInspectorService(IConfiguration config, ILogger<PropertyInspectorService> logger, IHttpContextAccessor httpContextAccessor)
  {
    _config = config;
    _logger = logger;
    _httpContextAccessor = httpContextAccessor;
    _typeCache = new Dictionary<string, Type>();
    CacheTypes();
  }

  private void CacheTypes()
  {
    var types = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(a => a.GetTypes())
        .Where(t => t.IsClass && !t.IsAbstract);

    foreach (var type in types)
    {
      _typeCache[type.Name] = type;
      _typeCache[type.FullName] = type;
    }
  }

  public PropertyMetadata PropertyMetadata(string className)
  {
    try
    {
      if (!_typeCache.TryGetValue(className, out var type))
      {
        throw new InvalidOperationException($"Type '{className}' not found!");
      }

      var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

      return new PropertyMetadata
      {
        ClassName = type.Name,
        FullName = type.FullName,
        Properties = properties.Select(p => new PropertyDetail
        {
          Name = p.Name,
          TypeName = p.PropertyType.Name,
          FullTypeName = p.PropertyType.FullName,
          IsNullable = Nullable.GetUnderlyingType(p.PropertyType) != null,
          IsComplex = !p.PropertyType.IsPrimitive &&
                        p.PropertyType != typeof(string) &&
                        p.PropertyType != typeof(DateTime),
          CanRead = p.CanRead,
          CanWrite = p.CanWrite
        }).ToList()
      };
    }
    catch (Exception ex)
    {
      _logger.LogError("Error getting metadata for {ClassName}" + className);
      throw;
    }
  }

  public T CreateInstance<T>() where T : class
  {
    return Activator.CreateInstance<T>();
  }

  public Dictionary<string, object> FlattenedProperties(object instance)
  {
    return FlattenObject(instance);
  }

  private Dictionary<string, object> FlattenObject(object obj, string prefix = "")
  {
    var result = new Dictionary<string, object>();

    if (obj == null) return result;

    var type = obj.GetType();
    var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

    foreach (var prop in properties)
    {
      try
      {
        var value = prop.GetValue(obj);
        var key = string.IsNullOrEmpty(prefix) ? prop.Name : $"{prefix}.{prop.Name}";

        if (value == null)
        {
          result[key] = null;
          continue;
        }

        var propType = prop.PropertyType;

        // Simple types
        if (IsSimpleType(propType))
        {
          result[key] = value;
        }
        // Collections
        else if (value is System.Collections.IEnumerable enumerable && !(value is string))
        {
          var list = enumerable.Cast<object>().ToList();
          result[key] = list;

          // Collection items flatten
          for (int i = 0; i < list.Count; i++)
          {
            var itemDict = FlattenObject(list[i], $"{key}[{i}]");
            foreach (var item in itemDict)
            {
              result[item.Key] = item.Value;
            }
          }
        }
        // Complex objects - Recursive
        else
        {
          var nested = FlattenObject(value, key);
          foreach (var item in nested)
          {
            result[item.Key] = item.Value;
          }
        }
      }
      catch (Exception ex)
      {
        _logger.LogWarning("Error reading property {PropertyName}: {ErrorMessage}", prop.Name, ex.Message);
      }
    }

    return result;
  }

  private bool IsSimpleType(Type type)
  {
    return type.IsPrimitive ||
           type.IsEnum ||
           type == typeof(string) ||
           type == typeof(decimal) ||
           type == typeof(DateTime) ||
           type == typeof(DateTimeOffset) ||
           type == typeof(TimeSpan) ||
           type == typeof(Guid) ||
           Nullable.GetUnderlyingType(type) != null;
  }
}

