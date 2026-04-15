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
/// DMS document tag management endpoints.
///
/// [AuthorizeUser] at class-level ensures:
///    - Every request validates user via attribute
///    - CurrentUser / CurrentUserId available from BaseApiController
///    - No auth checks needed in controller methods
///    - Exceptions handled by StandardExceptionMiddleware
/// </summary>
[AuthorizeUser]
public class DmsDocumentTagController : BaseApiController
{
    public DmsDocumentTagController(IServiceManager serviceManager) : base(serviceManager)
    {
    }

    /// <summary>
    /// Retrieves all document tags for dropdown list.
    /// </summary>
    [HttpGet("dms-document-tag-ddl")]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> TagsForDDLAsync(CancellationToken cancellationToken = default)
    {
        var tags = await _serviceManager.DmsDocumentTags.TagsDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!tags.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<DmsDocumentTagDDL>(), "No tags found."));

        return Ok(ApiResponseHelper.Success(tags, "Tags retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of document tags.
    /// </summary>
    [HttpPost("dms-document-tag-summary")]
    public async Task<IActionResult> TagSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.DmsDocumentTags.TagsSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<DmsDocumentTagDto>(), "No tags found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Tag summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new document tag record.
    /// </summary>
    [HttpPost("dms-document-tag")]
    public async Task<IActionResult> CreateTagAsync([FromBody] DmsDocumentTagDto dto, CancellationToken cancellationToken = default)
    {
        if (dto == null)
            throw new NullModelBadRequestException(nameof(DmsDocumentTagDto));

        var createdTag = await _serviceManager.DmsDocumentTags.CreateTagAsync(dto, cancellationToken);

        if (createdTag.TagId <= 0)
            throw new InvalidCreateOperationException("Failed to create tag record.");

        return Ok(ApiResponseHelper.Created(createdTag, "Tag created successfully."));
    }

    /// <summary>
    /// Updates an existing document tag record.
    /// </summary>
    [HttpPut("dms-document-tag/{key}")]
    public async Task<IActionResult> UpdateTagAsync([FromRoute] int key, [FromBody] DmsDocumentTagDto dto, CancellationToken cancellationToken = default)
    {
        if (dto == null)
            throw new NullModelBadRequestException(nameof(DmsDocumentTagDto));

        if (key != dto.TagId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(DmsDocumentTagDto));

        var updatedTag = await _serviceManager.DmsDocumentTags.UpdateTagAsync(key, dto, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedTag, "Tag updated successfully."));
    }

    /// <summary>
    /// Deletes a document tag record.
    /// </summary>
    [HttpDelete("dms-document-tag/{key}")]
    public async Task<IActionResult> DeleteTagAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        if (key <= 0)
            throw new IdParametersBadRequestException();

        await _serviceManager.DmsDocumentTags.DeleteTagAsync(key, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Tag deleted successfully"));
    }

    /// <summary>
    /// Retrieves a document tag by ID.
    /// </summary>
    [HttpGet("dms-document-tag/{tagId:int}")]
    public async Task<IActionResult> TagAsync([FromRoute] int tagId, CancellationToken cancellationToken = default)
    {
        if (tagId <= 0)
            throw new IdParametersBadRequestException();

        var tag = await _serviceManager.DmsDocumentTags.TagAsync(tagId, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(tag, "Tag retrieved successfully"));
    }
}
