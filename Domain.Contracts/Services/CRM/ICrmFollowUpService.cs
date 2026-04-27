using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmFollowUpService
{
    Task<CrmFollowUpDto> CreateAsync(CreateCrmFollowUpRecord record, CancellationToken cancellationToken = default);
    Task<CrmFollowUpDto> UpdateAsync(UpdateCrmFollowUpRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmFollowUpRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmFollowUpDto> FollowUpAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmFollowUpDto>> FollowUpsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmFollowUpDto>> FollowUpsByLeadIdAsync(int leadId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmFollowUpDto>> FollowUpForDDLAsync(CancellationToken cancellationToken = default);
    Task<GridEntity<CrmFollowUpDto>> FollowUpsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
    Task<CrmFollowUpDto> ChangeStatusAsync(ChangeCrmFollowUpStatusRecord record, CancellationToken cancellationToken = default);
    Task<int> ProcessOverdueFollowUpsAsync(CancellationToken cancellationToken = default);
    Task<int> ProcessUnresponsiveLeadsAsync(CancellationToken cancellationToken = default);
}
