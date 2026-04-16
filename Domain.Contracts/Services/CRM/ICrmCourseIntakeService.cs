using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.CRM;

public interface ICrmCourseIntakeService
{
    Task<CrmCourseIntakeDto> CreateAsync(CreateCrmCourseIntakeRecord record, CancellationToken cancellationToken = default);
    Task<CrmCourseIntakeDto> UpdateAsync(UpdateCrmCourseIntakeRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmCourseIntakeRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmCourseIntakeDto> CourseIntakeAsync(int courseIntakeId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCourseIntakeDto>> CourseIntakesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCourseIntakeDDLDto>> CourseIntakesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCourseIntakeDto>> CourseIntakesByCourseIdAsync(int courseId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<CrmCourseIntakeDto>> CourseIntakesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
