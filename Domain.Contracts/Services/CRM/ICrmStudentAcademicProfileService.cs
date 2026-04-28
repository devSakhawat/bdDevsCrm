using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmStudentAcademicProfileService
{
    Task<CrmStudentAcademicProfileDto> CreateAsync(CreateCrmStudentAcademicProfileRecord record, CancellationToken cancellationToken = default);
    Task<CrmStudentAcademicProfileDto> UpdateAsync(UpdateCrmStudentAcademicProfileRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmStudentAcademicProfileRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmStudentAcademicProfileDto> StudentAcademicProfileAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmStudentAcademicProfileDto>> StudentAcademicProfilesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmStudentAcademicProfileDto>> StudentAcademicProfilesByStudentIdAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<CrmStudentAcademicProfileDto>> StudentAcademicProfilesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
