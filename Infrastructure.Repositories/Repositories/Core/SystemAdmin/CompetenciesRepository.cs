using Domain.Contracts.Repositories.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Infrastructure.Repositories.Repositories.Common;
using Infrastructure.Sql;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories.Core.SystemAdmin;

internal sealed class CompetenciesRepository : RepositoryBase<Competencies>, ICompetenciesRepository
{
    public CompetenciesRepository(ApplicationDbContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<Competencies?> CompetencyAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(c => c.Id == id, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<Competencies>> CompetenciesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<Competencies>> ActiveCompetenciesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByWhereAsync(c => c.IsActive == 1, trackChanges, cancellationToken);
    }
}
