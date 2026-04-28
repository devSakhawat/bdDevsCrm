using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmScholarshipApplicationRepository : IRepositoryBase<CrmScholarshipApplication>
{
    Task<IEnumerable<CrmScholarshipApplication>> ScholarshipApplicationsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmScholarshipApplication?> ScholarshipApplicationAsync(int scholarshipApplicationId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmScholarshipApplication>> ScholarshipApplicationsByApplicationIdAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default);
}
