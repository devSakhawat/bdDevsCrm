using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IDocmdetailshistoryService
{
    Task<DocmdetailshistoryDto> CreateAsync(CreateDocmdetailshistoryRecord record, CancellationToken cancellationToken = default);
    Task<DocmdetailshistoryDto> UpdateAsync(UpdateDocmdetailshistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteDocmdetailshistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<DocmdetailshistoryDto> DocmdetailshistoryAsync(int documentHistoryId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DocmdetailshistoryDto>> DocmdetailshistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DocmdetailshistoryDto>> DocmdetailshistoriesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<DocmdetailshistoryDto>> DocmdetailshistoriesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
