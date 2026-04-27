using Application.Shared.Grid;
using bdDevs.Shared;
using bdDevs.Shared.Constants;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;
using Domain.Contracts.Services;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Presentation.ActionFilters;
using Presentation.AuthorizeAttributes;

namespace Presentation.Controllers.CRM;

[AuthorizeUser]
public class CrmApplicationStatusController : BaseApiController
{
  private readonly IMemoryCache _cache;

  public CrmApplicationStatusController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
  {
    _cache = cache;
  }

  [HttpGet(RouteConstants.CrmApplicationStatusDDL)]
  [ResponseCache(Duration = 300)]
  public async Task<IActionResult> ForDDLAsync(CancellationToken cancellationToken = default)
  {
    var records = await _serviceManager.CrmApplicationStatuses.ApplicationStatusForDDLAsync(cancellationToken);
    if (!records.Any())
      return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmApplicationStatusDDLDto>(), "No records found."));

    return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
  }

  [HttpPost(RouteConstants.CrmApplicationStatusSummary)]
  public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
  {
    if (options == null)
      throw new NullModelBadRequestException(nameof(GridOptions));

    var summary = await _serviceManager.CrmApplicationStatuses.ApplicationStatusSummaryAsync(options, cancellationToken);
    if (!summary.Items.Any())
      return Ok(ApiResponseHelper.Success(new GridEntity<CrmApplicationStatusDto>(), "No records found."));

    return Ok(ApiResponseHelper.Success(summary, "Records retrieved successfully"));
  }

  [HttpPost(RouteConstants.CreateCrmApplicationStatus)]
  [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
  public async Task<IActionResult> CreateAsync([FromBody] CreateCrmApplicationStatusRecord record, CancellationToken cancellationToken = default)
  {
    var created = await _serviceManager.CrmApplicationStatuses.CreateAsync(record, cancellationToken);
    if (created.ApplicationStatusId <= 0)
      throw new InvalidCreateOperationException("Failed to create record.");

    return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
  }

  [HttpPut(RouteConstants.UpdateCrmApplicationStatus)]
  [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
  public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmApplicationStatusRecord record, CancellationToken cancellationToken = default)
  {
    if (key != record.ApplicationStatusId)
      throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmApplicationStatusRecord));

    var updated = await _serviceManager.CrmApplicationStatuses.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);
    return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
  }

  [HttpDelete(RouteConstants.DeleteCrmApplicationStatus)]
  public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
  {
    var deleteRecord = new DeleteCrmApplicationStatusRecord(key);
    await _serviceManager.CrmApplicationStatuses.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
    return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
  }

  [HttpGet(RouteConstants.ReadCrmApplicationStatus)]
  public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
  {
    if (id <= 0)
      throw new IdParametersBadRequestException();

    var record = await _serviceManager.CrmApplicationStatuses.ApplicationStatusAsync(id, trackChanges: false, cancellationToken: cancellationToken);
    return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
  }

  [HttpGet(RouteConstants.ReadCrmApplicationStatuses)]
  public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
  {
    var records = await _serviceManager.CrmApplicationStatuses.ApplicationStatusesAsync(trackChanges: false, cancellationToken: cancellationToken);
    if (!records.Any())
      return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmApplicationStatusDto>(), "No records found."));

    return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
  }
}
