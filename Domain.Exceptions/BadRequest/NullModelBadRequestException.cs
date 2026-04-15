namespace Domain.Exceptions;

public sealed class NullModelBadRequestException : BadRequestException
{
    public NullModelBadRequestException(string modelName)
        : base($"The {modelName} object sent from client is null.") { }
}
