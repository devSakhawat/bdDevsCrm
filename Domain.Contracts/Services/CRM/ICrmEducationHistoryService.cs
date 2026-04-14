// ICrmEducationHistoryService.cs
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.CRM;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.CRM;

/// <summary>
/// Service contract for CRM education history management operations.
/// Defines methods for creating, updating, deleting, and retrieving education history data.
/// </summary>
public interface ICrmEducationHistoryService
{
	/// <summary>
	/// Creates a new education history record.
	/// </summary>
	/// <param name="entityForCreate">The DTO containing data for the new education history.</param>
	/// <param name="currentUser">The DTO containing current user information.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The created <see cref="EducationHistoryDto"/> with the newly assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
	/// <exception cref="InvalidCreateOperationException">Thrown when EducationHistoryId is not 0 for new creation.</exception>
	Task<EducationHistoryDto> CreateEducationHistoryAsync(EducationHistoryDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing education history record.
	/// </summary>
	/// <param name="educationHistoryId">The ID of the education history to update.</param>
	/// <param name="modelDto">The DTO containing updated field values.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="EducationHistoryDto"/> reflecting the saved state.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no education history is found for the given ID.</exception>
	Task<EducationHistoryDto> UpdateEducationHistoryAsync(int educationHistoryId, EducationHistoryDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes an education history record identified by the given ID.
	/// </summary>
	/// <param name="educationHistoryId">The ID of the education history to delete.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The number of affected rows.</returns>
	/// <exception cref="IdParametersBadRequestException">Thrown when <paramref name="educationHistoryId"/> is zero or negative.</exception>
	/// <exception cref="NotFoundException">Thrown when no education history record is found for the given ID.</exception>
	Task<int> DeleteEducationHistoryAsync(int educationHistoryId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single education history record by its ID.
	/// </summary>
	/// <param name="id">The ID of the education history to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="EducationHistoryDto"/> matching the specified ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no education history is found for the given ID.</exception>
	Task<EducationHistoryDto> EducationHistoryAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves all education history records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of all <see cref="EducationHistoryDto"/> records.</returns>
	Task<IEnumerable<EducationHistoryDto>> EducationHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves active education history records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of active <see cref="EducationHistoryDto"/> records.</returns>
	Task<IEnumerable<EducationHistoryDto>> ActiveEducationHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves education histories by the specified applicant ID.
	/// </summary>
	/// <param name="applicantId">The ID of the applicant.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="EducationHistoryDto"/> for the specified applicant.</returns>
	Task<IEnumerable<EducationHistoryDto>> EducationHistoriesByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves an education history by institution name.
	/// </summary>
	/// <param name="institution">The institution name to search for.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="EducationHistoryDto"/> matching the specified institution.</returns>
	/// <exception cref="NotFoundException">Thrown when no education history is found for the given institution.</exception>
	Task<EducationHistoryDto> EducationHistoryByInstitutionAsync(string institution, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a lightweight list of all education histories suitable for use in dropdown lists.
	/// </summary>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="EducationHistoryDto"/> for dropdown binding.</returns>
	Task<IEnumerable<EducationHistoryDto>> EducationHistoryForDDLAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated summary grid of all education histories.
	/// </summary>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{EducationHistoryDto}"/> containing the paged education history data.</returns>
	Task<GridEntity<EducationHistoryDto>> EducationHistoriesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}

//using Application.Shared.Grid;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.CRM;

//namespace bdDevCRM.ServiceContract.CRM;

//public interface ICrmEducationHistoryService
//{
//  Task<IEnumerable<EducationHistoryDto>> EducationHistoriesDDLAsync(bool trackChanges = false);
//  Task<IEnumerable<EducationHistoryDto>> ActiveEducationHistoriesAsync(bool trackChanges = false);
//  Task<IEnumerable<EducationHistoryDto>> EducationHistoriesAsync(bool trackChanges = false);
//  Task<EducationHistoryDto> EducationHistoryAsync(int id, bool trackChanges = false);
//  Task<IEnumerable<EducationHistoryDto>> EducationHistoriesByApplicantIdAsync(int applicantId, bool trackChanges = false);
//  Task<EducationHistoryDto> CreateNewRecordAsync(EducationHistoryDto dto, UsersDto currentUser);
//  Task<string> UpdateRecordAsync(int key, EducationHistoryDto dto, bool trackChanges);
//  Task<string> DeleteRecordAsync(int key, EducationHistoryDto dto);
//  Task<EducationHistoryDto> EducationHistoryByInstitutionAsync(string institution, bool trackChanges = false);
//  Task<GridEntity<EducationHistoryDto>> SummaryGrid(GridOptions options);
//}