using Domain.Contracts.CRM;
using Domain.Entities.Entities.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmVisaStatusHistoryRepository : RepositoryBase<CrmVisaStatusHistory>, ICrmVisaStatusHistoryRepository
{
    public CrmVisaStatusHistoryRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmVisaStatusHistory>> VisaStatusHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.VisaStatusHistoryId, trackChanges, cancellationToken);

    public async Task<CrmVisaStatusHistory?> VisaStatusHistoryAsync(int visaStatusHistoryId, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.VisaStatusHistoryId == visaStatusHistoryId, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmVisaStatusHistory>> VisaStatusHistoriesByVisaApplicationIdAsync(int visaApplicationId, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByConditionAsync(x => x.VisaApplicationId == visaApplicationId, x => x.VisaStatusHistoryId, trackChanges, false, cancellationToken);
}
