// ICrmPermanentAddressService.cs
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.CRM;

/// <summary>
/// Service contract for CRM permanent address management operations.
/// Defines methods for creating, updating, deleting, and retrieving permanent address data.
/// </summary>
public interface ICrmPermanentAddressService
{
	/// <summary>
	/// Creates a new permanent address record.
	/// </summary>
	/// <param name="entityForCreate">The DTO containing data for the new permanent address.</param>
	/// <param name="currentUser">The DTO containing current user information.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The created <see cref="PermanentAddressDto"/> with the newly assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
	/// <exception cref="InvalidCreateOperationException">Thrown when PermanentAddressId is not 0 for new creation.</exception>
	/// <exception cref="DuplicateRecordException">Thrown when permanent address already exists for the applicant.</exception>
	Task<PermanentAddressDto> CreatePermanentAddressAsync(PermanentAddressDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing permanent address record.
	/// </summary>
	/// <param name="permanentAddressId">The ID of the permanent address to update.</param>
	/// <param name="modelDto">The DTO containing updated field values.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="PermanentAddressDto"/> reflecting the saved state.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no permanent address is found for the given ID.</exception>
	Task<PermanentAddressDto> UpdatePermanentAddressAsync(int permanentAddressId, PermanentAddressDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes a permanent address record identified by the given ID.
	/// </summary>
	/// <param name="permanentAddressId">The ID of the permanent address to delete.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The number of affected rows.</returns>
	/// <exception cref="IdParametersBadRequestException">Thrown when <paramref name="permanentAddressId"/> is zero or negative.</exception>
	/// <exception cref="NotFoundException">Thrown when no permanent address record is found for the given ID.</exception>
	Task<int> DeletePermanentAddressAsync(int permanentAddressId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single permanent address record by its ID.
	/// </summary>
	/// <param name="id">The ID of the permanent address to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="PermanentAddressDto"/> matching the specified ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no permanent address is found for the given ID.</exception>
	Task<PermanentAddressDto> PermanentAddressAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves all permanent address records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of all <see cref="PermanentAddressDto"/> records.</returns>
	Task<IEnumerable<PermanentAddressDto>> PermanentAddressesAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves active permanent address records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of active <see cref="PermanentAddressDto"/> records.</returns>
	Task<IEnumerable<PermanentAddressDto>> ActivePermanentAddressesAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves permanent address by the specified applicant ID.
	/// </summary>
	/// <param name="applicantId">The ID of the applicant.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="PermanentAddressDto"/> for the specified applicant.</returns>
	/// <exception cref="NotFoundException">Thrown when no permanent address is found for the given applicant ID.</exception>
	Task<PermanentAddressDto> PermanentAddressByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves permanent addresses by the specified country ID.
	/// </summary>
	/// <param name="countryId">The ID of the country.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="PermanentAddressDto"/> for the specified country.</returns>
	Task<IEnumerable<PermanentAddressDto>> PermanentAddressesByCountryIdAsync(int countryId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a lightweight list of all permanent addresses suitable for use in dropdown lists.
	/// </summary>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="PermanentAddressDto"/> for dropdown binding.</returns>
	Task<IEnumerable<PermanentAddressDto>> PermanentAddressForDDLAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated summary grid of all permanent addresses.
	/// </summary>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{PermanentAddressDto}"/> containing the paged permanent address data.</returns>
	Task<GridEntity<PermanentAddressDto>> PermanentAddressesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}

//using Application.Shared.Grid;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.CRM;

//namespace bdDevCRM.ServiceContract.CRM;

//public interface ICrmPermanentAddressService
//{
//  Task<IEnumerable<PermanentAddressDto>> PermanentAddressesDDLAsync(bool trackChanges = false);
//  Task<IEnumerable<PermanentAddressDto>> ActivePermanentAddressesAsync(bool trackChanges = false);
//  Task<IEnumerable<PermanentAddressDto>> PermanentAddressesAsync(bool trackChanges = false);
//  Task<PermanentAddressDto> PermanentAddressAsync(int id, bool trackChanges = false);
//  Task<PermanentAddressDto> PermanentAddressByApplicantIdAsync(int applicantId, bool trackChanges = false);
//  Task<IEnumerable<PermanentAddressDto>> PermanentAddressesByCountryIdAsync(int countryId, bool trackChanges = false);
//  Task<PermanentAddressDto> CreateNewRecordAsync(PermanentAddressDto dto, UsersDto currentUser);
//  Task<string> UpdateRecordAsync(int key, PermanentAddressDto dto, bool trackChanges);
//  Task<string> DeleteRecordAsync(int key, PermanentAddressDto dto);
//  Task<GridEntity<PermanentAddressDto>> SummaryGrid(GridOptions options);
//}