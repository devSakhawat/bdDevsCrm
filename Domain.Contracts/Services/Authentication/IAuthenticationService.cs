using Application.Shared.DataTransferObjects.Authentication;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Domain.Entities.Entities.Token;

namespace Domain.Contracts.Services.Authentication;

public interface IAuthenticationService
{
  /// <summary>
  /// Validate user credentials
  /// </summary>
  Task<bool> ValidateUser(UserForAuthenticationDto userForAuth, CancellationToken cancellationToken = default);

  /// <summary>
  /// Validate user credentials
  /// </summary>
  Task<LoginValidationResult> ValidateUserLogin(UserForAuthenticationDto userForAuth, UsersDto user, CancellationToken cancellationToken = default);

  /// <summary>
  /// Create JWT token with refresh token
  /// </summary>
  Task<TokenResponse> CreateToken(UserForAuthenticationDto userForAuth, CancellationToken cancellationToken = default);

  /// <summary>
  /// Refresh access token using refresh token
  /// </summary>
  Task<TokenResponse> RefreshTokenAsync(string refreshToken, string ipAddress, CancellationToken cancellationToken = default);

  /// <summary>
  /// Revoke refresh token
  /// </summary>
  Task<bool> RevokeTokenAsync(string refreshToken, string ipAddress, CancellationToken cancellationToken = default);

  /// <summary>
  /// Remove expired refresh tokens from database
  /// </summary>
  Task RemoveExpiredTokensAsync(CancellationToken cancellationToken = default);

  /// <summary>
  /// Revokes all active refresh tokens for a user (e.g., on logout from all devices or password change)
  /// </summary>
  Task RevokeAllUserTokensAsync(int userId, string ipAddress, CancellationToken cancellationToken = default);

}