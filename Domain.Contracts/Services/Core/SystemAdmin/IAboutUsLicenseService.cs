using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IAboutUsLicenseService
{
    Task<AboutUsLicenseDto> CreateAsync(CreateAboutUsLicenseRecord record, CancellationToken cancellationToken = default);
    Task<AboutUsLicenseDto> UpdateAsync(UpdateAboutUsLicenseRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteAboutUsLicenseRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<AboutUsLicenseDto> AboutUsLicenseAsync(int aboutUsLicenseId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<AboutUsLicenseDto>> AboutUsLicensesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<AboutUsLicenseDDLDto>> AboutUsLicensesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<AboutUsLicenseDto>> AboutUsLicensesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
