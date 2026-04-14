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
/// Group management endpoints.
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
public class GroupController : BaseApiController
{
	private readonly IMemoryCache _cache;

	// Removed ILinkFactory dependency
	public GroupController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
	{
		_cache = cache;
	}

	#region ── CUD Operations (First) ─────────────────────────────
	/// <summary>
	/// Creates a new group.
	/// POST: /bdDevs-crm/group
	/// </summary>
	[HttpPost(RouteConstants.CreateGroup)]
	[ServiceFilter(typeof(EmptyObjectFilterAttribute))]
	public async Task<IActionResult> CreateGroupAsync( [FromBody] GroupDto modelDto, CancellationToken cancellationToken = default)
	{
		var createdGroup = await _serviceManager.Groups.CreateAsync( modelDto, cancellationToken: cancellationToken);

		if (createdGroup.GroupId <= 0)
			throw new InvalidCreateOperationException("Failed to create group record.");

		// Returns plain DTO
		return Ok(ApiResponseHelper.Created( createdGroup, "Group created successfully."));
	}

	/// <summary>
	/// Updates an existing group.
	/// PUT: /bdDevs-crm/group/{key}
	/// </summary>
	[HttpPut(RouteConstants.UpdateGroup)]
	[ServiceFilter(typeof(EmptyObjectFilterAttribute))]
	public async Task<IActionResult> UpdateGroupAsync( [FromRoute] int key, [FromBody] GroupDto modelDto, CancellationToken cancellationToken = default)
	{
		if (key != modelDto.GroupId)
			throw new IdMismatchBadRequestException(key.ToString(), nameof(GroupDto));

		var updatedGroup = await _serviceManager.Groups.UpdateAsync(key, modelDto, trackChanges: false, cancellationToken: cancellationToken);

		if (updatedGroup.GroupId <= 0)
			throw new InvalidUpdateOperationException("Failed to update group record.");

		// Returns plain DTO
		return Ok(ApiResponseHelper.Updated( updatedGroup, "Group updated successfully."));
	}

	/// <summary>
	/// Deletes a group by ID.
	/// DELETE: /bdDevs-crm/group/{key}
	/// </summary>
	[HttpDelete(RouteConstants.DeleteGroup)]
	public async Task<IActionResult> DeleteGroupAsync( [FromRoute] int key, CancellationToken cancellationToken = default)
	{
		await _serviceManager.Groups.DeleteAsync( key, trackChanges: false, cancellationToken: cancellationToken);
		return Ok(ApiResponseHelper.NoContent<object>("Group deleted successfully"));
	}
	#endregion ── CUD Operations End ──────────────────────────────


	#region ── Read Operations (High to Low Data Volume) ─────────
	/// <summary>
	/// Retrieves paginated summary grid of groups.
	/// [Largest Data Volume] POST: /bdDevs-crm/group-summary
	/// </summary>
	[HttpPost(RouteConstants.GroupSummary)]
	public async Task<IActionResult> GroupSummaryAsync( [FromBody] CRMGridOptions options, CancellationToken cancellationToken = default)
	{
		if (options == null)
			throw new NullModelBadRequestException(nameof(CRMGridOptions));

		var groupSummary = await _serviceManager.Groups.GroupSummaryAsync( trackChanges: false, options, cancellationToken: cancellationToken);

		if (!groupSummary.Items.Any())
			return Ok(ApiResponseHelper.Success( new GridEntity<GroupSummaryDto>(), "No groups found."));

		// Returns plain GridEntity
		return Ok(ApiResponseHelper.Success( groupSummary, "Groups retrieved successfully"));
	}

