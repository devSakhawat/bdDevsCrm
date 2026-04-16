using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Core.SystemAdmin;

public interface ITimesheetRepository : IRepositoryBase<Timesheet>
{
    Task<Timesheet?> TimesheetAsync(int timesheetId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<Timesheet>> TimesheetsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<Timesheet>> TimesheetsByEmployeeAsync(int hrRecordId, bool trackChanges, CancellationToken cancellationToken = default);
}
