using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmApplicationService
{
    Task<CrmApplicationDto> CreateAsync(CreateCrmApplicationRecord record, CancellationToken cancellationToken = default);
    Task<CrmApplicationDto> UpdateAsync(UpdateCrmApplicationRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmApplicationRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmApplicationDto> ApplicationAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmApplicationDto>> ApplicationsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmApplicationDto>> ApplicationsByStudentIdAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<CrmApplicationGridDto>> ApplicationsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmApplicationGridDto>> ApplicationsBoardAsync(CancellationToken cancellationToken = default);
    Task<CrmApplicationDto> ChangeStatusAsync(ChangeCrmApplicationStatusRecord record, CancellationToken cancellationToken = default);
}
