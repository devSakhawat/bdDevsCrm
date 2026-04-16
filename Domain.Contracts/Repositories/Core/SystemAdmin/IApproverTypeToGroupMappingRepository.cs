using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Core.SystemAdmin;

public interface IApproverTypeToGroupMappingRepository : IRepositoryBase<ApproverTypeToGroupMapping>
{
    Task<ApproverTypeToGroupMapping?> ApproverTypeToGroupMappingAsync(int approverTypeMapId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<ApproverTypeToGroupMapping>> ApproverTypeToGroupMappingsAsync(bool trackChanges, CancellationToken cancellationToken = default);
}
