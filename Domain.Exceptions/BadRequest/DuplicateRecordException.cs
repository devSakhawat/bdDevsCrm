using Domain.Exceptions.Base;

namespace Domain.Exceptions;

public class DuplicateRecordException : AppException
{
  public DuplicateRecordException(string entityName, string fieldName)
      : base($"A {entityName} with the same {fieldName} already exists.", 409, "DUPLICATE_RECORD")
  {
  }
}
