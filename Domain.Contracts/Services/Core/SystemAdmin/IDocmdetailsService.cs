using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IDocmdetailsService
{
    Task<DocmdetailsDto> CreateAsync(CreateDocmdetailsRecord record, CancellationToken cancellationToken = default);
    Task<DocmdetailsDto> UpdateAsync(UpdateDocmdetailsRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteDocmdetailsRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<DocmdetailsDto> DocmdetailsAsync(int documentId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DocmdetailsDto>> DocmdetailsListAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DocmdetailsDto>> DocmdetailsForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<DocmdetailsDto>> DocmdetailsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
