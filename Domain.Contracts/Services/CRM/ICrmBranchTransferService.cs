using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

/// <summary>Service contract for CRM branch transfer management operations.</summary>
public interface ICrmBranchTransferService
{
    /// <summary>Creates a new branch transfer request.</summary>
    Task<CrmBranchTransferDto> CreateAsync(CreateCrmBranchTransferRecord record, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing branch transfer record.</summary>
    Task<CrmBranchTransferDto> UpdateAsync(UpdateCrmBranchTransferRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Deletes a branch transfer record.</summary>
    Task DeleteAsync(DeleteCrmBranchTransferRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a single branch transfer record by ID.</summary>
    Task<CrmBranchTransferDto> BranchTransferAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves all branch transfer records.</summary>
    Task<IEnumerable<CrmBranchTransferDto>> BranchTransfersAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a paginated summary grid of branch transfers.</summary>
    Task<GridEntity<CrmBranchTransferDto>> BranchTransfersSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);

    /// <summary>Retrieves branch transfers by entity type and entity ID.</summary>
    Task<IEnumerable<CrmBranchTransferDto>> BranchTransfersByEntityAsync(byte entityType, int entityId, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Approves a pending branch transfer.</summary>
    Task<CrmBranchTransferDto> ApproveAsync(ApproveCrmBranchTransferRecord record, bool trackChanges, CancellationToken cancellationToken = default);
}
