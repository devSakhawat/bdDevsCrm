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
public class CrmCommissionController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CrmCommissionController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager) => _cache = cache;

    [HttpPost(RouteConstants.CrmCommissionSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        return Ok(ApiResponseHelper.Success(await _serviceManager.CrmCommissions.CommissionsSummaryAsync(options, cancellationToken), "Commissions retrieved successfully."));
    }

    [HttpPost(RouteConstants.CreateCrmCommission)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmCommissionRecord record, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Created(await _serviceManager.CrmCommissions.CreateAsync(record, cancellationToken), "Commission created successfully."));

    [HttpPut(RouteConstants.UpdateCrmCommission)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmCommissionRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.CommissionId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmCommissionRecord));
        return Ok(ApiResponseHelper.Updated(await _serviceManager.CrmCommissions.UpdateAsync(record, false, cancellationToken), "Commission updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmCommission)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.CrmCommissions.DeleteAsync(new DeleteCrmCommissionRecord(key), false, cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Commission deleted successfully."));
    }

    [HttpGet(RouteConstants.ReadCrmCommission)]
    public async Task<IActionResult> GetAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmCommissions.CommissionAsync(id, false, cancellationToken), "Commission retrieved successfully."));

    [HttpGet(RouteConstants.ReadCrmCommissions)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmCommissions.CommissionsAsync(false, cancellationToken), "Commissions retrieved successfully."));

    [HttpGet(RouteConstants.CrmCommissionsByApplicationId)]
    public async Task<IActionResult> ByApplicationAsync([FromRoute] int applicationId, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmCommissions.CommissionsByApplicationIdAsync(applicationId, false, cancellationToken), "Application commissions retrieved successfully."));

    [HttpGet(RouteConstants.CrmCommissionsByAgentId)]
    public async Task<IActionResult> ByAgentAsync([FromRoute] int agentId, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmCommissions.CommissionsByAgentIdAsync(agentId, false, cancellationToken), "Agent commissions retrieved successfully."));

    [HttpPost(RouteConstants.CrmCommissionStatusTransition)]
    public async Task<IActionResult> ChangeStatusAsync([FromBody] ChangeCrmCommissionStatusRecord record, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Updated(await _serviceManager.CrmCommissions.ChangeStatusAsync(record, cancellationToken), "Commission status updated successfully."));

    [HttpGet(RouteConstants.CrmCommissionInvoice)]
    public async Task<IActionResult> InvoiceAsync([FromRoute] int commissionId, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmCommissions.GenerateInvoiceAsync(commissionId, cancellationToken), "Commission invoice generated successfully."));

    [HttpGet(RouteConstants.CrmCommissionDashboard)]
    public async Task<IActionResult> DashboardAsync(CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmCommissions.DashboardAsync(cancellationToken), "Commission dashboard retrieved successfully."));

    [HttpGet(RouteConstants.CrmCommissionAgingReport)]
    public async Task<IActionResult> AgingReportAsync(CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmCommissions.AgingReportAsync(cancellationToken), "Commission aging report retrieved successfully."));

    [HttpGet(RouteConstants.CrmCommissionAgentSummary)]
    public async Task<IActionResult> AgentSummaryAsync(CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmCommissions.AgentSummaryAsync(cancellationToken), "Commission agent summary retrieved successfully."));
}
