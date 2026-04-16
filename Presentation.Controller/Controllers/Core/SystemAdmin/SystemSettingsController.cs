using Presentation.AuthorizeAttributes;
using Domain.Contracts.Services;
using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Domain.Exceptions;
using bdDevs.Shared.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Presentation.Controllers.Core.SystemAdmin;

/// <summary>
/// System Settings management endpoints.
/// </summary>
[AuthorizeUser]
public class SystemSettingsController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public SystemSettingsController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves system settings by company ID.
    /// </summary>
    [HttpGet(RouteConstants.SystemSettingsByCompanyId)]
    public async Task<IActionResult> SystemSettingsByCompanyIdAsync([FromRoute] int companyId, CancellationToken cancellationToken = default)
    {
        if (companyId <= 0)
            throw new IdParametersBadRequestException();

        var systemSettings = await _serviceManager.SystemSettings.SystemSettingsByCompanyIdAsync(companyId, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(systemSettings, "System settings retrieved successfully"));
    }

    /// <summary>
    /// Retrieves assembly information.
    /// </summary>
    [HttpGet(RouteConstants.AssemblyInfo)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> AssemblyInfoAsync(CancellationToken cancellationToken = default)
    {
        var assemblyInfo = await _serviceManager.SystemSettings.AssemblyInfoAsync(trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(assemblyInfo, "Assembly info retrieved successfully"));
    }

    /// <summary>
    /// Updates system settings.
    /// </summary>
    [HttpPut(RouteConstants.UpdateSystemSettings)]
    public async Task<IActionResult> UpdateSystemSettingsAsync([FromBody] SystemSettingsDto modelDto, CancellationToken cancellationToken = default)
    {
        if (modelDto == null)
            throw new NullModelBadRequestException(nameof(SystemSettingsDto));

        var updatedSettings = await _serviceManager.SystemSettings.UpdateSystemSettingsAsync(modelDto, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedSettings, "System settings updated successfully"));
    }

    /// <summary>
    /// Retrieves all system settings.
    /// </summary>
    [HttpGet(RouteConstants.ReadSystemSettings)]
    public async Task<IActionResult> SystemSettingsAsync(CancellationToken cancellationToken = default)
    {
        var systemSettings = await _serviceManager.SystemSettings.SystemSettingsAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!systemSettings.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<SystemSettingsDto>(), "No system settings found."));

        return Ok(ApiResponseHelper.Success(systemSettings, "System settings retrieved successfully"));
    }
}
