using bdDevs.Shared.DataTransferObjects.DMS;
using bdDevs.Shared.Records.DMS;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.DMS;

public interface IDmsFileUpdateHistoryService
{
    Task<DmsFileUpdateHistoryDto> CreateAsync(CreateDmsFileUpdateHistoryRecord record, CancellationToken cancellationToken = default);
    Task<DmsFileUpdateHistoryDto> UpdateAsync(UpdateDmsFileUpdateHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteDmsFileUpdateHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<DmsFileUpdateHistoryDto> DmsFileUpdateHistoryAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DmsFileUpdateHistoryDto>> DmsFileUpdateHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DmsFileUpdateHistoryDto>> DmsFileUpdateHistoriesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<DmsFileUpdateHistoryDto>> DmsFileUpdateHistoriesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
