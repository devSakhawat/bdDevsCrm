namespace Domain.Exceptions;

public sealed class TokenBlacklistNotFoundException : NotFoundException
{
    public TokenBlacklistNotFoundException(Guid tokenId)
        : base($"Token blacklist entry with id: {tokenId} doesn't exist in the database.")
    {
    }
}
