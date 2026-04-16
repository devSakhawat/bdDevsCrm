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
/// Timesheet management endpoints.
/// </summary>
[AuthorizeUser]
public class TimesheetController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public TimesheetController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves all timesheets for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.TimesheetDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> TimesheetsForDDLAsync(CancellationToken cancellationToken = default)
    {
        var timesheets = await _serviceManager.Timesheets.TimesheetsForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!timesheets.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<TimesheetDDLDto>(), "No timesheets found."));

        return Ok(ApiResponseHelper.Success(timesheets, "Timesheets retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of timesheets.
    /// </summary>
    [HttpPost(RouteConstants.TimesheetSummary)]
    public async Task<IActionResult> TimesheetSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.Timesheets.TimesheetsSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<TimesheetDto>(), "No timesheets found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Timesheet summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new timesheet record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateTimesheet)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateTimesheetAsync([FromBody] CreateTimesheetRecord record, CancellationToken cancellationToken = default)
    {
        var createdTimesheet = await _serviceManager.Timesheets.CreateAsync(record, cancellationToken);

        if (createdTimesheet.Timesheetid <= 0)
            throw new InvalidCreateOperationException("Failed to create timesheet record.");

        return Ok(ApiResponseHelper.Created(createdTimesheet, "Timesheet created successfully."));
    }

    /// <summary>
    /// Updates an existing timesheet record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateTimesheet)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateTimesheetAsync([FromRoute] int key, [FromBody] UpdateTimesheetRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.Timesheetid)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateTimesheetRecord));

        var updatedTimesheet = await _serviceManager.Timesheets.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedTimesheet, "Timesheet updated successfully."));
    }

    /// <summary>
    /// Deletes a timesheet record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteTimesheet)]
    public async Task<IActionResult> DeleteTimesheetAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteTimesheetRecord(key);
        await _serviceManager.Timesheets.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Timesheet deleted successfully"));
    }

    /// <summary>
    /// Retrieves a timesheet by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadTimesheet)]
    public async Task<IActionResult> TimesheetAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var timesheet = await _serviceManager.Timesheets.TimesheetAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(timesheet, "Timesheet retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all timesheets.
    /// </summary>
    [HttpGet(RouteConstants.ReadTimesheets)]
    public async Task<IActionResult> TimesheetsAsync(CancellationToken cancellationToken = default)
    {
        var timesheets = await _serviceManager.Timesheets.TimesheetsAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!timesheets.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<TimesheetDto>(), "No timesheets found."));

        return Ok(ApiResponseHelper.Success(timesheets, "Timesheets retrieved successfully"));
    }

    /// <summary>
    /// Retrieves timesheets by employee.
    /// </summary>
    [HttpGet(RouteConstants.TimesheetsByEmployee)]
    public async Task<IActionResult> TimesheetsByEmployeeAsync([FromRoute] int hrRecordId, CancellationToken cancellationToken = default)
    {
        if (hrRecordId <= 0)
            throw new IdParametersBadRequestException();

        var timesheets = await _serviceManager.Timesheets.TimesheetsByEmployeeAsync(hrRecordId, trackChanges: false, cancellationToken: cancellationToken);

        if (!timesheets.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<TimesheetDto>(), "No timesheets found for this employee."));

        return Ok(ApiResponseHelper.Success(timesheets, "Timesheets retrieved successfully"));
    }
}
