using Domain.Contracts.CRM;
using Domain.Entities.Entities.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmCommissionTypeRepository : RepositoryBase<CrmCommissionType>, ICrmCommissionTypeRepository
{
    public CrmCommissionTypeRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmCommissionType>> CommissionTypesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(x => x.CommissionTypeId, trackChanges, cancellationToken);
    }

    public async Task<CrmCommissionType?> CommissionTypeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(x => x.CommissionTypeId == id, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<CrmCommissionType>> CommissionTypesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByIdsAsync(x => ids.Contains(x.CommissionTypeId), trackChanges, cancellationToken);
    }

    public async Task<CrmCommissionType> CreateCrmCommissionTypeAsync(CrmCommissionType entity, CancellationToken cancellationToken = default)
    {
        var newId = await CreateAndIdAsync(entity, cancellationToken);
        entity.CommissionTypeId = newId;
        return entity;
    }

    public void UpdateCrmCommissionType(CrmCommissionType entity) => UpdateByState(entity);

    public async Task DeleteCrmCommissionTypeAsync(CrmCommissionType entity, bool trackChanges, CancellationToken cancellationToken = default)
    {
        await DeleteAsync(x => x.CommissionTypeId == entity.CommissionTypeId, trackChanges, cancellationToken);
    }
}
