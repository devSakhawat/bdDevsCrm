using Domain.Contracts.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Infrastructure.Repositories.Repositories.Common;
using Infrastructure.Sql;

namespace Infrastructure.Repositories.Repositories.Core.SystemAdmin;

internal sealed class TimesheetRepository : RepositoryBase<Timesheet>, ITimesheetRepository
{
    public TimesheetRepository(ApplicationDbContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<Timesheet?> TimesheetAsync(int timesheetId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(t => t.Timesheetid == timesheetId, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<Timesheet>> TimesheetsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<Timesheet>> TimesheetsByEmployeeAsync(int hrRecordId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByWhereAsync(t => t.Hrrecordid == hrRecordId, trackChanges, cancellationToken);
    }
}
