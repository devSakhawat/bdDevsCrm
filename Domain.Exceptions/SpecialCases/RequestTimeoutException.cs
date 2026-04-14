using Domain.Exceptions.Base;

namespace Domain.Exceptions.SpecialCases;

public class RequestTimeoutException : AppException
{
  public RequestTimeoutException(string message = "Request Timeout")
      : base(message, 408, "REQUEST_TIMEOUT")
  {
  }
}

// Usage Example:
// throw new RequestTimeoutException("The request took too long to process", "TIMEOUT_OCCURRED");

