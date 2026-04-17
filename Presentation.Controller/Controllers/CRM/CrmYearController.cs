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
/// CrmYear management endpoints.
/// </summary>
[AuthorizeUser]
public class CrmYearController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CrmYearController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves paginated summary grid.
    /// </summary>
    [HttpPost(RouteConstants.CrmYearSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.CrmYears.YearsSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<CrmYearDto>(), "No records found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateCrmYear)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmYearRecord record, CancellationToken cancellationToken = default)
    {
        var created = await _serviceManager.CrmYears.CreateAsync(record, cancellationToken);

        if (created.YearId <= 0)
            throw new InvalidCreateOperationException("Failed to create record.");

        return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
    }

    /// <summary>
    /// Updates an existing record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateCrmYear)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmYearRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.YearId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmYearRecord));

        var updated = await _serviceManager.CrmYears.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
    }

    /// <summary>
    /// Deletes a record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteCrmYear)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteCrmYearRecord(key);
        await _serviceManager.CrmYears.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
    }

    /// <summary>
    /// Retrieves a record by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadCrmYear)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var record = await _serviceManager.CrmYears.YearAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all records.
    /// </summary>
    [HttpGet(RouteConstants.ReadCrmYears)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmYears.YearsAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!records.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmYearDto>(), "No records found."));

        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

}
