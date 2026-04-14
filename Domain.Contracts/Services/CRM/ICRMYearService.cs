// ICrmYearService.cs
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.CRM;

/// <summary>
/// Service contract for CRM year and workflow state management operations.
/// Defines methods for retrieving workflow states and actions.
/// </summary>
public interface ICrmYearService
{
	/// <summary>
	/// Creates a new year record.
	/// </summary>
	/// <param name="entityForCreate">The DTO containing data for the new year.</param>
	/// <param name="currentUser">The DTO containing current user information.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The created <see cref="CrmYearDto"/> with the newly assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
	/// <exception cref="InvalidCreateOperationException">Thrown when YearId is not 0 for new creation.</exception>
	Task<CrmYearDto> CreateYearAsync(CrmYearDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing year record.
	/// </summary>
	/// <param name="yearId">The ID of the year to update.</param>
	/// <param name="modelDto">The DTO containing updated field values.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="CrmYearDto"/> reflecting the saved state.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no year is found for the given ID.</exception>
	Task<CrmYearDto> UpdateYearAsync(int yearId, CrmYearDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);
	/// <summary>
	/// Deletes a year record identified by the given ID.
	/// </summary>
	/// <param name="yearId">The ID of the year to delete.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The number of affected rows.</returns>
	/// <exception cref="IdParametersBadRequestException">Thrown when <paramref name="yearId"/> is zero or negative.</exception>
	/// <exception cref="NotFoundException">Thrown when no year record is found for the given ID.</exception>
	Task<int> DeleteYearAsync(int yearId, bool trackChanges, CancellationToken cancellationToken = default);

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
//using bdDevCRM.Shared.DataTransferObjects;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.CRM;

//namespace bdDevCRM.ServicesContract.CRM;

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
