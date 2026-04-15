
// ICrmApplicantCourseService.cs
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.CRM;

/// <summary>
/// Service contract for CRM applicant course management operations.
/// Defines methods for creating, updating, deleting, and retrieving applicant course data.
/// </summary>
public interface ICrmApplicantCourseService
{
	/// <summary>
	/// Creates a new applicant course record.
	/// </summary>
	/// <param name="entityForCreate">The DTO containing data for the new course.</param>
	/// <param name="currentUser">The DTO containing current user information.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The created <see cref="ApplicantCourseDto"/> with the newly assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
	/// <exception cref="InvalidCreateOperationException">Thrown when ApplicantCourseId is not 0 for new creation.</exception>
	/// <exception cref="DuplicateRecordException">Thrown when an applicant-course combination already exists.</exception>
	Task<ApplicantCourseDto> CreateApplicantCourseAsync(ApplicantCourseDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing applicant course record.
	/// </summary>
	/// <param name="applicantCourseId">The ID of the course to update.</param>
	/// <param name="modelDto">The DTO containing updated field values.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="ApplicantCourseDto"/> reflecting the saved state.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no course is found for the given ID.</exception>
	Task<ApplicantCourseDto> UpdateApplicantCourseAsync(int applicantCourseId, ApplicantCourseDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes an applicant course record identified by the given ID.
	/// </summary>
	/// <param name="applicantCourseId">The ID of the course to delete.</param>
	/// <param name="modelDto">The DTO containing course data for validation.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The number of affected rows.</returns>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no course record is found for the given ID.</exception>
	Task<int> DeleteApplicantCourseAsync(int applicantCourseId, ApplicantCourseDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single applicant course record by its ID.
	/// </summary>
	/// <param name="id">The ID of the course to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="ApplicantCourseDto"/> matching the specified ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no course is found for the given ID.</exception>
	Task<ApplicantCourseDto> ApplicantCourseAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves all applicant course records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of all <see cref="ApplicantCourseDto"/> records.</returns>
	/// <exception cref="GenericListNotFoundException">Thrown when no courses are found.</exception>
	Task<IEnumerable<ApplicantCourseDto>> ApplicantCoursesAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves active applicant course records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of active <see cref="ApplicantCourseDto"/> records.</returns>
	/// <exception cref="GenericListNotFoundException">Thrown when no active courses are found.</exception>
	Task<IEnumerable<ApplicantCourseDto>> ActiveApplicantCoursesAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves applicant courses by the specified applicant ID.
	/// </summary>
	/// <param name="applicantId">The ID of the applicant.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="ApplicantCourseDto"/> for the specified applicant.</returns>
	Task<IEnumerable<ApplicantCourseDto>> ApplicantCoursesByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single applicant course by applicant ID and course ID.
	/// </summary>
	/// <param name="applicantId">The ID of the applicant.</param>
	/// <param name="courseId">The ID of the course.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="ApplicantCourseDto"/> matching the specified IDs.</returns>
	/// <exception cref="NotFoundException">Thrown when no course is found for the given IDs.</exception>
	Task<ApplicantCourseDto> ApplicantCourseByApplicantAndCourseIdAsync(int applicantId, int courseId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a lightweight list of all applicant courses suitable for use in dropdown lists.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="ApplicantCourseDto"/> for dropdown binding.</returns>
	/// <exception cref="GenericListNotFoundException">Thrown when no courses are found.</exception>
	Task<IEnumerable<ApplicantCourseDto>> ApplicantCourseForDDLAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated summary grid of all applicant courses.
	/// </summary>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{ApplicantCourseDto}"/> containing the paged course data.</returns>
	Task<GridEntity<ApplicantCourseDto>> ApplicantCoursesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}







//using Application.Shared.Grid;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.CRM;

//namespace Domain.Contracts.Services.CRM;

//public interface ICrmApplicantCourseService
//{
//  Task<IEnumerable<ApplicantCourseDto>> ApplicantCoursesDDLAsync(bool trackChanges = false);
//  Task<IEnumerable<ApplicantCourseDto>> ActiveApplicantCoursesAsync(bool trackChanges = false);
//  Task<IEnumerable<ApplicantCourseDto>> ApplicantCoursesAsync(bool trackChanges = false);
//  Task<ApplicantCourseDto> ApplicantCourseAsync(int id, bool trackChanges = false);
//  Task<IEnumerable<ApplicantCourseDto>> ApplicantCoursesByApplicantIdAsync(int applicantId, bool trackChanges = false);
//  Task<ApplicantCourseDto> CreateNewRecordAsync(ApplicantCourseDto dto, UsersDto currentUser);
//  Task<string> UpdateRecordAsync(int key, ApplicantCourseDto dto, bool trackChanges);
//  Task<string> DeleteRecordAsync(int key, ApplicantCourseDto dto);
//  Task<ApplicantCourseDto> ApplicantCourseByApplicantAndCourseIdAsync(int applicantId, int courseId, bool trackChanges = false);
//  Task<GridEntity<ApplicantCourseDto>> SummaryGrid(GridOptions options);
//}