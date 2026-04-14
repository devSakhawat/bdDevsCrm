namespace Domain.Entities.Entities.Token;

/// <summary>
/// Enhanced token response with refresh token support
/// </summary>
public class TokenResponse
{
  /// <summary>
  /// JWT access token (short-lived, 15 minutes)
  /// </summary>
  public string AccessToken { get; set; }

  /// <summary>
  /// Refresh token (long-lived, 7 days)
  /// </summary>
  public string RefreshToken { get; set; }

  /// <summary>
  /// Access token expiry time (UTC)
  /// </summary>
  public DateTime AccessTokenExpiry { get; set; }

  /// <summary>
  /// Refresh token expiry time (UTC)
  /// </summary>
  public DateTime RefreshTokenExpiry { get; set; }

  /// <summary>
  /// Token type (always "Bearer")
  /// </summary>
  public string TokenType { get; set; } = "Bearer";

  /// <summary>
  /// Access token validity in seconds
  /// </summary>
  public int ExpiresIn { get; set; }
}