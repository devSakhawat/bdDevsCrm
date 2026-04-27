using Presentation.AuthorizeAttributes;
using Domain.Contracts.Services;
using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using bdDevs.Shared.Constants;
using Application.Shared.Grid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Presentation.ActionFilters;

namespace Presentation.Controllers.CRM;

[AuthorizeUser]
public class CrmStudentController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CrmStudentController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    [HttpPost(RouteConstants.CrmStudentSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        var summaryGrid = await _serviceManager.CrmStudents.StudentsSummaryAsync(options, cancellationToken);
        return Ok(ApiResponseHelper.Success(summaryGrid, "Summary retrieved successfully"));
    }

    [HttpPost(RouteConstants.CreateCrmStudent)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmStudentRecord record, CancellationToken cancellationToken = default)
    {
        var created = await _serviceManager.CrmStudents.CreateAsync(record, cancellationToken);
        if (created.StudentId <= 0) throw new InvalidCreateOperationException("Failed to create record.");
        return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
    }

    [HttpPut(RouteConstants.UpdateCrmStudent)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmStudentRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.StudentId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmStudentRecord));
        var updated = await _serviceManager.CrmStudents.UpdateAsync(record, false, cancellationToken);
        return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
    }

    [HttpPost(RouteConstants.CrmStudentStatusTransition)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> ChangeStatusAsync([FromBody] ChangeCrmStudentStatusRecord record, CancellationToken cancellationToken = default)
    {
        var updated = await _serviceManager.CrmStudents.ChangeStatusAsync(record, cancellationToken);
        return Ok(ApiResponseHelper.Updated(updated, "Student status updated successfully."));
    }

    [HttpGet(RouteConstants.CrmStudentApplicationReadyCheck)]
    public async Task<IActionResult> ApplicationReadyCheckAsync([FromRoute] int studentId, CancellationToken cancellationToken = default)
    {
        if (studentId <= 0) throw new IdParametersBadRequestException();
        var result = await _serviceManager.CrmStudents.ApplicationReadyCheckAsync(studentId, cancellationToken);
        return Ok(ApiResponseHelper.Success(result, "Application-ready check completed successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmStudent)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.CrmStudents.DeleteAsync(new DeleteCrmStudentRecord(key), false, cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmStudent)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0) throw new IdParametersBadRequestException();
        var record = await _serviceManager.CrmStudents.StudentAsync(id, false, cancellationToken);
        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmStudents)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmStudents.StudentsAsync(false, cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

    [HttpGet(RouteConstants.CrmStudentsByBranchId)]
    public async Task<IActionResult> GetByBranchIdAsync([FromRoute] int branchId, CancellationToken cancellationToken = default)
    {
        if (branchId <= 0) throw new IdParametersBadRequestException();
        var records = await _serviceManager.CrmStudents.StudentsByBranchIdAsync(branchId, false, cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

    [HttpGet(RouteConstants.CrmStudentDDL)]
    public async Task<IActionResult> GetForDDLAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmStudents.StudentForDDLAsync(cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

    [HttpPost(RouteConstants.CrmConvertLeadToStudentPreflight)]
    public async Task<IActionResult> ConvertLeadToStudentPreflightAsync([FromBody] ConvertToStudentRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _serviceManager.CrmStudents.EvaluateLeadConversionAsync(request, cancellationToken);
        if (result.HardErrors.Any())
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, new ApiResponse<ConvertToStudentResultDto>
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity,
                Success = false,
                Message = "HARD_BLOCK",
                Data = result,
                CorrelationId = HttpContext.TraceIdentifier,
                Timestamp = DateTime.UtcNow
            });
        }
        return Ok(ApiResponseHelper.Success(result, result.SoftWarnings.Any() ? "SOFT_WARNING" : "Lead conversion preflight passed."));
    }

    [HttpPost(RouteConstants.CrmConvertLeadToStudent)]
    public async Task<IActionResult> ConvertLeadToStudentAsync([FromBody] ConvertToStudentRequestDto request, CancellationToken cancellationToken = default)
    {
        request.RequestedBy = request.RequestedBy is > 0 ? request.RequestedBy : 1;
        var result = await _serviceManager.CrmStudents.ConvertLeadToStudentAsync(request, cancellationToken);
        if (result.ResultType == "HARD_BLOCK")
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, new ApiResponse<ConvertToStudentResultDto>
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity,
                Success = false,
                Message = "HARD_BLOCK",
                Data = result,
                CorrelationId = HttpContext.TraceIdentifier,
                Timestamp = DateTime.UtcNow
            });
        }
        if (result.ResultType == "SOFT_WARNING")
        {
            return Ok(ApiResponseHelper.Success(result, "SOFT_WARNING"));
        }
        return Ok(ApiResponseHelper.Success(result, "Lead converted to student successfully."));
    }

}
