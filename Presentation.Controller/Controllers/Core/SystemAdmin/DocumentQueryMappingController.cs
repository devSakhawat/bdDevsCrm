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
/// Document Query Mapping management endpoints.
/// </summary>
[AuthorizeUser]
public class DocumentQueryMappingController : BaseApiController
{
    public DocumentQueryMappingController(IServiceManager serviceManager) : base(serviceManager)
    {
    }

    /// <summary>
    /// Retrieves all document query mappings for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.DocumentQueryMappingDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> DocumentQueryMappingsForDDLAsync(CancellationToken cancellationToken = default)
    {
        var documentQueryMappings = await _serviceManager.DocumentQueryMappings.DocumentQueryMappingsForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!documentQueryMappings.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<DocumentQueryMappingDDLDto>(), "No document query mappings found."));

        return Ok(ApiResponseHelper.Success(documentQueryMappings, "Document query mappings retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of document query mappings.
    /// </summary>
    [HttpPost(RouteConstants.DocumentQueryMappingSummary)]
    public async Task<IActionResult> DocumentQueryMappingSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.DocumentQueryMappings.DocumentQueryMappingsSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<DocumentQueryMappingDto>(), "No document query mappings found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Document query mapping summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new document query mapping record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateDocumentQueryMapping)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateDocumentQueryMappingAsync([FromBody] CreateDocumentQueryMappingRecord record, CancellationToken cancellationToken = default)
    {
        var createdDocumentQueryMapping = await _serviceManager.DocumentQueryMappings.CreateAsync(record, cancellationToken);

        if (createdDocumentQueryMapping.DocumentQueryId <= 0)
            throw new InvalidCreateOperationException("Failed to create document query mapping record.");

        return Ok(ApiResponseHelper.Created(createdDocumentQueryMapping, "Document query mapping created successfully."));
    }

    /// <summary>
    /// Updates an existing document query mapping record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateDocumentQueryMapping)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateDocumentQueryMappingAsync([FromRoute] int key, [FromBody] UpdateDocumentQueryMappingRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.DocumentQueryId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateDocumentQueryMappingRecord));

        var updatedDocumentQueryMapping = await _serviceManager.DocumentQueryMappings.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedDocumentQueryMapping, "Document query mapping updated successfully."));
    }

    /// <summary>
    /// Deletes a document query mapping record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteDocumentQueryMapping)]
    public async Task<IActionResult> DeleteDocumentQueryMappingAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteDocumentQueryMappingRecord(key);
        await _serviceManager.DocumentQueryMappings.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Document query mapping deleted successfully"));
    }

    /// <summary>
    /// Retrieves a document query mapping by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadDocumentQueryMapping)]
    public async Task<IActionResult> DocumentQueryMappingAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var documentQueryMapping = await _serviceManager.DocumentQueryMappings.DocumentQueryMappingAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(documentQueryMapping, "Document query mapping retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all document query mappings.
    /// </summary>
    [HttpGet(RouteConstants.ReadDocumentQueryMappings)]
    public async Task<IActionResult> DocumentQueryMappingsAsync(CancellationToken cancellationToken = default)
    {
        var documentQueryMappings = await _serviceManager.DocumentQueryMappings.DocumentQueryMappingsAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!documentQueryMappings.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<DocumentQueryMappingDto>(), "No document query mappings found."));

        return Ok(ApiResponseHelper.Success(documentQueryMappings, "Document query mappings retrieved successfully"));
    }
}
