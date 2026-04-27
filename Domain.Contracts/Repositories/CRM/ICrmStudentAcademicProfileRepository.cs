using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmStudentAcademicProfileRepository : IRepositoryBase<CrmStudentAcademicProfile>
{
    Task<IEnumerable<CrmStudentAcademicProfile>> StudentAcademicProfilesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmStudentAcademicProfile?> CrmStudentAcademicProfileAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmStudentAcademicProfile>> StudentAcademicProfilesByStudentIdAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmStudentAcademicProfile>> StudentAcademicProfilesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}