	/// <summary>
	/// Retrieves all permissions for a specific group.
	/// [Medium Data Volume] GET: /bdDevs-crm/group-permissions/{groupId:int}
	/// </summary>
	[HttpGet(RouteConstants.GroupPermissionsByGroupId)]
	public async Task<IActionResult> GroupPermissionsByGroupIdAsync( [FromRoute] int groupId, CancellationToken cancellationToken = default)
	{
		if (groupId <= 0)
			throw new IdParametersBadRequestException();

		var permissions = await _serviceManager.Groups.GroupPermissionsByGroupIdAsync( groupId, cancellationToken: cancellationToken);

		if (!permissions.Any())
			return Ok(ApiResponseHelper.Success( Enumerable.Empty<GroupPermissionDto>(), "No permissions found for the specified group."));

		return Ok(ApiResponseHelper.Success( permissions, "Group permissions retrieved successfully"));
	}

	/// <summary>
	/// Retrieves all available access controls.
	/// [Small Data Volume - Reference Data] GET: /bdDevs-crm/access-controls
	/// </summary>
	[HttpGet(RouteConstants.AccessControls)]
	[ResponseCache(Duration = 300)]
	public async Task<IActionResult> AccessControlsAsync(CancellationToken cancellationToken = default)
	{
		var accessControls = await _serviceManager.Groups.AccessControlsAsync(cancellationToken: cancellationToken);

		if (!accessControls.Any())
			return Ok(ApiResponseHelper.Success( Enumerable.Empty<AccessControlDto>(), "No access controls found."));

		return Ok(ApiResponseHelper.Success( accessControls, "Access controls retrieved successfully"));
	}

	/// <summary>
	/// Retrieves groups for dropdown list.
	/// [Small Data Volume - DDL] GET: /bdDevs-crm/groups-ddl
	/// </summary>
	[HttpGet(RouteConstants.GroupForDDL)]
	[ResponseCache(Duration = 300)]
	public async Task<IActionResult> GroupsForDDLAsync(CancellationToken cancellationToken = default)
	{
		var groups = await _serviceManager.Groups.GroupsForDDLAsync(cancellationToken: cancellationToken);

		if (!groups.Any())
			return Ok(ApiResponseHelper.Success( Enumerable.Empty<GroupDDLDto>(), "No groups found."));

		return Ok(ApiResponseHelper.Success( groups, "Groups retrieved successfully"));
	}

