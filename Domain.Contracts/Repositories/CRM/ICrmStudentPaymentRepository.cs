using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmStudentPaymentRepository : IRepositoryBase<CrmStudentPayment>
{
    Task<IEnumerable<CrmStudentPayment>> StudentPaymentsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmStudentPayment?> StudentPaymentAsync(int studentPaymentId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmStudentPayment>> StudentPaymentsByStudentIdAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmStudentPayment>> StudentPaymentsByApplicationIdAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default);
}
