// ICrmWorkExperienceService.cs
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.CRM;

/// <summary>
/// Service contract for CRM work experience management operations.
/// Defines methods for creating, updating, deleting, and retrieving work experience data.
/// </summary>
public interface ICrmWorkExperienceService
{
	/// <summary>
	/// Creates a new work experience record.
	/// </summary>
	/// <param name="entityForCreate">The DTO containing data for the new work experience.</param>
	/// <param name="currentUser">The DTO containing current user information.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The created <see cref="WorkExperienceHistoryDto"/> with the newly assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
	/// <exception cref="InvalidCreateOperationException">Thrown when WorkExperienceId is not 0 for new creation.</exception>
	Task<WorkExperienceHistoryDto> CreateWorkExperienceAsync(WorkExperienceHistoryDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing work experience record.
	/// </summary>
	/// <param name="workExperienceId">The ID of the work experience to update.</param>
	/// <param name="modelDto">The DTO containing updated field values.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="WorkExperienceHistoryDto"/> reflecting the saved state.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no work experience is found for the given ID.</exception>
	Task<WorkExperienceHistoryDto> UpdateWorkExperienceAsync(int workExperienceId, WorkExperienceHistoryDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes a work experience record identified by the given ID.
	/// </summary>
	/// <param name="workExperienceId">The ID of the work experience to delete.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The number of affected rows.</returns>
	/// <exception cref="IdParametersBadRequestException">Thrown when <paramref name="workExperienceId"/> is zero or negative.</exception>
	/// <exception cref="NotFoundException">Thrown when no work experience record is found for the given ID.</exception>
	Task<int> DeleteWorkExperienceAsync(int workExperienceId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single work experience record by its ID.
	/// </summary>
	/// <param name="id">The ID of the work experience to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="WorkExperienceHistoryDto"/> matching the specified ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no work experience is found for the given ID.</exception>
	Task<WorkExperienceHistoryDto> WorkExperienceAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves all work experience records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of all <see cref="WorkExperienceHistoryDto"/> records.</returns>
	Task<IEnumerable<WorkExperienceHistoryDto>> WorkExperiencesAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves active work experience records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of active <see cref="WorkExperienceHistoryDto"/> records.</returns>
	Task<IEnumerable<WorkExperienceHistoryDto>> ActiveWorkExperiencesAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves work experiences by the specified applicant ID.
	/// </summary>
	/// <param name="applicantId">The ID of the applicant.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="WorkExperienceHistoryDto"/> for the specified applicant.</returns>
	Task<IEnumerable<WorkExperienceHistoryDto>> WorkExperiencesByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a lightweight list of all work experiences suitable for use in dropdown lists.
	/// </summary>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="WorkExperienceHistoryDto"/> for dropdown binding.</returns>
	Task<IEnumerable<WorkExperienceHistoryDto>> WorkExperienceForDDLAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated summary grid of all work experiences.
	/// </summary>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{WorkExperienceHistoryDto}"/> containing the paged work experience data.</returns>
	Task<GridEntity<WorkExperienceHistoryDto>> WorkExperiencesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}


//using Application.Shared.Grid;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.CRM;

//namespace Domain.Contracts.Services.CRM;

//public interface ICrmWorkExperienceService
//{
//  Task<IEnumerable<WorkExperienceHistoryDto>> WorkExperiencesDDLAsync(bool trackChanges = false);
//  Task<IEnumerable<WorkExperienceHistoryDto>> ActiveWorkExperiencesAsync(bool trackChanges = false);
//  Task<IEnumerable<WorkExperienceHistoryDto>> WorkExperiencesAsync(bool trackChanges = false);
//  Task<WorkExperienceHistoryDto> WorkExperienceAsync(int id, bool trackChanges = false);
//  Task<IEnumerable<WorkExperienceHistoryDto>> WorkExperiencesByApplicantIdAsync(int applicantId, bool trackChanges = false);
//  Task<WorkExperienceHistoryDto> CreateNewRecordAsync(WorkExperienceHistoryDto dto, UsersDto currentUser);
//  Task<string> UpdateRecordAsync(int key, WorkExperienceHistoryDto dto, bool trackChanges);
//  Task<string> DeleteRecordAsync(int key, WorkExperienceHistoryDto dto);
//  Task<GridEntity<WorkExperienceHistoryDto>> SummaryGrid(GridOptions options);
//}