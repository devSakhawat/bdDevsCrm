using Domain.Contracts.CRM;
using Domain.Entities.Entities.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmApplicationStatusRepository : RepositoryBase<CrmApplicationStatus>, ICrmApplicationStatusRepository
{
  public CrmApplicationStatusRepository(CrmContext context) : base(context) { }

  public async Task<IEnumerable<CrmApplicationStatus>> ApplicationStatusesAsync(bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListAsync(x => x.ApplicationStatusId, trackChanges, cancellationToken);
  }

  public async Task<CrmApplicationStatus?> ApplicationStatusAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await FirstOrDefaultAsync(x => x.ApplicationStatusId == id, trackChanges, cancellationToken);
  }

  public async Task<IEnumerable<CrmApplicationStatus>> ApplicationStatusesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListByIdsAsync(x => ids.Contains(x.ApplicationStatusId), trackChanges, cancellationToken);
  }

  public async Task<CrmApplicationStatus> CreateCrmApplicationStatusAsync(CrmApplicationStatus entity, CancellationToken cancellationToken = default)
  {
    var newId = await CreateAndIdAsync(entity, cancellationToken);
    entity.ApplicationStatusId = newId;
    return entity;
  }

  public void UpdateCrmApplicationStatus(CrmApplicationStatus entity) => UpdateByState(entity);

  public async Task DeleteCrmApplicationStatusAsync(CrmApplicationStatus entity, bool trackChanges, CancellationToken cancellationToken = default)
  {
    await DeleteAsync(x => x.ApplicationStatusId == entity.ApplicationStatusId, trackChanges, cancellationToken);
  }
}
