using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmSessionProgramShortlistService
{
    Task<CrmSessionProgramShortlistDto> CreateAsync(CreateCrmSessionProgramShortlistRecord record, CancellationToken cancellationToken = default);
    Task<CrmSessionProgramShortlistDto> UpdateAsync(UpdateCrmSessionProgramShortlistRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmSessionProgramShortlistRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmSessionProgramShortlistDto> SessionProgramShortlistAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmSessionProgramShortlistDto>> SessionProgramShortlistsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmSessionProgramShortlistDto>> SessionProgramShortlistsBySessionIdAsync(int sessionId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<CrmSessionProgramShortlistDto>> SessionProgramShortlistsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
