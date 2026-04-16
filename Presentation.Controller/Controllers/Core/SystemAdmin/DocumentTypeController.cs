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
/// Document Type management endpoints.
/// </summary>
[AuthorizeUser]
public class DocumentTypeController : BaseApiController
{
    public DocumentTypeController(IServiceManager serviceManager) : base(serviceManager)
    {
    }

    /// <summary>
    /// Retrieves all document types for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.DocumentTypeDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> DocumentTypesForDDLAsync(CancellationToken cancellationToken = default)
    {
        var documentTypes = await _serviceManager.DocumentTypes.DocumentTypesForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!documentTypes.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<DocumentTypeDDLDto>(), "No document types found."));

        return Ok(ApiResponseHelper.Success(documentTypes, "Document types retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of document types.
    /// </summary>
    [HttpPost(RouteConstants.DocumentTypeSummary)]
    public async Task<IActionResult> DocumentTypeSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.DocumentTypes.DocumentTypesSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<DocumentTypeDto>(), "No document types found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Document type summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new document type record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateDocumentType)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateDocumentTypeAsync([FromBody] CreateDocumentTypeRecord record, CancellationToken cancellationToken = default)
    {
        var createdDocumentType = await _serviceManager.DocumentTypes.CreateAsync(record, cancellationToken);

        if (createdDocumentType.DocumenttypeId <= 0)
            throw new InvalidCreateOperationException("Failed to create document type record.");

        return Ok(ApiResponseHelper.Created(createdDocumentType, "Document type created successfully."));
    }

    /// <summary>
    /// Updates an existing document type record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateDocumentType)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateDocumentTypeAsync([FromRoute] int key, [FromBody] UpdateDocumentTypeRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.DocumenttypeId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateDocumentTypeRecord));

        var updatedDocumentType = await _serviceManager.DocumentTypes.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedDocumentType, "Document type updated successfully."));
    }

    /// <summary>
    /// Deletes a document type record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteDocumentType)]
    public async Task<IActionResult> DeleteDocumentTypeAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteDocumentTypeRecord(key);
        await _serviceManager.DocumentTypes.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Document type deleted successfully"));
    }

    /// <summary>
    /// Retrieves a document type by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadDocumentType)]
    public async Task<IActionResult> DocumentTypeAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var documentType = await _serviceManager.DocumentTypes.DocumentTypeAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(documentType, "Document type retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all document types.
    /// </summary>
    [HttpGet(RouteConstants.ReadDocumentTypes)]
    public async Task<IActionResult> DocumentTypesAsync(CancellationToken cancellationToken = default)
    {
        var documentTypes = await _serviceManager.DocumentTypes.DocumentTypesAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!documentTypes.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<DocumentTypeDto>(), "No document types found."));

        return Ok(ApiResponseHelper.Success(documentTypes, "Document types retrieved successfully"));
    }
}
