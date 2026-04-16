using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IEmployeetypeService
{
    Task<EmployeeTypeDto> CreateAsync(CreateEmployeetypeRecord record, CancellationToken cancellationToken = default);
    Task<EmployeeTypeDto> UpdateAsync(UpdateEmployeetypeRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteEmployeetypeRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<EmployeeTypeDto> EmployeetypeAsync(int employeetypeid, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<EmployeeTypeDto>> EmployeetypesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<EmployeeTypeDto>> EmployeetypesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<EmployeeTypeDto>> EmployeetypesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
