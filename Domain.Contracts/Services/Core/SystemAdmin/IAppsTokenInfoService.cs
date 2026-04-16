using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IAppsTokenInfoService
{
    Task<AppsTokenInfoDto> CreateAsync(CreateAppsTokenInfoRecord record, CancellationToken cancellationToken = default);
    Task<AppsTokenInfoDto> UpdateAsync(UpdateAppsTokenInfoRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteAppsTokenInfoRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<AppsTokenInfoDto> AppsTokenInfoAsync(int appsTokenInfoId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppsTokenInfoDto>> AppsTokenInfosAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppsTokenInfoDDLDto>> AppsTokenInfosForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<AppsTokenInfoDto>> AppsTokenInfosSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
