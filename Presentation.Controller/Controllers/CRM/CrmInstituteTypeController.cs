using Application.Shared.Grid;
using bdDevs.Shared;
using bdDevs.Shared.Constants;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Extensions;
using bdDevs.Shared.Records.CRM;
using Domain.Contracts.Services;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Presentation.AuthorizeAttributes;

namespace Presentation.Controllers.CRM;

/// <summary>
/// CrmInstituteType management endpoints.
/// </summary>
[AuthorizeUser]
public class CrmInstituteTypeController : BaseApiController
{
  private readonly IMemoryCache _cache;

  public CrmInstituteTypeController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
  {
    _cache = cache;
  }

  /// <summary>
  /// Retrieves paginated summary grid.
  /// </summary>
  [HttpPost(RouteConstants.CrmInstituteTypeSummary)]
  public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
  {
    if (options == null)
      throw new NullModelBadRequestException(nameof(GridOptions));

    var summaryGrid = await _serviceManager.CrmInstituteTypes.InstituteTypesSummaryAsync(options, cancellationToken);

    if (!summaryGrid.Items.Any())
      return Ok(ApiResponseHelper.Success(new GridEntity<CrmInstituteTypeDto>(), "No records found."));

    return Ok(ApiResponseHelper.Success(summaryGrid, "Summary retrieved successfully"));
  }

  /// <summary>
  /// Creates a new record using CRUD Record pattern.
  /// </summary>
  [HttpPost(RouteConstants.CreateCrmInstituteType)]
  [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
  public async Task<IActionResult> CreateAsync([FromBody] CreateCrmInstituteTypeRecord record, CancellationToken cancellationToken = default)
  {
    var dto = record.MapTo<CrmInstituteTypeDto>();
    var currentUser = await GetCurrentUserAsync();

    var created = await _serviceManager.CrmInstituteTypes.CreateAsync(record, currentUser, cancellationToken);

    if (created.InstituteTypeId <= 0)
      throw new InvalidCreateOperationException("Failed to create record.");

    return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
  }

  /// <summary>
  /// Updates an existing record using CRUD Record pattern.
  /// </summary>
  [HttpPut(RouteConstants.UpdateCrmInstituteType)]
  [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
  public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmInstituteTypeRecord record, CancellationToken cancellationToken = default)
  {
    if (key != record.InstituteTypeId)
      throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmInstituteTypeRecord));

    var dto = record.MapTo<CrmInstituteTypeDto>();
    var currentUser = await GetCurrentUserAsync();
    var updated = await _serviceManager.CrmInstituteTypes.UpdateAsync(record, currentUser, trackChanges: false, cancellationToken: cancellationToken);

    return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
  }

  /// <summary>
  /// Deletes a record using CRUD Record pattern.
  /// </summary>
  [HttpDelete(RouteConstants.DeleteCrmInstituteType)]
  public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
  {
    var deleteRecord = new DeleteCrmInstituteTypeRecord(key);
    var currentUser = await GetCurrentUserAsync();
    var dto = new CrmInstituteTypeDto { InstituteTypeId = key };
    await _serviceManager.CrmInstituteTypes.DeleteAsync(deleteRecord, currentUser, trackChanges: false, cancellationToken: cancellationToken);
    return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
  }

  /// <summary>
  /// Retrieves a record by ID.
  /// </summary>
  [HttpGet(RouteConstants.ReadCrmInstituteType)]
  public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
  {
    if (id <= 0)
      throw new IdParametersBadRequestException();

    //var record = await _serviceManager.CrmInstituteTypes.InstituteTypesAsync(id, trackChanges: false, cancellationToken: cancellationToken);
    var record = await _serviceManager.CrmInstituteTypes.InstituteTypesAsync(trackChanges: false, cancellationToken: cancellationToken);

    return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
  }

  /// <summary>
  /// Retrieves all records.
  /// </summary>
  [HttpGet(RouteConstants.ReadCrmInstituteTypes)]
  public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
  {
    //var records = await _serviceManager.CrmInstituteTypes.CrmInstituteTypesAsync(trackChanges: false, cancellationToken: cancellationToken);
    var records = await _serviceManager.CrmInstituteTypes.InstituteTypesAsync(trackChanges: false, cancellationToken: cancellationToken);

    if (!records.Any())
      return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmInstituteTypeDto>(), "No records found."));

    return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
  }

  private async Task<UsersDto> GetCurrentUserAsync()
  {
    var userId = User?.FindFirst("UserId")?.Value;
    if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int parsedUserId))
    {
      return new UsersDto { UserId = 1, Username = "system" };
    }
    return new UsersDto { UserId = parsedUserId };
  }
}
