using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmMasterDataSuggestionRepository : IRepositoryBase<CrmMasterDataSuggestion>
{
    Task<IEnumerable<CrmMasterDataSuggestion>> CrmMasterDataSuggestionsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmMasterDataSuggestion?> CrmMasterDataSuggestionAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmMasterDataSuggestion>> CrmMasterDataSuggestionsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}
