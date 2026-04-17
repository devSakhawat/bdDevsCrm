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
/// Board/Institute management endpoints.
/// </summary>
[AuthorizeUser]
public class BoardInstituteController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public BoardInstituteController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves all board/institutes for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.BoardInstituteDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> BoardInstitutesForDDLAsync(CancellationToken cancellationToken = default)
    {
        var boardInstitutes = await _serviceManager.BoardInstitutes.BoardInstitutesForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!boardInstitutes.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<BoardInstituteDDLDto>(), "No board/institutes found."));

        return Ok(ApiResponseHelper.Success(boardInstitutes, "Board/institutes retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of board/institutes.
    /// </summary>
    [HttpPost(RouteConstants.BoardInstituteSummary)]
    public async Task<IActionResult> BoardInstituteSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.BoardInstitutes.BoardInstitutesSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<BoardInstituteDto>(), "No board/institutes found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Board/institute summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new board/institute record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateBoardInstitute)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateBoardInstituteAsync([FromBody] CreateBoardInstituteRecord record, CancellationToken cancellationToken = default)
    {
        var createdBoardInstitute = await _serviceManager.BoardInstitutes.CreateAsync(record, cancellationToken);

        if (createdBoardInstitute.Id <= 0)
            throw new InvalidCreateOperationException("Failed to create board/institute record.");

        return Ok(ApiResponseHelper.Created(createdBoardInstitute, "Board/institute created successfully."));
    }

    /// <summary>
    /// Updates an existing board/institute record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateBoardInstitute)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateBoardInstituteAsync([FromRoute] int key, [FromBody] UpdateBoardInstituteRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.BoardInstituteId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateBoardInstituteRecord));

        var updatedBoardInstitute = await _serviceManager.BoardInstitutes.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedBoardInstitute, "Board/institute updated successfully."));
    }

    /// <summary>
    /// Deletes a board/institute record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteBoardInstitute)]
    public async Task<IActionResult> DeleteBoardInstituteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteBoardInstituteRecord(key);
        await _serviceManager.BoardInstitutes.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Board/institute deleted successfully"));
    }

    /// <summary>
    /// Retrieves a board/institute by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadBoardInstitute)]
    public async Task<IActionResult> BoardInstituteAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var boardInstitute = await _serviceManager.BoardInstitutes.BoardInstituteAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(boardInstitute, "Board/institute retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all board/institutes.
    /// </summary>
    [HttpGet(RouteConstants.ReadBoardInstitutes)]
    public async Task<IActionResult> BoardInstitutesAsync(CancellationToken cancellationToken = default)
    {
        var boardInstitutes = await _serviceManager.BoardInstitutes.BoardInstitutesAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!boardInstitutes.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<BoardInstituteDto>(), "No board/institutes found."));

        return Ok(ApiResponseHelper.Success(boardInstitutes, "Board/institutes retrieved successfully"));
    }
}
