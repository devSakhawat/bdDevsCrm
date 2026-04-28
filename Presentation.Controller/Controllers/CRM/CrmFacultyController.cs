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
public class CrmFacultyController : BaseApiController
{
    public CrmFacultyController(IServiceManager serviceManager) : base(serviceManager) { }

    [HttpPost(RouteConstants.CrmFacultySummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        var summary = await _serviceManager.CrmFaculties.FacultiesSummaryAsync(options, cancellationToken);
        return Ok(ApiResponseHelper.Success(summary, "Summary retrieved successfully"));
    }

    [HttpPost(RouteConstants.CreateCrmFaculty)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmFacultyRecord record, CancellationToken cancellationToken = default)
    {
        var created = await _serviceManager.CrmFaculties.CreateAsync(record, cancellationToken);
        if (created.FacultyId <= 0) throw new InvalidCreateOperationException("Failed to create record.");
        return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
    }

    [HttpPut(RouteConstants.UpdateCrmFaculty)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmFacultyRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.FacultyId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmFacultyRecord));
        var updated = await _serviceManager.CrmFaculties.UpdateAsync(record, false, cancellationToken);
        return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmFaculty)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.CrmFaculties.DeleteAsync(new DeleteCrmFacultyRecord(key), false, cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmFaculty)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0) throw new IdParametersBadRequestException();
        var record = await _serviceManager.CrmFaculties.FacultyAsync(id, false, cancellationToken);
        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmFaculties)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmFaculties.FacultiesAsync(false, cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

    [HttpGet(RouteConstants.CrmFacultyDDL)]
    public async Task<IActionResult> GetDDLAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmFaculties.FacultiesForDDLAsync(cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "DDL retrieved successfully"));
    }

    [HttpGet(RouteConstants.CrmFacultiesByInstituteId)]
    public async Task<IActionResult> GetByInstituteIdAsync([FromRoute] int instituteId, CancellationToken cancellationToken = default)
    {
        if (instituteId <= 0) throw new IdParametersBadRequestException();
        var records = await _serviceManager.CrmFaculties.FacultiesByInstituteIdAsync(instituteId, false, cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }
}
