using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM
{
  public interface ICrmCourseRepository : IRepositoryBase<CrmCourse>
  {
    /// <summary>
    /// Retrieves all CrmCourse records asynchronously.
    /// </summary>
    Task<IEnumerable<CrmCourse>> CrmCoursesAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single CrmCourse record by ID asynchronously.
    /// </summary>
    Task<CrmCourse?> CrmCourseAsync(int CourseId, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves CrmCourse records by a collection of IDs asynchronously.
    /// </summary>
    Task<IEnumerable<CrmCourse>> CrmCoursesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves CrmCourse records by parent ID asynchronously.
    /// </summary>
    Task<IEnumerable<CrmCourse>> CrmCoursesByParentIdAsync(int parentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new CrmCourse record.
    /// </summary>
    Task<CrmCourse> CreateCrmCourse(CrmCourse entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing CrmCourse record.
    /// </summary>
    void UpdateCrmCourse(CrmCourse entity);

    /// <summary>
    /// Deletes a CrmCourse record.
    /// </summary>
    Task DeleteCrmCourse(CrmCourse entity, bool trackChanges, CancellationToken cancellationToken = default);


    //// List
    //Task<IEnumerable<CrmCourse>> CoursesAsync(bool trackChanges, CancellationToken cancellationToken = default);

    //// Single
    //Task<CrmCourse?> CourseAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

    //// CUD
    //void CreateCourse(CrmCourse course);
    //void UpdateCourse(CrmCourse course);
    //void DeleteCourse(CrmCourse course);
  }
}




//using Domain.Entities.Entities.CRM;

//namespace bdDevCRM.RepositoriesContracts.Core.SystemAdmin;

//public interface ICrmCourseRepository : IRepositoryBase<CrmCourse>
//{
//  Task<IEnumerable<CrmCourse>> ActiveCoursesAsync(bool track);
//  Task<IEnumerable<CrmCourse>> CoursesAsync(bool track);

//  Task<CrmCourse?> CourseAsync(int id, bool track);
//}
