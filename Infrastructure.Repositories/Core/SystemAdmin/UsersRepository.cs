using Domain.Entities.Entities.System;
using Domain.Contracts.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.Core.HR;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.Core.SystemAdmin;

/// <summary>
/// Repository for user data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class UsersRepository : RepositoryBase<Users>, IUsersRepository
{
	public UsersRepository(CrmContext context) : base(context) { }

	/// <summary>
	/// Retrieves all users asynchronously.
	/// </summary>
	public async Task<IEnumerable<Users>> UsersAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(u => u.UserId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves a single user by ID asynchronously.
	/// </summary>
	public async Task<Users?> UserAsync(int userId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(u => u.UserId.Equals(userId), trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves a user by login ID asynchronously.
	/// </summary>
	public async Task<Users?> UserByLoginIdAsync(string loginId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(
				u => u.LoginId.Trim().ToLower() == loginId.Trim().ToLower(),
				trackChanges,
				cancellationToken);
	}

	/// <summary>
	/// Retrieves user details by login ID using raw SQL asynchronously.
	/// </summary>
	public async Task<Users?> UserByLoginIdRawAsync(string loginId, CancellationToken cancellationToken = default)
	{
		string query = $@"SELECT Users.UserId, Users.CompanyId, Users.LoginId, Users.UserName, 
            Users.Password, Users.EmployeeId, Users.CreatedDate, Employee.HRRecordId,
            Users.LastUpdatedDate, Users.LastLoginDate, Users.FailedLoginNo, 
            Users.IsActive, Users.IsExpired, Users.Theme, Employment.EmployeeId as EmployeeId,
            Users.AccessParentCompany, Users.DefaultDashboard, Employee.ProfilePicture
            FROM Users 
            INNER JOIN Employee ON Users.EmployeeId = Employee.HRRecordId
            INNER JOIN Employment ON Employee.HRRecordId = Employment.HRRecordId
            WHERE RTRIM(LTRIM(LOWER(LoginId))) = '{loginId.Trim().ToLower()}'";

		return await AdoExecuteSingleDataAsync<Users>(query, null, cancellationToken);
	}

	/// <summary>
	/// Retrieves password history for a user.
	/// </summary>
	public async Task<IQueryable<PasswordHistory>> PasswordHistoriesAsync(int userId, int passRestriction, CancellationToken cancellationToken = default)
	{
		var quary = string.Format("SELECT TOP {1} [HistoryId],[UserId],[OldPassword],[PasswordChangeDate] FROM [dbo].[PasswordHistory] WHERE [UserId] = {0} ORDER BY [PasswordChangeDate] DESC", userId, passRestriction);
		IEnumerable<PasswordHistory> passwordHistory = await AdoExecuteListQueryAsync<PasswordHistory>(quary, null, cancellationToken);
		return passwordHistory.AsQueryable();
	}

	/// <summary>
	/// Retrieves users by a collection of IDs asynchronously.
	/// </summary>
	public async Task<IEnumerable<Users>> UsersByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(u => ids.Contains(u.UserId), trackChanges, cancellationToken);
	}

	/// <summary>
	/// Creates a new user.
	/// </summary>
	public async Task<Users> CreateUserAsync(Users user, CancellationToken cancellationToken = default)
	{
		int userId = await CreateAndIdAsync(user, cancellationToken);
		user.UserId = userId;
		return user;
	}

	/// <summary>
	/// Updates an existing user.
	/// </summary>
	public void UpdateUser(Users user) => UpdateByState(user);

	/// <summary>
	/// Deletes a user.
	/// </summary>
	public async Task DeleteUserAsync(Users user, bool trackChanges, CancellationToken cancellationToken = default)
		=> await DeleteAsync(x => x.UserId == user.UserId, trackChanges, cancellationToken);
}
