using Domain.Contracts.CRM;
using Domain.Entities.Entities.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmCounsellingTypeRepository : RepositoryBase<CrmCounsellingType>, ICrmCounsellingTypeRepository
{
    public CrmCounsellingTypeRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmCounsellingType>> CounsellingTypesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(x => x.CounsellingTypeId, trackChanges, cancellationToken);
    }

    public async Task<CrmCounsellingType?> CounsellingTypeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(x => x.CounsellingTypeId == id, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<CrmCounsellingType>> CounsellingTypesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByIdsAsync(x => ids.Contains(x.CounsellingTypeId), trackChanges, cancellationToken);
    }

    public async Task<CrmCounsellingType> CreateCrmCounsellingTypeAsync(CrmCounsellingType entity, CancellationToken cancellationToken = default)
    {
        var newId = await CreateAndIdAsync(entity, cancellationToken);
        entity.CounsellingTypeId = newId;
        return entity;
    }

    public void UpdateCrmCounsellingType(CrmCounsellingType entity) => UpdateByState(entity);

    public async Task DeleteCrmCounsellingTypeAsync(CrmCounsellingType entity, bool trackChanges, CancellationToken cancellationToken = default)
    {
        await DeleteAsync(x => x.CounsellingTypeId == entity.CounsellingTypeId, trackChanges, cancellationToken);
    }
}
