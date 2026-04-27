using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmStudentAcademicProfileRepository : RepositoryBase<CrmStudentAcademicProfile>, ICrmStudentAcademicProfileRepository
{
    public CrmStudentAcademicProfileRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmStudentAcademicProfile>> StudentAcademicProfilesAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.StudentAcademicProfileId, trackChanges, cancellationToken);

    public async Task<CrmStudentAcademicProfile?> CrmStudentAcademicProfileAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.StudentAcademicProfileId == id, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmStudentAcademicProfile>> StudentAcademicProfilesByStudentIdAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByConditionAsync(x => x.StudentId == studentId, x => x.StudentAcademicProfileId, trackChanges, false, cancellationToken);

    public async Task<IEnumerable<CrmStudentAcademicProfile>> StudentAcademicProfilesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByIdsAsync(x => ids.Contains(x.StudentAcademicProfileId), trackChanges, cancellationToken);
}
