namespace Domain.Exceptions;

public sealed class GenericListNotFoundException : NotFoundException
{
    public GenericListNotFoundException(string entityName)
        : base($"No {entityName} records found.") { }
}
