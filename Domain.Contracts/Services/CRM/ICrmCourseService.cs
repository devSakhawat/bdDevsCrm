// ICrmCourseService.cs
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.CRM;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.CRM;

/// <summary>
/// Service contract for CRM course management operations.
/// Defines methods for creating, updating, deleting, and retrieving course data.
/// </summary>
public interface ICrmCourseService
{
	/// <summary>
	/// Creates a new course record using CRUD Record pattern.
	/// </summary>
	/// <param name="record">The Record containing data for the new course.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The created <see cref="CrmCourseDto"/> with the newly assigned ID.</returns>
	Task<CrmCourseDto> CreateAsync(CreateCrmCourseRecord record, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing course record using CRUD Record pattern.
	/// </summary>
	/// <param name="record">The Record containing updated field values.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="CrmCourseDto"/> reflecting the saved state.</returns>
	Task<CrmCourseDto> UpdateAsync(UpdateCrmCourseRecord record, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes a course record using CRUD Record pattern.
	/// </summary>
	/// <param name="record">The Record containing the course ID to delete.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	Task DeleteAsync(DeleteCrmCourseRecord record, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single course record by its ID.
	/// </summary>
	/// <param name="id">The ID of the course to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="CrmCourseDto"/> matching the specified ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no course is found for the given ID.</exception>
	Task<CrmCourseDto> CourseAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves all course records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of all <see cref="CrmCourseDto"/> records.</returns>
	Task<IEnumerable<CrmCourseDto>> CoursesAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves active course records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of active <see cref="CrmCourseDto"/> records.</returns>
	Task<IEnumerable<CrmCourseDto>> ActiveCoursesAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves courses by the specified institute ID.
	/// </summary>
	/// <param name="instituteId">The ID of the institute.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="CrmCourseDto"/> for the specified institute.</returns>
	Task<IEnumerable<CrmCourseDDLDto>> CoursesByInstituteIdAsync(int instituteId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a course by its title.
	/// </summary>
	/// <param name="title">The course title to search for.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="CrmCourseDto"/> matching the specified title.</returns>
	/// <exception cref="NotFoundException">Thrown when no course is found for the given title.</exception>
	Task<CrmCourseDto> CourseByTitleAsync(string title, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a lightweight list of all courses suitable for use in dropdown lists.
	/// </summary>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="CrmCourseDto"/> for dropdown binding.</returns>
	Task<IEnumerable<CrmCourseDDLDto>> CourseForDDLAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated summary grid of all courses.
	/// </summary>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{CrmCourseDto}"/> containing the paged course data.</returns>
	Task<GridEntity<CrmCourseDto>> CoursesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}



//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.CRM;
//using Application.Shared.Grid;
//namespace Domain.Contracts.Services.CRM;

//public interface ICrmCourseService
//{
//    Task<IEnumerable<CrmCourseDto>> CoursesDDLAsync(bool trackChanges = false);
//    Task<IEnumerable<CRMCourseDDLDto>> CourseByInstituteIdDDLAsync(int instituteId, bool trackChanges = false);
//    Task<IEnumerable<CrmCourseDto>> ActiveCoursesAsync(bool trackChanges = false);
//    Task<IEnumerable<CrmCourseDto>> CoursesAsync(bool trackChanges = false);
//    Task<CrmCourseDto> CourseAsync(int id, bool trackChanges = false);
//    Task<CrmCourseDto> CreateNewRecordAsync(CrmCourseDto dto, UsersDto currentUser);
//    Task<string> UpdateRecordAsync(int key, CrmCourseDto dto, bool trackChanges);
//    Task<string> DeleteRecordAsync(int key, CrmCourseDto dto);
//    Task<CrmCourseDto> CourseByTitleAsync(string title, bool trackChanges = false);

//    Task<GridEntity<CrmCourseDto>> SummaryGrid(GridOptions options);
//}