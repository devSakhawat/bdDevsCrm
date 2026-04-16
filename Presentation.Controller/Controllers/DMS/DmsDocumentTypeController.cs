using Presentation.AuthorizeAttributes;
using Domain.Contracts.Services;
using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects.DMS;
using Domain.Exceptions;
using bdDevs.Shared.Constants;
using Application.Shared.Grid;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.DMS;

/// <summary>
/// DMS document type management endpoints.
///
/// [AuthorizeUser] at class-level ensures:
///    - Every request validates user via attribute
///    - CurrentUser / CurrentUserId available from BaseApiController
///    - No auth checks needed in controller methods
///    - Exceptions handled by StandardExceptionMiddleware
/// </summary>
[AuthorizeUser]
public class DmsDocumentTypeController : BaseApiController
{
    public DmsDocumentTypeController(IServiceManager serviceManager) : base(serviceManager)
    {
    }

    /// <summary>
    /// Retrieves all document types for dropdown list.
    /// </summary>
    [HttpGet("dms-document-type-ddl")]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> TypesForDDLAsync(CancellationToken cancellationToken = default)
    {
        var types = await _serviceManager.DmsDocumentTypes.TypesDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!types.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<DmsDocumentTypeDDL>(), "No document types found."));

        return Ok(ApiResponseHelper.Success(types, "Document types retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of document types.
    /// </summary>
    [HttpPost("dms-document-type-summary")]
    public async Task<IActionResult> DocumentTypeSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.DmsDocumentTypes.TypesSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<DmsDocumentTypeDto>(), "No document types found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Document type summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new document type record.
    /// </summary>
    [HttpPost("dms-document-type")]
    public async Task<IActionResult> CreateDocumentTypeAsync([FromBody] DmsDocumentTypeDto dto, CancellationToken cancellationToken = default)
    {
        if (dto == null)
            throw new NullModelBadRequestException(nameof(DmsDocumentTypeDto));

        var createdDocumentType = await _serviceManager.DmsDocumentTypes.CreateDocumentTypeAsync(dto, cancellationToken);

        if (createdDocumentType.DocumentTypeId <= 0)
            throw new InvalidCreateOperationException("Failed to create document type record.");

        return Ok(ApiResponseHelper.Created(createdDocumentType, "Document type created successfully."));
    }

    /// <summary>
    /// Updates an existing document type record.
    /// </summary>
    [HttpPut("dms-document-type/{key}")]
    public async Task<IActionResult> UpdateDocumentTypeAsync([FromRoute] int key, [FromBody] DmsDocumentTypeDto dto, CancellationToken cancellationToken = default)
    {
        if (dto == null)
            throw new NullModelBadRequestException(nameof(DmsDocumentTypeDto));

        if (key != dto.DocumentTypeId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(DmsDocumentTypeDto));

        var updatedDocumentType = await _serviceManager.DmsDocumentTypes.UpdateDocumentTypeAsync(key, dto, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedDocumentType, "Document type updated successfully."));
    }

    /// <summary>
    /// Deletes a document type record.
    /// </summary>
    [HttpDelete("dms-document-type/{key}")]
    public async Task<IActionResult> DeleteDocumentTypeAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        if (key <= 0)
            throw new IdParametersBadRequestException();

        await _serviceManager.DmsDocumentTypes.DeleteDocumentTypeAsync(key, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Document type deleted successfully"));
    }

    /// <summary>
    /// Retrieves a document type by ID.
    /// </summary>
    [HttpGet("dms-document-type/{documentTypeId:int}")]
    public async Task<IActionResult> DocumentTypeAsync([FromRoute] int documentTypeId, CancellationToken cancellationToken = default)
    {
        if (documentTypeId <= 0)
            throw new IdParametersBadRequestException();

        var documentType = await _serviceManager.DmsDocumentTypes.DocumentTypeAsync(documentTypeId, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(documentType, "Document type retrieved successfully"));
    }
}
