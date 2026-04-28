using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmSystemConfigurationService
{
    Task<CrmSystemConfigurationDto> CreateAsync(CreateCrmSystemConfigurationRecord record, CancellationToken cancellationToken = default);
    Task<CrmSystemConfigurationDto> UpdateAsync(UpdateCrmSystemConfigurationRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmSystemConfigurationRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmSystemConfigurationDto> SystemConfigurationAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmSystemConfigurationDto?> SystemConfigurationByKeyAsync(string key, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmSystemConfigurationDto>> SystemConfigurationsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<CrmSystemConfigurationDto>> SystemConfigurationsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
