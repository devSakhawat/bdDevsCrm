using bdDevCRM.Presentation.AuthorizeAttributes;
using bdDevCRM.ServicesContract;
using bdDevCRM.Shared.ApiResponse;
using bdDevCRM.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevCRM.Shared.Exceptions;
using bdDevCRM.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Presentation.ActionFIlters;
using Presentation.LinkFactories;

namespace Presentation.Controllers.Core.SystemAdmin;

/// <summary>
/// Company management endpoints.
///
/// [AuthorizeUser] at class-level ensures:
///    - Every request validates user via attribute
///    - CurrentUser / CurrentUserId available from BaseApiController
///    - No auth checks needed in controller methods
///    - Exceptions handled by StandardExceptionMiddleware
/// </summary>
[AuthorizeUser]
public class CompaniesController : BaseApiController
{
    private readonly IMemoryCache _cache;
    private readonly ILinkFactory<CompanyDto> _linkFactory;

    public CompaniesController(IServiceManager serviceManager, IMemoryCache cache, ILinkFactory<CompanyDto> linkFactory) : base(serviceManager)
    {
        _cache = cache;
        _linkFactory = linkFactory;
    }

    /// <summary>
    /// Retrieves all companies.
    /// </summary>
    [HttpGet(RouteConstants.Companies)]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> CompaniesAsync(CancellationToken cancellationToken = default)
    {
        var companies = await _serviceManager.Companies.CompaniesAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!companies.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CompanyDto>(), "No companies found."));

        return Ok(ApiResponseHelper.Success(companies, "Companies retrieved successfully"));
    }

    /// <summary>
    /// Retrieves a specific company by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadCompany)]
    public async Task<IActionResult> CompanyAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var company = await _serviceManager.Companies.CompanyAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(company, "Company retrieved successfully"));
    }

    /// <summary>
    /// Creates a new company.
    /// </summary>
    [HttpPost(RouteConstants.CreateCompany)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateCompanyAsync([FromBody] CompanyDto modelDto, CancellationToken cancellationToken = default)
    {
        var model = await _serviceManager.Companies.CreateAsync(modelDto, cancellationToken);

        if (model.CompanyId <= 0)
            throw new InvalidCreateOperationException("Failed to create company record.");

        return Ok(ApiResponseHelper.Created(model, "Company created successfully."));
    }

    /// <summary>
    /// Updates an existing company.
    /// </summary>
    [HttpPut(RouteConstants.UpdateCompany)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateCompanyAsync([FromRoute] int key, [FromBody] CompanyDto modelDto, CancellationToken cancellationToken = default)
    {
        CompanyDto returnData = await _serviceManager.Companies.UpdateAsync(key, modelDto, trackChanges: false, cancellationToken: cancellationToken);

        if (returnData.CompanyId <= 0)
            throw new InvalidUpdateOperationException("Failed to update company record.");

        return Ok(ApiResponseHelper.Updated(returnData, "Company updated successfully."));
    }

    /// <summary>
    /// Deletes a company by ID.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteCompany)]
    public async Task<IActionResult> DeleteCompanyAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.Companies.DeleteAsync(key, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Company deleted successfully"));
    }

    /// <summary>
    /// Retrieves companies by a collection of IDs.
    /// </summary>
    [HttpGet("collection/({ids})", Name = "CompanyCollection")]
    public async Task<IActionResult> CompanyCollectionAsync([FromRoute] IEnumerable<int> ids)
    {
        if (ids == null || !ids.Any())
            throw new IdParametersBadRequestException();

        var companies = await _serviceManager.Companies.CompaniesByIdsAsync(ids, trackChanges: false);

        if (!companies.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CompanyDto>(), "No companies found."));

        return Ok(ApiResponseHelper.Success(companies, "Companies retrieved successfully"));
    }

    /// <summary>
    /// Retrieves mother company for the current user.
    /// </summary>
    [HttpGet(RouteConstants.MotherCompany)]
    public async Task<IActionResult> MotherCompanyAsync()
    {
        var currentUser = CurrentUser;

        if (currentUser?.HrRecordId == 0 || currentUser?.HrRecordId == null)
            throw new IdParametersBadRequestException();

        int companyId = (int)currentUser.CompanyId;

        var res = await _serviceManager.Companies.MotherCompanyAsync(companyId, currentUser);

        if (!res.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CompanyDto>(), "No company found."));

        return Ok(ApiResponseHelper.Success(res, "Companies retrieved successfully"));
    }

    /// <summary>
    /// Retrieves companies for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.CompaniesDDL)]
    public async Task<IActionResult> CompaniesForDDLAsync()
    {
        var companies = await _serviceManager.Companies.CompaniesForDDLAsync();

        if (!companies.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CompanyDDLDto>(), "No companies found."));

        return Ok(ApiResponseHelper.Success(companies, "Companies retrieved successfully"));
    }
}
