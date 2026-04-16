using Presentation.AuthorizeAttributes;
using Domain.Contracts.Services;
using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;
using Domain.Exceptions;
using bdDevs.Shared.Constants;
using Application.Shared.Grid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Presentation.ActionFilters;
using bdDevs.Shared.Extensions;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

namespace Presentation.Controllers.CRM;

/// <summary>
/// CRM Institute management endpoints.
/// </summary>
[AuthorizeUser]
public class CrmInstituteController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CrmInstituteController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves all institutes for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.CrmInstituteDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> InstitutesForDDLAsync(CancellationToken cancellationToken = default)
    {
        var institutes = await _serviceManager.CrmInstitutes.InstituteForDDLAsync(cancellationToken: cancellationToken);

        if (!institutes.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmInstituteDDLDto>(), "No institutes found."));

        return Ok(ApiResponseHelper.Success(institutes, "Institutes retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of institutes.
    /// </summary>
    [HttpPost(RouteConstants.CrmInstituteSummary)]
    public async Task<IActionResult> InstituteSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.CrmInstitutes.InstitutesSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<CrmInstituteDto>(), "No institutes found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Institute summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new institute record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateCrmInstitute)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateInstituteAsync([FromBody] CreateCrmInstituteRecord record, CancellationToken cancellationToken = default)
    {
        var dto = record.MapTo<CrmInstituteDto>();
        var currentUser = await GetCurrentUserAsync();

        var createdInstitute = await _serviceManager.CrmInstitutes.CreateInstituteAsync(dto, currentUser, cancellationToken);

        if (createdInstitute.InstituteId <= 0)
            throw new InvalidCreateOperationException("Failed to create institute record.");

        return Ok(ApiResponseHelper.Created(createdInstitute, "Institute created successfully."));
    }

    /// <summary>
    /// Updates an existing institute record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateCrmInstitute)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateInstituteAsync([FromRoute] int key, [FromBody] UpdateCrmInstituteRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.InstituteId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmInstituteRecord));

        var dto = record.MapTo<CrmInstituteDto>();
        var updatedInstitute = await _serviceManager.CrmInstitutes.UpdateInstituteAsync(key, dto, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedInstitute, "Institute updated successfully."));
    }

    /// <summary>
    /// Deletes an institute record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteCrmInstitute)]
    public async Task<IActionResult> DeleteInstituteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteCrmInstituteRecord(key);
        await _serviceManager.CrmInstitutes.DeleteInstituteAsync(key, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Institute deleted successfully"));
    }

    /// <summary>
    /// Retrieves an institute by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadCrmInstitute)]
    public async Task<IActionResult> InstituteAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var institute = await _serviceManager.CrmInstitutes.InstituteAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(institute, "Institute retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all institutes.
    /// </summary>
    [HttpGet(RouteConstants.ReadCrmInstitutes)]
    public async Task<IActionResult> InstitutesAsync(CancellationToken cancellationToken = default)
    {
        var institutes = await _serviceManager.CrmInstitutes.InstitutesAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!institutes.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmInstituteDto>(), "No institutes found."));

        return Ok(ApiResponseHelper.Success(institutes, "Institutes retrieved successfully"));
    }

    /// <summary>
    /// Retrieves institutes by country ID.
    /// </summary>
    [HttpGet(RouteConstants.CrmInstitutesByCountryId)]
    public async Task<IActionResult> InstitutesByCountryIdAsync([FromRoute] int countryId, CancellationToken cancellationToken = default)
    {
        if (countryId <= 0)
            throw new IdParametersBadRequestException();

        var institutes = await _serviceManager.CrmInstitutes.InstitutesByCountryIdAsync(countryId, trackChanges: false, cancellationToken: cancellationToken);

        if (!institutes.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmInstituteDDLDto>(), "No institutes found for this country."));

        return Ok(ApiResponseHelper.Success(institutes, "Institutes retrieved successfully"));
    }

    private async Task<UsersDto> GetCurrentUserAsync()
    {
        var userId = User?.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int parsedUserId))
        {
            return new UsersDto { UserId = 1, Username = "system" };
        }
        return new UsersDto { UserId = parsedUserId };
    }
}
