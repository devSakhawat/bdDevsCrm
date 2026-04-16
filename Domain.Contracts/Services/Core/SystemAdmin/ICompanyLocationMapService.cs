using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface ICompanyLocationMapService
{
    Task<CompanyLocationMapDto> CreateAsync(CreateCompanyLocationMapRecord record, CancellationToken cancellationToken = default);
    Task<CompanyLocationMapDto> UpdateAsync(UpdateCompanyLocationMapRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCompanyLocationMapRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CompanyLocationMapDto> CompanyLocationMapAsync(int sbuLocationMapId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CompanyLocationMapDto>> CompanyLocationMapsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CompanyLocationMapDto>> CompanyLocationMapsForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<CompanyLocationMapDto>> CompanyLocationMapsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
