using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

/// <summary>Service contract for CRM office management operations.</summary>
public interface ICrmOfficeService
{
    /// <summary>Creates a new office record.</summary>
    Task<CrmOfficeDto> CreateAsync(CreateCrmOfficeRecord record, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing office record.</summary>
    Task<CrmOfficeDto> UpdateAsync(UpdateCrmOfficeRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Deletes an office record.</summary>
    Task DeleteAsync(DeleteCrmOfficeRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a single office record by ID.</summary>
    Task<CrmOfficeDto> OfficeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves all office records.</summary>
    Task<IEnumerable<CrmOfficeDto>> OfficesAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a lightweight list for dropdown binding.</summary>
    Task<IEnumerable<CrmOfficeDto>> OfficeForDDLAsync(CancellationToken cancellationToken = default);

    /// <summary>Retrieves a paginated summary grid of offices.</summary>
    Task<GridEntity<CrmOfficeDto>> OfficesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
