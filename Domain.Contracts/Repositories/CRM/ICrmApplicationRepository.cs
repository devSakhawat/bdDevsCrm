using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmApplicationRepository : IRepositoryBase<CrmApplication>
{
    Task<IEnumerable<CrmApplication>> CrmApplicationsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmApplication?> CrmApplicationAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmApplication>> CrmApplicationsByStudentIdAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmApplication>> CrmApplicationsByStatusAsync(byte status, bool trackChanges, CancellationToken cancellationToken = default);
}
