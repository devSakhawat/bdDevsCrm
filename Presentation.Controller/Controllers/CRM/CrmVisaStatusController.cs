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
public class CrmVisaStatusController : BaseApiController
{
  private readonly IMemoryCache _cache;

  public CrmVisaStatusController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
  {
    _cache = cache;
  }

  [HttpGet(RouteConstants.CrmVisaStatusDDL)]
  [ResponseCache(Duration = 300)]
  public async Task<IActionResult> ForDDLAsync(CancellationToken cancellationToken = default)
  {
    var records = await _serviceManager.CrmVisaStatuses.VisaStatusForDDLAsync(cancellationToken);
    if (!records.Any())
      return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmVisaStatusDDLDto>(), "No records found."));

    return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
  }

  [HttpPost(RouteConstants.CrmVisaStatusSummary)]
  public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
  {
    if (options == null)
      throw new NullModelBadRequestException(nameof(GridOptions));

    var summary = await _serviceManager.CrmVisaStatuses.VisaStatusSummaryAsync(options, cancellationToken);
    if (!summary.Items.Any())
      return Ok(ApiResponseHelper.Success(new GridEntity<CrmVisaStatusDto>(), "No records found."));

    return Ok(ApiResponseHelper.Success(summary, "Records retrieved successfully"));
  }

  [HttpPost(RouteConstants.CreateCrmVisaStatus)]
  [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
  public async Task<IActionResult> CreateAsync([FromBody] CreateCrmVisaStatusRecord record, CancellationToken cancellationToken = default)
  {
    var created = await _serviceManager.CrmVisaStatuses.CreateAsync(record, cancellationToken);
    if (created.VisaStatusId <= 0)
      throw new InvalidCreateOperationException("Failed to create record.");

    return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
  }

  [HttpPut(RouteConstants.UpdateCrmVisaStatus)]
  [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
  public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmVisaStatusRecord record, CancellationToken cancellationToken = default)
  {
    if (key != record.VisaStatusId)
      throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmVisaStatusRecord));

    var updated = await _serviceManager.CrmVisaStatuses.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);
    return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
  }

  [HttpDelete(RouteConstants.DeleteCrmVisaStatus)]
  public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
  {
    var deleteRecord = new DeleteCrmVisaStatusRecord(key);
    await _serviceManager.CrmVisaStatuses.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
    return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
  }

  [HttpGet(RouteConstants.ReadCrmVisaStatus)]
  public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
  {
    if (id <= 0)
      throw new IdParametersBadRequestException();

    var record = await _serviceManager.CrmVisaStatuses.VisaStatusAsync(id, trackChanges: false, cancellationToken: cancellationToken);
    return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
  }

  [HttpGet(RouteConstants.ReadCrmVisaStatuses)]
  public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
  {
    var records = await _serviceManager.CrmVisaStatuses.VisaStatusesAsync(trackChanges: false, cancellationToken: cancellationToken);
    if (!records.Any())
      return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmVisaStatusDto>(), "No records found."));

    return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
  }
}
