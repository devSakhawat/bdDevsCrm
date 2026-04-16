using bdDevs.Shared.DataTransferObjects.Core.HR;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IEmploymentService
{
    Task<EmploymentDto> CreateAsync(CreateEmploymentRecord record, CancellationToken cancellationToken = default);
    Task<EmploymentDto> UpdateAsync(UpdateEmploymentRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteEmploymentRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<EmploymentDto> EmploymentAsync(int hrrecordId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<EmploymentDto>> EmploymentsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<EmploymentDto>> EmploymentsForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<EmploymentDto>> EmploymentsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
