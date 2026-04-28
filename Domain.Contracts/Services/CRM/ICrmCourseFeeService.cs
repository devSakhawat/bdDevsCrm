using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmCourseFeeService
{
    Task<CrmCourseFeeDto> CreateAsync(CreateCrmCourseFeeRecord record, CancellationToken cancellationToken = default);
    Task<CrmCourseFeeDto> UpdateAsync(UpdateCrmCourseFeeRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmCourseFeeRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmCourseFeeDto> CourseFeeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCourseFeeDto>> CourseFeesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCourseFeeDto>> CourseFeesByCourseIdAsync(int courseId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<CrmCourseFeeDto>> CourseFeesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
