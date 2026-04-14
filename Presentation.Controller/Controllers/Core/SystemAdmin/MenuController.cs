using Presentation.AuthorizeAttributes;
using Domain.Contracts.Services;
using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Domain.Exceptions;
using bdDevs.Shared.Constants;
using Application.Shared.Grid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Presentation.ActionFilters;
using Presentation.LinkFactories;

namespace Presentation.Controllers.Core.SystemAdmin;


/// <summary>
/// Menu management endpoints.
///
/// [AuthorizeUser] at class-level ensures:
///    - Every request validates user via attribute
///    - CurrentUser / CurrentUserId available from BaseApiController
///    - No auth checks needed in controller methods
///    - Exceptions handled by StandardExceptionMiddleware
/// </summary>
[AuthorizeUser]
public class MenuController : BaseApiController
{
	private readonly IMemoryCache _cache;
  private readonly ILinkFactory<MenuDto> _linkFactory; 
  public MenuController(IServiceManager serviceManager, IMemoryCache cache, ILinkFactory<MenuDto> linkFactory) : base(serviceManager)
	{
		_cache = cache;
    _linkFactory = linkFactory;
  }

	[HttpPost(RouteConstants.CreateMenu)]
	[ServiceFilter(typeof(EmptyObjectFilterAttribute))]
	public async Task<IActionResult> CreateMenu([FromBody] MenuDto modelDto, CancellationToken cancellationToken = default)
	{
		var model = await _serviceManager.Menus.CreateMenuAsync(modelDto, cancellationToken);

		if (model.MenuId <= 0)
			throw new InvalidCreateOperationException("Failed to create menu record.");

		//return Ok(ApiResponseHelper.Created(model, "Menu created successfully."));
		//return CreatedResponse(data:model, "Menu created successfully.");
		return CreatedResponse();
  }

	[HttpPut(RouteConstants.UpdateMenu)]
	[ServiceFilter(typeof(EmptyObjectFilterAttribute))]
	public async Task<IActionResult> UpdateMenuAsync([FromRoute] int key, [FromBody] MenuDto modelDto, CancellationToken cancellationToken = default)
	{
		MenuDto returnData = await _serviceManager.Menus.UpdateMenuAsync(key, modelDto, trackChanges: false, cancellationToken: cancellationToken);

		if (returnData.MenuId <= 0)
			throw new InvalidUpdateOperationException("Failed to update menu record.");

		return Ok(ApiResponseHelper.Updated(returnData, "Menu updated successfully."));
	}

	[HttpDelete(RouteConstants.DeleteMenu)]
	public async Task<IActionResult> DeleteMenuAsync([FromRoute] int key, CancellationToken cancellationToken = default)
	{
		await _serviceManager.Menus.DeleteMenuAsync(key, trackChanges: false, cancellationToken: cancellationToken);
		return Ok(ApiResponseHelper.NoContent<object>("Menu deleted successfully"));
	}

	/// <summary>
	///  menu summary with pagination + HATEOAS
	/// </summary>
	[HttpPost(RouteConstants.MenuSummary)]
	public async Task<IActionResult> MenuSummary([FromBody] CRMGridOptions options, CancellationToken cancellationToken = default)
	{
		//var res = await _serviceManager.Menus.MenuSummary(trackChanges: false, options);

		//if (res == null || !res.Items.Any())
		//	return Ok(ApiResponseHelper.Success(new GridEntity<MenuDto>(), "No menus found."));

		var res = await _serviceManager.Menus.MenuSummaryAsync(trackChanges: false, options, cancellationToken: cancellationToken);

		if (res == null || !res.Items.Any())
			return Ok(ApiResponseHelper.Success(new GridEntity<LinkedResource<MenuDto>>(), "No menus found."));

		// ── HATEOAS: wrap each row with links ──────────────────────────
		var linkedGrid = ToLinkedGrid(res, _linkFactory, x => (int)x.MenuId);

		// ── Resource-level navigation links ───────────────────────────
		var resourceLinks = _linkFactory.GenerateResourceLinks();

    //return Ok(ApiResponseHelper.SuccessGrid(
    //		data: linkedGrid,
    //		currentPage: options.page,
    //		pageSize: options.pageSize,
    //		totalCount: res.TotalCount,
    //		resourceLinks: resourceLinks,
    //		message: "Menu summary retrieved successfully"
    //));
    return ToLinkedGridResponse(res, _linkFactory, x => (int)x.MenuId, options, "Menu summary retrieved successfully");
  }

