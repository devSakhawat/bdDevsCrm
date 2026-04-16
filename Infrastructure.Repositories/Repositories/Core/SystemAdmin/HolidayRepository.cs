using Domain.Contracts.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Infrastructure.Repositories.Repositories.Common;
using Infrastructure.Sql;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories.Core.SystemAdmin;

internal sealed class HolidayRepository : RepositoryBase<Holiday>, IHolidayRepository
{
    public HolidayRepository(ApplicationDbContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<Holiday?> HolidayAsync(int holidayId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(h => h.HolidayId == holidayId, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<Holiday>> HolidaysAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<Holiday>> ActiveHolidaysAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByWhereAsync(h => h.HolidayDate >= DateOnly.FromDateTime(DateTime.Now), trackChanges, cancellationToken);
    }
}
