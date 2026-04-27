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
public class CrmCountryDocumentRequirementController : BaseApiController
{
    public CrmCountryDocumentRequirementController(IServiceManager serviceManager) : base(serviceManager) { }

    [HttpPost(RouteConstants.CrmCountryDocReqSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        var summary = await _serviceManager.CrmCountryDocumentRequirements.CountryDocumentRequirementsSummaryAsync(options, cancellationToken);
        return Ok(ApiResponseHelper.Success(summary, "Summary retrieved successfully"));
    }

    [HttpPost(RouteConstants.CreateCrmCountryDocReq)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmCountryDocumentRequirementRecord record, CancellationToken cancellationToken = default)
    {
        var created = await _serviceManager.CrmCountryDocumentRequirements.CreateAsync(record, cancellationToken);
        if (created.RequirementId <= 0) throw new InvalidCreateOperationException("Failed to create record.");
        return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
    }

    [HttpPut(RouteConstants.UpdateCrmCountryDocReq)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmCountryDocumentRequirementRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.RequirementId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmCountryDocumentRequirementRecord));
        var updated = await _serviceManager.CrmCountryDocumentRequirements.UpdateAsync(record, false, cancellationToken);
        return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmCountryDocReq)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.CrmCountryDocumentRequirements.DeleteAsync(new DeleteCrmCountryDocumentRequirementRecord(key), false, cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmCountryDocReq)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0) throw new IdParametersBadRequestException();
        var record = await _serviceManager.CrmCountryDocumentRequirements.CountryDocumentRequirementAsync(id, false, cancellationToken);
        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmCountryDocReqs)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmCountryDocumentRequirements.CountryDocumentRequirementsAsync(false, cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

    [HttpGet(RouteConstants.CrmCountryDocReqsByCountryId)]
    public async Task<IActionResult> GetByCountryIdAsync([FromRoute] int countryId, CancellationToken cancellationToken = default)
    {
        if (countryId <= 0) throw new IdParametersBadRequestException();
        var records = await _serviceManager.CrmCountryDocumentRequirements.RequirementsByCountryIdAsync(countryId, false, cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }
}
