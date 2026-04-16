using Domain.Contracts.CRM;
using Domain.Entities.Entities.CRM;
using Infrastructure.Repositories;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

internal sealed class CrmCourseIntakeRepository : RepositoryBase<CrmCourseIntake>, ICrmCourseIntakeRepository
{
    public CrmCourseIntakeRepository(CrmContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<CrmCourseIntake?> CourseIntakeAsync(int courseIntakeId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(ci => ci.CourseIntakeId == courseIntakeId, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<CrmCourseIntake>> CourseIntakesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(null, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<CrmCourseIntake>> CourseIntakesByCourseIdAsync(int courseId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(ci => ci.CourseId == courseId, null, trackChanges, false, cancellationToken);
    }
}
