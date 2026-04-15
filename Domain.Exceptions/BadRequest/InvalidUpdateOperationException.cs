namespace Domain.Exceptions;

public sealed class InvalidUpdateOperationException : BadRequestException
{
    public InvalidUpdateOperationException(string message)
        : base(message) { }
}
