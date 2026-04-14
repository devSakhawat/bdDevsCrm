using Domain.Exceptions.Base;

namespace Domain.Exceptions.SpecialCases;

public class NotAcceptableException : AppException
{
  public NotAcceptableException(string message, string errorCode)
    : base(message, 406, errorCode)
  {
  }
}
