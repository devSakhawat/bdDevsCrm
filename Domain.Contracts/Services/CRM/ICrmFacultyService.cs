using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmFacultyService
{
    Task<CrmFacultyDto> CreateAsync(CreateCrmFacultyRecord record, CancellationToken cancellationToken = default);
    Task<CrmFacultyDto> UpdateAsync(UpdateCrmFacultyRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmFacultyRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmFacultyDto> FacultyAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmFacultyDto>> FacultiesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmFacultyDto>> FacultiesForDDLAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmFacultyDto>> FacultiesByInstituteIdAsync(int instituteId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<CrmFacultyDto>> FacultiesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
