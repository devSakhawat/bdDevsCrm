using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface ITimesheetService
{
    Task<TimesheetDto> CreateAsync(CreateTimesheetRecord record, CancellationToken cancellationToken = default);
    Task<TimesheetDto> UpdateAsync(UpdateTimesheetRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteTimesheetRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<TimesheetDto> TimesheetAsync(int timesheetId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<TimesheetDto>> TimesheetsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<TimesheetDDLDto>> TimesheetsForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<TimesheetDto>> TimesheetsByEmployeeAsync(int hrRecordId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<TimesheetDto>> TimesheetsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
