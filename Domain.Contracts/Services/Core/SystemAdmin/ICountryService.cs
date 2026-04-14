using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.CRM;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface ICrmCountryService
{
  /// <summary>
  /// Retrieves all countries for dropdown list asynchronously.
  /// </summary>
  Task<IEnumerable<CrmCountryDDL>> CountriesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves paginated summary grid of countries asynchronously.
  /// </summary>
  Task<GridEntity<CrmCountryDto>> CountrySummaryAsync(GridOptions options, CancellationToken cancellationToken = default);

  /// <summary>
  /// Creates a new country asynchronously.
  /// </summary>
  Task<CrmCountryDto> CreateAsync(CrmCountryDto modelDto, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an existing country asynchronously.
  /// </summary>
  Task<CrmCountryDto> UpdateAsync(int key, CrmCountryDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Deletes a country by ID asynchronously.
  /// </summary>
  Task DeleteAsync(int key, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves a country by ID asynchronously.
  /// </summary>
  Task<CrmCountryDto> CountryAsync(int countryId, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves all countries asynchronously.
  /// </summary>
  Task<IEnumerable<CrmCountryDto>> CountriesAsync(bool trackChanges, CancellationToken cancellationToken = default);

  //Task<IEnumerable<CrmCountryDDL>> CountriesDDLAsync(bool trackChanges);
  //Task<GridEntity<CrmCountryDto>> SummaryGrid(GridOptions options);
  //Task<string> CreateNewRecordAsync(CrmCountryDto modelDto);
  //Task<string> UpdateNewRecordAsync(int key, CrmCountryDto modelDto, bool trackChanges);
  //Task<string> DeleteRecordAsync(int key, CrmCountryDto modelDto);
  //Task<string> SaveOrUpdate(int key, CrmCountryDto modelDto);


  //Task<IEnumerable<CrmCountryDto>> CountriesAsync(bool trackChanges);
  //Task<CrmCountryDto> CountryAsync(int countryId, bool trackChanges);
  //Task<CrmCountryDto> CreateCountryAsync(CrmCountryDto entityForCreate);
}
