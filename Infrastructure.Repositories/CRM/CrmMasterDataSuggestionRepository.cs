using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmMasterDataSuggestionRepository : RepositoryBase<CrmMasterDataSuggestion>, ICrmMasterDataSuggestionRepository
{
    public CrmMasterDataSuggestionRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmMasterDataSuggestion>> CrmMasterDataSuggestionsAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.SuggestionId, trackChanges, cancellationToken);

    public async Task<CrmMasterDataSuggestion?> CrmMasterDataSuggestionAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.SuggestionId == id, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmMasterDataSuggestion>> CrmMasterDataSuggestionsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByIdsAsync(x => ids.Contains(x.SuggestionId), trackChanges, cancellationToken);
}
