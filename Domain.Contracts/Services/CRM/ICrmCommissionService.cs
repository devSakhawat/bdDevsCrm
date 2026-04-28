using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmCommissionService
{
    Task<CrmCommissionDto> CreateAsync(CreateCrmCommissionRecord record, CancellationToken cancellationToken = default);
    Task<CrmCommissionDto> UpdateAsync(UpdateCrmCommissionRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmCommissionRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmCommissionDto> CommissionAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCommissionDto>> CommissionsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCommissionDto>> CommissionsByApplicationIdAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCommissionDto>> CommissionsByAgentIdAsync(int agentId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<CrmCommissionDto>> CommissionsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
    Task<CrmCommissionDto> ChangeStatusAsync(ChangeCrmCommissionStatusRecord record, CancellationToken cancellationToken = default);
    Task<CrmCommissionDto> GenerateInvoiceAsync(int commissionId, CancellationToken cancellationToken = default);
    Task<CrmCommissionDashboardDto> DashboardAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCommissionAgingDto>> AgingReportAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmAgentCommissionSummaryDto>> AgentSummaryAsync(CancellationToken cancellationToken = default);
    Task<CrmCommissionDto?> EnsureCommissionForEnrollmentAsync(int applicationId, int changedBy, CancellationToken cancellationToken = default);
    Task<CrmCommissionDto?> RecalculateByApplicationAsync(int applicationId, int changedBy, CancellationToken cancellationToken = default);
    Task ReverseByRefundAsync(int paymentId, decimal refundAmount, DateTime refundDate, int changedBy, string? reason, CancellationToken cancellationToken = default);
}
