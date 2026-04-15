using Domain.Contracts.Core.SystemAdmin;
using Domain.Entities.Entities.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.Core.SystemAdmin;

/// <summary>
/// Repository for country data access operations.
/// Implements enterprise patterns with async support.
/// </summary>
public class CrmCountryRepository : RepositoryBase<CrmCountry>, ICrmCountryRepository
{
  public CrmCountryRepository(CrmContext context) : base(context) { }

  /// <summary>
  /// Retrieves all countries asynchronously.
  /// </summary>
  public async Task<IEnumerable<CrmCountry>> CountriesAsync(bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListAsync(c => c.CountryId, trackChanges, cancellationToken);
  }

  /// <summary>
  /// Retrieves all active countries asynchronously.
  /// </summary>
  public async Task<IEnumerable<CrmCountry>> ActiveCountriesAsync(bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListByConditionAsync(
        x => x.Status == 1,
        c => c.CountryId,
        trackChanges,
        cancellationToken: cancellationToken);
  }

  /// <summary>
  /// Retrieves a single country by ID asynchronously.
  /// </summary>
  public async Task<CrmCountry?> CountryAsync(int countryId, bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await FirstOrDefaultAsync(
        x => x.CountryId.Equals(countryId),
        trackChanges,
        cancellationToken);
  }

  /// <summary>
  /// Creates a new country.
  /// </summary>
  public async Task<CrmCountry> CreateCountryAsync(CrmCountry country, CancellationToken cancellationToken = default)
  {
    int countryId = await CreateAndIdAsync(country, cancellationToken);
    country.CountryId = countryId;
    return country;
  }

  /// <summary>
  /// Updates an existing country.
  /// </summary>
  public void UpdateCountry(CrmCountry country) => UpdateByState(country);

  /// <summary>
  /// Deletes a country.
  /// </summary>
  public async Task DeleteCountryAsync(CrmCountry country, CancellationToken cancellationToken = default)
    => await DeleteAsync(x => x.CountryId == country.CountryId, true, cancellationToken);
}
