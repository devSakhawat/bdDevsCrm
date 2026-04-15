using Domain.Exceptions.Base;

namespace Domain.Exceptions.DomainSpecific;

public class InvalidOperationExceptionEx : AppException
{
  public InvalidOperationExceptionEx(string message)
      : base(message, 400, "INVALID_OPERATION")
  {
  }
}

// Usage Example:
// throw new InvalidOperationExceptionEx("The operation is not valid in the current state", "INVALID_OPERATION");
