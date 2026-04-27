using Application.Shared.Grid;
using bdDevs.Shared;
using bdDevs.Shared.Constants;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Extensions;
using bdDevs.Shared.Records.CRM;
using Domain.Contracts.Services;
using Domain.Contracts.Services.Core.Infrastructure;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Presentation.AuthorizeAttributes;
using Presentation.Controllers.BaseController;

namespace Presentation.Controllers.CRM;

/// <summary>
/// CRM Application management endpoints.
/// </summary>
[AuthorizeUser]
public class CrmApplicationController : BaseApiController
{
  private readonly IMemoryCache _cache;
  private readonly IFileUploadService _fileUploadService;

  public CrmApplicationController(IServiceManager serviceManager, IMemoryCache cache, IFileUploadService fileUploadService) : base(serviceManager)
  {
    _cache = cache;
    _fileUploadService = fileUploadService;
  }

  /// <summary>
  /// Retrieves paginated summary grid of applications.
  /// </summary>
  [HttpPost(RouteConstants.CrmApplicationSummary)]
  public async Task<IActionResult> ApplicationSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
  {
    if (options == null)
      throw new NullModelBadRequestException(nameof(GridOptions));
    var currentUser = await GetCurrentUserAsync();
    var menu = await ManageMenu.Async(this, cancellationToken: cancellationToken);

    var summaryGrid = await _serviceManager.CrmApplications.ApplicationsSummaryAsync(options, 0, currentUser, menu, cancellationToken);

    if (!summaryGrid.Items.Any())
      return Ok(ApiResponseHelper.Success(new GridEntity<CrmApplicationDto>(), "No applications found."));

    return Ok(ApiResponseHelper.Success(summaryGrid, "Application summary retrieved successfully"));
  }

  /// <summary>
  /// Creates a new application record using CRUD Record pattern.
  /// </summary>
  [HttpPost(RouteConstants.CreateCrmApplication)]
  [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
  public async Task<IActionResult> CreateApplicationAsync([FromBody] CreateCrmApplicationRecord record, CancellationToken cancellationToken = default)
  {
    var dto = record.MapTo<CrmApplicationDto>();
    var currentUser = await GetCurrentUserAsync();

    var createdApplication = await _serviceManager.CrmApplications.CreateApplicationAsync(dto, currentUser, cancellationToken);

    if (createdApplication.ApplicationId <= 0)
      throw new InvalidCreateOperationException("Failed to create application record.");

    return Ok(ApiResponseHelper.Created(createdApplication, "Application created successfully."));
  }

  /// <summary>
  /// Updates an existing application record using CRUD Record pattern.
  /// </summary>
  [HttpPut(RouteConstants.UpdateCrmApplication)]
  [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
  public async Task<IActionResult> UpdateApplicationAsync([FromRoute] int key, [FromBody] UpdateCrmApplicationRecord record, CancellationToken cancellationToken = default)
  {
    if (key != record.ApplicationId)
      throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmApplicationRecord));

    var currentUser = await GetCurrentUserAsync();
    var dto = record.MapTo<CrmApplicationDto>();
    //var updatedApplication = await _serviceManager.CrmApplications.UpdateApplicationAsync(key, dto, trackChanges: false, cancellationToken: cancellationToken);
    var updatedApplication = await _serviceManager.CrmApplications.UpdateApplicationAsync(key, dto, currentUser, trackChanges: false, cancellationToken: cancellationToken);

    return Ok(ApiResponseHelper.Updated(updatedApplication, "Application updated successfully."));
  }

