using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

/// <summary>Service contract for CRM follow-up management operations.</summary>
public interface ICrmFollowUpService
{
    /// <summary>Creates a new follow-up record.</summary>
    Task<CrmFollowUpDto> CreateAsync(CreateCrmFollowUpRecord record, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing follow-up record.</summary>
    Task<CrmFollowUpDto> UpdateAsync(UpdateCrmFollowUpRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Deletes a follow-up record.</summary>
    Task DeleteAsync(DeleteCrmFollowUpRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a single follow-up record by ID.</summary>
    Task<CrmFollowUpDto> FollowUpAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves all follow-up records.</summary>
    Task<IEnumerable<CrmFollowUpDto>> FollowUpsAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a lightweight list for dropdown binding.</summary>
    Task<IEnumerable<CrmFollowUpDto>> FollowUpForDDLAsync(CancellationToken cancellationToken = default);

    /// <summary>Retrieves a paginated summary grid of follow-ups.</summary>
    Task<GridEntity<CrmFollowUpDto>> FollowUpsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
