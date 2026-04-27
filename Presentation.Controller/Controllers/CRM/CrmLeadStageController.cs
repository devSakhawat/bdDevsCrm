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
public class CrmLeadStageController : BaseApiController
{
  private readonly IMemoryCache _cache;

  public CrmLeadStageController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
  {
    _cache = cache;
  }

  [HttpGet(RouteConstants.CrmLeadStageDDL)]
  [ResponseCache(Duration = 300)]
  public async Task<IActionResult> ForDDLAsync(CancellationToken cancellationToken = default)
  {
    var records = await _serviceManager.CrmLeadStages.LeadStageForDDLAsync(cancellationToken);
    if (!records.Any())
      return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmLeadStageDDLDto>(), "No records found."));

    return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
  }

  [HttpPost(RouteConstants.CrmLeadStageSummary)]
  public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
  {
    if (options == null)
      throw new NullModelBadRequestException(nameof(GridOptions));

    var summary = await _serviceManager.CrmLeadStages.LeadStageSummaryAsync(options, cancellationToken);
    if (!summary.Items.Any())
      return Ok(ApiResponseHelper.Success(new GridEntity<CrmLeadStageDto>(), "No records found."));

    return Ok(ApiResponseHelper.Success(summary, "Records retrieved successfully"));
  }

  [HttpPost(RouteConstants.CreateCrmLeadStage)]
  [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
  public async Task<IActionResult> CreateAsync([FromBody] CreateCrmLeadStageRecord record, CancellationToken cancellationToken = default)
  {
    var created = await _serviceManager.CrmLeadStages.CreateAsync(record, cancellationToken);
    if (created.LeadStageId <= 0)
      throw new InvalidCreateOperationException("Failed to create record.");

    return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
  }

  [HttpPut(RouteConstants.UpdateCrmLeadStage)]
  [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
  public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmLeadStageRecord record, CancellationToken cancellationToken = default)
  {
    if (key != record.LeadStageId)
      throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmLeadStageRecord));

    var updated = await _serviceManager.CrmLeadStages.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);
    return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
  }

  [HttpDelete(RouteConstants.DeleteCrmLeadStage)]
  public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
  {
    var deleteRecord = new DeleteCrmLeadStageRecord(key);
    await _serviceManager.CrmLeadStages.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
    return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
  }

  [HttpGet(RouteConstants.ReadCrmLeadStage)]
  public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
  {
    if (id <= 0)
      throw new IdParametersBadRequestException();

    var record = await _serviceManager.CrmLeadStages.LeadStageAsync(id, trackChanges: false, cancellationToken: cancellationToken);
    return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
  }

  [HttpGet(RouteConstants.ReadCrmLeadStages)]
  public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
  {
    var records = await _serviceManager.CrmLeadStages.LeadStagesAsync(trackChanges: false, cancellationToken: cancellationToken);
    if (!records.Any())
      return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmLeadStageDto>(), "No records found."));

    return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
  }
}