  ///// <summary>
  ///// Deletes an application record using CRUD Record pattern.
  ///// </summary>
  //[HttpDelete(RouteConstants.DeleteCrmApplication)]
  //public async Task<IActionResult> DeleteApplicationAsync([FromRoute] int key, CancellationToken cancellationToken = default)
  //{
  //  var currentUser = await GetCurrentUserAsync();
  //  var deleteRecord = new DeleteCrmApplicationRecord(key);
  //  //await _serviceManager.CrmApplications.DeleteApplicationAsync(key, currentUser, trackChanges: false, cancellationToken: cancellationToken);
  //  await _serviceManager.CrmApplications.delete(key, currentUser, trackChanges: false, cancellationToken: cancellationToken);
  //  return Ok(ApiResponseHelper.NoContent<object>("Application deleted successfully"));
  //}

  /// <summary>
  /// Retrieves an application by ID.
  /// </summary>
  [HttpGet(RouteConstants.ReadCrmApplication)]
  public async Task<IActionResult> ApplicationAsync([FromRoute] int id, CancellationToken cancellationToken = default)
  {
    if (id <= 0)
      throw new IdParametersBadRequestException();

    var application = await _serviceManager.CrmApplications.ApplicationAsync(id, trackChanges: false, cancellationToken: cancellationToken);

    return Ok(ApiResponseHelper.Success(application, "Application retrieved successfully"));
  }

  /// <summary>
  /// Retrieves all applications.
  /// </summary>
  [HttpGet(RouteConstants.ReadCrmApplications)]
  public async Task<IActionResult> ApplicationsAsync(int applicationId, CancellationToken cancellationToken = default)
  {
    //var applications = await _serviceManager.CrmApplications.ApplicationsAsync(trackChanges: false, cancellationToken: cancellationToken);
    var applications = await _serviceManager.CrmApplications.ApplicationAsync(applicationId, trackChanges: false, cancellationToken: cancellationToken);

    return applications == null
      ? Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmApplicationDto>(), "No applications found."))
      : Ok(ApiResponseHelper.Success(applications, "Applications retrieved successfully"));
  }

  /// <summary>
  /// Uploads an application document and returns the saved relative file path.
  /// </summary>
  [HttpPost(RouteConstants.UploadCrmApplicationDocument)]
  public async Task<IActionResult> UploadDocumentAsync(
    [FromForm] IFormFile file,
    [FromForm] string? documentType,
    [FromForm] int? applicantId,
    [FromForm] int? applicationId,
    CancellationToken cancellationToken = default)
  {
    var safeDocumentType = string.IsNullOrWhiteSpace(documentType) ? "general" : documentType.Trim();
    var relativeFilePath = await _fileUploadService.SaveApplicationDocumentAsync(file, cancellationToken);
    var resolvedApplicantId = await ResolveApplicantIdAsync(applicantId, applicationId, cancellationToken);
    var currentUser = await GetCurrentUserAsync();

    var createdDocument = await _serviceManager.AdditionalDocuments.CreateAdditionalDocumentAsync(new AdditionalDocumentDto
    {
      ApplicantId = resolvedApplicantId,
      DocumentTitle = safeDocumentType,
      DocumentName = Path.GetFileName(file.FileName),
      DocumentPath = relativeFilePath,
      RecordType = safeDocumentType
    }, currentUser, cancellationToken);

    return Ok(ApiResponseHelper.Success(new
    {
      documentId = createdDocument.AdditionalDocumentId,
      filePath = relativeFilePath,
      fileName = createdDocument.DocumentName,
      documentType = safeDocumentType
    }, "Application document uploaded successfully"));
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

  /// <summary>
  /// Resolves the applicant identifier required for document persistence.
  /// </summary>
  private async Task<int> ResolveApplicantIdAsync(int? applicantId, int? applicationId, CancellationToken cancellationToken)
  {
    if (applicantId.HasValue && applicantId.Value > 0)
      return applicantId.Value;

    if (applicationId.HasValue && applicationId.Value > 0)
    {
      var application = await _serviceManager.CrmApplications.ApplicationAsync(applicationId.Value, trackChanges: false, cancellationToken: cancellationToken);
      if (application.ApplicantId > 0)
        return application.ApplicantId;
    }

    throw new BadRequestException("ApplicantId or ApplicationId is required for document upload.");
  }
}
