using Presentation.AuthorizeAttributes;
using Domain.Contracts.Services;
using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Domain.Exceptions;
using bdDevs.Shared.Constants;
using Application.Shared.Grid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Presentation.ActionFilters;
using bdDevs.Shared.Extensions;

namespace Presentation.Controllers.Core.SystemAdmin;

/// <summary>
/// Company management endpoints.
/// </summary>
[AuthorizeUser]
public class CompanyController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CompanyController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves all companies for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.CompaniesDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> CompaniesForDDLAsync(CancellationToken cancellationToken = default)
    {
        var companies = await _serviceManager.Companies.CompaniesForDDLAsync(cancellationToken: cancellationToken);

        if (!companies.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CompanyDDLDto>(), "No companies found."));

        return Ok(ApiResponseHelper.Success(companies, "Companies retrieved successfully"));
    }

    /// <summary>
    /// Creates a new company record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateCompany)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateCompanyAsync([FromBody] CreateCompanyRecord record, CancellationToken cancellationToken = default)
    {
        var dto = record.MapTo<CompanyDto>();
        var createdCompany = await _serviceManager.Companies.CreateAsync(dto, cancellationToken);

        if (createdCompany.CompanyId <= 0)
            throw new InvalidCreateOperationException("Failed to create company record.");

        return Ok(ApiResponseHelper.Created(createdCompany, "Company created successfully."));
    }

    /// <summary>
    /// Updates an existing company record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateCompany)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateCompanyAsync([FromRoute] int key, [FromBody] UpdateCompanyRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.CompanyId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCompanyRecord));

        var dto = record.MapTo<CompanyDto>();
        var updatedCompany = await _serviceManager.Companies.UpdateAsync(key, dto, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedCompany, "Company updated successfully."));
    }

    /// <summary>
    /// Deletes a company record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteCompany)]
    public async Task<IActionResult> DeleteCompanyAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteCompanyRecord(key);
        await _serviceManager.Companies.DeleteAsync(key, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Company deleted successfully"));
    }

    /// <summary>
    /// Retrieves a company by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadCompany)]
    public async Task<IActionResult> CompanyAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        if (key <= 0)
            throw new IdParametersBadRequestException();

        var company = await _serviceManager.Companies.CompanyAsync(key, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(company, "Company retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all companies.
    /// </summary>
    [HttpGet(RouteConstants.Companies)]
    public async Task<IActionResult> CompaniesAsync(CancellationToken cancellationToken = default)
    {
        var companies = await _serviceManager.Companies.CompaniesAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!companies.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CompanyDto>(), "No companies found."));

        return Ok(ApiResponseHelper.Success(companies, "Companies retrieved successfully"));
    }

    /// <summary>
    /// Retrieves companies by a collection of IDs.
    /// </summary>
    [HttpPost("companies-by-ids")]
    public async Task<IActionResult> CompaniesByIdsAsync([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken = default)
    {
        if (ids == null || !ids.Any())
            throw new IdParametersBadRequestException();

        var companies = await _serviceManager.Companies.CompaniesByIdsAsync(ids, trackChanges: false, cancellationToken: cancellationToken);

        if (!companies.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CompanyDto>(), "No companies found for the provided IDs."));

        return Ok(ApiResponseHelper.Success(companies, "Companies retrieved successfully"));
    }

    /// <summary>
    /// Retrieves mother company for the current user.
    /// </summary>
    [HttpGet("mother-company/{companyId:int}")]
    public async Task<IActionResult> MotherCompanyAsync([FromRoute] int companyId, CancellationToken cancellationToken = default)
    {
        if (companyId <= 0)
            throw new IdParametersBadRequestException();

        var currentUser = await GetCurrentUserAsync();
        var motherCompanies = await _serviceManager.Companies.MotherCompanyAsync(companyId, currentUser, cancellationToken);

        if (!motherCompanies.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CompanyDto>(), "No mother companies found."));

        return Ok(ApiResponseHelper.Success(motherCompanies, "Mother companies retrieved successfully"));
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
