using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IAssemblyInfoService
{
    Task<AssemblyInfoDto> CreateAsync(CreateAssemblyInfoRecord record, CancellationToken cancellationToken = default);
    Task<AssemblyInfoDto> UpdateAsync(UpdateAssemblyInfoRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteAssemblyInfoRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<AssemblyInfoDto> AssemblyInfoAsync(int assemblyInfoId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<AssemblyInfoDto>> AssemblyInfosAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<AssemblyInfoDto>> AssemblyInfosForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<AssemblyInfoDto>> AssemblyInfosSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
