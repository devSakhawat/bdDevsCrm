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
public class CrmDegreeLevelController : BaseApiController
{
    public CrmDegreeLevelController(IServiceManager serviceManager) : base(serviceManager) { }

    [HttpPost(RouteConstants.CrmDegreeLevelSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        var summary = await _serviceManager.CrmDegreeLevels.DegreeLevelsSummaryAsync(options, cancellationToken);
        return Ok(ApiResponseHelper.Success(summary, "Summary retrieved successfully"));
    }

    [HttpPost(RouteConstants.CreateCrmDegreeLevel)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmDegreeLevelRecord record, CancellationToken cancellationToken = default)
    {
        var created = await _serviceManager.CrmDegreeLevels.CreateAsync(record, cancellationToken);
        if (created.DegreeLevelId <= 0) throw new InvalidCreateOperationException("Failed to create record.");
        return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
    }

    [HttpPut(RouteConstants.UpdateCrmDegreeLevel)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmDegreeLevelRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.DegreeLevelId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmDegreeLevelRecord));
        var updated = await _serviceManager.CrmDegreeLevels.UpdateAsync(record, false, cancellationToken);
        return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmDegreeLevel)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.CrmDegreeLevels.DeleteAsync(new DeleteCrmDegreeLevelRecord(key), false, cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmDegreeLevel)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0) throw new IdParametersBadRequestException();
        var record = await _serviceManager.CrmDegreeLevels.DegreeLevelAsync(id, false, cancellationToken);
        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmDegreeLevels)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmDegreeLevels.DegreeLevelsAsync(false, cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

    [HttpGet(RouteConstants.CrmDegreeLevelDDL)]
    public async Task<IActionResult> GetDDLAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmDegreeLevels.DegreeLevelsForDDLAsync(cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "DDL retrieved successfully"));
    }
}
