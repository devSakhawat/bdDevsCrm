using Presentation.AuthorizeAttributes;
using Domain.Contracts.Services;
using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Domain.Exceptions;
using bdDevs.Shared.Constants;
using Application.Shared.Grid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Presentation.ActionFilters;
using Presentation.LinkFactories;

namespace Presentation.Controllers.Core.SystemAdmin;

/// <summary>
/// Currency management endpoints.
/// [AuthorizeUser] at class-level ensures:
///    - Every request validates user via attribute
///    - CurrentUser / CurrentUserId available from BaseApiController
///    - No auth checks needed in controller methods
///    - Exceptions handled by StandardExceptionMiddleware
/// </summary>
[AuthorizeUser]
public class CurrencyController : BaseApiController
{
	private readonly IMemoryCache _cache;
	private readonly ILinkFactory<CurrencyDto> _linkFactory;

	public CurrencyController(IServiceManager serviceManager, IMemoryCache cache, ILinkFactory<CurrencyDto> linkFactory) : base(serviceManager)
	{
		_cache = cache;
		_linkFactory = linkFactory;
	}

	#region ── CUD Operations (First) ─────────────────────────────
	/// <summary>
	/// Creates a new currency record.
	/// POST: /bdDevs-crm/currency
	/// </summary>
	[HttpPost(RouteConstants.CreateCurrency)]
	[ServiceFilter(typeof(EmptyObjectFilterAttribute))]
	public async Task<IActionResult> CreateCurrencyAsync( [FromBody] CurrencyDto modelDto, CancellationToken cancellationToken = default)
	{
		var createdCurrency = await _serviceManager.Currencies.CreateAsync(modelDto, cancellationToken: cancellationToken);
		if (createdCurrency.CurrencyId <= 0)
			throw new InvalidCreateOperationException("Failed to create currency record.");

		return Ok(ApiResponseHelper.Created(createdCurrency, "Currency created successfully."));
	}

	/// <summary>
	/// Updates an existing currency record.
	/// PUT: /bdDevs-crm/currency/{key}
	/// </summary>
	[HttpPut(RouteConstants.UpdateCurrency)]
	[ServiceFilter(typeof(EmptyObjectFilterAttribute))]
	public async Task<IActionResult> UpdateCurrencyAsync([FromRoute] int key, [FromBody] CurrencyDto modelDto, CancellationToken cancellationToken = default)
	{
		if (key != modelDto.CurrencyId)
			throw new IdMismatchBadRequestException(key.ToString(), nameof(CurrencyDto));

		var updatedCurrency = await _serviceManager.Currencies.UpdateAsync(key, modelDto, trackChanges: false, cancellationToken: cancellationToken);
		if (updatedCurrency.CurrencyId <= 0)
			throw new InvalidUpdateOperationException("Failed to update currency record.");

		return Ok(ApiResponseHelper.Updated(updatedCurrency, "Currency updated successfully."));
	}


	/// <summary>
	/// Deletes a currency record.
	/// DELETE: /bdDevs-crm/currency/{key}
	/// </summary>
	[HttpDelete(RouteConstants.DeleteCurrency)]
	public async Task<IActionResult> DeleteCurrencyAsync([FromRoute] int key, CancellationToken cancellationToken = default)
	{
		await _serviceManager.Currencies.DeleteAsync(key, trackChanges: false, cancellationToken: cancellationToken);
		return Ok(ApiResponseHelper.NoContent<object>("Currency deleted successfully"));
	}
	#endregion ── CUD Operations End ──────────────────────────────


	#region ── Read Operations (High to Low Data Volume) ─────────

	/// <summary>
	/// Retrieves paginated summary grid of currencies.
	/// [Largest Data Volume] POST: /bdDevs-crm/currency-summary
	/// </summary>
	[HttpPost(RouteConstants.CurrencySummary)]
	public async Task<IActionResult> CurrencySummaryAsync([FromBody] CRMGridOptions options, CancellationToken cancellationToken = default)
	{
		if (options == null)
			throw new NullModelBadRequestException(nameof(CRMGridOptions));

		var currencySummary = await _serviceManager.Currencies.CurrencySummaryAsync( options, cancellationToken: cancellationToken);

		if (!currencySummary.Items.Any())
			return Ok(ApiResponseHelper.Success(new GridEntity<CurrencyDto>(), "No currencies found."));

		return Ok(ApiResponseHelper.Success(currencySummary, "Currency summary retrieved successfully"));
	}


	/// <summary>
	/// Retrieves all currencies (list view).
	/// [Medium Data Volume] GET: /bdDevs-crm/currencies
	/// </summary>
	[HttpGet(RouteConstants.ReadCurrencies)]
	[ResponseCache(Duration = 60)]
	public async Task<IActionResult> ReadCurrenciesAsync(CancellationToken cancellationToken = default)
	{
		IEnumerable<CurrencyDto> currencies = await _serviceManager.Currencies.CurrenciesAsync(trackChanges: false, cancellationToken: cancellationToken);

		if (!currencies.Any())
			return Ok(ApiResponseHelper.Success(
					Enumerable.Empty<CurrencyDto>(),
					"No currencies found."));

		return Ok(ApiResponseHelper.Success(
				currencies,
				"Currencies retrieved successfully"));
	}

