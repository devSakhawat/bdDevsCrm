using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IReportBuilderService
{
    Task<ReportBuilderDto> CreateAsync(CreateReportBuilderRecord record, CancellationToken cancellationToken = default);
    Task<ReportBuilderDto> UpdateAsync(UpdateReportBuilderRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteReportBuilderRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<ReportBuilderDto> ReportBuilderAsync(int reportHeaderId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<ReportBuilderDto>> ReportBuildersAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<ReportBuilderDto>> ReportBuildersForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<ReportBuilderDto>> ReportBuildersSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
