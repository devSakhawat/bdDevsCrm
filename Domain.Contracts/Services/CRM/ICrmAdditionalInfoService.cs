// ICrmAdditionalInfoService.cs
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.CRM;

/// <summary>
/// Service contract for CRM additional info management operations.
/// Defines methods for creating, updating, deleting, and retrieving additional info data.
/// </summary>
public interface ICrmAdditionalInfoService
{
	/// <summary>
	/// Creates a new additional info record.
	/// </summary>
	/// <param name="entityForCreate">The DTO containing data for the new info.</param>
	/// <param name="currentUser">The DTO containing current user information.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The created <see cref="AdditionalInfoDto"/> with the newly assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
	/// <exception cref="InvalidCreateOperationException">Thrown when AdditionalInfoId is not 0 for new creation.</exception>
	Task<AdditionalInfoDto> CreateAdditionalInfoAsync(AdditionalInfoDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing additional info record.
	/// </summary>
	/// <param name="additionalInfoId">The ID of the info to update.</param>
	/// <param name="modelDto">The DTO containing updated field values.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="AdditionalInfoDto"/> reflecting the saved state.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no info is found for the given ID.</exception>
	Task<AdditionalInfoDto> UpdateAdditionalInfoAsync(int additionalInfoId, AdditionalInfoDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes an additional info record identified by the given ID.
	/// </summary>
	/// <param name="additionalInfoId">The ID of the info to delete.</param>
	/// <param name="modelDto">The DTO containing info data for validation.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The number of affected rows.</returns>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no info record is found for the given ID.</exception>
	Task<int> DeleteAdditionalInfoAsync(int additionalInfoId, AdditionalInfoDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single additional info record by its ID.
	/// </summary>
	/// <param name="id">The ID of the info to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="AdditionalInfoDto"/> matching the specified ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no info is found for the given ID.</exception>
	Task<AdditionalInfoDto> AdditionalInfoAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves all additional info records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of all <see cref="AdditionalInfoDto"/> records.</returns>
	/// <exception cref="GenericListNotFoundException">Thrown when no info records are found.</exception>
	Task<IEnumerable<AdditionalInfoDto>> AdditionalInfosAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves active additional info records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of active <see cref="AdditionalInfoDto"/> records.</returns>
	/// <exception cref="GenericListNotFoundException">Thrown when no active info records are found.</exception>
	Task<IEnumerable<AdditionalInfoDto>> ActiveAdditionalInfosAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves additional infos by the specified applicant ID.
	/// </summary>
	/// <param name="applicantId">The ID of the applicant.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="AdditionalInfoDto"/> for the specified applicant.</returns>
	Task<IEnumerable<AdditionalInfoDto>> AdditionalInfosByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a lightweight list of all additional infos suitable for use in dropdown lists.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="AdditionalInfoDto"/> for dropdown binding.</returns>
	/// <exception cref="GenericListNotFoundException">Thrown when no info records are found.</exception>
	Task<IEnumerable<AdditionalInfoDto>> AdditionalInfoForDDLAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated summary grid of all additional infos.
	/// </summary>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{AdditionalInfoDto}"/> containing the paged info data.</returns>
	Task<GridEntity<AdditionalInfoDto>> AdditionalInfosSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}


//using Application.Shared.Grid;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.CRM;

//namespace bdDevCRM.ServiceContract.CRM;

//public interface ICrmAdditionalInfoService
//{
//  Task<IEnumerable<AdditionalInfoDto>> AdditionalInfosDDLAsync(bool trackChanges = false);
//  Task<IEnumerable<AdditionalInfoDto>> ActiveAdditionalInfosAsync(bool trackChanges = false);
//  Task<IEnumerable<AdditionalInfoDto>> AdditionalInfosAsync(bool trackChanges = false);
//  Task<AdditionalInfoDto> AdditionalInfoAsync(int id, bool trackChanges = false);
//  Task<IEnumerable<AdditionalInfoDto>> AdditionalInfosByApplicantIdAsync(int applicantId, bool trackChanges = false);
//  Task<AdditionalInfoDto> CreateNewRecordAsync(AdditionalInfoDto dto, UsersDto currentUser);
//  Task<string> UpdateRecordAsync(int key, AdditionalInfoDto dto, bool trackChanges);
//  Task<string> DeleteRecordAsync(int key, AdditionalInfoDto dto);
//  Task<GridEntity<AdditionalInfoDto>> SummaryGrid(GridOptions options);
//}