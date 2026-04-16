using Domain.Contracts.Repositories.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Infrastructure.Repositories.Repositories.Common;
using Infrastructure.Sql;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories.Core.SystemAdmin;

internal sealed class BoardInstituteRepository : RepositoryBase<BoardInstitute>, IBoardInstituteRepository
{
    public BoardInstituteRepository(ApplicationDbContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<BoardInstitute?> BoardInstituteAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(b => b.Id == id, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<BoardInstitute>> BoardInstitutesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<BoardInstitute>> ActiveBoardInstitutesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByWhereAsync(b => b.IsActive == 1, trackChanges, cancellationToken);
    }
}
