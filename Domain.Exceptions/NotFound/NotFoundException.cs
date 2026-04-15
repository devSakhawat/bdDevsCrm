using Domain.Exceptions.Base;

namespace Domain.Exceptions;

public class NotFoundException : AppException
{
  public NotFoundException(string message, string errorCode = "NOT_FOUND")
      : base(message, 404, errorCode)
  {
  }

  public NotFoundException(string entityName, string fieldName, string fieldValue)
      : base($"{entityName} with {fieldName} '{fieldValue}' was not found.", 404, "NOT_FOUND")
  {
  }
}