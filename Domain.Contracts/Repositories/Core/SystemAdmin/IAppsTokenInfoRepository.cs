using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Repositories.Core.SystemAdmin;

public interface IAppsTokenInfoRepository : IRepositoryBase<AppsTokenInfo>
{
    Task<AppsTokenInfo?> AppsTokenInfoAsync(int appsTokenInfoId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppsTokenInfo>> AppsTokenInfosAsync(bool trackChanges, CancellationToken cancellationToken = default);
}
