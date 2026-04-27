using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

/// <summary>Service contract for CRM counselor management operations.</summary>
public interface ICrmCounselorService
{
    /// <summary>Creates a new counselor record.</summary>
    Task<CrmCounselorDto> CreateAsync(CreateCrmCounselorRecord record, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing counselor record.</summary>
    Task<CrmCounselorDto> UpdateAsync(UpdateCrmCounselorRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Deletes a counselor record.</summary>
    Task DeleteAsync(DeleteCrmCounselorRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a single counselor record by ID.</summary>
    Task<CrmCounselorDto> CounselorAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves all counselor records.</summary>
    Task<IEnumerable<CrmCounselorDto>> CounselorsAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a lightweight list for dropdown binding.</summary>
    Task<IEnumerable<CrmCounselorDto>> CounselorForDDLAsync(CancellationToken cancellationToken = default);

    /// <summary>Retrieves a paginated summary grid of counselors.</summary>
    Task<GridEntity<CrmCounselorDto>> CounselorsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
