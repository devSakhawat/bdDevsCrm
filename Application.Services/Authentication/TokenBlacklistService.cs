using Domain.Contracts.Repositories;
using Domain.Contracts.Services.Authentication;
using Domain.Entities.Entities.System;
using Domain.Exceptions;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using bdDevs.Shared.Extensions;
using Application.Shared.Grid;
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

  // CRUD Records Pattern Implementation
  public async Task<TokenBlacklistDto> CreateAsync(CreateTokenBlacklistRecord record, CancellationToken cancellationToken = default)
  {
    var entity = new TokenBlacklist
    {
      TokenId = Guid.NewGuid(),
      Token = record.Token,
      TokenHash = record.TokenHash ?? HashToken(record.Token),
      ExpiryDate = record.ExpiryDate,
      CreatedAt = record.CreatedAt
    };

    await _repository.TokenBlacklists.AddAsync(entity, cancellationToken);
    await _repository.SaveAsync(cancellationToken);

    return entity.MapTo<TokenBlacklistDto>();
  }

  public async Task<TokenBlacklistDto> UpdateAsync(UpdateTokenBlacklistRecord record, bool trackChanges, CancellationToken cancellationToken = default)
  {
    var entity = await _repository.TokenBlacklists.GetByConditionAsync(
        t => t.TokenId == record.TokenId, trackChanges, cancellationToken);

    if (entity == null)
      throw new TokenBlacklistNotFoundException(record.TokenId);

    entity.Token = record.Token;
    entity.TokenHash = record.TokenHash ?? HashToken(record.Token);
    entity.ExpiryDate = record.ExpiryDate;
    entity.CreatedAt = record.CreatedAt;

    await _repository.SaveAsync(cancellationToken);

    return entity.MapTo<TokenBlacklistDto>();
  }

  public async Task DeleteAsync(DeleteTokenBlacklistRecord record, bool trackChanges, CancellationToken cancellationToken = default)
  {
    var entity = await _repository.TokenBlacklists.GetByConditionAsync(
        t => t.TokenId == record.TokenId, trackChanges, cancellationToken);

    if (entity == null)
      throw new TokenBlacklistNotFoundException(record.TokenId);

    _repository.TokenBlacklists.Delete(entity);
    await _repository.SaveAsync(cancellationToken);
  }

  public async Task<TokenBlacklistDto> TokenBlacklistAsync(Guid tokenId, bool trackChanges, CancellationToken cancellationToken = default)
  {
    var entity = await _repository.TokenBlacklists.GetByConditionAsync(
        t => t.TokenId == tokenId, trackChanges, cancellationToken);

    if (entity == null)
      throw new TokenBlacklistNotFoundException(tokenId);

    return entity.MapTo<TokenBlacklistDto>();
  }

  public async Task<IEnumerable<TokenBlacklistDto>> TokenBlacklistsAsync(bool trackChanges, CancellationToken cancellationToken = default)
  {
    var entities = await _repository.TokenBlacklists.GetAllAsync(trackChanges, cancellationToken);
    return entities.MapToList<TokenBlacklistDto>();
  }

  public async Task<GridEntity<TokenBlacklistDto>> TokenBlacklistsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
  {
    var gridEntity = await _repository.TokenBlacklists.GetGridDataAsync<TokenBlacklist, TokenBlacklistDto>(options, cancellationToken);
    return gridEntity;
  }

  // Authentication-specific Methods
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
      TokenId = Guid.NewGuid(),
      Token = token,
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
}
