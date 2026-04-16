using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Core.SystemAdmin;

public interface IApproverDetailsRepository : IRepositoryBase<ApproverDetails>
{
    Task<ApproverDetails?> ApproverDetailsAsync(int remarksId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<ApproverDetails>> ApproverDetailsListAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<ApproverDetails>> ActiveApproverDetailsAsync(bool trackChanges, CancellationToken cancellationToken = default);
}
