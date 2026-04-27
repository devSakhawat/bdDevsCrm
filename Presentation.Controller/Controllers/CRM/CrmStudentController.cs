using Presentation.AuthorizeAttributes;
using Domain.Contracts.Services;
using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;
using Domain.Exceptions;
using bdDevs.Shared.Constants;
using Application.Shared.Grid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Presentation.ActionFilters;

namespace Presentation.Controllers.CRM;

/// <summary>CrmStudent management endpoints.</summary>
[AuthorizeUser]
public class CrmStudentController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CrmStudentController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>Retrieves paginated summary grid.</summary>
    [HttpPost(RouteConstants.CrmStudentSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        var summaryGrid = await _serviceManager.CrmStudents.StudentsSummaryAsync(options, cancellationToken);
        if (!summaryGrid.Items.Any()) return Ok(ApiResponseHelper.Success(new GridEntity<CrmStudentDto>(), "No records found."));
        return Ok(ApiResponseHelper.Success(summaryGrid, "Summary retrieved successfully"));
    }

    /// <summary>Creates a new student record.</summary>
    [HttpPost(RouteConstants.CreateCrmStudent)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmStudentRecord record, CancellationToken cancellationToken = default)
    {
        var created = await _serviceManager.CrmStudents.CreateAsync(record, cancellationToken);
        if (created.StudentId <= 0) throw new InvalidCreateOperationException("Failed to create record.");
        return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
    }

    /// <summary>Updates an existing student record.</summary>
    [HttpPut(RouteConstants.UpdateCrmStudent)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmStudentRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.StudentId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmStudentRecord));
        var updated = await _serviceManager.CrmStudents.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
    }

    /// <summary>Deletes a student record.</summary>
    [HttpDelete(RouteConstants.DeleteCrmStudent)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteCrmStudentRecord(key);
        await _serviceManager.CrmStudents.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
    }

    /// <summary>Retrieves a student record by ID.</summary>
    [HttpGet(RouteConstants.ReadCrmStudent)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0) throw new IdParametersBadRequestException();
        var record = await _serviceManager.CrmStudents.StudentAsync(id, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }

    /// <summary>Retrieves all student records.</summary>
    [HttpGet(RouteConstants.ReadCrmStudents)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmStudents.StudentsAsync(trackChanges: false, cancellationToken: cancellationToken);
        if (!records.Any()) return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmStudentDto>(), "No records found."));
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

    /// <summary>Retrieves students for dropdown list.</summary>
    [HttpGet(RouteConstants.CrmStudentDDL)]
    public async Task<IActionResult> GetForDDLAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmStudents.StudentForDDLAsync(cancellationToken: cancellationToken);
        if (!records.Any()) return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmStudentDto>(), "No records found."));
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }
}
