using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IHolidayService
{
    Task<HolidayDto> CreateAsync(CreateHolidayRecord record, CancellationToken cancellationToken = default);
    Task<HolidayDto> UpdateAsync(UpdateHolidayRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteHolidayRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<HolidayDto> HolidayAsync(int holidayId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<HolidayDto>> HolidaysAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<HolidayDDLDto>> HolidaysForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<HolidayDto>> HolidaysSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
