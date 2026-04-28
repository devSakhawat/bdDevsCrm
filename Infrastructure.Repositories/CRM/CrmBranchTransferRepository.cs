using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.CRM;

/// <summary>Repository for CrmBranchTransfer data access operations.</summary>
public class CrmBranchTransferRepository : RepositoryBase<CrmBranchTransfer>, ICrmBranchTransferRepository
{
    public CrmBranchTransferRepository(CrmContext context) : base(context) { }

    /// <summary>Retrieves all CrmBranchTransfer records ordered by RequestedDate descending.</summary>
    public async Task<IEnumerable<CrmBranchTransfer>> CrmBranchTransfersAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(x => x.RequestedDate, trackChanges, cancellationToken);
    }

    /// <summary>Retrieves a single CrmBranchTransfer record by ID asynchronously.</summary>
    public async Task<CrmBranchTransfer?> CrmBranchTransferAsync(int transferId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(
            x => x.TransferId == transferId,
            trackChanges,
            cancellationToken);
    }

    /// <summary>Retrieves CrmBranchTransfer records by entity type and entity ID.</summary>
    public async Task<IEnumerable<CrmBranchTransfer>> CrmBranchTransfersByEntityAsync(byte entityType, int entityId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByIdsAsync(
            x => x.EntityType == entityType && x.EntityId == entityId,
            trackChanges,
            cancellationToken);
    }
}
