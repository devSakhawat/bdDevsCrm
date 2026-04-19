using Application.Shared.Grid;
using bdDevs.Shared;
using bdDevs.Shared.Constants;
using bdDevs.Shared.DataTransferObjects.DMS;
using Domain.Contracts.Services;
using Domain.Entities.Entities.System;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.AuthorizeAttributes;

namespace Presentation.Controllers.DMS;

/// <summary>
/// DMS document management endpoints.
///
/// [AuthorizeUser] at class-level ensures:
///    - Every request validates user via attribute
///    - CurrentUser / CurrentUserId available from BaseApiController
///    - No auth checks needed in controller methods
///    - Exceptions handled by StandardExceptionMiddleware
/// </summary>
[AuthorizeUser]
public class DmsDocumentController : BaseApiController
{
    public DmsDocumentController(IServiceManager serviceManager) : base(serviceManager)
    {
    }

    /// <summary>
    /// Retrieves all documents for dropdown list.
    /// </summary>
    [HttpGet("dms-document-ddl")]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> DocumentsForDDLAsync(CancellationToken cancellationToken = default)
    {
        var documents = await _serviceManager.DmsDocuments.DocumentsDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!documents.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<DmsDocumentDDL>(), "No documents found."));

        return Ok(ApiResponseHelper.Success(documents, "Documents retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of documents.
    /// </summary>
    [HttpPost("dms-document-summary")]
    public async Task<IActionResult> DocumentSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.DmsDocuments.DocumentsSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<DmsDocumentDto>(), "No documents found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Document summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new document record.
    /// </summary>
    [HttpPost("dms-document")]
    public async Task<IActionResult> CreateDocumentAsync([FromBody] DmsDocumentDto dto, CancellationToken cancellationToken = default)
    {
        if (dto == null)
            throw new NullModelBadRequestException(nameof(DmsDocumentDto));

        var createdDocument = await _serviceManager.DmsDocuments.CreateDocumentAsync(dto, cancellationToken);

        if (createdDocument.DocumentId <= 0)
            throw new InvalidCreateOperationException("Failed to create document record.");

        return Ok(ApiResponseHelper.Created(createdDocument, "Document created successfully."));
    }

    /// <summary>
    /// Updates an existing document record.
    /// </summary>
    [HttpPut("dms-document/{key}")]
    public async Task<IActionResult> UpdateDocumentAsync([FromRoute] int key, [FromBody] DmsDocumentDto dto, CancellationToken cancellationToken = default)
    {
        if (dto == null)
            throw new NullModelBadRequestException(nameof(DmsDocumentDto));

        if (key != dto.DocumentId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(DmsDocumentDto));

        var updatedDocument = await _serviceManager.DmsDocuments.UpdateDocumentAsync(key, dto, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedDocument, "Document updated successfully."));
    }

    /// <summary>
    /// Deletes a document record.
    /// </summary>
    [HttpDelete("dms-document/{key}")]
    public async Task<IActionResult> DeleteDocumentAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        if (key <= 0)
            throw new IdParametersBadRequestException();

        await _serviceManager.DmsDocuments.DeleteDocumentAsync(key, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Document deleted successfully"));
    }

    /// <summary>
    /// Retrieves a document by ID.
    /// </summary>
    [HttpGet("dms-document/{documentId:int}")]
    public async Task<IActionResult> DocumentAsync([FromRoute] int documentId, CancellationToken cancellationToken = default)
    {
        if (documentId <= 0)
            throw new IdParametersBadRequestException();

        var document = await _serviceManager.DmsDocuments.DocumentAsync(documentId, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(document, "Document retrieved successfully"));
    }

    /// <summary>
    /// Saves file and document with all DMS metadata.
    /// </summary>
    [HttpPost("dms-document-upload")]
    public async Task<IActionResult> SaveFileAndDocumentAsync(IFormFile file, [FromForm] string allAboutDMS, CancellationToken cancellationToken = default)
    {
        if (file == null)
            throw new NullModelBadRequestException(nameof(file));

        if (string.IsNullOrWhiteSpace(allAboutDMS))
            throw new NullModelBadRequestException(nameof(allAboutDMS));

        var result = await _serviceManager.DmsDocuments.SaveFileAndDocumentWithAllDmsAsync(file, allAboutDMS, cancellationToken);

    return Ok(ApiResponseHelper.Success<string>(result, "File and document saved successfully"));
  }
}
