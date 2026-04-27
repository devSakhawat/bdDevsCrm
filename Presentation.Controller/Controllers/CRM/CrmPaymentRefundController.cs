using Application.Shared.Grid;
using bdDevs.Shared;
using bdDevs.Shared.Constants;
using bdDevs.Shared.Records.CRM;
using Domain.Contracts.Services;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Presentation.ActionFilters;
using Presentation.AuthorizeAttributes;

namespace Presentation.Controllers.CRM;

[AuthorizeUser]
public class CrmPaymentRefundController : BaseApiController
{
    private readonly IMemoryCache _cache;
    public CrmPaymentRefundController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager) => _cache = cache;

    [HttpPost(RouteConstants.CrmPaymentRefundSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        return Ok(ApiResponseHelper.Success(await _serviceManager.CrmPaymentRefunds.PaymentRefundsSummaryAsync(options, cancellationToken), "Payment refunds retrieved successfully."));
    }

    [HttpPost(RouteConstants.CreateCrmPaymentRefund)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmPaymentRefundRecord record, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Created(await _serviceManager.CrmPaymentRefunds.CreateAsync(record, cancellationToken), "Payment refund created successfully."));

    [HttpPut(RouteConstants.UpdateCrmPaymentRefund)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmPaymentRefundRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.PaymentRefundId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmPaymentRefundRecord));
        return Ok(ApiResponseHelper.Updated(await _serviceManager.CrmPaymentRefunds.UpdateAsync(record, false, cancellationToken), "Payment refund updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmPaymentRefund)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.CrmPaymentRefunds.DeleteAsync(new DeleteCrmPaymentRefundRecord(key), false, cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Payment refund deleted successfully."));
    }

    [HttpGet(RouteConstants.ReadCrmPaymentRefund)]
    public async Task<IActionResult> GetAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmPaymentRefunds.PaymentRefundAsync(id, false, cancellationToken), "Payment refund retrieved successfully."));

    [HttpGet(RouteConstants.ReadCrmPaymentRefunds)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmPaymentRefunds.PaymentRefundsAsync(false, cancellationToken), "Payment refunds retrieved successfully."));

    [HttpGet(RouteConstants.PaymentRefundsByPaymentId)]
    public async Task<IActionResult> ByPaymentAsync([FromRoute] int paymentId, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmPaymentRefunds.PaymentRefundsByPaymentIdAsync(paymentId, false, cancellationToken), "Payment refunds retrieved successfully."));
}
