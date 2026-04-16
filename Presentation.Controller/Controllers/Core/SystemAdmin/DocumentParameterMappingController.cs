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
/// Document Parameter Mapping management endpoints.
/// </summary>
[AuthorizeUser]
public class DocumentParameterMappingController : BaseApiController
{
    public DocumentParameterMappingController(IServiceManager serviceManager) : base(serviceManager)
    {
    }

    /// <summary>
    /// Retrieves all document parameter mappings for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.DocumentParameterMappingDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> DocumentParameterMappingsForDDLAsync(CancellationToken cancellationToken = default)
    {
        var documentParameterMappings = await _serviceManager.DocumentParameterMappings.DocumentParameterMappingsForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!documentParameterMappings.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<DocumentParameterMappingDDLDto>(), "No document parameter mappings found."));

        return Ok(ApiResponseHelper.Success(documentParameterMappings, "Document parameter mappings retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of document parameter mappings.
    /// </summary>
    [HttpPost(RouteConstants.DocumentParameterMappingSummary)]
    public async Task<IActionResult> DocumentParameterMappingSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.DocumentParameterMappings.DocumentParameterMappingsSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<DocumentParameterMappingDto>(), "No document parameter mappings found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Document parameter mapping summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new document parameter mapping record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateDocumentParameterMapping)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateDocumentParameterMappingAsync([FromBody] CreateDocumentParameterMappingRecord record, CancellationToken cancellationToken = default)
    {
        var createdDocumentParameterMapping = await _serviceManager.DocumentParameterMappings.CreateAsync(record, cancellationToken);

        if (createdDocumentParameterMapping.MappingId <= 0)
            throw new InvalidCreateOperationException("Failed to create document parameter mapping record.");

        return Ok(ApiResponseHelper.Created(createdDocumentParameterMapping, "Document parameter mapping created successfully."));
    }

    /// <summary>
    /// Updates an existing document parameter mapping record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateDocumentParameterMapping)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateDocumentParameterMappingAsync([FromRoute] int key, [FromBody] UpdateDocumentParameterMappingRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.MappingId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateDocumentParameterMappingRecord));

        var updatedDocumentParameterMapping = await _serviceManager.DocumentParameterMappings.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedDocumentParameterMapping, "Document parameter mapping updated successfully."));
    }

    /// <summary>
    /// Deletes a document parameter mapping record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteDocumentParameterMapping)]
    public async Task<IActionResult> DeleteDocumentParameterMappingAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteDocumentParameterMappingRecord(key);
        await _serviceManager.DocumentParameterMappings.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Document parameter mapping deleted successfully"));
    }

    /// <summary>
    /// Retrieves a document parameter mapping by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadDocumentParameterMapping)]
    public async Task<IActionResult> DocumentParameterMappingAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var documentParameterMapping = await _serviceManager.DocumentParameterMappings.DocumentParameterMappingAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(documentParameterMapping, "Document parameter mapping retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all document parameter mappings.
    /// </summary>
    [HttpGet(RouteConstants.ReadDocumentParameterMappings)]
    public async Task<IActionResult> DocumentParameterMappingsAsync(CancellationToken cancellationToken = default)
    {
        var documentParameterMappings = await _serviceManager.DocumentParameterMappings.DocumentParameterMappingsAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!documentParameterMappings.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<DocumentParameterMappingDto>(), "No document parameter mappings found."));

        return Ok(ApiResponseHelper.Success(documentParameterMappings, "Document parameter mappings retrieved successfully"));
    }
}
