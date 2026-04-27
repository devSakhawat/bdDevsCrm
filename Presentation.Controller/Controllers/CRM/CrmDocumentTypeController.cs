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
public class CrmDocumentTypeController : BaseApiController
{
  private readonly IMemoryCache _cache;

  public CrmDocumentTypeController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
  {
    _cache = cache;
  }

  [HttpGet(RouteConstants.CrmDocumentTypeDDL)]
  [ResponseCache(Duration = 300)]
  public async Task<IActionResult> ForDDLAsync(CancellationToken cancellationToken = default)
  {
    var records = await _serviceManager.CrmDocumentTypes.DocumentTypeForDDLAsync(cancellationToken);
    if (!records.Any())
      return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmDocumentTypeDDLDto>(), "No records found."));

    return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
  }

  [HttpPost(RouteConstants.CrmDocumentTypeSummary)]
  public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
  {
    if (options == null)
      throw new NullModelBadRequestException(nameof(GridOptions));

    var summary = await _serviceManager.CrmDocumentTypes.DocumentTypeSummaryAsync(options, cancellationToken);
    if (!summary.Items.Any())
      return Ok(ApiResponseHelper.Success(new GridEntity<CrmDocumentTypeDto>(), "No records found."));

    return Ok(ApiResponseHelper.Success(summary, "Records retrieved successfully"));
  }

  [HttpPost(RouteConstants.CreateCrmDocumentType)]
  [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
  public async Task<IActionResult> CreateAsync([FromBody] CreateCrmDocumentTypeRecord record, CancellationToken cancellationToken = default)
  {
    var created = await _serviceManager.CrmDocumentTypes.CreateAsync(record, cancellationToken);
    if (created.DocumentTypeId <= 0)
      throw new InvalidCreateOperationException("Failed to create record.");

    return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
  }

  [HttpPut(RouteConstants.UpdateCrmDocumentType)]
  [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
  public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmDocumentTypeRecord record, CancellationToken cancellationToken = default)
  {
    if (key != record.DocumentTypeId)
      throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmDocumentTypeRecord));

    var updated = await _serviceManager.CrmDocumentTypes.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);
    return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
  }

  [HttpDelete(RouteConstants.DeleteCrmDocumentType)]
  public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
  {
    var deleteRecord = new DeleteCrmDocumentTypeRecord(key);
    await _serviceManager.CrmDocumentTypes.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
    return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
  }

  [HttpGet(RouteConstants.ReadCrmDocumentType)]
  public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
  {
    if (id <= 0)
      throw new IdParametersBadRequestException();

    var record = await _serviceManager.CrmDocumentTypes.DocumentTypeAsync(id, trackChanges: false, cancellationToken: cancellationToken);
    return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
  }

  [HttpGet(RouteConstants.ReadCrmDocumentTypes)]
  public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
  {
    var records = await _serviceManager.CrmDocumentTypes.DocumentTypesAsync(trackChanges: false, cancellationToken: cancellationToken);
    if (!records.Any())
      return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmDocumentTypeDto>(), "No records found."));

    return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
  }
}
