using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmDegreeLevelService
{
    Task<CrmDegreeLevelDto> CreateAsync(CreateCrmDegreeLevelRecord record, CancellationToken cancellationToken = default);
    Task<CrmDegreeLevelDto> UpdateAsync(UpdateCrmDegreeLevelRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmDegreeLevelRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmDegreeLevelDto> DegreeLevelAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmDegreeLevelDto>> DegreeLevelsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmDegreeLevelDto>> DegreeLevelsForDDLAsync(CancellationToken cancellationToken = default);
    Task<GridEntity<CrmDegreeLevelDto>> DegreeLevelsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
