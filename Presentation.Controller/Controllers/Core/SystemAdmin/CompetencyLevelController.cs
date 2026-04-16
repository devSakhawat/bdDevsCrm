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
/// Competency Level management endpoints.
/// </summary>
[AuthorizeUser]
public class CompetencyLevelController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CompetencyLevelController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves all competency levels for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.CompetencyLevelDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> CompetencyLevelsForDDLAsync(CancellationToken cancellationToken = default)
    {
        var competencyLevels = await _serviceManager.CompetencyLevels.CompetencyLevelsForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!competencyLevels.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CompetencyLevelDDLDto>(), "No competency levels found."));

        return Ok(ApiResponseHelper.Success(competencyLevels, "Competency levels retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of competency levels.
    /// </summary>
    [HttpPost(RouteConstants.CompetencyLevelSummary)]
    public async Task<IActionResult> CompetencyLevelSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.CompetencyLevels.CompetencyLevelsSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<CompetencyLevelDto>(), "No competency levels found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Competency level summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new competency level record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateCompetencyLevel)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateCompetencyLevelAsync([FromBody] CreateCompetencyLevelRecord record, CancellationToken cancellationToken = default)
    {
        var createdCompetencyLevel = await _serviceManager.CompetencyLevels.CreateAsync(record, cancellationToken);

        if (createdCompetencyLevel.LevelId <= 0)
            throw new InvalidCreateOperationException("Failed to create competency level record.");

        return Ok(ApiResponseHelper.Created(createdCompetencyLevel, "Competency level created successfully."));
    }

    /// <summary>
    /// Updates an existing competency level record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateCompetencyLevel)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateCompetencyLevelAsync([FromRoute] int key, [FromBody] UpdateCompetencyLevelRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.LevelId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCompetencyLevelRecord));

        var updatedCompetencyLevel = await _serviceManager.CompetencyLevels.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedCompetencyLevel, "Competency level updated successfully."));
    }

    /// <summary>
    /// Deletes a competency level record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteCompetencyLevel)]
    public async Task<IActionResult> DeleteCompetencyLevelAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteCompetencyLevelRecord(key);
        await _serviceManager.CompetencyLevels.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Competency level deleted successfully"));
    }

    /// <summary>
    /// Retrieves a competency level by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadCompetencyLevel)]
    public async Task<IActionResult> CompetencyLevelAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var competencyLevel = await _serviceManager.CompetencyLevels.CompetencyLevelAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(competencyLevel, "Competency level retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all competency levels.
    /// </summary>
    [HttpGet(RouteConstants.ReadCompetencyLevels)]
    public async Task<IActionResult> CompetencyLevelsAsync(CancellationToken cancellationToken = default)
    {
        var competencyLevels = await _serviceManager.CompetencyLevels.CompetencyLevelsAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!competencyLevels.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CompetencyLevelDto>(), "No competency levels found."));

        return Ok(ApiResponseHelper.Success(competencyLevels, "Competency levels retrieved successfully"));
    }
}
