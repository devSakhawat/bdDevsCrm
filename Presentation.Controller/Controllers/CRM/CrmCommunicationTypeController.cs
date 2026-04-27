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
public class CrmCommunicationTypeController : BaseApiController
{
  private readonly IMemoryCache _cache;

  public CrmCommunicationTypeController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
  {
    _cache = cache;
  }

  [HttpGet(RouteConstants.CrmCommunicationTypeDDL)]
  [ResponseCache(Duration = 300)]
  public async Task<IActionResult> ForDDLAsync(CancellationToken cancellationToken = default)
  {
    var records = await _serviceManager.CrmCommunicationTypes.CommunicationTypeForDDLAsync(cancellationToken);
    if (!records.Any())
      return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmCommunicationTypeDDLDto>(), "No records found."));

    return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
  }

  [HttpPost(RouteConstants.CrmCommunicationTypeSummary)]
  public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
  {
    if (options == null)
      throw new NullModelBadRequestException(nameof(GridOptions));

    var summary = await _serviceManager.CrmCommunicationTypes.CommunicationTypeSummaryAsync(options, cancellationToken);
    if (!summary.Items.Any())
      return Ok(ApiResponseHelper.Success(new GridEntity<CrmCommunicationTypeDto>(), "No records found."));

    return Ok(ApiResponseHelper.Success(summary, "Records retrieved successfully"));
  }

  [HttpPost(RouteConstants.CreateCrmCommunicationType)]
  [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
  public async Task<IActionResult> CreateAsync([FromBody] CreateCrmCommunicationTypeRecord record, CancellationToken cancellationToken = default)
  {
    var created = await _serviceManager.CrmCommunicationTypes.CreateAsync(record, cancellationToken);
    if (created.CommunicationTypeId <= 0)
      throw new InvalidCreateOperationException("Failed to create record.");

    return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
  }

  [HttpPut(RouteConstants.UpdateCrmCommunicationType)]
  [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
  public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmCommunicationTypeRecord record, CancellationToken cancellationToken = default)
  {
    if (key != record.CommunicationTypeId)
      throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmCommunicationTypeRecord));

    var updated = await _serviceManager.CrmCommunicationTypes.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);
    return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
  }

  [HttpDelete(RouteConstants.DeleteCrmCommunicationType)]
  public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
  {
    var deleteRecord = new DeleteCrmCommunicationTypeRecord(key);
    await _serviceManager.CrmCommunicationTypes.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
    return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
  }

  [HttpGet(RouteConstants.ReadCrmCommunicationType)]
  public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
  {
    if (id <= 0)
      throw new IdParametersBadRequestException();

    var record = await _serviceManager.CrmCommunicationTypes.CommunicationTypeAsync(id, trackChanges: false, cancellationToken: cancellationToken);
    return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
  }

  [HttpGet(RouteConstants.ReadCrmCommunicationTypes)]
  public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
  {
    var records = await _serviceManager.CrmCommunicationTypes.CommunicationTypesAsync(trackChanges: false, cancellationToken: cancellationToken);
    if (!records.Any())
      return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmCommunicationTypeDto>(), "No records found."));

    return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
  }
}
