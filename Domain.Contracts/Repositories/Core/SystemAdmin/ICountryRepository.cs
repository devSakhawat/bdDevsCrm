// Interface: ICrmCountryRepository
using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.Core.SystemAdmin
{
  public interface ICrmCountryRepository : IRepositoryBase<CrmCountry>
  {
    /// <summary>
    /// Retrieves all countries asynchronously.
    /// </summary>
    Task<IEnumerable<CrmCountry>> CountriesAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all active countries asynchronously.
    /// </summary>
    Task<IEnumerable<CrmCountry>> ActiveCountriesAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single country by ID asynchronously.
    /// </summary>
    Task<CrmCountry?> CountryAsync(int countryId, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new country.
    /// </summary>
    Task<CrmCountry> CreateCountryAsync(CrmCountry country, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing country.
    /// </summary>
    void UpdateCountry(CrmCountry country);

    /// <summary>
    /// Deletes a country.
    /// </summary>
    Task DeleteCountryAsync(CrmCountry country, CancellationToken cancellationToken = default);
  }
}









//using Domain.Entities.Entities.CRM;

//namespace bdDevCRM.RepositoriesContracts.Core.SystemAdmin;

//public interface ICrmCountryRepository : IRepositoryBase<CrmCountry>
//{
//  Task<IEnumerable<CrmCountry>> CountriesAsync(bool trackChanges);
//  Task<IEnumerable<CrmCountry>> ActiveCountriesAsync(bool trackChanges);
//  Task<CrmCountry> CountryAsync(int companyId, bool trackChanges);
//  void CreateCountry(CrmCountry country);
//  void UpdateCountry(CrmCountry country);
//  void DeleteCountry(CrmCountry country);
//}
