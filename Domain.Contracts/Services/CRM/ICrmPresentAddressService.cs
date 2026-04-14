// ICrmPresentAddressService.cs
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.CRM;

/// <summary>
/// Service contract for CRM present address management operations.
/// Defines methods for creating, updating, deleting, and retrieving present address data.
/// </summary>
public interface ICrmPresentAddressService
{
	/// <summary>
	/// Creates a new present address record.
	/// </summary>
	/// <param name="entityForCreate">The DTO containing data for the new present address.</param>
	/// <param name="currentUser">The DTO containing current user information.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The created <see cref="PresentAddressDto"/> with the newly assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
	/// <exception cref="InvalidCreateOperationException">Thrown when PresentAddressId is not 0 for new creation.</exception>
	/// <exception cref="DuplicateRecordException">Thrown when present address already exists for the applicant.</exception>
	Task<PresentAddressDto> CreatePresentAddressAsync(PresentAddressDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing present address record.
	/// </summary>
	/// <param name="presentAddressId">The ID of the present address to update.</param>
	/// <param name="modelDto">The DTO containing updated field values.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="PresentAddressDto"/> reflecting the saved state.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no present address is found for the given ID.</exception>
	Task<PresentAddressDto> UpdatePresentAddressAsync(int presentAddressId, PresentAddressDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes a present address record identified by the given ID.
	/// </summary>
	/// <param name="presentAddressId">The ID of the present address to delete.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The number of affected rows.</returns>
	/// <exception cref="IdParametersBadRequestException">Thrown when <paramref name="presentAddressId"/> is zero or negative.</exception>
	/// <exception cref="NotFoundException">Thrown when no present address record is found for the given ID.</exception>
	Task<int> DeletePresentAddressAsync(int presentAddressId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single present address record by its ID.
	/// </summary>
	/// <param name="id">The ID of the present address to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="PresentAddressDto"/> matching the specified ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no present address is found for the given ID.</exception>
	Task<PresentAddressDto> PresentAddressAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves all present address records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of all <see cref="PresentAddressDto"/> records.</returns>
	Task<IEnumerable<PresentAddressDto>> PresentAddressesAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves active present address records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of active <see cref="PresentAddressDto"/> records.</returns>
	Task<IEnumerable<PresentAddressDto>> ActivePresentAddressesAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves present address by the specified applicant ID.
	/// </summary>
	/// <param name="applicantId">The ID of the applicant.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="PresentAddressDto"/> for the specified applicant.</returns>
	/// <exception cref="NotFoundException">Thrown when no present address is found for the given applicant ID.</exception>
	Task<PresentAddressDto> PresentAddressByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves present addresses by the specified country ID.
	/// </summary>
	/// <param name="countryId">The ID of the country.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="PresentAddressDto"/> for the specified country.</returns>
	Task<IEnumerable<PresentAddressDto>> PresentAddressesByCountryIdAsync(int countryId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a lightweight list of all present addresses suitable for use in dropdown lists.
	/// </summary>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="PresentAddressDto"/> for dropdown binding.</returns>
	Task<IEnumerable<PresentAddressDto>> PresentAddressForDDLAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated summary grid of all present addresses.
	/// </summary>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{PresentAddressDto}"/> containing the paged present address data.</returns>
	Task<GridEntity<PresentAddressDto>> PresentAddressesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}


//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.CRM;
//using Application.Shared.Grid;

//namespace Domain.Contracts.Services.CRM;

//public interface ICrmPresentAddressService
//{
//	Task<IEnumerable<PresentAddressDto>> PresentAddressesDDLAsync(bool trackChanges = false);
//	Task<IEnumerable<PresentAddressDto>> ActivePresentAddressesAsync(bool trackChanges = false);
//	Task<IEnumerable<PresentAddressDto>> PresentAddressesAsync(bool trackChanges = false);
//	Task<PresentAddressDto> PresentAddressAsync(int id, bool trackChanges = false);
//	Task<PresentAddressDto> PresentAddressByApplicantIdAsync(int applicantId, bool trackChanges = false);
//	Task<IEnumerable<PresentAddressDto>> PresentAddressesByCountryIdAsync(int countryId, bool trackChanges = false);
//	Task<PresentAddressDto> CreateNewRecordAsync(PresentAddressDto dto, UsersDto currentUser);
//	Task<string> UpdateRecordAsync(int key, PresentAddressDto dto, bool trackChanges);
//	Task<string> DeleteRecordAsync(int key, PresentAddressDto dto);
//	Task<GridEntity<PresentAddressDto>> SummaryGrid(GridOptions options);
//}