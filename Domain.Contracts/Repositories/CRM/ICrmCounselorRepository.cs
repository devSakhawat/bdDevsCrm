using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmCounselorRepository : IRepositoryBase<CrmCounselor>
{
    /// <summary>Retrieves all CrmCounselor records asynchronously.</summary>
    Task<IEnumerable<CrmCounselor>> CrmCounselorsAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a single CrmCounselor record by ID asynchronously.</summary>
    Task<CrmCounselor?> CrmCounselorAsync(int counselorId, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves CrmCounselor records by a collection of IDs asynchronously.</summary>
    Task<IEnumerable<CrmCounselor>> CrmCounselorsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}
