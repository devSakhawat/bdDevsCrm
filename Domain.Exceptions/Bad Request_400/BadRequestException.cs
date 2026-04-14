using Domain.Exceptions.Base;

namespace Domain.Exceptions;

public class BadRequestException : AppException
{
  public BadRequestException(string message, string errorCode = "BAD_REQUEST")
      : base(message, 400, errorCode)
  {
  }
}


// Usage Example:
//throw new BadRequestException("Id mismatch", "ID_MISMATCH");