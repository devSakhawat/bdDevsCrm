using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

/// <summary>
/// Service contract for user management operations.
/// Defines methods for creating, updating, deleting, and retrieving user data.
/// </summary>
public interface IUsersService
{
	/// <summary>
	/// Creates a new user record after validating for null input and duplicate login ID.
	/// </summary>
	/// <param name="entityForCreate">The DTO containing data for the new user.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The created <see cref="UsersDto"/> with the newly assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
	/// <exception cref="DuplicateRecordException">Thrown when a user with the same login ID already exists.</exception>
	Task<UsersDto> CreateUserAsync(UsersDto entityForCreate, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing user record by merging only the changed values from the provided DTO.
	/// Validates ID consistency, null input, record existence, and duplicate login ID constraints.
	/// </summary>
	/// <param name="userId">The ID of the user to update.</param>
	/// <param name="modelDto">The DTO containing updated field values.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="UsersDto"/> reflecting the saved state.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no user is found for the given ID.</exception>
	/// <exception cref="DuplicateRecordException">Thrown when another user with the same login ID already exists.</exception>
	Task<UsersDto> UpdateUserAsync(int userId, UsersDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes a user record identified by the given ID.
	/// Validates that the ID is positive and that the record exists before deletion.
	/// </summary>
	/// <param name="userId">The ID of the user to delete.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The number of affected rows.</returns>
	/// <exception cref="IdParametersBadRequestException">Thrown when <paramref name="userId"/> is zero or negative.</exception>
	/// <exception cref="NotFoundException">Thrown when no user record is found for the given ID.</exception>
	Task<int> DeleteUserAsync(int userId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single user record by its ID.
	/// </summary>
	/// <param name="id">The ID of the user to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="UsersDto"/> matching the specified ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no user is found for the given ID.</exception>
	Task<UsersDto> UserAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves all user records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of all <see cref="UsersDto"/> records.</returns>
	/// <exception cref="GenericListNotFoundException">Thrown when no users are found.</exception>
	Task<IEnumerable<UsersDto>> UsersAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves multiple user records by their IDs.
	/// Validates that all requested IDs are found in the database.
	/// </summary>
	/// <param name="ids">The collection of user IDs to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="UsersDto"/> matching the provided IDs.</returns>
	/// <exception cref="IdParametersBadRequestException">Thrown when <paramref name="ids"/> is null.</exception>
	/// <exception cref="CollectionByIdsBadRequestException">Thrown when the number of found records does not match the requested IDs.</exception>
	Task<IEnumerable<UsersDto>> UsersByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a user record by login ID.
	/// </summary>
	/// <param name="loginId">The login ID of the user to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="UsersDto"/> matching the specified login ID, or null if not found.</returns>
	Task<UsersDto?> UserByLoginIdAsync(string loginId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves password history for the specified user.
	/// </summary>
	/// <param name="userId">The ID of the user whose password history is to be retrieved.</param>
	/// <param name="passRestriction">The password restriction count.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="PasswordHistoryDto"/> for the specified user.</returns>
	/// <exception cref="NotFoundException">Thrown when no password history is found for the given user ID.</exception>
	Task<IEnumerable<PasswordHistoryDto>> PasswordHistoryByUserIdAsync(int userId, int passRestriction, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated summary grid of all users with company, branch, and department information.
	/// </summary>
	/// <param name="companyId">The ID of the company.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="currentUser">The DTO containing current user information for access restriction.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{UsersDto}"/> containing the paged user summary data.</returns>
	Task<GridEntity<UsersDto>> UsersSummaryAsync(int companyId, bool trackChanges, GridOptions options, UsersDto currentUser, CancellationToken cancellationToken = default);

	/// <summary>
	/// Saves a user record (create or update) with validation and group member assignment.
	/// </summary>
	/// <param name="modelDto">The DTO containing user data.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The saved <see cref="UsersDto"/> with the assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="CommonBadReuqestException">Thrown when system settings are not configured.</exception>
	/// <exception cref="InvalidOperationExceptionEx">Thrown when validation fails.</exception>
	Task<UsersDto> SaveUserAsync(UsersDto modelDto, CancellationToken cancellationToken = default);

}










//namespace Application.Services.Core.SystemAdmin;

//public interface IUsersService
//{
//  Task<GridEntity<UsersDto>> UsersSummary(int companyId ,bool trackChanges, GridOptions options, UsersDto user);


//  IEnumerable<UsersDto> Users(bool trackChanges);
//  UsersDto User(int UsersId, bool trackChanges);
//  IEnumerable<UsersDto> ByIds(IEnumerable<int> ids, bool trackChanges);

//  Task<IEnumerable<UsersDto>> UsersAsync(bool trackChanges);
//  Task<UsersDto> UserAsync(int UsersId, bool trackChanges);
//  UsersDto? UserByLoginIdRaw(string loginId, bool trackChanges);
//  void CreateUser(UsersDto model);
//  void UpdateUser(UsersDto model);
//  void DeleteUser(UsersDto model);

//  Task<UsersDto> CreateUserAsync(UsersDto entityForCreate);
//  Task DeleteUserAsync(int userId, bool trackChanges);
//  Task UpdateUserAsync(int userId, UsersDto model, bool trackChanges);
//  Task<UsersDto> SaveUser(UsersDto usersDto);




//  Task<IQueryable<PasswordHistoryDto>> PasswordHistory(int userId, int passRestriction);
//}

