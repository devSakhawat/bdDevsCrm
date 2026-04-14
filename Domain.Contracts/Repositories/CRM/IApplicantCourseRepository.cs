using Domain.Entities.Entities.CRM;
using Domain.Contracts.Repositories;

namespace Domain.Contracts.CRM;

public interface ICrmApplicantCourseRepository : IRepositoryBase<CrmApplicantCourse>
{
  /// <summary>
  /// Retrieves all CrmApplicantCourse records asynchronously.
  /// </summary>
  Task<IEnumerable<CrmApplicantCourse>> CrmApplicantCoursesAsync(bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves a single CrmApplicantCourse record by ID asynchronously.
  /// </summary>
  Task<CrmApplicantCourse?> CrmApplicantCourseAsync(int crmapplicantcourseid, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmApplicantCourse records by a collection of IDs asynchronously.
  /// </summary>
  Task<IEnumerable<CrmApplicantCourse>> CrmApplicantCoursesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmApplicantCourse records by a applicationId asynchronously.
  /// </summary>
  Task<IEnumerable<CrmApplicantCourse>> CrmApplicantCoursesByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves CrmApplicantCourse records by parent ID asynchronously.
	/// </summary>
	Task<IEnumerable<CrmApplicantCourse>> CrmApplicantCoursesByParentIdAsync(int parentId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Creates a new CrmApplicantCourse record.
	/// </summary>
	Task<CrmApplicantCourse> CreateCrmApplicantCourse(CrmApplicantCourse entity, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an existing CrmApplicantCourse record.
  /// </summary>
  void UpdateCrmApplicantCourse(CrmApplicantCourse entity);

  /// <summary>
  /// Deletes a CrmApplicantCourse record.
  /// </summary>
  Task DeleteCrmApplicantCourse(CrmApplicantCourse entity, bool trackChanges, CancellationToken cancellationToken = default);
}

//public interface ICrmApplicantCourseRepository : IRepositoryBase<CrmApplicantCourse>
//{
//  Task<IEnumerable<CrmApplicantCourse>> ActiveApplicantCoursesAsync(bool track);
//  Task<IEnumerable<CrmApplicantCourse>> ApplicantCoursesAsync(bool track);
//  Task<CrmApplicantCourse?> ApplicantCourseAsync(int id, bool track);
//  Task<IEnumerable<CrmApplicantCourse>> ApplicantCoursesByApplicantIdAsync(int applicantId, bool track);
//  Task<CrmApplicantCourse?> ApplicantCourseByApplicantAndCourseIdAsync(int applicantId, int courseId, bool track);
//}