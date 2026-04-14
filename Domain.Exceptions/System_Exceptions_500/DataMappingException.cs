namespace Domain.Exceptions.ServerError;

/// <summary>
/// Thrown when a data reader value cannot be mapped to a DTO/entity property.
/// </summary>
public sealed class DataMappingException : Exception
{
  public string? ColumnName { get; }
  public string? PropertyName { get; }
  public string? PropertyType { get; }
  public string? EntityType { get; }
  public object? RawValue { get; }

  public DataMappingException(string message) : base(message) { }

  public DataMappingException(string message, Exception? inner) : base(message, inner) { }

  public DataMappingException(
    string message,
    string? columnName,
    string? propertyName,
    string? propertyType,
    string? entityType,
    object? rawValue,
    Exception? inner = null)
    : base(message, inner)
  {
    ColumnName = columnName;
    PropertyName = propertyName;
    PropertyType = propertyType;
    EntityType = entityType;
    RawValue = rawValue;
  }
}