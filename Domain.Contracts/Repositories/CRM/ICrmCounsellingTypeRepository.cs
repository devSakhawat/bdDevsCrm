using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmCounsellingTypeRepository : IRepositoryBase<CrmCounsellingType>
{
    Task<IEnumerable<CrmCounsellingType>> CounsellingTypesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmCounsellingType?> CounsellingTypeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCounsellingType>> CounsellingTypesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmCounsellingType> CreateCrmCounsellingTypeAsync(CrmCounsellingType entity, CancellationToken cancellationToken = default);
    void UpdateCrmCounsellingType(CrmCounsellingType entity);
    Task DeleteCrmCounsellingTypeAsync(CrmCounsellingType entity, bool trackChanges, CancellationToken cancellationToken = default);
}
