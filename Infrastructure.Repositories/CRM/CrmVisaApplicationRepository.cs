using Domain.Contracts.CRM;
using Domain.Entities.Entities.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmVisaApplicationRepository : RepositoryBase<CrmVisaApplication>, ICrmVisaApplicationRepository
{
    public CrmVisaApplicationRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmVisaApplication>> VisaApplicationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.VisaApplicationId, trackChanges, cancellationToken);

    public async Task<CrmVisaApplication?> VisaApplicationAsync(int visaApplicationId, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.VisaApplicationId == visaApplicationId, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmVisaApplication>> VisaApplicationsByApplicationIdAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByConditionAsync(x => x.ApplicationId == applicationId, x => x.VisaApplicationId, trackChanges, false, cancellationToken);

    public async Task<IEnumerable<CrmVisaApplication>> VisaApplicationsByStudentIdAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByConditionAsync(x => x.StudentId == studentId, x => x.VisaApplicationId, trackChanges, false, cancellationToken);
}
