using Application.Shared.Grid;
using bdDevs.Shared;
using bdDevs.Shared.Constants;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;
using Domain.Contracts.Services;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Presentation.ActionFilters;
using Presentation.AuthorizeAttributes;

namespace Presentation.Controllers.CRM;

[AuthorizeUser]
public class CrmStudentDocumentController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CrmStudentDocumentController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager) => _cache = cache;

    [HttpPost(RouteConstants.CrmStudentDocumentSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        return Ok(ApiResponseHelper.Success(await _serviceManager.CrmStudentDocuments.StudentDocumentsSummaryAsync(options, cancellationToken), "Student documents retrieved successfully."));
    }

    [HttpPost(RouteConstants.CreateCrmStudentDocument)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmStudentDocumentRecord record, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Created(await _serviceManager.CrmStudentDocuments.CreateAsync(record, cancellationToken), "Student document created successfully."));

    [HttpPut(RouteConstants.UpdateCrmStudentDocument)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmStudentDocumentRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.StudentDocumentId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmStudentDocumentRecord));
        return Ok(ApiResponseHelper.Updated(await _serviceManager.CrmStudentDocuments.UpdateAsync(record, false, cancellationToken), "Student document updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmStudentDocument)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.CrmStudentDocuments.DeleteAsync(new DeleteCrmStudentDocumentRecord(key), false, cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Student document deleted successfully."));
    }

    [HttpGet(RouteConstants.ReadCrmStudentDocument)]
    public async Task<IActionResult> StudentDocumentAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmStudentDocuments.StudentDocumentAsync(id, false, cancellationToken), "Student document retrieved successfully."));

    [HttpGet(RouteConstants.ReadCrmStudentDocuments)]
    public async Task<IActionResult> StudentDocumentsAsync(CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmStudentDocuments.StudentDocumentsAsync(false, cancellationToken), "Student documents retrieved successfully."));

    [HttpGet(RouteConstants.StudentDocumentsByStudentId)]
    public async Task<IActionResult> ByStudentAsync([FromRoute] int studentId, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmStudentDocuments.StudentDocumentsByStudentIdAsync(studentId, false, cancellationToken), "Student documents retrieved successfully."));

    [HttpPost(RouteConstants.CrmStudentDocumentUpload)]
    public async Task<IActionResult> UploadAsync([FromForm] StudentDocumentUploadRequestDto request, IFormFile file, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Created(await _serviceManager.CrmStudentDocuments.UploadAsync(request, file, cancellationToken), "Student document uploaded successfully."));

    [HttpPost(RouteConstants.CrmStudentDocumentStatusTransition)]
    public async Task<IActionResult> ChangeStatusAsync([FromBody] ChangeCrmStudentDocumentStatusRecord record, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Updated(await _serviceManager.CrmStudentDocuments.ChangeStatusAsync(record, cancellationToken), "Student document status updated successfully."));
}
