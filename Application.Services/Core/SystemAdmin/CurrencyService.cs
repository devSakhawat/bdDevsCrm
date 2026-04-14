using Application.Services.Mappings;
using Application.Shared.Grid;
using Application.Services.Mappings;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services.Core.SystemAdmin;
using Domain.Entities.Entities.CRM;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.Core.SystemAdmin;

/// <summary>
/// Currency service implementing business logic for currency management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CurrencyService : ICurrencyService
{
  private readonly IRepositoryManager _repository;
  private readonly ILogger<CurrencyService> _logger;
  private readonly IConfiguration _configuration;

  public CurrencyService(IRepositoryManager repository, ILogger<CurrencyService> logger, IConfiguration configuration)
  {
    _repository = repository;
    _logger = logger;
    _configuration = configuration;
  }

  /// <summary>
  /// Retrieves all currencies for dropdown list asynchronously.
  /// </summary>
  public async Task<IEnumerable<CurrencyDDLDto>> CurrenciesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
  {
    _logger.LogInformation("Fetching currencies for dropdown list");

    var currencies = await _repository.Currencies.ListWithSelectAsync(
        selector: x => new CrmCurrencyInfo
        {
          CurrencyId = x.CurrencyId,
          CurrencyName = x.CurrencyName
        },
        orderBy: x => x.CurrencyName,
        trackChanges: false
    );

    var currencyDtos = MyMapper.JsonCloneIEnumerableToList<CrmCurrencyInfo, CurrencyDDLDto>(currencies);
    return currencyDtos;
  }


  /// <summary>
  /// Retrieves all currencies asynchronously.
  /// </summary>
  public async Task<IEnumerable<CurrencyDto>> CurrenciesAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
  {
    _logger.LogInformation("Fetching currencies for dropdown list");

    var currencies = await _repository.Currencies.ListAsync(
        orderBy: x => x.CurrencyName,
        trackChanges: false,
        cancellationToken: cancellationToken
    );

    var currencyDtos = MyMapper.JsonCloneIEnumerableToList<CrmCurrencyInfo, CurrencyDto>(currencies);
    return currencyDtos;
  }

  /// <summary>
  /// Retrieves paginated summary grid of currencies asynchronously.
  /// </summary>
  public async Task<GridEntity<CurrencyDto>> CurrencySummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
  {
    _logger.LogInformation("Fetching currency summary grid");

    string query = "SELECT * FROM CrmCurrencyInfo";
    string orderBy = "CurrencyName ASC";

    var gridEntity = await _repository.Currencies.AdoGridDataAsync<CurrencyDto>(query, options, orderBy, "", cancellationToken);

    return gridEntity;
  }

  /// <summary>
  /// Creates a new currency asynchronously.
  /// </summary>
  public async Task<CurrencyDto> CreateAsync(CurrencyDto modelDto, CancellationToken cancellationToken = default)
  {
    if (modelDto == null)
    {
      throw new BadRequestException("Id mismatch", "ID_MISMATCH");
    }
    //throw new badre(nameof(CurrencyDto));

    if (modelDto.CurrencyId != 0)
      throw new InvalidOperationException("CurrencyId must be 0 for creating a new currency.");

    _logger.LogInformation("Creating new currency: {CurrencyName}", modelDto.CurrencyName);

    // Check if default currency already exists
    if (modelDto.IsDefault == 1)
    {
      bool defaultExists = await _repository.Currencies.ExistsAsync(
          x => x.IsDefault == 1, cancellationToken: cancellationToken);

      if (defaultExists)
        throw new BadRequestException("Only one currency can be marked as default.");
    }

    // Check for duplicate currency name
    bool currencyExists = await _repository.Currencies.ExistsAsync(
        x => x.CurrencyName.Trim().ToLower() == modelDto.CurrencyName.Trim().ToLower(), cancellationToken: cancellationToken);

    if (currencyExists)
      throw new ConflictException("Currency", "CurrencyName");

    // Map and create
    CrmCurrencyInfo currency = MyMapper.JsonClone<CurrencyDto, CrmCurrencyInfo>(modelDto);
    modelDto.CurrencyId = await _repository.Currencies.CreateAndIdAsync(currency, cancellationToken);
    await _repository.SaveAsync(cancellationToken);

    _logger.LogInformation("Currency created successfully with ID: {CurrencyId}", modelDto.CurrencyId);

    return modelDto;
  }

  /// <summary>
  /// Updates an existing currency asynchronously.
  /// </summary>
  public async Task<CurrencyDto> UpdateAsync(int key, CurrencyDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (modelDto == null)
      throw new BadRequestException(nameof(CurrencyDto));

    if (key != modelDto.CurrencyId)
      throw new BadRequestException(key.ToString(), nameof(CurrencyDto));

    _logger.LogInformation("Updating currency with ID: {CurrencyId}", key);

    // Check if currency exists
    var existingCurrency = await _repository.Currencies.ByIdAsync(
        x => x.CurrencyId == key, trackChanges: false, cancellationToken: cancellationToken);

    if (existingCurrency == null)
      throw new NotFoundException("Data not found ");

    // Check if default currency already exists (excluding current record)
    if (modelDto.IsDefault == 1)
    {
      bool defaultExists = await _repository.Currencies.ExistsAsync(
          x => x.IsDefault == 1 && x.CurrencyId != key, cancellationToken: cancellationToken);

      if (defaultExists)
        throw new BadRequestException("Only one currency can be marked as default.");
    }

    // Check for duplicate name (excluding current record)
    bool duplicateExists = await _repository.Currencies.ExistsAsync(
        x => x.CurrencyName.Trim().ToLower() == modelDto.CurrencyName.Trim().ToLower()
             && x.CurrencyId != key, cancellationToken: cancellationToken);

    if (duplicateExists)
      throw new ConflictException("Duplicate Data Found!");

    // Map and update
    CrmCurrencyInfo currency = MyMapper.JsonClone<CurrencyDto, CrmCurrencyInfo>(modelDto);
    _repository.Currencies.UpdateByState(currency);
    await _repository.SaveAsync(cancellationToken);

    _logger.LogInformation("Currency updated successfully: {CurrencyId}", key);

    return modelDto;
  }

  /// <summary>
  /// Deletes a currency by ID asynchronously.
  /// </summary>
  public async Task DeleteAsync(int key, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (key <= 0)
      throw new BadRequestException("Invalid request!");

    _logger.LogInformation("Deleting currency with ID: {CurrencyId}", key);

    var currency = await _repository.Currencies.ByIdAsync(
        x => x.CurrencyId == key, trackChanges: false, cancellationToken: cancellationToken);

    if (currency == null)
      throw new NotFoundException("Data not found!");

    await _repository.Currencies.DeleteAsync(x => x.CurrencyId == key, trackChanges: false, cancellationToken: cancellationToken);
    await _repository.SaveAsync(cancellationToken);

    _logger.LogInformation("Currency deleted successfully: {CurrencyId}", key);
  }

  /// <summary>
  /// Retrieves a currency by ID asynchronously.
  /// </summary>
  public async Task<CurrencyDto> CurrencyAsync(int currencyId, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (currencyId <= 0)
    {
      _logger.LogWarning("CurrencyAsync called with invalid id: {CurrencyId}", currencyId);
      throw new BadRequestException("Invalid request!");
    }

    _logger.LogInformation("Fetching currency with ID: {CurrencyId}", currencyId);

    var currency = await _repository.Currencies.CurrencyAsync(currencyId, trackChanges, cancellationToken);

    if (currency == null)
    {
      _logger.LogWarning("Currency not found with ID: {CurrencyId}", currencyId);
      throw new NotFoundException("Data not found!");
    }

    var currencyDto = MyMapper.JsonClone<CrmCurrencyInfo, CurrencyDto>(currency);
    return currencyDto;
  }
}
