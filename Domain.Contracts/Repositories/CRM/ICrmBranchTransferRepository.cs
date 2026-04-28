using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmBranchTransferRepository : IRepositoryBase<CrmBranchTransfer>
{
    /// <summary>Retrieves all CrmBranchTransfer records asynchronously.</summary>
    Task<IEnumerable<CrmBranchTransfer>> CrmBranchTransfersAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a single CrmBranchTransfer record by ID asynchronously.</summary>
    Task<CrmBranchTransfer?> CrmBranchTransferAsync(int transferId, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves CrmBranchTransfer records by entity type and entity ID.</summary>
    Task<IEnumerable<CrmBranchTransfer>> CrmBranchTransfersByEntityAsync(byte entityType, int entityId, bool trackChanges, CancellationToken cancellationToken = default);
}
