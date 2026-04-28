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
public class CrmBranchTargetController : BaseApiController
{
    public CrmBranchTargetController(IServiceManager serviceManager) : base(serviceManager) { }

    [HttpPost(RouteConstants.CrmBranchTargetSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        var summary = await _serviceManager.CrmBranchTargets.BranchTargetsSummaryAsync(options, cancellationToken);
        return Ok(ApiResponseHelper.Success(summary, "Summary retrieved successfully"));
    }

    [HttpPost(RouteConstants.CreateCrmBranchTarget)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmBranchTargetRecord record, CancellationToken cancellationToken = default)
    {
        var created = await _serviceManager.CrmBranchTargets.CreateAsync(record, cancellationToken);
        if (created.BranchTargetId <= 0) throw new InvalidCreateOperationException("Failed to create record.");
        return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
    }

    [HttpPut(RouteConstants.UpdateCrmBranchTarget)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmBranchTargetRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.BranchTargetId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmBranchTargetRecord));
        var updated = await _serviceManager.CrmBranchTargets.UpdateAsync(record, false, cancellationToken);
        return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmBranchTarget)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.CrmBranchTargets.DeleteAsync(new DeleteCrmBranchTargetRecord(key), false, cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmBranchTarget)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0) throw new IdParametersBadRequestException();
        var record = await _serviceManager.CrmBranchTargets.BranchTargetAsync(id, false, cancellationToken);
        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmBranchTargets)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmBranchTargets.BranchTargetsAsync(false, cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

    [HttpGet(RouteConstants.CrmBranchTargetsByBranchId)]
    public async Task<IActionResult> GetByBranchIdAsync([FromRoute] int branchId, CancellationToken cancellationToken = default)
    {
        if (branchId <= 0) throw new IdParametersBadRequestException();
        var records = await _serviceManager.CrmBranchTargets.BranchTargetsByBranchIdAsync(branchId, false, cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }
}
