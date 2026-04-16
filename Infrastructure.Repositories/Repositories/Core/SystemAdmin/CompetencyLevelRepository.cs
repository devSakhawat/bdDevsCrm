using Domain.Contracts.Repositories.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Infrastructure.Repositories.Repositories.Common;
using Infrastructure.Sql;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories.Core.SystemAdmin;

internal sealed class CompetencyLevelRepository : RepositoryBase<CompetencyLevel>, ICompetencyLevelRepository
{
    public CompetencyLevelRepository(ApplicationDbContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<CompetencyLevel?> CompetencyLevelAsync(int levelId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(c => c.LevelId == levelId, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<CompetencyLevel>> CompetencyLevelsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(trackChanges, cancellationToken);
    }
}
