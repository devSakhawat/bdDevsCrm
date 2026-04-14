namespace Domain.Exceptions;

public sealed class GenericBadRequestException : BadRequestException
{
    public GenericBadRequestException(string message)
        : base(message) { }
}
