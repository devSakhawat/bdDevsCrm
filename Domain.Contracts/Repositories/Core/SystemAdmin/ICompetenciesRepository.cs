using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Repositories.Core.SystemAdmin;

public interface ICompetenciesRepository : IRepositoryBase<Competencies>
{
    Task<Competencies?> CompetencyAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<Competencies>> CompetenciesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<Competencies>> ActiveCompetenciesAsync(bool trackChanges, CancellationToken cancellationToken = default);
}
