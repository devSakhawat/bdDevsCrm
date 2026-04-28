using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmVisaApplicationService
{
    Task<CrmVisaApplicationDto> CreateAsync(CreateCrmVisaApplicationRecord record, CancellationToken cancellationToken = default);
    Task<CrmVisaApplicationDto> UpdateAsync(UpdateCrmVisaApplicationRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmVisaApplicationRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmVisaApplicationDto> VisaApplicationAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmVisaApplicationDto>> VisaApplicationsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmVisaApplicationDto>> VisaApplicationsByApplicationIdAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmVisaApplicationDto>> VisaApplicationsByStudentIdAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<CrmVisaApplicationDto>> VisaApplicationsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
    Task<CrmVisaApplicationDto> ChangeStatusAsync(ChangeCrmVisaApplicationStatusRecord record, CancellationToken cancellationToken = default);
}
