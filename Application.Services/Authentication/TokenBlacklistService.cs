using Domain.Contracts.Repositories;
using Domain.Contracts.Services.Authentication;
using Domain.Entities.Entities.System;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services.Authentication;

public class TokenBlacklistService : ITokenBlacklistService
{

  private readonly IRepositoryManager _repository;
  private readonly ILogger<TokenBlacklistService> _logger;

  public TokenBlacklistService(
      IRepositoryManager repository,
      ILogger<TokenBlacklistService> logger)
  {
    _repository = repository;
    _logger = logger;
  }

  public async Task<bool> IsTokenBlacklistedAsync(
      string token,
      CancellationToken ct = default)
  {
    if (string.IsNullOrEmpty(token))
      return false;

    // Raw token hash করো — DB-তে hash store করা safe
    var hash = HashToken(token);

    return await _repository.TokenBlacklists
        .IsBlacklistedAsync(hash, ct);
  }

  public async Task BlacklistTokenAsync(
      string token,
      DateTime expiry,
      CancellationToken ct = default)
  {
    if (string.IsNullOrEmpty(token))
      return;

    var hash = HashToken(token);

    // Already blacklisted কিনা check করো
    bool exists = await _repository.TokenBlacklists
        .IsBlacklistedAsync(hash, ct);

    if (exists)
      return;

    var blacklistedToken = new TokenBlacklist
    {
      TokenHash = hash,
      ExpiryDate = expiry,
      CreatedAt = DateTime.UtcNow,
    };

    await _repository.TokenBlacklists
        .AddAsync(blacklistedToken, ct);

    await _repository.SaveAsync(ct);

    _logger.LogInformation(
        "Token blacklisted. Expiry: {Expiry}", expiry);
  }

  public async Task RemoveExpiredTokensAsync(
      CancellationToken ct = default)
  {
    await _repository.TokenBlacklists
        .RemoveExpiredAsync(ct);

    await _repository.SaveAsync(ct);

    _logger.LogInformation(
        "Expired blacklisted tokens removed at {Time}",
        DateTime.UtcNow);
  }

  // SHA-256 hash — token directly store করা unsafe
  private static string HashToken(string token)
  {
    var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
    return Convert.ToHexString(bytes).ToLowerInvariant();
  }

  //private readonly IConfiguration _configuration;
  //private readonly IRepositoryManager _repository;
  //private readonly ILogger<TokenBlacklistService> _logger;

  //public TokenBlacklistService(IConfiguration configuration, IRepositoryManager repository, ILogger<TokenBlacklistService> logger)
  //{
  //  _configuration = configuration;
  //  _repository = repository;
  //  _logger = logger;
  //}
  //public async Task<TokenBlacklist> AddToBlacklistAsync(string token, bool trackChanges, CancellationToken cancellationToken = default)
  //{

  //  if (string.IsNullOrEmpty(token)) throw new BadRequestException("TokenBlacklist");

  //  var handler = new JwtSecurityTokenHandler();
  //  var jwtToken = handler.ReadJwtToken(token);
  //  var expiryDate = jwtToken.ValidTo;
  //  var tokenBlacklist = new TokenBlacklist
  //  {
  //    TokenId = Guid.NewGuid(),
  //    Token = token,
  //    ExpiryDate = expiryDate
  //  };

  //  var tokenEntity = await _repository.TokenBlacklists.AddToBlacklistAsync(tokenBlacklist, cancellationToken);
  //  await _repository.SaveAsync(cancellationToken);
  //  return tokenBlacklist;
  //}

  //public Task<bool> IsTokenBlacklisted(string token, bool trackChanges, CancellationToken cancellationToken = default)
  //{
  //  var isBlacklisted = _repository.TokenBlacklists.IsTokenBlacklistedAsync(token, cancellationToken);
  //  return isBlacklisted;
  //}


  //public async Task<bool> IsTokenBlacklistedAsync(
  //      string token,
  //      CancellationToken ct = default)
  //{
  //  if (string.IsNullOrEmpty(token))
  //    return false;

  //  // Raw token hash করো — DB-তে hash store করা safe
  //  var hash = HashToken(token);

  //  return await _repository.TokenBlacklists.IsBlacklistedAsync(hash, ct);
  //}

  //public async Task BlacklistTokenAsync(
  //    string token,
  //    DateTime expiry,
  //    CancellationToken ct = default)
  //{
  //  if (string.IsNullOrEmpty(token))
  //    return;

  //  var hash = HashToken(token);

  //  // Already blacklisted কিনা check করো
  //  bool exists = await _repository.TokenBlacklists
  //      .IsBlacklistedAsync(hash, ct);

  //  if (exists)
  //    return;

  //  var blacklistedToken = new TokenBlacklist
  //  {
  //    TokenHash = hash,
  //    ExpiryDate = expiry,
  //    CreatedAt = DateTime.UtcNow,
  //  };

  //  await _repository.TokenBlacklists
  //      .AddAsync(blacklistedToken, ct);

  //  await _repository.SaveAsync(ct);

  //  _logger.LogInformation(
  //      "Token blacklisted. Expiry: {Expiry}", expiry);
  //}

  //public async Task RemoveExpiredTokensAsync(
  //    CancellationToken ct = default)
  //{
  //  await _repository.TokenBlacklists.RemoveExpiredAsync(ct);

  //  await _repository.SaveAsync(ct);

  //  _logger.LogInformation(
  //      "Expired blacklisted tokens removed at {Time}",
  //      DateTime.UtcNow);
  //}

  //// SHA-256 hash — token directly store করা unsafe
  //private static string HashToken(string token)
  //{
  //  var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
  //  return Convert.ToHexString(bytes).ToLowerInvariant();
  //}







}
