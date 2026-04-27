using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmMasterDataSuggestionService
{
    Task<CrmMasterDataSuggestionDto> CreateAsync(CreateCrmMasterDataSuggestionRecord record, CancellationToken cancellationToken = default);
    Task<CrmMasterDataSuggestionDto> UpdateAsync(UpdateCrmMasterDataSuggestionRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmMasterDataSuggestionRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmMasterDataSuggestionDto> MasterDataSuggestionAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmMasterDataSuggestionDto>> MasterDataSuggestionsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<CrmMasterDataSuggestionDto>> MasterDataSuggestionsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
