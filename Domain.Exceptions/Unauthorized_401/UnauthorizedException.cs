using Domain.Exceptions.Base;

namespace Domain.Exceptions;

public class UnauthorizedException : AppException
{
  public UnauthorizedException(string message, string errorCode = "UNAUTHORIZED")
      : base(message, 401, errorCode)
  {
  }
}