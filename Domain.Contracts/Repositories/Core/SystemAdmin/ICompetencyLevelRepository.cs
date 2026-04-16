using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Repositories.Core.SystemAdmin;

public interface ICompetencyLevelRepository : IRepositoryBase<CompetencyLevel>
{
    Task<CompetencyLevel?> CompetencyLevelAsync(int levelId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CompetencyLevel>> CompetencyLevelsAsync(bool trackChanges, CancellationToken cancellationToken = default);
}
