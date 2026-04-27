using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmFacultyRepository : RepositoryBase<CrmFaculty>, ICrmFacultyRepository
{
    public CrmFacultyRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmFaculty>> CrmFacultiesAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.FacultyName, trackChanges, cancellationToken);

    public async Task<CrmFaculty?> CrmFacultyAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.FacultyId == id, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmFaculty>> CrmFacultiesByInstituteIdAsync(int instituteId, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByIdsAsync(x => x.InstituteId == instituteId, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmFaculty>> CrmFacultiesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByIdsAsync(x => ids.Contains(x.FacultyId), trackChanges, cancellationToken);
}
