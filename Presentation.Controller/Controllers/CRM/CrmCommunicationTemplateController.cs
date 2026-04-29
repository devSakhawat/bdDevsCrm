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
public class CrmCommunicationTemplateController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CrmCommunicationTemplateController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    [HttpGet(RouteConstants.CrmCommunicationTemplateDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> ForDDLAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmCommunicationTemplates.CommunicationTemplateForDDLAsync(cancellationToken);
        if (!records.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmCommunicationTemplateDDLDto>(), "No records found."));

        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

    [HttpPost(RouteConstants.CrmCommunicationTemplateSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summary = await _serviceManager.CrmCommunicationTemplates.CommunicationTemplateSummaryAsync(options, cancellationToken);
        if (!summary.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<CrmCommunicationTemplateDto>(), "No records found."));

        return Ok(ApiResponseHelper.Success(summary, "Records retrieved successfully"));
    }

    [HttpPost(RouteConstants.CreateCrmCommunicationTemplate)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmCommunicationTemplateRecord record, CancellationToken cancellationToken = default)
    {
        var created = await _serviceManager.CrmCommunicationTemplates.CreateAsync(record, cancellationToken);
        if (created.CommunicationTemplateId <= 0)
            throw new InvalidCreateOperationException("Failed to create record.");

        return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
    }

    [HttpPut(RouteConstants.UpdateCrmCommunicationTemplate)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmCommunicationTemplateRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.CommunicationTemplateId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmCommunicationTemplateRecord));

        var updated = await _serviceManager.CrmCommunicationTemplates.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmCommunicationTemplate)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteCrmCommunicationTemplateRecord(key);
        await _serviceManager.CrmCommunicationTemplates.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmCommunicationTemplate)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var record = await _serviceManager.CrmCommunicationTemplates.CommunicationTemplateAsync(id, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmCommunicationTemplates)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmCommunicationTemplates.CommunicationTemplatesAsync(trackChanges: false, cancellationToken: cancellationToken);
        if (!records.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmCommunicationTemplateDto>(), "No records found."));

        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }
}
