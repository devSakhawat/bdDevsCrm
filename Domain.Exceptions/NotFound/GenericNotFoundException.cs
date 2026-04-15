namespace Domain.Exceptions;

public sealed class GenericNotFoundException : NotFoundException
{
    public GenericNotFoundException(string message)
        : base(message) { }

    public GenericNotFoundException(string entityName, string fieldName, string fieldValue)
        : base(entityName, fieldName, fieldValue) { }
}
