using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

/// <summary>Service contract for CRM lead management operations.</summary>
public interface ICrmLeadService
{
    /// <summary>Creates a new lead record.</summary>
    Task<CrmLeadDto> CreateAsync(CreateCrmLeadRecord record, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing lead record.</summary>
    Task<CrmLeadDto> UpdateAsync(UpdateCrmLeadRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Deletes a lead record.</summary>
    Task DeleteAsync(DeleteCrmLeadRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a single lead record by ID.</summary>
    Task<CrmLeadDto> LeadAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves all lead records.</summary>
    Task<IEnumerable<CrmLeadDto>> LeadsAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a lightweight list for dropdown binding.</summary>
    Task<IEnumerable<CrmLeadDto>> LeadForDDLAsync(CancellationToken cancellationToken = default);

    /// <summary>Retrieves a paginated summary grid of leads.</summary>
    Task<GridEntity<CrmLeadDto>> LeadsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
