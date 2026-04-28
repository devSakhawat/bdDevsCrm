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
public class CrmStudentPaymentController : BaseApiController
{
    private readonly IMemoryCache _cache;
    public CrmStudentPaymentController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager) => _cache = cache;

    [HttpPost(RouteConstants.CrmStudentPaymentSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        return Ok(ApiResponseHelper.Success(await _serviceManager.CrmStudentPayments.StudentPaymentsSummaryAsync(options, cancellationToken), "Student payments retrieved successfully."));
    }

    [HttpPost(RouteConstants.CreateCrmStudentPayment)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmStudentPaymentRecord record, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Created(await _serviceManager.CrmStudentPayments.CreateAsync(record, cancellationToken), "Student payment created successfully."));

    [HttpPut(RouteConstants.UpdateCrmStudentPayment)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmStudentPaymentRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.StudentPaymentId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmStudentPaymentRecord));
        return Ok(ApiResponseHelper.Updated(await _serviceManager.CrmStudentPayments.UpdateAsync(record, false, cancellationToken), "Student payment updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmStudentPayment)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.CrmStudentPayments.DeleteAsync(new DeleteCrmStudentPaymentRecord(key), false, cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Student payment deleted successfully."));
    }

    [HttpGet(RouteConstants.ReadCrmStudentPayment)]
    public async Task<IActionResult> GetAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmStudentPayments.StudentPaymentAsync(id, false, cancellationToken), "Student payment retrieved successfully."));

    [HttpGet(RouteConstants.ReadCrmStudentPayments)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmStudentPayments.StudentPaymentsAsync(false, cancellationToken), "Student payments retrieved successfully."));

    [HttpGet(RouteConstants.StudentPaymentsByStudentId)]
    public async Task<IActionResult> ByStudentAsync([FromRoute] int studentId, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmStudentPayments.StudentPaymentsByStudentIdAsync(studentId, false, cancellationToken), "Student payments retrieved successfully."));

    [HttpGet(RouteConstants.StudentPaymentsByApplicationId)]
    public async Task<IActionResult> ByApplicationAsync([FromRoute] int applicationId, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmStudentPayments.StudentPaymentsByApplicationIdAsync(applicationId, false, cancellationToken), "Student payments retrieved successfully."));

    [HttpPost(RouteConstants.CrmStudentPaymentStatusTransition)]
    public async Task<IActionResult> ChangeStatusAsync([FromBody] ChangeCrmStudentPaymentStatusRecord record, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Updated(await _serviceManager.CrmStudentPayments.ChangeStatusAsync(record, cancellationToken), "Student payment status updated successfully."));

    [HttpGet(RouteConstants.CrmStudentPaymentReceipt)]
    public async Task<IActionResult> ReceiptAsync([FromRoute] int paymentId, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmStudentPayments.GenerateReceiptAsync(paymentId, cancellationToken), "Student payment receipt generated successfully."));
}
