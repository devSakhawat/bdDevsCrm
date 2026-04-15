using Domain.Exceptions.Base;

namespace Domain.Exceptions;

/// <summary>
/// Exception thrown when CRM-specific unauthorized access is detected.
/// </summary>
public class UnauthorizedAccessCRMException : AppException
{
  public UnauthorizedAccessCRMException(string message)
      : base(message, 401, "UNAUTHORIZED_ACCESS")
  {
  }
}
