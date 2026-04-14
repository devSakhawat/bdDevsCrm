using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;
using Domain.Entities.Entities.Token;

namespace Domain.Contracts.Core.Authentication;

public interface IRefreshTokenRepository : IRepositoryBase<RefreshToken>
{
  /// <summary>
  /// Revoke all tokens for a user
  /// </summary>
  /// <param name="userId">User ID</param>
  Task RevokeAllTokensByUserIdAsync(int userId, CancellationToken cancellationToken = default);

  /// <summary>
  ///  all expired tokens
  /// </summary>
  /// <returns>List of expired RefreshToken</returns>
  Task<IEnumerable<RefreshToken>> ExpiredTokensAsync(CancellationToken cancellationToken = default);
}




///// <summary>
///// Refresh token repository interface
///// </summary>
//public interface IRefreshTokenRepository
//{
//  /// <summary>
//  ///  refresh token by token value
//  /// Token value দিয়ে refresh token খুঁজে বের করা
//  /// </summary>
//  /// <param name="token">Hashed token value</param>
//  /// <param name="trackChanges">Entity tracking enable করবে কিনা</param>
//  /// <returns>RefreshToken entity অথবা null</returns>
//  Task<RefreshToken> ByTokenAsync(string token, bool trackChanges);

//  /// <summary>
//  ///  all refresh tokens by user ID
//  /// একটা user এর সব refresh tokens
//  /// </summary>
//  /// <param name="userId">User ID</param>
//  /// <param name="trackChanges">Entity tracking enable করবে কিনা</param>
//  /// <returns>List of RefreshToken</returns>
//  Task<IEnumerable<RefreshToken>> ByUserIdAsync(int userId, bool trackChanges);

//  /// <summary>
//  ///  all expired tokens
//  /// যেসব token expire হয়ে গেছে সেগুলো খুঁজে বের করা
//  /// </summary>
//  /// <returns>List of expired RefreshToken</returns>
//  Task<IEnumerable<RefreshToken>> ExpiredTokensAsync();

//  /// <summary>
//  ///  all active tokens by user ID
//  /// একটা user এর active (valid) tokens
//  /// </summary>
//  /// <param name="userId">User ID</param>
//  /// <param name="trackChanges">Entity tracking enable করবে কিনা</param>
//  /// <returns>List of active RefreshToken</returns>
//  Task<IEnumerable<RefreshToken>> ActiveTokensByUserIdAsync(int userId, bool trackChanges);

//  /// <summary>
//  /// Create new refresh token
//  /// নতুন refresh token create করা
//  /// </summary>
//  /// <param name="refreshToken">RefreshToken entity</param>
//  void Create(RefreshToken refreshToken);

//  /// <summary>
//  /// Update existing refresh token
//  /// Existing refresh token update করা
//  /// </summary>
//  /// <param name="refreshToken">RefreshToken entity</param>
//  void Update(RefreshToken refreshToken);

//  /// <summary>
//  /// Delete refresh token
//  /// Refresh token delete করা
//  /// </summary>
//  /// <param name="refreshToken">RefreshToken entity</param>
//  void Delete(RefreshToken refreshToken);

//  /// <summary>
//  /// Bulk delete multiple refresh tokens
//  /// একসাথে অনেকগুলো refresh token delete করা
//  /// </summary>
//  /// <param name="refreshTokens">List of RefreshToken to delete</param>
//  void BulkDelete(IEnumerable<RefreshToken> refreshTokens);

//  /// <summary>
//  /// Revoke all tokens for a user
//  /// একটা user এর সব tokens revoke করে দেওয়া
//  /// </summary>
//  /// <param name="userId">User ID</param>
//  Task RevokeAllTokensByUserIdAsync(int userId);
//}