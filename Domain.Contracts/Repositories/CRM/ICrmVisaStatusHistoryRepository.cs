using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmVisaStatusHistoryRepository : IRepositoryBase<CrmVisaStatusHistory>
{
    Task<IEnumerable<CrmVisaStatusHistory>> VisaStatusHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmVisaStatusHistory?> VisaStatusHistoryAsync(int visaStatusHistoryId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmVisaStatusHistory>> VisaStatusHistoriesByVisaApplicationIdAsync(int visaApplicationId, bool trackChanges, CancellationToken cancellationToken = default);
}
