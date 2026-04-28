using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmScholarshipApplicationService
{
    Task<CrmScholarshipApplicationDto> CreateAsync(CreateCrmScholarshipApplicationRecord record, CancellationToken cancellationToken = default);
    Task<CrmScholarshipApplicationDto> UpdateAsync(UpdateCrmScholarshipApplicationRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmScholarshipApplicationRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmScholarshipApplicationDto> ScholarshipApplicationAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmScholarshipApplicationDto>> ScholarshipApplicationsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmScholarshipApplicationDto>> ScholarshipApplicationsByApplicationIdAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<CrmScholarshipApplicationDto>> ScholarshipApplicationsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
    Task<CrmScholarshipCommissionImpactDto> CommissionImpactAsync(int applicationId, CancellationToken cancellationToken = default);
}
