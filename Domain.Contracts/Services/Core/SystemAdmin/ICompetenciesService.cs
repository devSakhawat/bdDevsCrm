using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface ICompetenciesService
{
    Task<CompetenciesDto> CreateAsync(CreateCompetenciesRecord record, CancellationToken cancellationToken = default);
    Task<CompetenciesDto> UpdateAsync(UpdateCompetenciesRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCompetenciesRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CompetenciesDto> CompetencyAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CompetenciesDto>> CompetenciesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CompetenciesDDLDto>> CompetenciesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<CompetenciesDto>> CompetenciesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
