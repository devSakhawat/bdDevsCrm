using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmCounsellingSessionService
{
    Task<CrmCounsellingSessionDto> CreateAsync(CreateCrmCounsellingSessionRecord record, CancellationToken cancellationToken = default);
    Task<CrmCounsellingSessionDto> UpdateAsync(UpdateCrmCounsellingSessionRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmCounsellingSessionRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmCounsellingSessionDto> CounsellingSessionAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCounsellingSessionDto>> CounsellingSessionsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCounsellingSessionDto>> CounsellingSessionsByLeadIdAsync(int leadId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<CrmCounsellingSessionDto>> CounsellingSessionsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmProgramEligibilityDto>> EligibleProgramsAsync(int studentId, CancellationToken cancellationToken = default);
}
