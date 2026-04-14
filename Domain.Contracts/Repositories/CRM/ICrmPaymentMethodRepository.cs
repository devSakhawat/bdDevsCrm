using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmPaymentMethodRepository : IRepositoryBase<CrmPaymentMethod>
{
  /// <summary>
  /// Retrieves all CrmPaymentMethod records asynchronously.
  /// </summary>
  Task<IEnumerable<CrmPaymentMethod>> CrmPaymentMethodsAsync(bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves a single CrmPaymentMethod record by ID asynchronously.
  /// </summary>
  Task<CrmPaymentMethod?> CrmPaymentMethodAsync(int crmpaymentmethodid, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmPaymentMethod records by a collection of IDs asynchronously.
  /// </summary>
  Task<IEnumerable<CrmPaymentMethod>> CrmPaymentMethodsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmPaymentMethod records by parent ID asynchronously.
  /// </summary>
  Task<IEnumerable<CrmPaymentMethod>> CrmPaymentMethodsByParentIdAsync(int parentId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Creates a new CrmPaymentMethod record.
  /// </summary>
  Task<CrmPaymentMethod> CreateCrmPaymentMethodAsync(CrmPaymentMethod entity, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an existing CrmPaymentMethod record.
  /// </summary>
  void UpdateCrmPaymentMethod(CrmPaymentMethod entity);

  /// <summary>
  /// Deletes a CrmPaymentMethod record.
  /// </summary>
  Task DeleteCrmPaymentMethodAsync(CrmPaymentMethod entity, bool trackChanges, CancellationToken cancellationToken = default);

  //Task<IEnumerable<CrmPaymentMethod>> ActivePaymentMethodsAsync(bool trackChanges);
  //Task<IEnumerable<CrmPaymentMethod>> OnlinePaymentMethodsAsync(bool trackChanges);
  //Task<CrmPaymentMethod?> PaymentMethodByIdAsync(int paymentMethodId, bool trackChanges);
  //void CreatePaymentMethod(CrmPaymentMethod paymentMethod);
  //void UpdatePaymentMethod(CrmPaymentMethod paymentMethod);
  //void DeletePaymentMethod(CrmPaymentMethod paymentMethod);
}