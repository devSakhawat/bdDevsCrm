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
/// CrmEducationHistory management endpoints.
/// </summary>
[AuthorizeUser]
public class CrmEducationHistoryController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CrmEducationHistoryController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves paginated summary grid.
    /// </summary>
    [HttpPost(RouteConstants.CrmEducationHistorySummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        //var summaryGrid = await _serviceManager.EducationHistories.CrmEducationHistorysSummaryAsync(options, cancellationToken);
        var summaryGrid = await _serviceManager.EducationHistories.EducationHistoriesSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<EducationHistoryDto>(), "No records found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateCrmEducationHistory)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmEducationHistoryRecord record, CancellationToken cancellationToken = default)
    {
        var dto = record.MapTo<EducationHistoryDto>();
        var currentUser = await GetCurrentUserAsync();

        var created = await _serviceManager.EducationHistories.CreateEducationHistoryAsync(dto, currentUser, cancellationToken);

        if (created.EducationHistoryId <= 0)
            throw new InvalidCreateOperationException("Failed to create record.");

        return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
    }

    /// <summary>
    /// Updates an existing record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateCrmEducationHistory)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmEducationHistoryRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.EducationHistoryId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmEducationHistoryRecord));

        var dto = record.MapTo<EducationHistoryDto>();
        var updated = await _serviceManager.EducationHistories.UpdateEducationHistoryAsync(key, dto, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
    }

    /// <summary>
    /// Deletes a record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteCrmEducationHistory)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteCrmEducationHistoryRecord(key);
        await _serviceManager.EducationHistories.DeleteEducationHistoryAsync(key, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
    }

    /// <summary>
    /// Retrieves a record by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadCrmEducationHistory)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var record = await _serviceManager.EducationHistories.EducationHistoryAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all records.
    /// </summary>
    [HttpGet(RouteConstants.ReadCrmEducationHistories)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.EducationHistories.EducationHistoriesAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!records.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<EducationHistoryDto>(), "No records found."));

        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
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
