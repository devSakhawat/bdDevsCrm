using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmInstituteRepository : IRepositoryBase<CrmInstitute>
{
  /// <summary>
  /// Retrieves all CrmInstitute records asynchronously.
  /// </summary>
  Task<IEnumerable<CrmInstitute>> CrmInstitutesAsync(bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves a single CrmInstitute record by ID asynchronously.
  /// </summary>
  Task<CrmInstitute?> CrmInstituteAsync(int crminstituteid, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmInstitute records by a collection of IDs asynchronously.
  /// </summary>
  Task<IEnumerable<CrmInstitute>> CrmInstitutesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmInstitute records by parent ID asynchronously.
  /// </summary>
  Task<IEnumerable<CrmInstitute>> CrmInstitutesByParentIdAsync(int parentId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Creates a new CrmInstitute record.
  /// </summary>
  Task<CrmInstitute> CreateCrmInstituteAsync(CrmInstitute entity, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an existing CrmInstitute record.
  /// </summary>
  void UpdateCrmInstitute(CrmInstitute entity);

  /// <summary>
  /// Deletes a CrmInstitute record.
  /// </summary>
  Task DeleteCrmInstituteAsync(CrmInstitute entity, bool trackChanges, CancellationToken cancellationToken = default);
}




//using Domain.Entities.Entities.CRM;

//namespace Domain.Contracts.Core.SystemAdmin;

//public interface ICrmCourseRepository : IRepositoryBase<CrmCourse>
//{
//  Task<IEnumerable<CrmCourse>> ActiveCoursesAsync(bool track);
//  Task<IEnumerable<CrmCourse>> CoursesAsync(bool track);

//  Task<CrmCourse?> CourseAsync(int id, bool track);
//}
