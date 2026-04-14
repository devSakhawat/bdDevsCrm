using Presentation.AuthorizeAttributes;
using Domain.Contracts.Services;
using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Domain.Exceptions;
using bdDevs.Shared.Constants;
using Application.Shared.Grid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Presentation.ActionFIlters;

namespace Presentation.Controllers.Core.SystemAdmin;

/// <summary>
/// User management endpoints.
/// 
/// Design Pattern: Follows MenuController structure
/// Method Order: CUD → Read (High to Low data volume)
/// HATEOAS Status: NOT IMPLEMENTED YET (Waiting for MenuController success)
/// 
/// [AuthorizeUser] at class-level ensures:
///    - Every request validates user via attribute
///    - CurrentUser / CurrentUserId available from BaseApiController
///    - No auth checks needed in controller methods
///    - Exceptions handled by StandardExceptionMiddleware
/// </summary>
[AuthorizeUser]
public class UsersController : BaseApiController
{
	private readonly IMemoryCache _cache;

	// Removed ILinkFactory dependency
	public UsersController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
	{
		_cache = cache;
	}

	#region ── CUD Operations (First) ─────────────────────────────

	/// <summary>
	/// Creates a new user.
	/// POST: /bdDevs-crm/user
	/// </summary>
	[HttpPost(RouteConstants.CreateUser)]
	[ServiceFilter(typeof(EmptyObjectFilterAttribute))]
	public async Task<IActionResult> CreateUserAsync([FromBody] UsersDto modelDto ,CancellationToken cancellationToken = default)
	{
		var createdUser = await _serviceManager.Users.CreateUserAsync(modelDto,cancellationToken: cancellationToken);

		if (createdUser.UserId <= 0)
			throw new InvalidCreateOperationException("Failed to create user record.");

		// Returns plain DTO
		return Ok(ApiResponseHelper.Created(createdUser,"User created successfully."));
	}

	/// <summary>
	/// Updates an existing user.
	/// PUT: /bdDevs-crm/user/{key}
	/// </summary>
	[HttpPut(RouteConstants.UpdateUser)]
	[ServiceFilter(typeof(EmptyObjectFilterAttribute))]
	public async Task<IActionResult> UpdateUserAsync([FromRoute] int key, [FromBody] UsersDto modelDto, CancellationToken cancellationToken = default)
	{
		if (key != modelDto.UserId)
			throw new IdMismatchBadRequestException(key.ToString(), nameof(UsersDto));

		var updatedUser = await _serviceManager.Users.UpdateUserAsync( key, modelDto, trackChanges: false, cancellationToken: cancellationToken);

		if (updatedUser.UserId <= 0)
			throw new InvalidUpdateOperationException("Failed to update user record.");

		// Returns plain DTO
		return Ok(ApiResponseHelper.Updated(updatedUser,"User updated successfully."));
	}

	/// <summary>
	/// Deletes a user by ID.
	/// DELETE: /bdDevs-crm/user/{key}
	/// </summary>
	[HttpDelete(RouteConstants.DeleteUser)]
	public async Task<IActionResult> DeleteUserAsync([FromRoute] int key, CancellationToken cancellationToken = default)
	{
		await _serviceManager.Users.DeleteUserAsync(key,trackChanges: false,cancellationToken: cancellationToken);

		return Ok(ApiResponseHelper.NoContent<object>("User deleted successfully"));
	}

	#endregion ── CUD Operations End ──────────────────────────────


	#region ── Read Operations (High to Low Data Volume) ─────────

	/// <summary>
	/// Retrieves paginated summary grid of users.
	/// [Largest Data Volume] POST: /bdDevs-crm/user-summary
	/// </summary>
	[HttpPost(RouteConstants.UserSummary)]
	public async Task<IActionResult> UserSummaryAsync(
			[FromBody] CRMGridOptions options,
			[FromQuery] int companyId,
			CancellationToken cancellationToken = default)
	{
		var summaryGrid = await _serviceManager.Users.UsersSummaryAsync(companyId, trackChanges: false, options, currentUser: CurrentUser,cancellationToken: cancellationToken);

		if (!summaryGrid.Items.Any())
			return Ok(ApiResponseHelper.Success(new GridEntity<UsersDto>(),	"No users found."));

		// Returns plain GridEntity
		return Ok(ApiResponseHelper.Success(summaryGrid, "Users retrieved successfully"));
	}

	/// <summary>
	/// Retrieves all users (list view).
	/// [Medium Data Volume] GET: /bdDevs-crm/users
	/// </summary>
	[HttpGet(RouteConstants.ReadUsers)]
	[ResponseCache(Duration = 60)]
	public async Task<IActionResult> ReadUsersAsync(
			CancellationToken cancellationToken = default)
	{
		IEnumerable<UsersDto> users = await _serviceManager.Users.UsersAsync(
				trackChanges: false,
				cancellationToken: cancellationToken);

		if (!users.Any())
			return Ok(ApiResponseHelper.Success(
					Enumerable.Empty<UsersDto>(),
					"No users found."));

		return Ok(ApiResponseHelper.Success(
				users,
				"Users retrieved successfully"));
	}

