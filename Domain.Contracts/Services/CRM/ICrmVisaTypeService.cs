using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

/// <summary>Service contract for CRM visa type management operations.</summary>
public interface ICrmVisaTypeService
{
    /// <summary>Creates a new visa type record.</summary>
    Task<CrmVisaTypeDto> CreateAsync(CreateCrmVisaTypeRecord record, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing visa type record.</summary>
    Task<CrmVisaTypeDto> UpdateAsync(UpdateCrmVisaTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Deletes a visa type record.</summary>
    Task DeleteAsync(DeleteCrmVisaTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a single visa type record by ID.</summary>
    Task<CrmVisaTypeDto> VisaTypeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves all visa type records.</summary>
    Task<IEnumerable<CrmVisaTypeDto>> VisaTypesAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a lightweight list for dropdown binding.</summary>
    Task<IEnumerable<CrmVisaTypeDto>> VisaTypeForDDLAsync(CancellationToken cancellationToken = default);

    /// <summary>Retrieves a paginated summary grid of visa types.</summary>
    Task<GridEntity<CrmVisaTypeDto>> VisaTypesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
