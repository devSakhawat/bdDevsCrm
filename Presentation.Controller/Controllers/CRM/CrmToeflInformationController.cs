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
/// CrmToeflInformation management endpoints.
/// </summary>
[AuthorizeUser]
public class CrmToeflInformationController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CrmToeflInformationController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves paginated summary grid.
    /// </summary>
    [HttpPost(RouteConstants.CrmToeflInformationSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        //var summaryGrid = await _serviceManager.TOEFLInformations.CrmToeflInformationsSummaryAsync(options, cancellationToken);
        var summaryGrid = await _serviceManager.TOEFLInformations.TOEFLInformationsSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<ToeflInformationDto>(), "No records found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateCrmToeflInformation)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmToeflInformationRecord record, CancellationToken cancellationToken = default)
    {
        var dto = record.MapTo<ToeflInformationDto>();
        var currentUser = await GetCurrentUserAsync();

        var created = await _serviceManager.TOEFLInformations.CreateTOEFLInformationAsync(dto, currentUser, cancellationToken);

        if (created.TOEFLInformationId <= 0)
            throw new InvalidCreateOperationException("Failed to create record.");

        return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
    }

    /// <summary>
    /// Updates an existing record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateCrmToeflInformation)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmToeflInformationRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.TOEFLInformationId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmToeflInformationRecord));

        var dto = record.MapTo<ToeflInformationDto>();
        var updated = await _serviceManager.TOEFLInformations.UpdateTOEFLInformationAsync(key, dto, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
    }

    /// <summary>
    /// Deletes a record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteCrmToeflInformation)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteCrmToeflInformationRecord(key);
        var dto = new ToeflInformationDto { TOEFLInformationId = key };
        await _serviceManager.TOEFLInformations.DeleteTOEFLInformationAsync(key, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
    }

    /// <summary>
    /// Retrieves a record by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadCrmToeflInformation)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var record = await _serviceManager.TOEFLInformations.TOEFLInformationAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all records.
    /// </summary>
    [HttpGet(RouteConstants.ReadCrmToeflInformations)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.TOEFLInformations.TOEFLInformationsAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!records.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<ToeflInformationDto>(), "No records found."));

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
