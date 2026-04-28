using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmStudentStatusHistoryService
{
    Task<CrmStudentStatusHistoryDto> CreateAsync(CreateCrmStudentStatusHistoryRecord record, CancellationToken cancellationToken = default);
    Task<CrmStudentStatusHistoryDto> UpdateAsync(UpdateCrmStudentStatusHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmStudentStatusHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmStudentStatusHistoryDto> StudentStatusHistoryAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmStudentStatusHistoryDto>> StudentStatusHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmStudentStatusHistoryDto>> StudentStatusHistoriesByStudentIdAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<CrmStudentStatusHistoryDto>> StudentStatusHistoriesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
