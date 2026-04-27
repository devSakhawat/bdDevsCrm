using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmFollowUpRepository : IRepositoryBase<CrmFollowUp>
{
    /// <summary>Retrieves all CrmFollowUp records asynchronously.</summary>
    Task<IEnumerable<CrmFollowUp>> CrmFollowUpsAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a single CrmFollowUp record by ID asynchronously.</summary>
    Task<CrmFollowUp?> CrmFollowUpAsync(int followUpId, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves CrmFollowUp records by a collection of IDs asynchronously.</summary>
    Task<IEnumerable<CrmFollowUp>> CrmFollowUpsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}
