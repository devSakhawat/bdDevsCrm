using Presentation.AuthorizeAttributes;
using Domain.Contracts.Services;
using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Domain.Exceptions;
using bdDevs.Shared.Constants;
using Application.Shared.Grid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Presentation.ActionFilters;

namespace Presentation.Controllers.Core.SystemAdmin;

/// <summary>
/// Token Blacklist management endpoints for security and authentication.
/// </summary>
[AuthorizeUser]
public class TokenBlacklistController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public TokenBlacklistController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves paginated summary grid of blacklisted tokens.
    /// </summary>
    [HttpPost(RouteConstants.TokenBlacklistSummary)]
    public async Task<IActionResult> TokenBlacklistSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.TokenBlacklist.TokenBlacklistsSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<TokenBlacklistDto>(), "No blacklisted tokens found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Token blacklist summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new token blacklist entry using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateTokenBlacklist)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateTokenBlacklistAsync([FromBody] CreateTokenBlacklistRecord record, CancellationToken cancellationToken = default)
    {
        var createdTokenBlacklist = await _serviceManager.TokenBlacklist.CreateAsync(record, cancellationToken);

        if (createdTokenBlacklist.TokenId <= 0)
            throw new InvalidCreateOperationException("Failed to create token blacklist record.");

        return Ok(ApiResponseHelper.Created(createdTokenBlacklist, "Token blacklisted successfully."));
    }

    /// <summary>
    /// Updates an existing token blacklist entry using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateTokenBlacklist)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateTokenBlacklistAsync([FromRoute] Guid key, [FromBody] UpdateTokenBlacklistRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.TokenId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateTokenBlacklistRecord));

        var updatedTokenBlacklist = await _serviceManager.TokenBlacklist.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedTokenBlacklist, "Token blacklist entry updated successfully."));
    }

    /// <summary>
    /// Deletes a token blacklist entry using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteTokenBlacklist)]
    public async Task<IActionResult> DeleteTokenBlacklistAsync([FromRoute] Guid key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteTokenBlacklistRecord(key);
        await _serviceManager.TokenBlacklist.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Token blacklist entry deleted successfully"));
    }

    /// <summary>
    /// Retrieves a token blacklist entry by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadTokenBlacklist)]
    public async Task<IActionResult> TokenBlacklistAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            throw new IdParametersBadRequestException();

        var tokenBlacklist = await _serviceManager.TokenBlacklist.TokenBlacklistAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(tokenBlacklist, "Token blacklist entry retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all token blacklist entries.
    /// </summary>
    [HttpGet(RouteConstants.ReadTokenBlacklists)]
    public async Task<IActionResult> TokenBlacklistsAsync(CancellationToken cancellationToken = default)
    {
        var tokenBlacklists = await _serviceManager.TokenBlacklist.TokenBlacklistsAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!tokenBlacklists.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<TokenBlacklistDto>(), "No blacklisted tokens found."));

        return Ok(ApiResponseHelper.Success(tokenBlacklists, "Token blacklist entries retrieved successfully"));
    }

    /// <summary>
    /// Checks if a specific token is blacklisted (for authentication middleware).
    /// </summary>
    [HttpPost(RouteConstants.IsTokenBlacklisted)]
    public async Task<IActionResult> IsTokenBlacklistedAsync([FromBody] string token, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(token))
            throw new BadRequestException("Token cannot be empty");

        var isBlacklisted = await _serviceManager.TokenBlacklist.IsTokenBlacklistedAsync(token, cancellationToken);

        return Ok(ApiResponseHelper.Success(isBlacklisted, $"Token is {(isBlacklisted ? "blacklisted" : "not blacklisted")}"));
    }

    /// <summary>
    /// Manually blacklists a token (for logout or revocation).
    /// </summary>
    [HttpPost(RouteConstants.BlacklistToken)]
    public async Task<IActionResult> BlacklistTokenAsync([FromBody] BlacklistTokenRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null || string.IsNullOrEmpty(request.Token))
            throw new BadRequestException("Token and expiry date are required");

        await _serviceManager.TokenBlacklist.BlacklistTokenAsync(request.Token, request.Expiry, cancellationToken);

        return Ok(ApiResponseHelper.Success<object>(null, "Token has been blacklisted successfully"));
    }

    /// <summary>
    /// Removes expired tokens from blacklist (cleanup operation).
    /// </summary>
    [HttpPost(RouteConstants.RemoveExpiredTokens)]
    public async Task<IActionResult> RemoveExpiredTokensAsync(CancellationToken cancellationToken = default)
    {
        await _serviceManager.TokenBlacklist.RemoveExpiredTokensAsync(cancellationToken);

        return Ok(ApiResponseHelper.Success<object>(null, "Expired tokens removed successfully"));
    }
}

/// <summary>
/// Request model for manual token blacklisting.
/// </summary>
public class BlacklistTokenRequest
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiry { get; set; }
}
