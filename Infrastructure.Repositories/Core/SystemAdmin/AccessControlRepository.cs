using Domain.Entities.Entities.System;
using Domain.Contracts.Core.SystemAdmin;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.Core.SystemAdmin;

/// <summary>
/// Repository for access control data access operations.
/// Implements enterprise patterns with async support.
/// </summary>
public class AccessControlRepository : RepositoryBase<AccessControl>, IAccessControlRepository
{
	public AccessControlRepository(CrmContext context) : base(context) { }

	/// <summary>
	/// Retrieves all access controls asynchronously.
	/// </summary>
	public async Task<IEnumerable<AccessControl>> AccessControlsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(a => a.AccessId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves a single access control by ID asynchronously.
	/// </summary>
	public async Task<AccessControl?> AccessControlAsync(int accessId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(
				a => a.AccessId.Equals(accessId),
				trackChanges,
				cancellationToken);
	}

	/// <summary>
	/// Creates a new access control.
	/// </summary>
	public async Task<AccessControl> CreateAccessControlAsync(AccessControl accessControl, CancellationToken cancellationToken = default)
	{
		int accessId = await CreateAndIdAsync(accessControl, cancellationToken);
		accessControl.AccessId = accessId;
		return accessControl;
	}

	/// <summary>
	/// Updates an existing access control.
	/// </summary>
	public void UpdateAccessControl(AccessControl accessControl) => UpdateByState(accessControl);

	/// <summary>
	/// Deletes an access control.
	/// </summary>
	public async Task DeleteAccessControlAsync(AccessControl accessControl, bool trackChanges, CancellationToken cancellationToken = default)
		=> await DeleteAsync(x => x.AccessId == accessControl.AccessId, trackChanges, cancellationToken);
}
