using Domain.Exceptions.Base;

namespace Domain.Exceptions;

public class GenericConflictException : AppException
{
  public GenericConflictException(string message)
      : base(message, 409, "CONFLICT")
  {
  }
}
