namespace Domain.Exceptions;

public sealed class GenericUnauthorizedException : UnauthorizedException
{
    public GenericUnauthorizedException(string message)
        : base(message) { }
}
