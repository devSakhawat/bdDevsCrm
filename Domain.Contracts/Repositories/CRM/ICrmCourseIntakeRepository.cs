using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmCourseIntakeRepository : IRepositoryBase<CrmCourseIntake>
{
    Task<CrmCourseIntake?> CourseIntakeAsync(int courseIntakeId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCourseIntake>> CourseIntakesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCourseIntake>> CourseIntakesByCourseIdAsync(int courseId, bool trackChanges, CancellationToken cancellationToken = default);
}
