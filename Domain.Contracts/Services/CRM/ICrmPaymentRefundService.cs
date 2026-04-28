using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmPaymentRefundService
{
    Task<CrmPaymentRefundDto> CreateAsync(CreateCrmPaymentRefundRecord record, CancellationToken cancellationToken = default);
    Task<CrmPaymentRefundDto> UpdateAsync(UpdateCrmPaymentRefundRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmPaymentRefundRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmPaymentRefundDto> PaymentRefundAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmPaymentRefundDto>> PaymentRefundsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmPaymentRefundDto>> PaymentRefundsByPaymentIdAsync(int paymentId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<CrmPaymentRefundDto>> PaymentRefundsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
