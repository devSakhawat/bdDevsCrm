using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmStudentStatus data access operations.
/// </summary>
public class CrmStudentStatusRepository : RepositoryBase<CrmStudentStatus>, ICrmStudentStatusRepository
{
    public CrmStudentStatusRepository(CrmContext context) : base(context) { }

    /// <summary>Retrieves all CrmStudentStatus records asynchronously.</summary>
    public async Task<IEnumerable<CrmStudentStatus>> CrmStudentStatusesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(x => x.StudentStatusId, trackChanges, cancellationToken);
    }

    /// <summary>Retrieves a single CrmStudentStatus record by ID asynchronously.</summary>
    public async Task<CrmStudentStatus?> CrmStudentStatusAsync(int crmstudentstatusid, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(
            x => x.StudentStatusId.Equals(crmstudentstatusid),
            trackChanges,
            cancellationToken);
    }

    /// <summary>Retrieves CrmStudentStatus records by a collection of IDs asynchronously.</summary>
    public async Task<IEnumerable<CrmStudentStatus>> CrmStudentStatusesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByIdsAsync(x => ids.Contains(x.StudentStatusId), trackChanges, cancellationToken);
    }
}
