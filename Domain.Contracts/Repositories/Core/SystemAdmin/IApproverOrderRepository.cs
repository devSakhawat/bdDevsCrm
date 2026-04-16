using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Core.SystemAdmin;

public interface IApproverOrderRepository : IRepositoryBase<ApproverOrder>
{
    Task<ApproverOrder?> ApproverOrderAsync(int approverOrderId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<ApproverOrder>> ApproverOrdersAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<ApproverOrder>> ActiveApproverOrdersAsync(bool trackChanges, CancellationToken cancellationToken = default);
}
