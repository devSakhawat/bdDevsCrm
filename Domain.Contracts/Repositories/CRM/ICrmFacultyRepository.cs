using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmFacultyRepository : IRepositoryBase<CrmFaculty>
{
    Task<IEnumerable<CrmFaculty>> CrmFacultiesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmFaculty?> CrmFacultyAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmFaculty>> CrmFacultiesByInstituteIdAsync(int instituteId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmFaculty>> CrmFacultiesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}