	[HttpGet(RouteConstants.ReadMenus)]
	[ResponseCache(Duration = 60)] // Browser caching for 5 minutes
	public async Task<IActionResult> ReadMenus(CancellationToken cancellationToken = default)
	{
		IEnumerable<MenuDto> menusDto = await _serviceManager.Menus.MenusAsync(trackChanges: false, cancellationToken: cancellationToken);
		if (!menusDto.Any())
			return Ok(ApiResponseHelper.Success(Enumerable.Empty<MenuDto>(), "No menus found."));
		return Ok(ApiResponseHelper.Success(menusDto, "Menus retrieved successfully"));
	}

	/// <summary>
	///  menus for dropdown list
	/// </summary>
	[HttpGet(RouteConstants.MenusDDL)]
	public async Task<IActionResult> MenusDDL(CancellationToken cancellationToken = default)
	{
		var menusDto = await _serviceManager.Menus.MenuForDDLAsync(cancellationToken: cancellationToken);

		if (!menusDto.Any())
			return Ok(ApiResponseHelper.Success(Enumerable.Empty<MenuForDDLDto>(), "No menus found."));

		return Ok(ApiResponseHelper.Success(menusDto, "Menus retrieved successfully"));
	}

	[HttpGet(RouteConstants.MenusByUserPermission)]
	[ResponseCache(Duration = 300)] // Browser caching for 5 minutes
	public async Task<IActionResult> MenusByUserPermission(CancellationToken cancellationToken = default)
	{
		var userId = CurrentUserId;

		if (userId <= 0)
			throw new GenericUnauthorizedException("User authentication required. Please log in again.");

		string cacheKey = $"menu_permissions_{userId}";

		if (_cache.TryGetValue(cacheKey, out IEnumerable<MenuDto> cachedMenus))
			return Ok(ApiResponseHelper.Success(cachedMenus, "Menus retrieved from cache"));

		var menusDtoTask = _serviceManager.Menus.MenusByUserPermissionAsync(userId, trackChanges: false, cancellationToken: cancellationToken);

		// Add timeout to prevent long-running queries
		var completedTask = await Task.WhenAny(menusDtoTask, Task.Delay(5000, cancellationToken));

		if (completedTask != menusDtoTask)
			throw new RequestTimeoutException("Request timeout while retrieving menu data");

		var menusDto = await menusDtoTask;

		if (!menusDto.Any())
			return Ok(ApiResponseHelper.Success(Enumerable.Empty<MenuDto>(), "No menus found for this user."));

		// Cache the result
		var cacheOptions = new MemoryCacheEntryOptions()
			.SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
			.SetPriority(CacheItemPriority.High);

		_cache.Set(cacheKey, menusDto, cacheOptions);

		return Ok(ApiResponseHelper.Success(menusDto, "Menus retrieved successfully"));
	}

	[HttpGet(RouteConstants.MenusByModuleId)]
	[ResponseCache(Duration = 60)]
	public async Task<IActionResult> MenusByModuleId([FromRoute] int moduleId, CancellationToken cancellationToken = default)
	{
		// No need for manual auth checks - [AuthorizeUser] at class level handles it
		// CurrentUser and CurrentUserId are available from BaseApiController

		var res = await _serviceManager.Menus.MenusByModuleIdAsync(moduleId, trackChanges: false, cancellationToken: cancellationToken);

		if (res == null || !res.Any())
			return Ok(ApiResponseHelper.Success(Enumerable.Empty<MenuDto>(), "No menus found for this module."));

		return Ok(ApiResponseHelper.Success(res, "Data retrieved successfully"));
	}

	[HttpGet(RouteConstants.ParentMenuByMenu)]
	public async Task<IActionResult> ParentMenuByMenu(int menuId, CancellationToken cancellationToken = default)
	{
		var menusDto = await _serviceManager.Menus.ParentMenusByMenuAsync(menuId, trackChanges: false, cancellationToken: cancellationToken);
		if (!menusDto.Any())
			return Ok(ApiResponseHelper.Success(Enumerable.Empty<MenuDto>(), "No parent menus found."));

		return Ok(ApiResponseHelper.Success(menusDto, "Parent menus retrieved successfully"));
	}


	[HttpGet(RouteConstants.ReadMenu)]
	[ResponseCache(Duration = 60)] // Browser caching for 5 minutes
	public async Task<IActionResult> ReadMenu(int menuId, CancellationToken cancellationToken = default)
	{
		MenuDto menuDto = await _serviceManager.Menus.MenuAsync(menuId, trackChanges: false, cancellationToken: cancellationToken);
		if (menuDto == null)
			return Ok(ApiResponseHelper.Success(null, "No menu found."));
		return Ok(ApiResponseHelper.Success(menuDto, "Menu retrieved successfully"));
	}


}











