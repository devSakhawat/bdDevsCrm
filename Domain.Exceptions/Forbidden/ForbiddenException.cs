using Domain.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions.Forbidden;

public class ForbiddenException : AppException
{
  public ForbiddenException(string message, string errorCode = "FORBIDDEN")
      : base(message, 403, errorCode)
  {
  }
}

// Usage Example:
// throw new ForbiddenException("Access denied", "ACCESS_DENIED");