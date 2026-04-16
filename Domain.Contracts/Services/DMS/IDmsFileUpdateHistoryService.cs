using bdDevs.Shared.DataTransferObjects.DMS;
using bdDevs.Shared.Records.DMS;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.DMS;

public interface IDmsFileUpdateHistoryService
{
    Task<DmsFileUpdateHistoryDto> CreateAsync(CreateDmsFileUpdateHistoryRecord record, CancellationToken cancellationToken = default);
    Task<DmsFileUpdateHistoryDto> UpdateAsync(UpdateDmsFileUpdateHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteDmsFileUpdateHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<DmsFileUpdateHistoryDto> FileUpdateHistoryAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DmsFileUpdateHistoryDto>> FileUpdateHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DmsFileUpdateHistoryDDLDto>> FileUpdateHistoriesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<DmsFileUpdateHistoryDto>> FileUpdateHistoriesByEntityAsync(string entityId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<DmsFileUpdateHistoryDto>> FileUpdateHistoriesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
