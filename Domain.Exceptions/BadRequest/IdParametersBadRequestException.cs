namespace Domain.Exceptions;

public sealed class IdParametersBadRequestException : BadRequestException
{
    public IdParametersBadRequestException()
        : base("Parameter id is null") { }

    public IdParametersBadRequestException(string message)
        : base(message) { }
}
