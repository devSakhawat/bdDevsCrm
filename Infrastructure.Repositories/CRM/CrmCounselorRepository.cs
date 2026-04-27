using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>Repository for CrmCounselor data access operations.</summary>
public class CrmCounselorRepository : RepositoryBase<CrmCounselor>, ICrmCounselorRepository
{
    public CrmCounselorRepository(CrmContext context) : base(context) { }

    /// <summary>Retrieves all CrmCounselor records asynchronously.</summary>
    public async Task<IEnumerable<CrmCounselor>> CrmCounselorsAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.CounselorId, trackChanges, cancellationToken);

    /// <summary>Retrieves a single CrmCounselor record by ID asynchronously.</summary>
    public async Task<CrmCounselor?> CrmCounselorAsync(int counselorId, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.CounselorId.Equals(counselorId), trackChanges, cancellationToken);

    /// <summary>Retrieves CrmCounselor records by a collection of IDs asynchronously.</summary>
    public async Task<IEnumerable<CrmCounselor>> CrmCounselorsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByIdsAsync(x => ids.Contains(x.CounselorId), trackChanges, cancellationToken);
}
