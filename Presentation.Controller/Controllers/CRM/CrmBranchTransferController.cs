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
public class CrmBranchTransferController : BaseApiController
{
    public CrmBranchTransferController(IServiceManager serviceManager) : base(serviceManager) { }

    [HttpPost(RouteConstants.CrmBranchTransferSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        var summary = await _serviceManager.CrmBranchTransfers.BranchTransfersSummaryAsync(options, cancellationToken);
        return Ok(ApiResponseHelper.Success(summary, "Summary retrieved successfully"));
    }

    [HttpPost(RouteConstants.CreateCrmBranchTransfer)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmBranchTransferRecord record, CancellationToken cancellationToken = default)
    {
        var created = await _serviceManager.CrmBranchTransfers.CreateAsync(record, cancellationToken);
        if (created.TransferId <= 0) throw new InvalidCreateOperationException("Failed to create record.");
        return Ok(ApiResponseHelper.Created(created, "Branch transfer request created successfully."));
    }

    [HttpPut(RouteConstants.UpdateCrmBranchTransfer)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmBranchTransferRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.TransferId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmBranchTransferRecord));
        var updated = await _serviceManager.CrmBranchTransfers.UpdateAsync(record, false, cancellationToken);
        return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmBranchTransfer)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.CrmBranchTransfers.DeleteAsync(new DeleteCrmBranchTransferRecord(key), false, cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmBranchTransfer)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0) throw new IdParametersBadRequestException();
        var record = await _serviceManager.CrmBranchTransfers.BranchTransferAsync(id, false, cancellationToken);
        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmBranchTransfers)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmBranchTransfers.BranchTransfersAsync(false, cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

    [HttpGet(RouteConstants.CrmBranchTransfersByEntity)]
    public async Task<IActionResult> GetByEntityAsync([FromRoute] byte entityType, [FromRoute] int entityId, CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmBranchTransfers.BranchTransfersByEntityAsync(entityType, entityId, false, cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

    [HttpPut(RouteConstants.ApproveCrmBranchTransfer)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> ApproveAsync([FromRoute] int key, [FromBody] ApproveCrmBranchTransferRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.TransferId) throw new IdMismatchBadRequestException(key.ToString(), nameof(ApproveCrmBranchTransferRecord));
        var approved = await _serviceManager.CrmBranchTransfers.ApproveAsync(record, false, cancellationToken);
        return Ok(ApiResponseHelper.Updated(approved, "Branch transfer approved successfully."));
    }
}
