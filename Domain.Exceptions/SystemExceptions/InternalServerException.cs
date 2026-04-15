using Domain.Exceptions.Base;

namespace Domain.Exceptions.SystemExceptions;

public class InternalServerException : AppException
{
  public InternalServerException(string message = "Internal Server Error")
      : base(message, 500, "INTERNAL_ERROR")
  {
  }
}

// Usage Example:
// throw new InternalServerException("Database connection failed", "DB_CONNECTION_FAILED");

