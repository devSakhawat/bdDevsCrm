// ICrmInstituteService.cs
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.CRM;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.CRM;

/// <summary>
/// Service contract for CRM institute management operations.
/// Defines methods for creating, updating, deleting, and retrieving institute data.
/// </summary>
public interface ICrmInstituteService
{
	/// <summary>
	/// Creates a new institute record using CRUD Record pattern.
	/// </summary>
	Task<CrmInstituteDto> CreateAsync(CreateCrmInstituteRecord record, UsersDto currentuser,CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing institute record using CRUD Record pattern.
	/// </summary>
	Task<CrmInstituteDto> UpdateAsync(UpdateCrmInstituteRecord record, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes an institute record using CRUD Record pattern.
	/// </summary>
	Task DeleteAsync(DeleteCrmInstituteRecord record, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single institute record by its ID.
	/// </summary>
	/// <param name="id">The ID of the institute to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="CrmInstituteDto"/> matching the specified ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no institute is found for the given ID.</exception>
	Task<CrmInstituteDto> InstituteAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves all institute records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of all <see cref="CrmInstituteDto"/> records.</returns>
	Task<IEnumerable<CrmInstituteDto>> InstitutesAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves active institute records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of active <see cref="CrmInstituteDto"/> records.</returns>
	Task<IEnumerable<CrmInstituteDto>> ActiveInstitutesAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves institutes by the specified country ID.
	/// </summary>
	/// <param name="countryId">The ID of the country.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="CrmInstituteDDLDto"/> for the specified country.</returns>
	Task<IEnumerable<CrmInstituteDDLDto>> InstitutesByCountryIdAsync(int countryId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves an institute by its name.
	/// </summary>
	/// <param name="name">The institute name to search for.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="CrmInstituteDto"/> matching the specified name.</returns>
	/// <exception cref="NotFoundException">Thrown when no institute is found for the given name.</exception>
	Task<CrmInstituteDto> InstituteByNameAsync(string name, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a lightweight list of all institutes suitable for use in dropdown lists.
	/// </summary>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="CrmInstituteDllDto"/> for dropdown binding.</returns>
	Task<IEnumerable<CrmInstituteDDLDto>> InstituteForDDLAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated summary grid of all institutes.
	/// </summary>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{CrmInstituteDto}"/> containing the paged institute data.</returns>
	Task<GridEntity<CrmInstituteDto>> InstitutesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}


//public interface ICrmInstituteService
//{
//  Task<IEnumerable<DDLInstituteDto>> CRMInstituteDLLByCountry(int countryId, bool trackChanges);


//  //Task<IEnumerable<CountryDto>> CountriesAsync(bool trackChanges);
//  //Task<CountryDto> CountryAsync(int countryId, bool trackChanges);
//  //Task<IEnumerable<CountryDDL>> CountriesDDLAsync(bool trackChanges);
//  //Task<CountryDto> CreateCountryAsync(CountryDto entityForCreate);
//  //Task<IEnumerable<CountryDto>> ByIdsAsync(IEnumerable<int> ids, bool trackChanges);
//  //Task<(IEnumerable<CountryDto> countries, string ids)> CreateCountryCollectionAsync
//  //  (IEnumerable<CountryDto> countryCollection);
//  //Task DeleteCountryAsync(int countryId, bool trackChanges);
//  //Task UpdateCountryAsync(int countryId, CountryDto countryForUpdate, bool trackChanges);
//}
