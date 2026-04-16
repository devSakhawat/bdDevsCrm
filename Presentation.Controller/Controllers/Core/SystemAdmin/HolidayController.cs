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
/// Holiday management endpoints.
/// </summary>
[AuthorizeUser]
public class HolidayController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public HolidayController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves all holidays for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.HolidayDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> HolidaysForDDLAsync(CancellationToken cancellationToken = default)
    {
        var holidays = await _serviceManager.Holidays.HolidaysForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!holidays.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<HolidayDDLDto>(), "No holidays found."));

        return Ok(ApiResponseHelper.Success(holidays, "Holidays retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of holidays.
    /// </summary>
    [HttpPost(RouteConstants.HolidaySummary)]
    public async Task<IActionResult> HolidaySummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.Holidays.HolidaysSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<HolidayDto>(), "No holidays found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Holiday summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new holiday record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateHoliday)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateHolidayAsync([FromBody] CreateHolidayRecord record, CancellationToken cancellationToken = default)
    {
        var createdHoliday = await _serviceManager.Holidays.CreateAsync(record, cancellationToken);

        if (createdHoliday.HolidayId <= 0)
            throw new InvalidCreateOperationException("Failed to create holiday record.");

        return Ok(ApiResponseHelper.Created(createdHoliday, "Holiday created successfully."));
    }

    /// <summary>
    /// Updates an existing holiday record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateHoliday)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateHolidayAsync([FromRoute] int key, [FromBody] UpdateHolidayRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.HolidayId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateHolidayRecord));

        var updatedHoliday = await _serviceManager.Holidays.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedHoliday, "Holiday updated successfully."));
    }

    /// <summary>
    /// Deletes a holiday record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteHoliday)]
    public async Task<IActionResult> DeleteHolidayAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteHolidayRecord(key);
        await _serviceManager.Holidays.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Holiday deleted successfully"));
    }

    /// <summary>
    /// Retrieves a holiday by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadHoliday)]
    public async Task<IActionResult> HolidayAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var holiday = await _serviceManager.Holidays.HolidayAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(holiday, "Holiday retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all holidays.
    /// </summary>
    [HttpGet(RouteConstants.ReadHolidays)]
    public async Task<IActionResult> HolidaysAsync(CancellationToken cancellationToken = default)
    {
        var holidays = await _serviceManager.Holidays.HolidaysAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!holidays.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<HolidayDto>(), "No holidays found."));

        return Ok(ApiResponseHelper.Success(holidays, "Holidays retrieved successfully"));
    }
}
