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
public class CrmStudentDocumentChecklistController : BaseApiController
{
    private readonly IMemoryCache _cache;
    public CrmStudentDocumentChecklistController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager) => _cache = cache;

    [HttpPost(RouteConstants.CrmStudentDocumentChecklistSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        return Ok(ApiResponseHelper.Success(await _serviceManager.CrmStudentDocumentChecklists.StudentDocumentChecklistsSummaryAsync(options, cancellationToken), "Document checklists retrieved successfully."));
    }

    [HttpPost(RouteConstants.CreateCrmStudentDocumentChecklist)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmStudentDocumentChecklistRecord record, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Created(await _serviceManager.CrmStudentDocumentChecklists.CreateAsync(record, cancellationToken), "Document checklist created successfully."));

    [HttpPut(RouteConstants.UpdateCrmStudentDocumentChecklist)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmStudentDocumentChecklistRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.StudentDocumentChecklistId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmStudentDocumentChecklistRecord));
        return Ok(ApiResponseHelper.Updated(await _serviceManager.CrmStudentDocumentChecklists.UpdateAsync(record, false, cancellationToken), "Document checklist updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmStudentDocumentChecklist)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.CrmStudentDocumentChecklists.DeleteAsync(new DeleteCrmStudentDocumentChecklistRecord(key), false, cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Document checklist deleted successfully."));
    }

    [HttpGet(RouteConstants.ReadCrmStudentDocumentChecklist)]
    public async Task<IActionResult> GetAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmStudentDocumentChecklists.StudentDocumentChecklistAsync(id, false, cancellationToken), "Document checklist retrieved successfully."));

    [HttpGet(RouteConstants.ReadCrmStudentDocumentChecklists)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmStudentDocumentChecklists.StudentDocumentChecklistsAsync(false, cancellationToken), "Document checklists retrieved successfully."));

    [HttpGet(RouteConstants.StudentDocumentChecklistsByStudentId)]
    public async Task<IActionResult> ByStudentAsync([FromRoute] int studentId, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmStudentDocumentChecklists.StudentDocumentChecklistsByStudentIdAsync(studentId, false, cancellationToken), "Document checklists retrieved successfully."));
}
