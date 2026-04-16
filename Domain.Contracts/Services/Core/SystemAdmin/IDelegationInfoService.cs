using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IDelegationInfoService
{
    Task<DelegationInfoDto> CreateAsync(CreateDelegationInfoRecord record, CancellationToken cancellationToken = default);
    Task<DelegationInfoDto> UpdateAsync(UpdateDelegationInfoRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteDelegationInfoRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<DelegationInfoDto> DelegationInfoAsync(int deligationId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DelegationInfoDto>> DelegationInfosAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DelegationInfoDto>> DelegationInfosForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<DelegationInfoDto>> DelegationInfosSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
