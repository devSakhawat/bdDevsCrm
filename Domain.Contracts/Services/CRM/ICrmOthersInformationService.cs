// ICrmOthersInformationService.cs
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.CRM;

/// <summary>
/// Service contract for CRM others information management operations.
/// Defines methods for creating, updating, deleting, and retrieving others information data.
/// </summary>
public interface ICrmOthersInformationService
{
	/// <summary>
	/// Creates a new others information record.
	/// </summary>
	/// <param name="entityForCreate">The DTO containing data for the new others information.</param>
	/// <param name="currentUser">The DTO containing current user information.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The created <see cref="OthersInformationDto"/> with the newly assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
	/// <exception cref="InvalidCreateOperationException">Thrown when OthersInformationId is not 0 for new creation.</exception>
	/// <exception cref="DuplicateRecordException">Thrown when others information already exists for the applicant.</exception>
	Task<OthersInformationDto> CreateOthersInformationAsync(OthersInformationDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default);
	/// <summary>
	/// Updates an existing others information record.
	/// </summary>
	/// <param name="othersInformationId">The ID of the others information to update.</param>
	/// <param name="modelDto">The DTO containing updated field values.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="OthersInformationDto"/> reflecting the saved state.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no others information is found for the given ID.</exception>
	Task<OthersInformationDto> UpdateOthersInformationAsync(int othersInformationId, OthersInformationDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);
	/// <summary>
	/// Deletes an others information record identified by the given ID.
	/// </summary>
	/// <param name="othersInformationId">The ID of the others information to delete.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The number of affected rows.</returns>
	/// <exception cref="IdParametersBadRequestException">Thrown when <paramref name="othersInformationId"/> is zero or negative.</exception>
	/// <exception cref="NotFoundException">Thrown when no others information record is found for the given ID.</exception>
	Task<int> DeleteOthersInformationAsync(int othersInformationId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single others information record by its ID.
	/// </summary>
	/// <param name="id">The ID of the others information to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="OthersInformationDto"/> matching the specified ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no others information is found for the given ID.</exception>
	Task<OthersInformationDto> OthersInformationAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves all others information records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of all <see cref="OthersInformationDto"/> records.</returns>
	Task<IEnumerable<OthersInformationDto>> OthersInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves active others information records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of active <see cref="OthersInformationDto"/> records.</returns>
	Task<IEnumerable<OthersInformationDto>> ActiveOthersInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves others information by the specified applicant ID.
	/// </summary>
	/// <param name="applicantId">The ID of the applicant.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="OthersInformationDto"/> for the specified applicant.</returns>
	/// <exception cref="NotFoundException">Thrown when no others information is found for the given applicant ID.</exception>
	Task<OthersInformationDto> OthersInformationByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a lightweight list of all others informations suitable for use in dropdown lists.
	/// </summary>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="OthersInformationDto"/> for dropdown binding.</returns>
	Task<IEnumerable<OthersInformationDto>> OthersInformationForDDLAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated summary grid of all others informations.
	/// </summary>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{OthersInformationDto}"/> containing the paged others information data.</returns>
	Task<GridEntity<OthersInformationDto>> OthersInformationsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}


//using Application.Shared.Grid;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.CRM;

//namespace bdDevCRM.ServiceContract.CRM;

//public interface ICrmOthersInformationService
//{
//  Task<IEnumerable<OTHERSInformationDto>> OthersinformationsDDLAsync(bool trackChanges = false);
//  Task<IEnumerable<OTHERSInformationDto>> ActiveOthersinformationsAsync(bool trackChanges = false);
//  Task<IEnumerable<OTHERSInformationDto>> OthersinformationsAsync(bool trackChanges = false);
//  Task<OTHERSInformationDto> OthersinformationAsync(int id, bool trackChanges = false);
//  Task<OTHERSInformationDto> OthersinformationByApplicantIdAsync(int applicantId, bool trackChanges = false);
//  Task<OTHERSInformationDto> CreateNewRecordAsync(OTHERSInformationDto dto, UsersDto currentUser);
//  Task<string> UpdateRecordAsync(int key, OTHERSInformationDto dto, bool trackChanges);
//  Task<string> DeleteRecordAsync(int key, OTHERSInformationDto dto);
//  Task<GridEntity<OTHERSInformationDto>> SummaryGrid(GridOptions options);
//}