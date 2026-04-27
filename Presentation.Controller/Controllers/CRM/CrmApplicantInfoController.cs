using Presentation.AuthorizeAttributes;
using Domain.Contracts.Services;
using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;
using Domain.Contracts.Services.Core.Infrastructure;
using Domain.Exceptions;
using bdDevs.Shared.Constants;
using Application.Shared.Grid;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Presentation.ActionFilters;
using bdDevs.Shared.Extensions;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

namespace Presentation.Controllers.CRM;

/// <summary>
/// CRM Applicant Info management endpoints.
/// </summary>
[AuthorizeUser]
public class CrmApplicantInfoController : BaseApiController
{
    private readonly IMemoryCache _cache;
    private readonly IFileUploadService _fileUploadService;

    public CrmApplicantInfoController(IServiceManager serviceManager, IMemoryCache cache, IFileUploadService fileUploadService) : base(serviceManager)
    {
        _cache = cache;
        _fileUploadService = fileUploadService;
    }

    /// <summary>
    /// Retrieves all applicant infos for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.CrmApplicantInfoDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> ApplicantInfosForDDLAsync(CancellationToken cancellationToken = default)
    {
        var applicantInfos = await _serviceManager.ApplicantInfos.ApplicantInfoForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!applicantInfos.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<ApplicantInfoDto>(), "No applicant infos found."));

        return Ok(ApiResponseHelper.Success(applicantInfos, "Applicant infos retrieved successfully"));
    }

    /// <summary>
    /// Retrieves genders for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.GenderDDL)]
    [ResponseCache(Duration = 300)]
    public IActionResult GendersForDDL()
    {
        var genders = new List<GenderDDLDto>
        {
            new() { GenderId = 1, GenderName = "Male" },
            new() { GenderId = 2, GenderName = "Female" },
            new() { GenderId = 3, GenderName = "Other" }
        };

        return Ok(ApiResponseHelper.Success(genders, "Genders retrieved successfully"));
    }

    /// <summary>
    /// Uploads applicant photo and returns the saved relative file path.
    /// </summary>
    [HttpPost(RouteConstants.UploadCrmApplicantPhoto)]
    public async Task<IActionResult> UploadPhotoAsync([FromForm] IFormFile file, CancellationToken cancellationToken = default)
    {
        var relativeFilePath = await _fileUploadService.SaveApplicantPhotoAsync(file, cancellationToken);

        return Ok(ApiResponseHelper.Success(new { filePath = relativeFilePath }, "Applicant photo uploaded successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of applicant infos.
    /// </summary>
    [HttpPost(RouteConstants.CrmApplicantInfoSummary)]
    public async Task<IActionResult> ApplicantInfoSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.ApplicantInfos.ApplicantInfosSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<ApplicantInfoDto>(), "No applicant infos found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Applicant info summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new applicant info record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateCrmApplicantInfo)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateApplicantInfoAsync([FromBody] CreateCrmApplicantInfoRecord record, CancellationToken cancellationToken = default)
    {
        var dto = record.MapTo<ApplicantInfoDto>();
        var currentUser = await GetCurrentUserAsync();

        var createdApplicantInfo = await _serviceManager.ApplicantInfos.CreateApplicantInfoAsync(dto, currentUser, cancellationToken);

        if (createdApplicantInfo.ApplicantId <= 0)
            throw new InvalidCreateOperationException("Failed to create applicant info record.");

        return Ok(ApiResponseHelper.Created(createdApplicantInfo, "Applicant info created successfully."));
    }

    /// <summary>
    /// Updates an existing applicant info record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateCrmApplicantInfo)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateApplicantInfoAsync([FromRoute] int key, [FromBody] UpdateCrmApplicantInfoRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.ApplicantId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmApplicantInfoRecord));

        var dto = record.MapTo<ApplicantInfoDto>();
        var updatedApplicantInfo = await _serviceManager.ApplicantInfos.UpdateApplicantInfoAsync(key, dto, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedApplicantInfo, "Applicant info updated successfully."));
    }

    /// <summary>
    /// Deletes an applicant info record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteCrmApplicantInfo)]
    public async Task<IActionResult> DeleteApplicantInfoAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteCrmApplicantInfoRecord(key);
        var dto = new ApplicantInfoDto { ApplicantId = key };
        await _serviceManager.ApplicantInfos.DeleteApplicantInfoAsync(key, dto, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Applicant info deleted successfully"));
    }

    /// <summary>
    /// Retrieves an applicant info by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadCrmApplicantInfo)]
    public async Task<IActionResult> ApplicantInfoAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var applicantInfo = await _serviceManager.ApplicantInfos.ApplicantInfoAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(applicantInfo, "Applicant info retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all applicant infos.
    /// </summary>
    [HttpGet(RouteConstants.ReadCrmApplicantInfos)]
    public async Task<IActionResult> ApplicantInfosAsync(CancellationToken cancellationToken = default)
    {
        var applicantInfos = await _serviceManager.ApplicantInfos.ApplicantInfosAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!applicantInfos.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<ApplicantInfoDto>(), "No applicant infos found."));

        return Ok(ApiResponseHelper.Success(applicantInfos, "Applicant infos retrieved successfully"));
    }

    /// <summary>
    /// Retrieves applicant info by application ID.
    /// </summary>
    [HttpGet(RouteConstants.CrmApplicantInfoByApplicationId)]
    public async Task<IActionResult> ApplicantInfoByApplicationIdAsync([FromRoute] int applicationId, CancellationToken cancellationToken = default)
    {
        if (applicationId <= 0)
            throw new IdParametersBadRequestException();

        var applicantInfo = await _serviceManager.ApplicantInfos.ApplicantInfoByApplicationIdAsync(applicationId, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(applicantInfo, "Applicant info retrieved successfully"));
    }

    /// <summary>
    /// Retrieves applicant info by email.
    /// </summary>
    [HttpGet(RouteConstants.CrmApplicantInfoByEmail)]
    public async Task<IActionResult> ApplicantInfoByEmailAsync([FromQuery] string email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new BadRequestException("Email parameter is required.");

        var applicantInfo = await _serviceManager.ApplicantInfos.ApplicantInfoByEmailAsync(email, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(applicantInfo, "Applicant info retrieved successfully"));
    }

    /// <summary>
    /// Helper method to get current user from service.
    /// </summary>
    private async Task<UsersDto> GetCurrentUserAsync()
    {
        // Get current user ID from HttpContext (assuming it's set by authentication middleware)
        var userId = User?.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int parsedUserId))
        {
            // Return a default user or throw exception based on requirements
            return new UsersDto { UserId = 1, Username = "system" }; // Default for now
        }

        return new UsersDto { UserId = parsedUserId };
    }
}
