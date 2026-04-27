using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

/// <summary>Service contract for CRM student status management operations.</summary>
public interface ICrmStudentStatusService
{
    /// <summary>Creates a new student status record.</summary>
    Task<CrmStudentStatusDto> CreateAsync(CreateCrmStudentStatusRecord record, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing student status record.</summary>
    Task<CrmStudentStatusDto> UpdateAsync(UpdateCrmStudentStatusRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Deletes a student status record.</summary>
    Task DeleteAsync(DeleteCrmStudentStatusRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a single student status record by ID.</summary>
    Task<CrmStudentStatusDto> StudentStatusAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves all student status records.</summary>
    Task<IEnumerable<CrmStudentStatusDto>> StudentStatusesAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a lightweight list for dropdown binding.</summary>
    Task<IEnumerable<CrmStudentStatusDto>> StudentStatusForDDLAsync(CancellationToken cancellationToken = default);

    /// <summary>Retrieves a paginated summary grid of student statuses.</summary>
    Task<GridEntity<CrmStudentStatusDto>> StudentStatusesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
