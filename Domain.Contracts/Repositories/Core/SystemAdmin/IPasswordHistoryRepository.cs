using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Repositories.Core.SystemAdmin;

public interface IPasswordHistoryRepository : IRepositoryBase<PasswordHistory>
{
    Task<PasswordHistory?> PasswordHistoryAsync(int historyId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<PasswordHistory>> PasswordHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default);
}
