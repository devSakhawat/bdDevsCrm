using Domain.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions;

public class ConflictException : AppException
{
  public ConflictException(string message, string errorCode = "CONFLICT")
      : base(message, 409, errorCode)
  {
  }
}

//Usage Example:
// throw new ConflictException("Resource already exists", "RESOURCE_EXISTS");
