using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IAccessRestrictionService
{
    Task<AccessRestrictionDto> CreateAsync(CreateAccessRestrictionRecord record, CancellationToken cancellationToken = default);
    Task<AccessRestrictionDto> UpdateAsync(UpdateAccessRestrictionRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteAccessRestrictionRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<AccessRestrictionDto> AccessRestrictionAsync(int accessRestrictionId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<AccessRestrictionDto>> AccessRestrictionsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<AccessRestrictionDDLDto>> AccessRestrictionsForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<AccessRestrictionDto>> AccessRestrictionsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
