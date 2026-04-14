using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.DataTransferObjects.Authentication;

/// <summary>
/// Enhanced token response with refresh token support
/// </summary>
public class TokenResponseDto
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

	public UserSessionData? UserSession { get; set; }
	public string Status { get; set; }

	public bool IsSuccess { get; set; }
}
