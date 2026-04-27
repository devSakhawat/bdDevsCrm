using Domain.Contracts.CRM;
using Domain.Entities.Entities.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmStudentPaymentRepository : RepositoryBase<CrmStudentPayment>, ICrmStudentPaymentRepository
{
    public CrmStudentPaymentRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmStudentPayment>> StudentPaymentsAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.StudentPaymentId, trackChanges, cancellationToken);

    public async Task<CrmStudentPayment?> StudentPaymentAsync(int studentPaymentId, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.StudentPaymentId == studentPaymentId, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmStudentPayment>> StudentPaymentsByStudentIdAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByConditionAsync(x => x.StudentId == studentId, x => x.StudentPaymentId, trackChanges, false, cancellationToken);

    public async Task<IEnumerable<CrmStudentPayment>> StudentPaymentsByApplicationIdAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByConditionAsync(x => x.ApplicationId == applicationId, x => x.StudentPaymentId, trackChanges, false, cancellationToken);
}
