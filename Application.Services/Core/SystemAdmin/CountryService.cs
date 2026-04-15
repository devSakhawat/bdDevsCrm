using Application.Services.Mappings;
using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.CRM;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services.Core.SystemAdmin;
using Domain.Entities.Entities.CRM;
using Domain.Exceptions;
using Domain.Exceptions.DomainSpecific;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.Core.SystemAdmin;

/// <summary>
/// Country service implementing business logic for country management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CrmCountryService : ICrmCountryService
{
  private readonly IRepositoryManager _repository;
  private readonly ILogger<CrmCountryService> _logger;
  private readonly IConfiguration _configuration;

  public CrmCountryService(IRepositoryManager repository, ILogger<CrmCountryService> logger, IConfiguration configuration)
  {
    _repository = repository;
    _logger = logger;
    _configuration = configuration;
  }

  /// <summary>
  /// Retrieves all countries for dropdown list asynchronously.
  /// </summary>
  public async Task<IEnumerable<CrmCountryDDL>> CountriesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
  {
    _logger.LogInformation("Fetching countries for dropdown list");

    var countries = await _repository.Countries.ActiveCountriesAsync(trackChanges, cancellationToken);

    if (!countries.Any())
    {
      _logger.LogWarning("No countries found for dropdown");
      return Enumerable.Empty<CrmCountryDDL>();
    }

    var countryDtos = MyMapper.JsonCloneIEnumerableToList<CrmCountry, CrmCountryDDL>(countries);
    return countryDtos;
  }

  /// <summary>
  /// Retrieves paginated summary grid of countries asynchronously.
  /// </summary>
  public async Task<GridEntity<CrmCountryDto>> CountrySummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
  {
    _logger.LogInformation("Fetching country summary grid");

    string query = "SELECT * FROM CrmCountry";
    string orderBy = "CountryName ASC";

    var gridEntity = await _repository.Countries.AdoGridDataAsync<CrmCountryDto>(query, options, orderBy, "", cancellationToken);

    return gridEntity;
  }

  /// <summary>
  /// Creates a new country asynchronously.
  /// </summary>
  public async Task<CrmCountryDto> CreateAsync(CrmCountryDto modelDto, CancellationToken cancellationToken = default)
  {
    if (modelDto == null)
      throw new BadRequestException(nameof(CrmCountryDto));

    if (modelDto.CountryId != 0)
      throw new InvalidOperationExceptionEx("CountryId must be 0 for creating a new country.");

    _logger.LogInformation("Creating new country: {CountryName}", modelDto.CountryName);

    // Check for duplicate country name
    bool countryExists = await _repository.Countries.ExistsAsync(
        x => x.CountryName.Trim().ToLower() == modelDto.CountryName.Trim().ToLower());

    if (countryExists)
      throw new ConflictException("Data not found!");

    // Map and create
    CrmCountry country = MyMapper.JsonClone<CrmCountryDto, CrmCountry>(modelDto);
    modelDto.CountryId = await _repository.Countries.CreateAndIdAsync(country, cancellationToken);
    await _repository.SaveAsync(cancellationToken);

    _logger.LogInformation("Country created successfully with ID: {CountryId}", modelDto.CountryId);

    return modelDto;
  }

  /// <summary>
  /// Updates an existing country asynchronously.
  /// </summary>
  public async Task<CrmCountryDto> UpdateAsync(int key, CrmCountryDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (modelDto == null)
      throw new BadRequestException(nameof(CrmCountryDto));

    if (key != modelDto.CountryId)
      throw new BadRequestException(key.ToString(), nameof(CrmCountryDto));

    _logger.LogInformation("Updating country with ID: {CountryId}", key);

    // Check if country exists
    var existingCountry = await _repository.Countries.ByIdAsync(
        x => x.CountryId == key, trackChanges: false, cancellationToken: cancellationToken);

    if (existingCountry == null)
      throw new NotFoundException("Data not found!");

    // Check for duplicate name (excluding current record)
    bool duplicateExists = await _repository.Countries.ExistsAsync(
        x => x.CountryName.Trim().ToLower() == modelDto.CountryName.Trim().ToLower()
             && x.CountryId != key, cancellationToken: cancellationToken);

    if (duplicateExists)
      throw new ConflictException("Duplicate data found!");

    // Map and update
    CrmCountry country = MyMapper.JsonClone<CrmCountryDto, CrmCountry>(modelDto);
    _repository.Countries.UpdateByState(country);
    await _repository.SaveAsync(cancellationToken);

    _logger.LogInformation("Country updated successfully: {CountryId}", key);

    return modelDto;
  }

  /// <summary>
  /// Deletes a country by ID asynchronously.
  /// </summary>
  public async Task DeleteAsync(int key, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (key <= 0)
      throw new BadRequestException("Invalid request!");

    _logger.LogInformation("Deleting country with ID: {CountryId}", key);

    var country = await _repository.Countries.ByIdAsync(
        x => x.CountryId == key, trackChanges: false);

    if (country == null)
      throw new NotFoundException("Data not found!");

    await _repository.Countries.DeleteAsync(x => x.CountryId == key, trackChanges: false, cancellationToken: cancellationToken);
    await _repository.SaveAsync(cancellationToken);

    _logger.LogInformation("Country deleted successfully: {CountryId}", key);
  }

  /// <summary>
  /// Retrieves a country by ID asynchronously.
  /// </summary>
  public async Task<CrmCountryDto> CountryAsync(int countryId, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (countryId <= 0)
    {
      _logger.LogWarning("CountryAsync called with invalid id: {CountryId}", countryId);
      throw new BadRequestException("Invalid request!");
    }

    _logger.LogInformation("Fetching country with ID: {CountryId}", countryId);

    CrmCountry country = await _repository.Countries.CountryAsync(countryId, trackChanges, cancellationToken);

    if (country == null)
    {
      _logger.LogWarning("Country not found with ID: {CountryId}", countryId);
      throw new NotFoundException("Data not found!");
    }

    CrmCountryDto countryDto = MyMapper.JsonClone<CrmCountry, CrmCountryDto>(country);
    return countryDto;
  }

  /// <summary>
  /// Retrieves all countries asynchronously.
  /// </summary>
  public async Task<IEnumerable<CrmCountryDto>> CountriesAsync(bool trackChanges, CancellationToken cancellationToken = default)
  {
    _logger.LogInformation("Fetching all countries");

    var countries = await _repository.Countries.CountriesAsync(trackChanges, cancellationToken);

    if (!countries.Any())
    {
      _logger.LogWarning("No countries found");
      return Enumerable.Empty<CrmCountryDto>();
    }

    return MyMapper.JsonCloneIEnumerableToList<CrmCountry, CrmCountryDto>(countries);
  }
}
