using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

/// <summary>Service contract for CRM lead source management operations.</summary>
public interface ICrmLeadSourceService
{
    /// <summary>Creates a new lead source record.</summary>
    Task<CrmLeadSourceDto> CreateAsync(CreateCrmLeadSourceRecord record, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing lead source record.</summary>
    Task<CrmLeadSourceDto> UpdateAsync(UpdateCrmLeadSourceRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Deletes a lead source record.</summary>
    Task DeleteAsync(DeleteCrmLeadSourceRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a single lead source record by ID.</summary>
    Task<CrmLeadSourceDto> LeadSourceAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves all lead source records.</summary>
    Task<IEnumerable<CrmLeadSourceDto>> LeadSourcesAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a lightweight list for dropdown binding.</summary>
    Task<IEnumerable<CrmLeadSourceDto>> LeadSourceForDDLAsync(CancellationToken cancellationToken = default);

    /// <summary>Retrieves a paginated summary grid of lead sources.</summary>
    Task<GridEntity<CrmLeadSourceDto>> LeadSourcesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
