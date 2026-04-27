using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmVisaApplicationRepository : IRepositoryBase<CrmVisaApplication>
{
    Task<IEnumerable<CrmVisaApplication>> VisaApplicationsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmVisaApplication?> VisaApplicationAsync(int visaApplicationId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmVisaApplication>> VisaApplicationsByApplicationIdAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmVisaApplication>> VisaApplicationsByStudentIdAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default);
}
