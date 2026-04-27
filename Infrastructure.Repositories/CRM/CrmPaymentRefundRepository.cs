using Domain.Contracts.CRM;
using Domain.Entities.Entities.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmPaymentRefundRepository : RepositoryBase<CrmPaymentRefund>, ICrmPaymentRefundRepository
{
    public CrmPaymentRefundRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmPaymentRefund>> PaymentRefundsAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.PaymentRefundId, trackChanges, cancellationToken);

    public async Task<CrmPaymentRefund?> PaymentRefundAsync(int paymentRefundId, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.PaymentRefundId == paymentRefundId, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmPaymentRefund>> PaymentRefundsByPaymentIdAsync(int paymentId, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByConditionAsync(x => x.PaymentId == paymentId, x => x.PaymentRefundId, trackChanges, false, cancellationToken);
}
