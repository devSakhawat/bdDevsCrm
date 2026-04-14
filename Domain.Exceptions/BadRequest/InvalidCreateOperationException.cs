using Domain.Exceptions.Base;

namespace Domain.Exceptions;

public class InvalidCreateOperationException : AppException
{
  public InvalidCreateOperationException(string message)
      : base(message, 400, "INVALID_CREATE_OPERATION")
  {
  }
}