//using Presentation.AuthorizeAttributes;
//using Presentation.Extensions;
//using Domain.Contracts.Services;
//using bdDevs.Shared;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using Domain.Exceptions;
//using bdDevs.Shared.Constants;
//using Application.Shared.Grid;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Caching.Memory;
//using Microsoft.IdentityModel.Tokens;

//namespace Presentation.Controllers.Core.SystemAdmin;


///// <summary>
///// Menu management endpoints.
/////
///// [AuthorizeUser] at class-level ensures:
/////    - Every request validates user via attribute
/////    - CurrentUser / CurrentUserId available from BaseApiController
/////    - No auth checks needed in controller methods
/////    - Exceptions handled by StandardExceptionMiddleware
///// </summary>
//[AuthorizeUser]
//public class MenuController : BaseApiController
//{
//	private readonly IMemoryCache _cache;

//	public MenuController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
//	{
//		_cache = cache;
//	}

//	[Produces("application/json")]
//	[HttpGet(RouteConstants.SelectMenuByUserPermission)]
//	[ResponseCache(Duration = 300)] // Browser caching for 5 minutes
//	public async Task<IActionResult> SelectMenuByUserPermission()
//	{
//		var userId = CurrentUserId;
//		if (userId <= 0)
//			throw new GenericUnauthorizedException("User authentication required. Please log in again.");


//		string cacheKey = $"menu_permissions_{userId}";
//		if (_cache.TryGetValue(cacheKey, out IEnumerable<MenuDto> cachedMenus))
//			return Ok(ApiResponseHelper.Success(cachedMenus, "Menus retrieved from cache"));

//		var menusDtoTask = _serviceManager.Menus.SelectMenuByUserPermission(userId, trackChanges: false);
//		// Add timeout to prevent long-running queries
//		var completedTask = await Task.WhenAny(menusDtoTask, Task.Delay(5000));
//		if (completedTask != menusDtoTask)
//			throw new RequestTimeoutException("Request timeout while retrieving menu data");

//		var menusDto = await menusDtoTask;
//		if (!menusDto.Any())
//			return Ok(ApiResponseHelper.Success(Enumerable.Empty<MenuDto>(), "No menus found for this user."));

//		// Cache the result
//		var cacheOptions = new MemoryCacheEntryOptions()
//			.SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
//			.SetPriority(CacheItemPriority.High);

//		_cache.Set(cacheKey, menusDto, cacheOptions);

//		return Ok(ApiResponseHelper.Success(menusDto, "Menus retrieved successfully"));
//	}

//	[HttpGet(RouteConstants.ParentMenuByMenu)]
//	public async Task<IActionResult> ParentMenuByMenu(int parentMenuId)
//	{
//		var menusDto = await _serviceManager.Menus.ParentMenuByMenu(parentMenuId, false);
//		if (!menusDto.Any())
//			return Ok(ApiResponseHelper.Success(Enumerable.Empty<MenuDto>(), "No parent menus found."));

//		return Ok(ApiResponseHelper.Success(menusDto, "Parent menus retrieved successfully"));
//	}

//	[HttpGet(RouteConstants.Menus)]
//	[ResponseCache(Duration = 60)] // Browser caching for 5 minutes
//	public async Task<IActionResult> Menus()
//	{
//		IEnumerable<MenuDto> menusDto = await _serviceManager.Menus.MenusAsync(trackChanges: false);
//		if (!menusDto.Any())
//			return Ok(ApiResponseHelper.Success(Enumerable.Empty<MenuDto>(), "No menus found."));
//		return Ok(ApiResponseHelper.Success(menusDto, "Menus retrieved successfully"));
//	}

//	[HttpGet(RouteConstants.MenusByModuleId)]
//	[ResponseCache(Duration = 60)]
//	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//	public async Task<IActionResult> MenusByModuleId([FromRoute] int moduleId)
//	{
//		var currentUser = HttpContext.CurrentUser();
//		var userId = HttpContext.UserId();

//		if (string.IsNullOrEmpty(userId.ToString()))
//			throw new GenericUnauthorizedException("User authentication required.");
//		if (currentUser == null)
//			throw new GenericUnauthorizedException("User session expired.");

//		//UsersDto currentUser2 = _serviceManager.Cache<UsersDto>(userId);
//		//if (currentUser2 == null)
//		//	throw new GenericUnauthorizedException("User session expired.");

//		var res = await _serviceManager.Menus.MenusByModuleId(moduleId, trackChanges: false);

//		if (res == null || !res.Any())
//			return Ok(ApiResponseHelper.Success(Enumerable.Empty<MenuDto>(), "No menus found for this module."));

//		return Ok(ApiResponseHelper.Success(res, "Data retrieved successfully"));
//	}


