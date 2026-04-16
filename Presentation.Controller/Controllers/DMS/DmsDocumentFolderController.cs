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
/// DMS document folder management endpoints.
///
/// [AuthorizeUser] at class-level ensures:
///    - Every request validates user via attribute
///    - CurrentUser / CurrentUserId available from BaseApiController
///    - No auth checks needed in controller methods
///    - Exceptions handled by StandardExceptionMiddleware
/// </summary>
[AuthorizeUser]
public class DmsDocumentFolderController : BaseApiController
{
    public DmsDocumentFolderController(IServiceManager serviceManager) : base(serviceManager)
    {
    }

    /// <summary>
    /// Retrieves all document folders for dropdown list.
    /// </summary>
    [HttpGet("dms-document-folder-ddl")]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> FoldersForDDLAsync(CancellationToken cancellationToken = default)
    {
        var folders = await _serviceManager.DmsDocumentFolders.FoldersDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!folders.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<DmsDocumentFolderDDL>(), "No folders found."));

        return Ok(ApiResponseHelper.Success(folders, "Folders retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of document folders.
    /// </summary>
    [HttpPost("dms-document-folder-summary")]
    public async Task<IActionResult> FolderSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.DmsDocumentFolders.FoldersSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<DmsDocumentFolderDto>(), "No folders found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Folder summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new document folder record.
    /// </summary>
    [HttpPost("dms-document-folder")]
    public async Task<IActionResult> CreateFolderAsync([FromBody] DmsDocumentFolderDto dto, CancellationToken cancellationToken = default)
    {
        if (dto == null)
            throw new NullModelBadRequestException(nameof(DmsDocumentFolderDto));

        var createdFolder = await _serviceManager.DmsDocumentFolders.CreateFolderAsync(dto, cancellationToken);

        if (createdFolder.FolderId <= 0)
            throw new InvalidCreateOperationException("Failed to create folder record.");

        return Ok(ApiResponseHelper.Created(createdFolder, "Folder created successfully."));
    }

    /// <summary>
    /// Updates an existing document folder record.
    /// </summary>
    [HttpPut("dms-document-folder/{key}")]
    public async Task<IActionResult> UpdateFolderAsync([FromRoute] int key, [FromBody] DmsDocumentFolderDto dto, CancellationToken cancellationToken = default)
    {
        if (dto == null)
            throw new NullModelBadRequestException(nameof(DmsDocumentFolderDto));

        if (key != dto.FolderId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(DmsDocumentFolderDto));

        var updatedFolder = await _serviceManager.DmsDocumentFolders.UpdateFolderAsync(key, dto, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedFolder, "Folder updated successfully."));
    }

    /// <summary>
    /// Deletes a document folder record.
    /// </summary>
    [HttpDelete("dms-document-folder/{key}")]
    public async Task<IActionResult> DeleteFolderAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        if (key <= 0)
            throw new IdParametersBadRequestException();

        await _serviceManager.DmsDocumentFolders.DeleteFolderAsync(key, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Folder deleted successfully"));
    }

    /// <summary>
    /// Retrieves a document folder by ID.
    /// </summary>
    [HttpGet("dms-document-folder/{folderId:int}")]
    public async Task<IActionResult> FolderAsync([FromRoute] int folderId, CancellationToken cancellationToken = default)
    {
        if (folderId <= 0)
            throw new IdParametersBadRequestException();

        var folder = await _serviceManager.DmsDocumentFolders.FolderAsync(folderId, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(folder, "Folder retrieved successfully"));
    }
}
