// ICrmYearService.cs
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.CRM;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.CRM;

/// <summary>
/// Service contract for CRM year and workflow state management operations.
/// Defines methods for retrieving workflow states and actions.
/// </summary>
public interface ICrmYearService
{
	/// <summary>
	/// Creates a new year record using CRUD Record pattern.
	/// </summary>
	Task<CrmYearDto> CreateAsync(CreateCrmYearRecord record, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing year record using CRUD Record pattern.
	/// </summary>
	Task<CrmYearDto> UpdateAsync(UpdateCrmYearRecord record, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes a year record using CRUD Record pattern.
	/// </summary>
	Task DeleteAsync(DeleteCrmYearRecord record, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single year record by its ID.
	/// </summary>
	/// <param name="id">The ID of the year to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="CrmYearDto"/> matching the specified ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no year is found for the given ID.</exception>
	Task<CrmYearDto> YearAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves all year records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of all <see cref="CrmYearDto"/> records.</returns>
	Task<IEnumerable<CrmYearDto>> YearsAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves active year records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of active <see cref="CrmYearDto"/> records.</returns>
	Task<IEnumerable<CrmYearDto>> ActiveYearsAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a lightweight list of all years suitable for use in dropdown lists.
	/// </summary>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="CrmYearDto"/> for dropdown binding.</returns>
	Task<IEnumerable<CrmYearDto>> YearForDDLAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves years by the specified applicant ID.
	/// </summary>
	/// <param name="applicantId">The ID of the applicant.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="CrmYearDto"/> for the specified applicant.</returns>
	Task<IEnumerable<CrmYearDto>> YearsByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated summary grid of all years.
	/// </summary>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{CrmYearDto}"/> containing the paged year data.</returns>
	Task<GridEntity<CrmYearDto>> YearsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}




//using Application.Shared.Grid;
//using bdDevs.Shared.DataTransferObjects;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.CRM;

//namespace Domain.Contracts.Services.CRM;

//public interface ICrmYearService
//{

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
