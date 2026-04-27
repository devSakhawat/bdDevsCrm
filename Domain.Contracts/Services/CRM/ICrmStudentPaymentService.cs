using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmStudentPaymentService
{
    Task<CrmStudentPaymentDto> CreateAsync(CreateCrmStudentPaymentRecord record, CancellationToken cancellationToken = default);
    Task<CrmStudentPaymentDto> UpdateAsync(UpdateCrmStudentPaymentRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmStudentPaymentRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmStudentPaymentDto> StudentPaymentAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmStudentPaymentDto>> StudentPaymentsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmStudentPaymentDto>> StudentPaymentsByStudentIdAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmStudentPaymentDto>> StudentPaymentsByApplicationIdAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<CrmStudentPaymentDto>> StudentPaymentsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
    Task<CrmStudentPaymentDto> ChangeStatusAsync(ChangeCrmStudentPaymentStatusRecord record, CancellationToken cancellationToken = default);
    Task<CrmStudentPaymentReceiptDto> GenerateReceiptAsync(int paymentId, CancellationToken cancellationToken = default);
}
