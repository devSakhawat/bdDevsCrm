using Domain.Contracts.Repositories.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Infrastructure.Repositories;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories.Core.SystemAdmin;

internal sealed class BoardInstituteRepository : RepositoryBase<BoardInstitute>, IBoardInstituteRepository
{
    public BoardInstituteRepository(CrmContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<BoardInstitute?> BoardInstituteAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(b => b.BoardInstituteId == id, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<BoardInstitute>> BoardInstitutesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(null, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<BoardInstitute>> ActiveBoardInstitutesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(b => b.IsActive == 1, null, trackChanges, false, cancellationToken);
    }
}
