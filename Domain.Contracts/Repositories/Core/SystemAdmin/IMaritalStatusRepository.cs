using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Repositories.Core.SystemAdmin;

public interface IMaritalStatusRepository : IRepositoryBase<MaritalStatus>
{
    Task<MaritalStatus?> MaritalStatusAsync(int maritalStatusId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<MaritalStatus>> MaritalStatusesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<MaritalStatus>> ActiveMaritalStatusesAsync(bool trackChanges, CancellationToken cancellationToken = default);
}
