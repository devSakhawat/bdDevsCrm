using Domain.Contracts.Repositories.Common;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Core.SystemAdmin;

public interface IThanaRepository : IRepositoryBase<Thana>
{
    Task<Thana?> ThanaAsync(int thanaId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<Thana>> ThanasAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<Thana>> ThanasByDistrictIdAsync(int districtId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<Thana>> ActiveThanasAsync(bool trackChanges, CancellationToken cancellationToken = default);
}
