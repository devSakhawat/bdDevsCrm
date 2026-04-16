using Presentation.AuthorizeAttributes;
using Domain.Contracts.Services;
using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Domain.Exceptions;
using bdDevs.Shared.Constants;
using Application.Shared.Grid;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.Core.SystemAdmin;

/// <summary>
/// Password History management endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AuthorizeUser]
public class PasswordHistoryController : BaseApiController
{
    public PasswordHistoryController(IServiceManager serviceManager) : base(serviceManager)
    {
    }

    /// <summary>
    /// Retrieves all password histories for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.PasswordHistoryDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> PasswordHistoriesForDDLAsync(CancellationToken cancellationToken = default)
    {
        var passwordHistories = await _serviceManager.PasswordHistories.PasswordHistoriesForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!passwordHistories.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<PasswordHistoryDDLDto>(), "No password histories found."));

        return Ok(ApiResponseHelper.Success(passwordHistories, "Password histories retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of password histories.
    /// </summary>
    [HttpPost(RouteConstants.PasswordHistorySummary)]
    public async Task<IActionResult> PasswordHistorySummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.PasswordHistories.PasswordHistoriesSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<PasswordHistoryDto>(), "No password histories found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Password history summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new password history record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreatePasswordHistory)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreatePasswordHistoryAsync([FromBody] CreatePasswordHistoryRecord record, CancellationToken cancellationToken = default)
    {
        var createdPasswordHistory = await _serviceManager.PasswordHistories.CreateAsync(record, cancellationToken);

        if (createdPasswordHistory.HistoryId <= 0)
            throw new InvalidCreateOperationException("Failed to create password history record.");

        return Ok(ApiResponseHelper.Created(createdPasswordHistory, "Password history created successfully."));
    }

    /// <summary>
    /// Updates an existing password history record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdatePasswordHistory)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdatePasswordHistoryAsync([FromRoute] int key, [FromBody] UpdatePasswordHistoryRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.HistoryId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdatePasswordHistoryRecord));

        var updatedPasswordHistory = await _serviceManager.PasswordHistories.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedPasswordHistory, "Password history updated successfully."));
    }

    /// <summary>
    /// Deletes a password history record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeletePasswordHistory)]
    public async Task<IActionResult> DeletePasswordHistoryAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeletePasswordHistoryRecord(key);
        await _serviceManager.PasswordHistories.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Password history deleted successfully"));
    }

    /// <summary>
    /// Retrieves a password history by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadPasswordHistory)]
    public async Task<IActionResult> PasswordHistoryAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var passwordHistory = await _serviceManager.PasswordHistories.PasswordHistoryAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(passwordHistory, "Password history retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all password histories.
    /// </summary>
    [HttpGet(RouteConstants.ReadPasswordHistories)]
    public async Task<IActionResult> PasswordHistoriesAsync(CancellationToken cancellationToken = default)
    {
        var passwordHistories = await _serviceManager.PasswordHistories.PasswordHistoriesAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!passwordHistories.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<PasswordHistoryDto>(), "No password histories found."));

        return Ok(ApiResponseHelper.Success(passwordHistories, "Password histories retrieved successfully"));
    }
}
