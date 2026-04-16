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
/// Document Parameter management endpoints.
/// </summary>
[AuthorizeUser]
public class DocumentParameterController : BaseApiController
{
    public DocumentParameterController(IServiceManager serviceManager) : base(serviceManager)
    {
    }

    /// <summary>
    /// Retrieves all document parameters for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.DocumentParameterDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> DocumentParametersForDDLAsync(CancellationToken cancellationToken = default)
    {
        var documentParameters = await _serviceManager.DocumentParameters.DocumentParametersForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!documentParameters.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<DocumentParameterDDLDto>(), "No document parameters found."));

        return Ok(ApiResponseHelper.Success(documentParameters, "Document parameters retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of document parameters.
    /// </summary>
    [HttpPost(RouteConstants.DocumentParameterSummary)]
    public async Task<IActionResult> DocumentParameterSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.DocumentParameters.DocumentParametersSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<DocumentParameterDto>(), "No document parameters found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Document parameter summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new document parameter record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateDocumentParameter)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateDocumentParameterAsync([FromBody] CreateDocumentParameterRecord record, CancellationToken cancellationToken = default)
    {
        var createdDocumentParameter = await _serviceManager.DocumentParameters.CreateAsync(record, cancellationToken);

        if (createdDocumentParameter.ParameterId <= 0)
            throw new InvalidCreateOperationException("Failed to create document parameter record.");

        return Ok(ApiResponseHelper.Created(createdDocumentParameter, "Document parameter created successfully."));
    }

    /// <summary>
    /// Updates an existing document parameter record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateDocumentParameter)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateDocumentParameterAsync([FromRoute] int key, [FromBody] UpdateDocumentParameterRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.ParameterId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateDocumentParameterRecord));

        var updatedDocumentParameter = await _serviceManager.DocumentParameters.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedDocumentParameter, "Document parameter updated successfully."));
    }

    /// <summary>
    /// Deletes a document parameter record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteDocumentParameter)]
    public async Task<IActionResult> DeleteDocumentParameterAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteDocumentParameterRecord(key);
        await _serviceManager.DocumentParameters.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Document parameter deleted successfully"));
    }

    /// <summary>
    /// Retrieves a document parameter by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadDocumentParameter)]
    public async Task<IActionResult> DocumentParameterAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var documentParameter = await _serviceManager.DocumentParameters.DocumentParameterAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(documentParameter, "Document parameter retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all document parameters.
    /// </summary>
    [HttpGet(RouteConstants.ReadDocumentParameters)]
    public async Task<IActionResult> DocumentParametersAsync(CancellationToken cancellationToken = default)
    {
        var documentParameters = await _serviceManager.DocumentParameters.DocumentParametersAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!documentParameters.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<DocumentParameterDto>(), "No document parameters found."));

        return Ok(ApiResponseHelper.Success(documentParameters, "Document parameters retrieved successfully"));
    }
}
