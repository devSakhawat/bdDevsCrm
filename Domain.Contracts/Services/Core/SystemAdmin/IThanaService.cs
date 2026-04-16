using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IThanaService
{
    Task<ThanaDto> CreateAsync(CreateThanaRecord record, CancellationToken cancellationToken = default);
    Task<ThanaDto> UpdateAsync(UpdateThanaRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteThanaRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<ThanaDto> ThanaAsync(int thanaId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<ThanaDto>> ThanasAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<ThanaDDLDto>> ThanasForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<ThanaDto>> ThanasByDistrictIdAsync(int districtId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<ThanaDto>> ThanasSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
