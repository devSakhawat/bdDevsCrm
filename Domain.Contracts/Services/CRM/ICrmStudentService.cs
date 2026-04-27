using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

/// <summary>Service contract for CRM student management operations.</summary>
public interface ICrmStudentService
{
    /// <summary>Creates a new student record.</summary>
    Task<CrmStudentDto> CreateAsync(CreateCrmStudentRecord record, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing student record.</summary>
    Task<CrmStudentDto> UpdateAsync(UpdateCrmStudentRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Deletes a student record.</summary>
    Task DeleteAsync(DeleteCrmStudentRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a single student record by ID.</summary>
    Task<CrmStudentDto> StudentAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves all student records.</summary>
    Task<IEnumerable<CrmStudentDto>> StudentsAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a lightweight list for dropdown binding.</summary>
    Task<IEnumerable<CrmStudentDto>> StudentForDDLAsync(CancellationToken cancellationToken = default);

    /// <summary>Retrieves a paginated summary grid of students.</summary>
    Task<GridEntity<CrmStudentDto>> StudentsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
