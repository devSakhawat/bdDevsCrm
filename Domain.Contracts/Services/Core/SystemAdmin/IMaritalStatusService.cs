using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IMaritalStatusService
{
    Task<MaritalStatusDto> CreateAsync(CreateMaritalStatusRecord record, CancellationToken cancellationToken = default);
    Task<MaritalStatusDto> UpdateAsync(UpdateMaritalStatusRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteMaritalStatusRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<MaritalStatusDto> MaritalStatusAsync(int maritalStatusId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<MaritalStatusDto>> MaritalStatusesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<MaritalStatusDDLDto>> MaritalStatusesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<MaritalStatusDto>> MaritalStatusesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
