using Domain.Contracts.CRM;
using Domain.Entities.Entities.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmVisaStatusRepository : RepositoryBase<CrmVisaStatus>, ICrmVisaStatusRepository
{
  public CrmVisaStatusRepository(CrmContext context) : base(context) { }

  public async Task<IEnumerable<CrmVisaStatus>> VisaStatusesAsync(bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListAsync(x => x.VisaStatusId, trackChanges, cancellationToken);
  }

  public async Task<CrmVisaStatus?> VisaStatusAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await FirstOrDefaultAsync(x => x.VisaStatusId == id, trackChanges, cancellationToken);
  }

  public async Task<IEnumerable<CrmVisaStatus>> VisaStatusesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListByIdsAsync(x => ids.Contains(x.VisaStatusId), trackChanges, cancellationToken);
  }

  public async Task<CrmVisaStatus> CreateCrmVisaStatusAsync(CrmVisaStatus entity, CancellationToken cancellationToken = default)
  {
    var newId = await CreateAndIdAsync(entity, cancellationToken);
    entity.VisaStatusId = newId;
    return entity;
  }

  public void UpdateCrmVisaStatus(CrmVisaStatus entity) => UpdateByState(entity);

  public async Task DeleteCrmVisaStatusAsync(CrmVisaStatus entity, bool trackChanges, CancellationToken cancellationToken = default)
  {
    await DeleteAsync(x => x.VisaStatusId == entity.VisaStatusId, trackChanges, cancellationToken);
  }
}
