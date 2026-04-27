using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmApplicationRepository : RepositoryBase<CrmApplication>, ICrmApplicationRepository
{
    public CrmApplicationRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmApplication>> CrmApplicationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.ApplicationId, trackChanges, cancellationToken);

    public async Task<CrmApplication?> CrmApplicationAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.ApplicationId == applicationId, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmApplication>> CrmApplicationsByStudentIdAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByConditionAsync(x => x.StudentId == studentId, x => x.ApplicationId, trackChanges, false, cancellationToken);

    public async Task<IEnumerable<CrmApplication>> CrmApplicationsByStatusAsync(byte status, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByConditionAsync(x => x.Status == status, x => x.ApplicationId, trackChanges, false, cancellationToken);
}
