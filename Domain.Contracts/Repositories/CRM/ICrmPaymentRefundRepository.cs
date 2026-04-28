using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmPaymentRefundRepository : IRepositoryBase<CrmPaymentRefund>
{
    Task<IEnumerable<CrmPaymentRefund>> PaymentRefundsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmPaymentRefund?> PaymentRefundAsync(int paymentRefundId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmPaymentRefund>> PaymentRefundsByPaymentIdAsync(int paymentId, bool trackChanges, CancellationToken cancellationToken = default);
}
