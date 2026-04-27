using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

/// <summary>Service contract for CRM lead status management operations.</summary>
public interface ICrmLeadStatusService
{
    /// <summary>Creates a new lead status record.</summary>
    Task<CrmLeadStatusDto> CreateAsync(CreateCrmLeadStatusRecord record, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing lead status record.</summary>
    Task<CrmLeadStatusDto> UpdateAsync(UpdateCrmLeadStatusRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Deletes a lead status record.</summary>
    Task DeleteAsync(DeleteCrmLeadStatusRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a single lead status record by ID.</summary>
    Task<CrmLeadStatusDto> LeadStatusAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves all lead status records.</summary>
    Task<IEnumerable<CrmLeadStatusDto>> LeadStatusesAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a lightweight list for dropdown binding.</summary>
    Task<IEnumerable<CrmLeadStatusDto>> LeadStatusForDDLAsync(CancellationToken cancellationToken = default);

    /// <summary>Retrieves a paginated summary grid of lead statuses.</summary>
    Task<GridEntity<CrmLeadStatusDto>> LeadStatusesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