//	/// <summary>
//	/// After login to system
//	/// </summary>
//	/// <param name="options"></param>
//	/// <returns></returns>
//	[HttpPost(RouteConstants.MenuSummary)]
//	public async Task<IActionResult> MenuSummary([FromBody] CRMGridOptions options)
//	{
//		//var currentUser = HttpContext.CurrentUser();
//		//var userId = HttpContext.UserId();

//		//if (string.IsNullOrEmpty(userId.ToString()))
//		//	throw new GenericUnauthorizedException("User authentication required.");
//		//if (currentUser == null)
//		//	throw new GenericUnauthorizedException("User session expired.");

//		var res = await _serviceManager.Menus.MenuSummary(trackChanges: false, options);

//		if (res == null || !res.Items.Any())
//			return Ok(ApiResponseHelper.Success(new GridEntity<MenuDto>(), "No menus found."));

//		// PaginationMetadata with response
//		int totalPages = (int)Math.Ceiling(res.TotalCount / (double)options.pageSize);
//		var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";
//		var links = ApiResponseHelper.GeneratePaginationLinks(
//				baseUrl, options.page, totalPages, options.pageSize);

//		return Ok(ApiResponseHelper.SuccessWithPagination(
//				data: res,
//				currentPage: options.page,
//				pageSize: options.pageSize,
//				totalCount: res.TotalCount,
//				message: "Menu summary retrieved successfully",
//				links: links
//		));
//	}


//	[HttpPost(RouteConstants.CreateMenu)]
//	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//	public async Task<IActionResult> CreateMenu([FromBody] MenuDto modelDto)
//	{
//		//var currentUser = HttpContext.CurrentUser();
//		//var userId = HttpContext.UserId();

//		//if (string.IsNullOrEmpty(userId.ToString()))
//		//	throw new GenericUnauthorizedException("User authentication required.");
//		//if (currentUser == null)
//		//	throw new GenericUnauthorizedException("User session expired.");

//		var model = await _serviceManager.Menus.CreateAsync(modelDto);
//		if (model.MenuId <= 0)
//			throw new InvalidCreateOperationException("Failed to create record.");

//		return Ok(ApiResponseHelper.Created(model, "Menu created successfully."));
//	}


//	[HttpPut(RouteConstants.UpdateMenu)]
//	public async Task<IActionResult> UpdateMenu([FromRoute] int key, [FromBody] MenuDto modelDto)
//	{
//		//var currentUser = HttpContext.CurrentUser();
//		//var userId = HttpContext.UserId();

//		//if (string.IsNullOrEmpty(userId.ToString()))
//		//	throw new GenericUnauthorizedException("User authentication required.");
//		//if (currentUser == null)
//		//	throw new GenericUnauthorizedException("User session expired.");

//		MenuDto returnData = await _serviceManager.Menus.UpdateAsync(key, modelDto);

//		if (returnData.MenuId <= 0)
//			throw new InvalidCreateOperationException("Failed to update record.");

//		return Ok(ApiResponseHelper.Updated(returnData, "Menu updated successfully."));
//	}

//	[HttpDelete(RouteConstants.DeleteMenu)]
//	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//	public async Task<IActionResult> DeleteMenu([FromRoute] int key)
//	{
//		//var currentUser = HttpContext.CurrentUser();
//		//var userId = HttpContext.UserId();

//		//if (string.IsNullOrEmpty(userId.ToString()))
//		//	throw new GenericUnauthorizedException("User authentication required.");
//		//if (currentUser == null)
//		//	throw new GenericUnauthorizedException("User session expired.");

//		await _serviceManager.Menus.DeleteAsync(key);
//		return Ok(ApiResponseHelper.Success("Menu deleted successfully.", null));
//	}


//	/// <summary>
//	/// menus for Dropdown list
//	/// </summary>
//	[HttpGet(RouteConstants.MenuForDDL)]
//	public async Task<IActionResult> MenuForDDL()
//	{
//		//var currentUser = HttpContext.CurrentUser();
//		//var userId = HttpContext.UserId();

//		//if (string.IsNullOrEmpty(userId.ToString()))
//		//	throw new GenericUnauthorizedException("User authentication required.");
//		//if (currentUser == null)
//		//	throw new GenericUnauthorizedException("User session expired.");

//		var menusDto = await _serviceManager.Menus.MenuForDDL();
//		if (!menusDto.Any())
//			return Ok(ApiResponseHelper.Success(Enumerable.Empty<MenuForDDLDto>(), "No menus found."));

//		return Ok(ApiResponseHelper.Success(menusDto, "Menus retrieved successfully"));
//	}


//}
