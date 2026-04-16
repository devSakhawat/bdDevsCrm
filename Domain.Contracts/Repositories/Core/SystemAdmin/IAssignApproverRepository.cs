using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Core.SystemAdmin;

public interface IAssignApproverRepository : IRepositoryBase<AssignApprover>
{
    Task<AssignApprover?> AssignApproverAsync(int assignApproverId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<AssignApprover>> AssignApproversAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<AssignApprover>> ActiveAssignApproversAsync(bool trackChanges, CancellationToken cancellationToken = default);
}
