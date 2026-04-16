using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IPasswordHistoryService
{
    Task<PasswordHistoryDto> CreateAsync(CreatePasswordHistoryRecord record, CancellationToken cancellationToken = default);
    Task<PasswordHistoryDto> UpdateAsync(UpdatePasswordHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeletePasswordHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<PasswordHistoryDto> PasswordHistoryAsync(int historyId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<PasswordHistoryDto>> PasswordHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<PasswordHistoryDDLDto>> PasswordHistoriesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<PasswordHistoryDto>> PasswordHistoriesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
