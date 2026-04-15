namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new token blacklist entry.
/// </summary>
public record CreateTokenBlacklistRecord(
    string Token,
    string? TokenHash,
    DateTime ExpiryDate,
    DateTime CreatedAt);

/// <summary>
/// Record for updating an existing token blacklist entry.
/// </summary>
public record UpdateTokenBlacklistRecord(
    Guid TokenId,
    string Token,
    string? TokenHash,
    DateTime ExpiryDate,
    DateTime CreatedAt);

/// <summary>
/// Record for deleting a token blacklist entry.
/// </summary>
public record DeleteTokenBlacklistRecord(Guid TokenId);
