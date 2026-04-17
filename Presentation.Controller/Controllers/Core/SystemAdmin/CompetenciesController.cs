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
/// Competencies management endpoints.
/// </summary>
[AuthorizeUser]
public class CompetenciesController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CompetenciesController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves all competencies for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.CompetenciesDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> CompetenciesForDDLAsync(CancellationToken cancellationToken = default)
    {
        var competencies = await _serviceManager.Competencies.CompetenciesForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!competencies.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CompetenciesDDLDto>(), "No competencies found."));

        return Ok(ApiResponseHelper.Success(competencies, "Competencies retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of competencies.
    /// </summary>
    [HttpPost(RouteConstants.CompetenciesSummary)]
    public async Task<IActionResult> CompetenciesSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.Competencies.CompetenciesSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<CompetenciesDto>(), "No competencies found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Competencies summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new competency record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateCompetency)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateCompetencyAsync([FromBody] CreateCompetenciesRecord record, CancellationToken cancellationToken = default)
    {
        var createdCompetency = await _serviceManager.Competencies.CreateAsync(record, cancellationToken);

        if (createdCompetency.Id <= 0)
            throw new InvalidCreateOperationException("Failed to create competency record.");

        return Ok(ApiResponseHelper.Created(createdCompetency, "Competency created successfully."));
    }

    /// <summary>
    /// Updates an existing competency record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateCompetency)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateCompetencyAsync([FromRoute] int key, [FromBody] UpdateCompetenciesRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.Id)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCompetenciesRecord));

        var updatedCompetency = await _serviceManager.Competencies.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedCompetency, "Competency updated successfully."));
    }

    /// <summary>
    /// Deletes a competency record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteCompetency)]
    public async Task<IActionResult> DeleteCompetencyAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteCompetenciesRecord(key);
        await _serviceManager.Competencies.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Competency deleted successfully"));
    }

    /// <summary>
    /// Retrieves a competency by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadCompetency)]
    public async Task<IActionResult> CompetencyAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var competency = await _serviceManager.Competencies.CompetencyAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(competency, "Competency retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all competencies.
    /// </summary>
    [HttpGet(RouteConstants.ReadCompetencies)]
    public async Task<IActionResult> CompetenciesAsync(CancellationToken cancellationToken = default)
    {
        var competencies = await _serviceManager.Competencies.CompetenciesAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!competencies.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CompetenciesDto>(), "No competencies found."));

        return Ok(ApiResponseHelper.Success(competencies, "Competencies retrieved successfully"));
    }
}
