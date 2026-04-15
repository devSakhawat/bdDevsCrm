using Domain.Exceptions.Base;

namespace Domain.Exceptions;

public class CollectionByIdsBadRequestException : AppException
{
  public CollectionByIdsBadRequestException(string entityName)
      : base($"The provided IDs for {entityName} collection are invalid or empty.", 400, "INVALID_COLLECTION_IDS")
  {
  }
}
