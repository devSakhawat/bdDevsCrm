namespace Domain.Exceptions;

public sealed class IdMismatchBadRequestException : BadRequestException
{
    public IdMismatchBadRequestException(string id, string entityName)
        : base($"The ID '{id}' in the URL does not match the ID in the {entityName} body.") { }
}
