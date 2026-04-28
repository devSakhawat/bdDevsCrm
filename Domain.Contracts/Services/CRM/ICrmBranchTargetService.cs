using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmBranchTargetService
{
    Task<CrmBranchTargetDto> CreateAsync(CreateCrmBranchTargetRecord record, CancellationToken cancellationToken = default);
    Task<CrmBranchTargetDto> UpdateAsync(UpdateCrmBranchTargetRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmBranchTargetRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmBranchTargetDto> BranchTargetAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmBranchTargetDto>> BranchTargetsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmBranchTargetDto>> BranchTargetsByBranchIdAsync(int branchId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<CrmBranchTargetDto>> BranchTargetsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