	/// <summary>
	/// Retrieves currencies for dropdown list.
	/// [Small Data Volume - DDL] GET: /bdDevs-crm/currencies-ddl
	/// </summary>
	[HttpGet(RouteConstants.CurrencyDDL)]
	[ResponseCache(Duration = 300)]
	public async Task<IActionResult> CurrenciesForDDLAsync(CancellationToken cancellationToken = default)
	{
		var currencies = await _serviceManager.Currencies.CurrenciesForDDLAsync(
				trackChanges: false,
				cancellationToken: cancellationToken);

		if (!currencies.Any())
			return Ok(ApiResponseHelper.Success(
					Enumerable.Empty<CurrencyDDLDto>(),
					"No currencies found."));

		return Ok(ApiResponseHelper.Success(
				currencies,
				"Currencies retrieved successfully"));
	}

	/// <summary>
	/// Retrieves a currency by ID.
	/// [Smallest Data Volume - Single] GET: /bdDevs-crm/currency/{id:int}
	/// </summary>
	[HttpGet(RouteConstants.ReadCurrency)]
	[ResponseCache(Duration = 60)]
	public async Task<IActionResult> CurrencyAsync(
			[FromRoute] int id,
			CancellationToken cancellationToken = default)
	{
		if (id <= 0)
			throw new IdParametersBadRequestException();

		var currency = await _serviceManager.Currencies.CurrencyAsync( id, trackChanges: false, cancellationToken: cancellationToken);

		if (currency == null)
			return Ok(ApiResponseHelper.Success<CurrencyDto>(null ,"Currency not found."));

		return Ok(ApiResponseHelper.Success(
				currency,
				"Currency retrieved successfully"));
	}

	#endregion ── Read Operations End ─────────────────────────────

	///// <summary>
	///// Retrieves all currencies for dropdown list.
	///// </summary>
	//[HttpGet(RouteConstants.CurrencyDDL)]
	//[ResponseCache(Duration = 300)]
	//public async Task<IActionResult> CurrenciesForDDLAsync(CancellationToken cancellationToken = default)
	//{
	//	var currencies = await _serviceManager.Currencies.CurrenciesForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

	//	if (!currencies.Any())
	//		return Ok(ApiResponseHelper.Success(Enumerable.Empty<CurrencyDDLDto>(), "No currencies found."));

	//	return Ok(ApiResponseHelper.Success(currencies, "Currencies retrieved successfully"));
	//}

	///// <summary>
	///// Retrieves paginated summary grid of currencies.
	///// </summary>
	//[HttpPost(RouteConstants.CurrencySummary)]
	//public async Task<IActionResult> CurrencySummaryAsync([FromBody] CRMGridOptions options)
	//{
	//	if (options == null)
	//		throw new NullModelBadRequestException(nameof(CRMGridOptions));

	//	var currencySummary = await _serviceManager.Currencies.CurrencySummaryAsync(options);

	//	if (!currencySummary.Items.Any())
	//		return Ok(ApiResponseHelper.Success(new GridEntity<CurrencyDto>(), "No currencies found."));

	//	return Ok(ApiResponseHelper.Success(currencySummary, "Currency summary retrieved successfully"));
	//}

	///// <summary>
	///// Creates a new currency record.
	///// </summary>
	//[HttpPost(RouteConstants.CreateCurrency)]
	//[ServiceFilter(typeof(EmptyObjectFilterAttribute))]
	//public async Task<IActionResult> CreateCurrencyAsync([FromBody] CurrencyDto modelDto)
	//{
	//	var createdCurrency = await _serviceManager.Currencies.CreateAsync(modelDto);

	//	if (createdCurrency.CurrencyId <= 0)
	//		throw new InvalidCreateOperationException("Failed to create currency record.");

	//	return Ok(ApiResponseHelper.Created(createdCurrency, "Currency created successfully."));
	//}

	///// <summary>
	///// Updates an existing currency record.
	///// </summary>
	//[HttpPut(RouteConstants.UpdateCurrency)]
	//[ServiceFilter(typeof(EmptyObjectFilterAttribute))]
	//public async Task<IActionResult> UpdateCurrencyAsync([FromRoute] int key, [FromBody] CurrencyDto modelDto)
	//{
	//	if (key != modelDto.CurrencyId)
	//		throw new IdMismatchBadRequestException(key.ToString(), nameof(CurrencyDto));

	//	var updatedCurrency = await _serviceManager.Currencies.UpdateAsync(key, modelDto);

	//	return Ok(ApiResponseHelper.Updated(updatedCurrency, "Currency updated successfully."));
	//}

	///// <summary>
	///// Deletes a currency record.
	///// </summary>
	//[HttpDelete(RouteConstants.DeleteCurrency)]
	//public async Task<IActionResult> DeleteCurrencyAsync([FromRoute] int key)
	//{
	//	await _serviceManager.Currencies.DeleteAsync(key);
	//	return Ok(ApiResponseHelper.NoContent<object>("Currency deleted successfully"));
	//}

	///// <summary>
	///// Retrieves a currency by ID.
	///// </summary>
	//[HttpGet(RouteConstants.ReadCurrency)]
	//public async Task<IActionResult> CurrencyAsync([FromRoute] int id)
	//{
	//	if (id <= 0)
	//		throw new IdParametersBadRequestException();

	//	var currency = await _serviceManager.Currencies.CurrencyAsync(id, trackChanges: false);

	//	return Ok(ApiResponseHelper.Success(currency, "Currency retrieved successfully"));
	//}


}
