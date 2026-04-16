using Presentation.AuthorizeAttributes;
using Domain.Contracts.Services;
using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Domain.Exceptions;
using bdDevs.Shared.Constants;
using Application.Shared.Grid;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.Core.SystemAdmin;

/// <summary>
/// Document Template management endpoints.
/// </summary>
[AuthorizeUser]
public class DocumentTemplateController : BaseApiController
{
    public DocumentTemplateController(IServiceManager serviceManager) : base(serviceManager)
    {
    }

    /// <summary>
    /// Retrieves all document templates for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.DocumentTemplateDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> DocumentTemplatesForDDLAsync(CancellationToken cancellationToken = default)
    {
        var documentTemplates = await _serviceManager.DocumentTemplates.DocumentTemplatesForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!documentTemplates.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<DocumentTemplateDDLDto>(), "No document templates found."));

        return Ok(ApiResponseHelper.Success(documentTemplates, "Document templates retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of document templates.
    /// </summary>
    [HttpPost(RouteConstants.DocumentTemplateSummary)]
    public async Task<IActionResult> DocumentTemplateSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.DocumentTemplates.DocumentTemplatesSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<DocumentTemplateDto>(), "No document templates found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Document template summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new document template record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateDocumentTemplate)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateDocumentTemplateAsync([FromBody] CreateDocumentTemplateRecord record, CancellationToken cancellationToken = default)
    {
        var createdDocumentTemplate = await _serviceManager.DocumentTemplates.CreateAsync(record, cancellationToken);

        if (createdDocumentTemplate.DocumentId <= 0)
            throw new InvalidCreateOperationException("Failed to create document template record.");

        return Ok(ApiResponseHelper.Created(createdDocumentTemplate, "Document template created successfully."));
    }

    /// <summary>
    /// Updates an existing document template record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateDocumentTemplate)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateDocumentTemplateAsync([FromRoute] int key, [FromBody] UpdateDocumentTemplateRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.DocumentId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateDocumentTemplateRecord));

        var updatedDocumentTemplate = await _serviceManager.DocumentTemplates.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedDocumentTemplate, "Document template updated successfully."));
    }

    /// <summary>
    /// Deletes a document template record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteDocumentTemplate)]
    public async Task<IActionResult> DeleteDocumentTemplateAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteDocumentTemplateRecord(key);
        await _serviceManager.DocumentTemplates.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Document template deleted successfully"));
    }

    /// <summary>
    /// Retrieves a document template by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadDocumentTemplate)]
    public async Task<IActionResult> DocumentTemplateAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var documentTemplate = await _serviceManager.DocumentTemplates.DocumentTemplateAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(documentTemplate, "Document template retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all document templates.
    /// </summary>
    [HttpGet(RouteConstants.ReadDocumentTemplates)]
    public async Task<IActionResult> DocumentTemplatesAsync(CancellationToken cancellationToken = default)
    {
        var documentTemplates = await _serviceManager.DocumentTemplates.DocumentTemplatesAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!documentTemplates.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<DocumentTemplateDto>(), "No document templates found."));

        return Ok(ApiResponseHelper.Success(documentTemplates, "Document templates retrieved successfully"));
    }
}
