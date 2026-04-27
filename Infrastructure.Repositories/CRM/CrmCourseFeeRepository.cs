using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmCourseFeeRepository : RepositoryBase<CrmCourseFee>, ICrmCourseFeeRepository
{
    public CrmCourseFeeRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmCourseFee>> CrmCourseFeesAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.CourseFeeId, trackChanges, cancellationToken);

    public async Task<CrmCourseFee?> CrmCourseFeeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.CourseFeeId == id, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmCourseFee>> CrmCourseFeesByCourseIdAsync(int courseId, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByConditionAsync(x => x.CourseId == courseId, x => x.CourseFeeId, trackChanges, false, cancellationToken);

    public async Task<IEnumerable<CrmCourseFee>> CrmCourseFeesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByIdsAsync(x => ids.Contains(x.CourseFeeId), trackChanges, cancellationToken);
}
