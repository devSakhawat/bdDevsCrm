namespace Domain.Exceptions.Base;

public abstract class AppException : Exception
{
  public string ErrorCode { get; }
  public int StatusCode { get; }

  /// <summary>
  /// User-friendly message (defaults to the exception message)
  /// </summary>
  public virtual string UserFriendlyMessage => Message;

  /// <summary>
  /// Additional contextual data for the exception
  /// </summary>
  public Dictionary<string, object> AdditionalData { get; } = new();

  protected AppException(string message, Exception innerException) : base(message, innerException) 
  { 
      StatusCode = 500;
      ErrorCode = "INTERNAL_ERROR";
  }

  protected AppException(string message, int statusCode, string errorCode) : base(message)
  {
    StatusCode = statusCode;
    ErrorCode = errorCode;
  }

  /// <summary>
  /// Fluent method to add additional data
  /// </summary>
  public AppException WithData(string key, object value)
  {
    AdditionalData[key] = value;
    return this;
  }

}
