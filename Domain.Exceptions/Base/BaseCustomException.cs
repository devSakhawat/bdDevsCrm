//using System.Net;

//namespace Domain.Exceptions.Base;

///// <summary>
///// Base class for all custom exceptions in the application.
///// Provides ErrorCode, AdditionalData, and standardized exception handling.
///// </summary>
//[Serializable]
//public abstract class BaseCustomException : Exception
//{
//    /// <summary>
//    /// HTTP status code for the exception
//    /// </summary>
//    public abstract int StatusCode { get; }

//    /// <summary>
//    /// Unique error code for tracking and categorization
//    /// </summary>
//    public abstract string ErrorCode { get; }

//    /// <summary>
//    /// User-friendly message (defaults to the exception message)
//    /// </summary>
//    public virtual string UserFriendlyMessage => Message;

//    /// <summary>
//    /// Additional contextual data for the exception
//    /// </summary>
//    public Dictionary<string, object> AdditionalData { get; } = new();

//    protected BaseCustomException(string message) : base(message) { }

//    protected BaseCustomException(string message, Exception innerException)
//        : base(message, innerException) { }

//    /// <summary>
//    /// Fluent method to add additional data
//    /// </summary>
//    public BaseCustomException WithData(string key, object value)
//    {
//        AdditionalData[key] = value;
//        return this;
//    }
//}
