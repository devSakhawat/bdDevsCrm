using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmCourseFeeRepository : IRepositoryBase<CrmCourseFee>
{
    Task<IEnumerable<CrmCourseFee>> CrmCourseFeesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmCourseFee?> CrmCourseFeeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCourseFee>> CrmCourseFeesByCourseIdAsync(int courseId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCourseFee>> CrmCourseFeesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}