	/// <summary>
	/// Retrieves a user by ID.
	/// [Smallest Data Volume - Single] GET: /bdDevs-crm/user/{id:int}
	/// </summary>
	[HttpGet(RouteConstants.ReadUser)]
	[ResponseCache(Duration = 60)]
	public async Task<IActionResult> UserAsync(
			[FromRoute] int id,
			CancellationToken cancellationToken = default)
	{
		if (id <= 0)
			throw new IdParametersBadRequestException();

		var user = await _serviceManager.Users.UserAsync(id,trackChanges: false,cancellationToken: cancellationToken);

		if (user == null)
			return Ok(ApiResponseHelper.Success<UsersDto>( null, "User not found."));

		return Ok(ApiResponseHelper.Success( user, "User retrieved successfully"));
	}

	#endregion ── Read Operations End ─────────────────────────────
}


//using Presentation.ActionFIlters;
//using Presentation.AuthorizeAttributes;
//using Presentation.LinkFactories;
//using Domain.Contracts.Services;
//using bdDevs.Shared;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using Domain.Exceptions;
//using bdDevs.Shared.Constants;
//using Application.Shared.Grid;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Caching.Memory;

//namespace Presentation.Controllers.Core.SystemAdmin;

///// <summary>
///// User management endpoints.
/////
///// [AuthorizeUser] at class-level ensures:
/////    - Every request validates user via attribute
/////    - CurrentUser / CurrentUserId available from BaseApiController
/////    - No auth checks needed in controller methods
/////    - Exceptions handled by StandardExceptionMiddleware
///// </summary>
//[AuthorizeUser]
//public class UsersController : BaseApiController
//{
//    private readonly IMemoryCache _cache;
//    private readonly ILinkFactory<UsersDto> _linkFactory;

//    public UsersController(IServiceManager serviceManager, IMemoryCache cache, ILinkFactory<UsersDto> linkFactory) : base(serviceManager)
//    {
//        _cache = cache;
//        _linkFactory = linkFactory;
//    }

//    /// <summary>
//    /// Retrieves paginated summary grid of users.
//    /// </summary>
//    [HttpPost(RouteConstants.UserSummary)]
//    public async Task<IActionResult> UserSummaryAsync([FromBody] CRMGridOptions options, [FromQuery] int companyId)
//    {
//        var summaryGrid = await _serviceManager.Users.UserSummaryAsync(companyId, trackChanges: false, options);

//        if (!summaryGrid.Items.Any())
//            return Ok(ApiResponseHelper.Success(new GridEntity<UsersDto>(), "No users found."));

//        return Ok(ApiResponseHelper.Success(summaryGrid, "Users retrieved successfully"));
//    }

//    /// <summary>
//    /// Creates a new user.
//    /// </summary>
//    [HttpPost(RouteConstants.CreateUser)]
//    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
//    public async Task<IActionResult> CreateUserAsync([FromBody] UsersDto modelDto)
//    {
//        var createdUser = await _serviceManager.Users.CreateAsync(modelDto);

//        if (createdUser.UserId <= 0)
//            throw new InvalidCreateOperationException("Failed to create user record.");

//        return Ok(ApiResponseHelper.Created(createdUser, "User created successfully."));
//    }

//    /// <summary>
//    /// Updates an existing user.
//    /// </summary>
//    [HttpPut(RouteConstants.UpdateUser)]
//    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
//    public async Task<IActionResult> UpdateUserAsync([FromRoute] int key, [FromBody] UsersDto modelDto)
//    {
//        if (key != modelDto.UserId)
//            throw new IdMismatchBadRequestException(key.ToString(), nameof(UsersDto));

//        var updatedUser = await _serviceManager.Users.UpdateAsync(key, modelDto);

//        return Ok(ApiResponseHelper.Updated(updatedUser, "User updated successfully."));
//    }

//    /// <summary>
//    /// Deletes a user by ID.
//    /// </summary>
//    [HttpDelete(RouteConstants.DeleteUser)]
//    public async Task<IActionResult> DeleteUserAsync([FromRoute] int key)
//    {
//        await _serviceManager.Users.DeleteAsync(key);
//        return Ok(ApiResponseHelper.NoContent<object>("User deleted successfully"));
//    }

//    /// <summary>
//    /// Retrieves a user by ID.
//    /// </summary>
//    [HttpGet(RouteConstants.ReadUser)]
//    public async Task<IActionResult> UserAsync([FromRoute] int id)
//    {
//        if (id <= 0)
//            throw new IdParametersBadRequestException();

//        var user = await _serviceManager.Users.UserAsync(id, trackChanges: false);

//        return Ok(ApiResponseHelper.Success(user, "User retrieved successfully"));
//    }

//    /// <summary>
//    /// Retrieves users for dropdown list.
//    /// </summary>
//    [HttpGet(RouteConstants.UserForDDL)]
//    public async Task<IActionResult> UsersForDDLAsync()
//    {
//        var users = await _serviceManager.Users.UsersForDDLAsync();

//        if (!users.Any())
//            return Ok(ApiResponseHelper.Success(Enumerable.Empty<UsersForDDLDto>(), "No users found."));

//        return Ok(ApiResponseHelper.Success(users, "Users retrieved successfully"));
//    }
//}
