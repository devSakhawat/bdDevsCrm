using Domain.Exceptions.Base;

namespace Domain.Exceptions;

public class NotFoundException : AppException
{
  public NotFoundException(string message, string errorCode = "NOT_FOUND")
      : base(message, 404, errorCode)
  {
  }
}

// Usage Example:
// throw new NotFoundException("Company not found", "COMPANY_NOT_FOUND");