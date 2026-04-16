using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Repositories.Core.SystemAdmin;

public interface IBoardInstituteRepository : IRepositoryBase<BoardInstitute>
{
    Task<BoardInstitute?> BoardInstituteAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<BoardInstitute>> BoardInstitutesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<BoardInstitute>> ActiveBoardInstitutesAsync(bool trackChanges, CancellationToken cancellationToken = default);
}