	/// <summary>
	/// Retrieves a group by ID.
	/// [Smallest Data Volume - Single] GET: /bdDevs-crm/group/{groupId:int}
	/// </summary>
	[HttpGet(RouteConstants.ReadGroup)]
	[ResponseCache(Duration = 60)]
	public async Task<IActionResult> GroupAsync( [FromRoute] int groupId, CancellationToken cancellationToken = default)
	{
		if (groupId <= 0)
			throw new IdParametersBadRequestException();

		var group = await _serviceManager.Groups.GroupAsync( groupId, trackChanges: false, cancellationToken: cancellationToken);

		if (group == null)
			return Ok(ApiResponseHelper.Success<GroupDto>( null, "Group not found."));

		return Ok(ApiResponseHelper.Success( group, "Group retrieved successfully"));
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
///// Group management endpoints.
/////
///// [AuthorizeUser] at class-level ensures:
/////    - Every request validates user via attribute
/////    - CurrentUser / CurrentUserId available from BaseApiController
/////    - No auth checks needed in controller methods
/////    - Exceptions handled by StandardExceptionMiddleware
///// </summary>
//[AuthorizeUser]
//public class GroupController : BaseApiController
//{
//    private readonly IMemoryCache _cache;
//    private readonly ILinkFactory<GroupDto> _linkFactory;

//    public GroupController(IServiceManager serviceManager, IMemoryCache cache, ILinkFactory<GroupDto> linkFactory) : base(serviceManager)
//    {
//        _cache = cache;
//        _linkFactory = linkFactory;
//    }

//    /// <summary>
//    /// Retrieves paginated summary grid of groups.
//    /// </summary>
//    [HttpPost(RouteConstants.GroupSummary)]
//    public async Task<IActionResult> GroupSummaryAsync([FromBody] CRMGridOptions options)
//    {
//        if (options == null)
//            throw new NullModelBadRequestException(nameof(CRMGridOptions));

//        var groupSummary = await _serviceManager.Groups.GroupSummaryAsync(trackChanges: false, options);

//        if (!groupSummary.Items.Any())
//            return Ok(ApiResponseHelper.Success(new GridEntity<GroupSummaryDto>(), "No groups found."));

//        return Ok(ApiResponseHelper.Success(groupSummary, "Groups retrieved successfully"));
//    }

//    /// <summary>
//    /// Retrieves all permissions for a specific group.
//    /// </summary>
//    [HttpGet(RouteConstants.GroupPermissionsByGroupId)]
//    public async Task<IActionResult> GroupPermissionsByGroupIdAsync([FromRoute] int groupId)
//    {
//        if (groupId <= 0)
//            throw new IdParametersBadRequestException();

//        var permissions = await _serviceManager.Groups.GroupPermissionsByGroupIdAsync(groupId);

//        if (!permissions.Any())
//            return Ok(ApiResponseHelper.Success(Enumerable.Empty<GroupPermissionDto>(), "No permissions found for the specified group."));

//        return Ok(ApiResponseHelper.Success(permissions, "Group permissions retrieved successfully"));
//    }

//    /// <summary>
//    /// Retrieves all available access controls.
//    /// </summary>
//    [HttpGet(RouteConstants.AccessControls)]
//    public async Task<IActionResult> AccessControlsAsync()
//    {
//        var accessControls = await _serviceManager.Groups.AccessControlsAsync();

//        if (!accessControls.Any())
//            return Ok(ApiResponseHelper.Success(Enumerable.Empty<AccessControlDto>(), "No access controls found."));

//        return Ok(ApiResponseHelper.Success(accessControls, "Access controls retrieved successfully"));
//    }

//    /// <summary>
//    /// Creates a new group.
//    /// </summary>
//    [HttpPost(RouteConstants.CreateGroup)]
//    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
//    public async Task<IActionResult> CreateGroupAsync([FromBody] GroupDto modelDto)
//    {
//        var createdGroup = await _serviceManager.Groups.CreateAsync(modelDto);

//        if (createdGroup.GroupId <= 0)
//            throw new InvalidCreateOperationException("Failed to create group record.");

//        return Ok(ApiResponseHelper.Created(createdGroup, "Group created successfully."));
//    }

//    /// <summary>
//    /// Updates an existing group.
//    /// </summary>
//    [HttpPut(RouteConstants.UpdateGroup)]
//    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
//    public async Task<IActionResult> UpdateGroupAsync([FromRoute] int key, [FromBody] GroupDto modelDto)
//    {
//        if (key != modelDto.GroupId)
//            throw new IdMismatchBadRequestException(key.ToString(), nameof(GroupDto));

//        var updatedGroup = await _serviceManager.Groups.UpdateAsync(key, modelDto);

//        return Ok(ApiResponseHelper.Updated(updatedGroup, "Group updated successfully."));
//    }

//    /// <summary>
//    /// Deletes a group by ID.
//    /// </summary>
//    [HttpDelete(RouteConstants.DeleteGroup)]
//    public async Task<IActionResult> DeleteGroupAsync([FromRoute] int key)
//    {
//        await _serviceManager.Groups.DeleteAsync(key);
//        return Ok(ApiResponseHelper.NoContent<object>("Group deleted successfully"));
//    }

//    /// <summary>
//    /// Retrieves groups for dropdown list.
//    /// </summary>
//    [HttpGet(RouteConstants.GroupForDDL)]
//    public async Task<IActionResult> GroupsForDDLAsync()
//    {
//        var groups = await _serviceManager.Groups.GroupsForDDLAsync();

//        if (!groups.Any())
//            return Ok(ApiResponseHelper.Success(Enumerable.Empty<GroupForDDLDto>(), "No groups found."));

//        return Ok(ApiResponseHelper.Success(groups, "Groups retrieved successfully"));
//    }
//}
