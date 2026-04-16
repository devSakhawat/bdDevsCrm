using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Core.SystemAdmin;

public interface IHolidayRepository : IRepositoryBase<Holiday>
{
    Task<Holiday?> HolidayAsync(int holidayId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<Holiday>> HolidaysAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<Holiday>> ActiveHolidaysAsync(bool trackChanges, CancellationToken cancellationToken = default);
}
