namespace Domain.Exceptions;

public sealed class GenericNotFoundException : NotFoundException
{
    public GenericNotFoundException(string message)
        : base(message) { }
}
