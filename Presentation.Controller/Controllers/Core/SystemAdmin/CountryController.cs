using Presentation.AuthorizeAttributes;
using Domain.Contracts.Services;
using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Domain.Exceptions;
using bdDevs.Shared.Constants;
using Application.Shared.Grid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Presentation.ActionFilters;
using Presentation.LinkFactories;

namespace Presentation.Controllers.Core.SystemAdmin;

/// <summary>
/// Country management endpoints.
///
/// [AuthorizeUser] at class-level ensures:
///    - Every request validates user via attribute
///    - CurrentUser / CurrentUserId available from BaseApiController
///    - No auth checks needed in controller methods
///    - Exceptions handled by StandardExceptionMiddleware
/// </summary>
[AuthorizeUser]
public class CountryController : BaseApiController
{
    private readonly IMemoryCache _cache;
    private readonly ILinkFactory<CrmCountryDto> _linkFactory;

    public CountryController(IServiceManager serviceManager, IMemoryCache cache, ILinkFactory<CrmCountryDto> linkFactory) : base(serviceManager)
    {
        _cache = cache;
        _linkFactory = linkFactory;
    }

    /// <summary>
    /// Retrieves all countries for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.CountryDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> CountriesForDDLAsync(CancellationToken cancellationToken = default)
    {
        var countries = await _serviceManager.CrmCountries.CountriesForDDLAsync(trackChanges: false ,cancellationToken: cancellationToken);

        if (!countries.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmCountryDDL>(), "No countries found."));

        return Ok(ApiResponseHelper.Success(countries, "Countries retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of countries.
    /// </summary>
    [HttpPost(RouteConstants.CountrySummary)]
    public async Task<IActionResult> CountrySummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.CrmCountries.CountrySummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<CrmCountryDto>(), "No countries found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Country summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new country record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateCountry)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateCountryAsync([FromBody] CreateCountryRecord record, CancellationToken cancellationToken = default)
    {
        var createdCountry = await _serviceManager.CrmCountries.CreateAsync(record, cancellationToken);

        if (createdCountry.CountryId <= 0)
            throw new InvalidCreateOperationException("Failed to create country record.");

        return Ok(ApiResponseHelper.Created(createdCountry, "Country created successfully."));
    }

    /// <summary>
    /// Updates an existing country record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateCountry)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateCountryAsync([FromRoute] int key, [FromBody] UpdateCountryRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.CountryId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCountryRecord));

        var updatedCountry = await _serviceManager.CrmCountries.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedCountry, "Country updated successfully."));
    }

    /// <summary>
    /// Deletes a country record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteCountry)]
    public async Task<IActionResult> DeleteCountryAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteCountryRecord(key);
        await _serviceManager.CrmCountries.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Country deleted successfully"));
    }

    /// <summary>
    /// Retrieves a country by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadCountries)]
    public async Task<IActionResult> CountryAsync([FromRoute] int countryId, CancellationToken cancellationToken = default)
    {
        if (countryId <= 0)
            throw new IdParametersBadRequestException();

        var country = await _serviceManager.CrmCountries.CountryAsync(countryId, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(country, "Country retrieved successfully"));
    }
}
