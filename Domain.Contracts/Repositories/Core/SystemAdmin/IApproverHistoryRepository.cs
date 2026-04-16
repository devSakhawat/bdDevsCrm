using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Core.SystemAdmin;

public interface IApproverHistoryRepository : IRepositoryBase<ApproverHistory>
{
    Task<ApproverHistory?> ApproverHistoryAsync(int assignApproverId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<ApproverHistory>> ApproverHistoryListAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<ApproverHistory>> ActiveApproverHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default);
}
