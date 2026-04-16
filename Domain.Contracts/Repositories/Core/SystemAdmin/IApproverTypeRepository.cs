using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Core.SystemAdmin;

public interface IApproverTypeRepository : IRepositoryBase<ApproverType>
{
    Task<ApproverType?> ApproverTypeAsync(int approverTypeId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<ApproverType>> ApproverTypesAsync(bool trackChanges, CancellationToken cancellationToken = default);
}
