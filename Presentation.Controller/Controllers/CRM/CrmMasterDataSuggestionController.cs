using Presentation.AuthorizeAttributes;
using Domain.Contracts.Services;
using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;
using Domain.Exceptions;
using bdDevs.Shared.Constants;
using Application.Shared.Grid;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;

namespace Presentation.Controllers.CRM;

[AuthorizeUser]
public class CrmMasterDataSuggestionController : BaseApiController
{
    public CrmMasterDataSuggestionController(IServiceManager serviceManager) : base(serviceManager) { }

    [HttpPost(RouteConstants.CrmMasterDataSuggestionSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        var summary = await _serviceManager.CrmMasterDataSuggestions.MasterDataSuggestionsSummaryAsync(options, cancellationToken);
        return Ok(ApiResponseHelper.Success(summary, "Summary retrieved successfully"));
    }

    [HttpPost(RouteConstants.CreateCrmMasterDataSuggestion)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmMasterDataSuggestionRecord record, CancellationToken cancellationToken = default)
    {
        var created = await _serviceManager.CrmMasterDataSuggestions.CreateAsync(record, cancellationToken);
        if (created.SuggestionId <= 0) throw new InvalidCreateOperationException("Failed to create record.");
        return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
    }

    [HttpPut(RouteConstants.UpdateCrmMasterDataSuggestion)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmMasterDataSuggestionRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.SuggestionId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmMasterDataSuggestionRecord));
        var updated = await _serviceManager.CrmMasterDataSuggestions.UpdateAsync(record, false, cancellationToken);
        return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmMasterDataSuggestion)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.CrmMasterDataSuggestions.DeleteAsync(new DeleteCrmMasterDataSuggestionRecord(key), false, cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmMasterDataSuggestion)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0) throw new IdParametersBadRequestException();
        var record = await _serviceManager.CrmMasterDataSuggestions.MasterDataSuggestionAsync(id, false, cancellationToken);
        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmMasterDataSuggestions)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmMasterDataSuggestions.MasterDataSuggestionsAsync(false, cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }
}
