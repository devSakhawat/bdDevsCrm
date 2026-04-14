using Domain.Entities.Entities.System;
using Domain.Contracts.Repositories;

namespace Domain.Contracts.Core.SystemAdmin;

public interface IAccessControlRepository : IRepositoryBase<AccessControl>
{
	/// <summary>
	/// Retrieves all access controls asynchronously.
	/// </summary>
	Task<IEnumerable<AccessControl>> AccessControlsAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single access control by ID asynchronously.
	/// </summary>
	Task<AccessControl?> AccessControlAsync(int accessId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Creates a new access control.
	/// </summary>
	Task<AccessControl> CreateAccessControlAsync(AccessControl accessControl, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing access control.
	/// </summary>
	public void UpdateAccessControl(AccessControl accessControl) => UpdateByState(accessControl);

	/// <summary>
	/// Deletes an access control.
	/// </summary>
	Task DeleteAccessControlAsync(AccessControl accessControl, bool trackChanges, CancellationToken cancellationToken = default);
}
