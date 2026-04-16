using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface ICompanyDepartmentMapService
{
    Task<CompanyDepartmentMapDto> CreateAsync(CreateCompanyDepartmentMapRecord record, CancellationToken cancellationToken = default);
    Task<CompanyDepartmentMapDto> UpdateAsync(UpdateCompanyDepartmentMapRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCompanyDepartmentMapRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CompanyDepartmentMapDto> CompanyDepartmentMapAsync(int sbuDepartmentMapId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CompanyDepartmentMapDto>> CompanyDepartmentMapsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CompanyDepartmentMapDto>> CompanyDepartmentMapsForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<CompanyDepartmentMapDto>> CompanyDepartmentMapsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
