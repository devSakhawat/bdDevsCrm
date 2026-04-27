using Domain.Contracts.CRM;
using Domain.Entities.Entities.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmScholarshipApplicationRepository : RepositoryBase<CrmScholarshipApplication>, ICrmScholarshipApplicationRepository
{
    public CrmScholarshipApplicationRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmScholarshipApplication>> ScholarshipApplicationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.ScholarshipApplicationId, trackChanges, cancellationToken);

    public async Task<CrmScholarshipApplication?> ScholarshipApplicationAsync(int scholarshipApplicationId, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.ScholarshipApplicationId == scholarshipApplicationId, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmScholarshipApplication>> ScholarshipApplicationsByApplicationIdAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByConditionAsync(x => x.ApplicationId == applicationId, x => x.ScholarshipApplicationId, trackChanges, false, cancellationToken);
}
