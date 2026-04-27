using Application.Shared.Grid;
using bdDevs.Shared;
using bdDevs.Shared.Constants;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Extensions;
using bdDevs.Shared.Records.CRM;
using Domain.Contracts.Services;
using Domain.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Presentation.AuthorizeAttributes;
using Presentation.Controllers.BaseController;
using System.IO;

namespace Presentation.Controllers.CRM;

/// <summary>
/// CRM Application management endpoints.
/// </summary>
[AuthorizeUser]
public class CrmApplicationController : BaseApiController
{
  private readonly IMemoryCache _cache;
  private readonly IWebHostEnvironment _environment;
  private static readonly HashSet<string> AllowedDocumentExtensions = new(StringComparer.OrdinalIgnoreCase) { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png" };
  private const long MaxUploadFileSizeBytes = 10 * 1024 * 1024;

  public CrmApplicationController(IServiceManager serviceManager, IMemoryCache cache, IWebHostEnvironment environment) : base(serviceManager)
  {
    _cache = cache;
    _environment = environment;
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
  public async Task<IActionResult> UploadDocumentAsync([FromForm] IFormFile file, [FromForm] string? documentType, CancellationToken cancellationToken = default)
  {
    ValidateFile(file);

    var safeDocumentType = string.IsNullOrWhiteSpace(documentType) ? "general" : documentType.Trim();
    var relativeFilePath = await SaveFileAsync(file, Path.Combine("Uploads", "crm", "applications", "documents"), cancellationToken);

    return Ok(ApiResponseHelper.Success(new
    {
      documentId = 0,
      filePath = relativeFilePath,
      fileName = file.FileName,
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
  /// Validates an uploaded document file.
  /// </summary>
  private static void ValidateFile(IFormFile? file)
  {
    if (file is null || file.Length <= 0)
      throw new BadRequestException("A valid document file is required.");

    if (file.Length > MaxUploadFileSizeBytes)
      throw new BadRequestException("Document file size must be 10 MB or less.");

    var extension = Path.GetExtension(file.FileName);
    if (string.IsNullOrWhiteSpace(extension) || !AllowedDocumentExtensions.Contains(extension))
      throw new BadRequestException("Unsupported document file type.");
  }

  /// <summary>
  /// Saves an uploaded file and returns the public relative path.
  /// </summary>
  private async Task<string> SaveFileAsync(IFormFile file, string relativeDirectory, CancellationToken cancellationToken)
  {
    var webRootPath = string.IsNullOrWhiteSpace(_environment.WebRootPath)
      ? Path.Combine(_environment.ContentRootPath, "wwwroot")
      : _environment.WebRootPath;

    var directoryPath = Path.Combine(webRootPath, relativeDirectory);
    Directory.CreateDirectory(directoryPath);

    var extension = Path.GetExtension(file.FileName);
    var fileName = $"{Guid.NewGuid():N}{extension}";
    var fullPath = Path.Combine(directoryPath, fileName);

    await using var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None);
    await file.CopyToAsync(stream, cancellationToken);

    return "/" + Path.Combine(relativeDirectory, fileName).Replace("\\", "/");
  }
}
