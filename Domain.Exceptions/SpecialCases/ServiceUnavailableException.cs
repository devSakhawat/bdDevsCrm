using Domain.Exceptions.Base;

namespace Domain.Exceptions.SpecialCases;

public class ServiceUnavailableException : AppException
{
  public ServiceUnavailableException(string message = "Service Unavailable")
      : base(message, 503, "SERVICE_UNAVAILABLE")
  {
  }
}

// Usage Example:
// throw new ServiceUnavailableException("The service is temporarily unavailable", "SERVICE_DOWN");