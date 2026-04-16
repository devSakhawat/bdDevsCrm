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

namespace Presentation.Controllers.Core.SystemAdmin;

/// <summary>
/// CurrencyRate management endpoints.
/// </summary>
[AuthorizeUser]
public class CurrencyRateController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CurrencyRateController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves all currency rates for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.CurrencyRateDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> CurrencyRatesForDDLAsync(CancellationToken cancellationToken = default)
    {
        var currencyRates = await _serviceManager.CurrencyRates.CurrencyRatesForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!currencyRates.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CurrencyRateDDLDto>(), "No currency rates found."));

        return Ok(ApiResponseHelper.Success(currencyRates, "Currency rates retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of currency rates.
    /// </summary>
    [HttpPost(RouteConstants.CurrencyRateSummary)]
    public async Task<IActionResult> CurrencyRateSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.CurrencyRates.CurrencyRatesSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<CurrencyRateDto>(), "No currency rates found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Currency rate summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new currency rate record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateCurrencyRate)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateCurrencyRateAsync([FromBody] CreateCurrencyRateRecord record, CancellationToken cancellationToken = default)
    {
        var createdCurrencyRate = await _serviceManager.CurrencyRates.CreateAsync(record, cancellationToken);

        if (createdCurrencyRate.CurencyRateId <= 0)
            throw new InvalidCreateOperationException("Failed to create currency rate record.");

        return Ok(ApiResponseHelper.Created(createdCurrencyRate, "Currency rate created successfully."));
    }

    /// <summary>
    /// Updates an existing currency rate record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateCurrencyRate)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateCurrencyRateAsync([FromRoute] int key, [FromBody] UpdateCurrencyRateRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.CurencyRateId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCurrencyRateRecord));

        var updatedCurrencyRate = await _serviceManager.CurrencyRates.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedCurrencyRate, "Currency rate updated successfully."));
    }

    /// <summary>
    /// Deletes a currency rate record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteCurrencyRate)]
    public async Task<IActionResult> DeleteCurrencyRateAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteCurrencyRateRecord(key);
        await _serviceManager.CurrencyRates.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Currency rate deleted successfully"));
    }

    /// <summary>
    /// Retrieves a currency rate by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadCurrencyRate)]
    public async Task<IActionResult> CurrencyRateAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var currencyRate = await _serviceManager.CurrencyRates.CurrencyRateAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(currencyRate, "Currency rate retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all currency rates.
    /// </summary>
    [HttpGet(RouteConstants.ReadCurrencyRates)]
    public async Task<IActionResult> CurrencyRatesAsync(CancellationToken cancellationToken = default)
    {
        var currencyRates = await _serviceManager.CurrencyRates.CurrencyRatesAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!currencyRates.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CurrencyRateDto>(), "No currency rates found."));

        return Ok(ApiResponseHelper.Success(currencyRates, "Currency rates retrieved successfully"));
    }

    /// <summary>
    /// Retrieves currency rates by currency ID.
    /// </summary>
    [HttpGet(RouteConstants.CurrencyRatesByCurrency)]
    public async Task<IActionResult> CurrencyRatesByCurrencyAsync([FromRoute] int currencyId, CancellationToken cancellationToken = default)
    {
        if (currencyId <= 0)
            throw new IdParametersBadRequestException();

        var currencyRates = await _serviceManager.CurrencyRates.CurrencyRatesByCurrencyIdAsync(currencyId, trackChanges: false, cancellationToken: cancellationToken);

        if (!currencyRates.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CurrencyRateDto>(), "No currency rates found for this currency."));

        return Ok(ApiResponseHelper.Success(currencyRates, "Currency rates retrieved successfully"));
    }
}
