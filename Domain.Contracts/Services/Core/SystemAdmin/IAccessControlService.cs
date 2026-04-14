using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IAccessControlService
{
  Task<AccessControlDto> CreateAsync(AccessControlDto modelDto, CancellationToken cancellation = default);
  Task<AccessControlDto> UpdateAsync(int accessControlId, AccessControlDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);
  Task<int> DeleteAsync(int key, bool trackChanges, CancellationToken cancellationToken = default);

  Task<GridEntity<AccessControlDto>> SummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves an access control by ID asynchronously.
  /// </summary>
  Task<AccessControlDto> AccessControlAsync(int accessId, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves access controls for dropdown list asynchronously.
  /// </summary>
  Task<IEnumerable<AccessControlDto>> AccessControlsForDDLAsync(CancellationToken cancellationToken = default);
}
