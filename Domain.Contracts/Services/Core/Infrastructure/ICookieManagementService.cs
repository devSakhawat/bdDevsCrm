namespace Domain.Contracts.Services.Core.Infrastructure;

/// <summary>
/// Service for managing HTTP cookies, particularly refresh tokens.
/// Encapsulates cookie operations for authentication flows.
/// </summary>
public interface ICookieManagementService
{
	/// <summary>
	/// Sets a refresh token cookie with secure options.
	/// </summary>
	/// <param name="refreshToken">The refresh token value</param>
	/// <param name="expiry">Token expiration date</param>
	void SetRefreshTokenCookie(string refreshToken, DateTime expiry);

	/// <summary>
	/// Clears the refresh token cookie.
	/// </summary>
	void ClearRefreshTokenCookie();

	/// <summary>
	/// Retrieves the refresh token from cookies.
	/// </summary>
	/// <returns>Refresh token if present, null otherwise</returns>
	string? RefreshToken();
}
