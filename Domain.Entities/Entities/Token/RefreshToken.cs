using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Entities.Token;

/// <summary>
/// Refresh token entity for database storage
/// </summary>
public class RefreshToken
{
  public int RefreshTokenId { get; set; }

  /// <summary>
  /// User ID associated with this token
  /// </summary>
  public int UserId { get; set; }

  /// <summary>
  /// Refresh token value (hashed)
  /// </summary>
  public string Token { get; set; }

  /// <summary>
  /// Token expiry date (UTC)
  /// </summary>
  public DateTime ExpiryDate { get; set; }

  /// <summary>
  /// Token creation date (UTC)
  /// </summary>
  public DateTime CreatedDate { get; set; }

	/// <summary>
	/// IP address where token was created
	/// </summary>
	public string CreatedByIp { get; set; }

	/// <summary>
	/// Is token revoked?
	/// </summary>
	public bool IsRevoked { get; set; }

  /// <summary>
  /// Date when token was revoked
  /// </summary>
  public DateTime? RevokedDate { get; set; }

  /// <summary>
  /// Token replaced by new refresh token (for rotation)
  /// </summary>
  public string? ReplacedByToken { get; set; }

  /// <summary>
  /// Is token active (not expired and not revoked)
  /// </summary>
  public bool IsActive => !IsRevoked && ExpiryDate > DateTime.UtcNow;
}