using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface ICompetencyLevelService
{
    Task<CompetencyLevelDto> CreateAsync(CreateCompetencyLevelRecord record, CancellationToken cancellationToken = default);
    Task<CompetencyLevelDto> UpdateAsync(UpdateCompetencyLevelRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCompetencyLevelRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CompetencyLevelDto> CompetencyLevelAsync(int levelId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CompetencyLevelDto>> CompetencyLevelsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CompetencyLevelDDLDto>> CompetencyLevelsForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<CompetencyLevelDto>> CompetencyLevelsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
