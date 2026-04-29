using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmCommissionTypeRepository : IRepositoryBase<CrmCommissionType>
{
    Task<IEnumerable<CrmCommissionType>> CommissionTypesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmCommissionType?> CommissionTypeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCommissionType>> CommissionTypesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmCommissionType> CreateCrmCommissionTypeAsync(CrmCommissionType entity, CancellationToken cancellationToken = default);
    void UpdateCrmCommissionType(CrmCommissionType entity);
    Task DeleteCrmCommissionTypeAsync(CrmCommissionType entity, bool trackChanges, CancellationToken cancellationToken = default);
}
