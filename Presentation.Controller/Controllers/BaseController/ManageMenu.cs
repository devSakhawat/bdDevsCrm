using Domain.Contracts.Services;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Domain.Exceptions;
using Domain.Exceptions;
using bdDevs.Shared.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.Controllers.BaseController;

public static class ManageMenu
{
	private static string MethodPathOnly(HttpRequest request)
	{
		var basePath = "/" + RouteConstants.BaseRoute;
		var path = request.Path.Value ?? string.Empty;

		// Remove base (basePath)
		var withoutBase = path.StartsWith(basePath, StringComparison.OrdinalIgnoreCase)
			? path[basePath.Length..]
			: path; // Method path only.

		var methodPath = withoutBase.Split('/', StringSplitOptions.RemoveEmptyEntries).ToList();
		static bool IsTailParam(string s) => int.TryParse(s, out _) || Guid.TryParse(s, out _);
		if (methodPath.Count > 0 && IsTailParam(methodPath[^1])) methodPath.RemoveAt(methodPath.Count - 1);

		return string.Join('/', methodPath);
		//return methodPath.ToString();
	}

	private static string NormalizedApiPath(HttpRequest request)
	{
		var methodPath = MethodPathOnly(request);
		//return "/" + RouteConstants.BaseRoute + "/" + methodPath;

		var returnPath = methodPath.StartsWith("/", StringComparison.Ordinal) ? methodPath : "/" + methodPath;
		return returnPath;
	}

	// Async + optional IServiceManager (DI or parameter)
	public static async Task<MenuDto> Async(ControllerBase filterContext, IServiceManager? serviceManager = null, CancellationToken cancellationToken = default)
	{
		try
		{
			if (filterContext is null)
				throw new GenericBadRequestException("Invalid controller context.");

			serviceManager ??= filterContext.HttpContext.RequestServices.GetRequiredService<IServiceManager>();

			var userIdClaim = filterContext.HttpContext.User.FindFirst("UserId")?.Value;
			if (string.IsNullOrWhiteSpace(userIdClaim))
				throw new GenericUnauthorizedException("User authentication required.");

			if (!int.TryParse(userIdClaim, out var userId) || userId <= 0)
				throw new GenericBadRequestException("Invalid user ID format.");

			var currentUser = serviceManager.Cache<UsersDto>(userId);
			if (currentUser == null)
				throw new GenericUnauthorizedException("User session expired.");

			var request = filterContext.HttpContext.Request;
			var apiPath = NormalizedApiPath(request);
			var rawUrl = $"..{apiPath}";

			//var apiPath = $"{request.PathBase}{request.Path}{request.QueryString}";
			//var fullURL = $"{request.Scheme}://{request.Host}{apiPath}";
			//var rawUrl = ".." + apiPath;

			var menu = await serviceManager.Groups.CheckMenuPermissionAsync(rawUrl, currentUser,cancellationToken);
			//var menu = await serviceManager.Groups.CheckMenuPermission(rawUrl, currentUser);
			var menuMinimal = new MenuDto
			{
				MenuPath = rawUrl,
				StatusCode = 204,
				ReponseMessage = "No menu permission found"
			};

			return await Task.FromResult(menu ?? menuMinimal);
		}
		catch (GenericBadRequestException) { throw; }
		catch (GenericUnauthorizedException) { throw; }
		catch (Exception ex)
		{
			throw new BadRequestException(ex.Message);
		}
	}

	// Async + optional IServiceManager (DI or parameter)
	public static async Task<MenuDto> ByMenuPathAsync(ControllerBase filterContext, string menuPath, UsersDto currentUser, CancellationToken cancellationToken = default)
	{
		try
		{
			if (filterContext is null)
				throw new GenericBadRequestException("Invalid controller context.");

			var serviceManager = filterContext.HttpContext.RequestServices.GetRequiredService<IServiceManager>();

			var menu = await serviceManager.Groups.CheckMenuPermissionAsync(menuPath, currentUser, cancellationToken);
			var menuMinimal = new MenuDto
			{
				MenuPath = menuPath,
				StatusCode = 204,
				ReponseMessage = "No menu permission found"
			};

			return await Task.FromResult(menu ?? menuMinimal);
		}
		catch (GenericBadRequestException) { throw; }
		catch (GenericUnauthorizedException) { throw; }
		catch (Exception ex)
		{
			throw new BadRequestException(ex.Message);
		}
	}


	// Check menu permission by MenuName for current user (permission aware via path) path from menu name controller.
	public static async Task<MenuDto> CheckByMenuName(ControllerBase filterContext, string menuName, IServiceManager? serviceManager = null, CancellationToken cancellationToken = default)
	{
		try
		{
			if (filterContext is null)
				throw new GenericBadRequestException("Invalid controller context.");

			serviceManager ??= filterContext.HttpContext.RequestServices.GetRequiredService<IServiceManager>();

			var userIdClaim = filterContext.HttpContext.User.FindFirst("UserId")?.Value;
			if (string.IsNullOrWhiteSpace(userIdClaim))
				throw new GenericUnauthorizedException("User authentication required.");

			if (!int.TryParse(userIdClaim, out var userId) || userId <= 0)
				throw new GenericBadRequestException("Invalid user ID format.");

			var currentUser = serviceManager.Cache<UsersDto>(userId);
			if (currentUser == null)
				throw new GenericUnauthorizedException("User session expired.");

			if (!MenuConstant.TryPath(menuName, out var menuPath))
				throw new GenericBadRequestException("Invalid menu name.");

			var rawUrl = $"..{menuPath}"; // only if your Menu.MenuPath uses this pattern

			var menu = await serviceManager.Groups.CheckMenuPermissionAsync(rawUrl, currentUser, cancellationToken);
			var menuMinimal = new MenuDto
			{
				MenuPath = rawUrl,
				StatusCode = 204,
				ReponseMessage = "No menu permission found"
			};

			return await Task.FromResult(menu ?? menuMinimal);
		}
		catch (GenericBadRequestException) { throw; }
		catch (GenericUnauthorizedException) { throw; }
		catch (Exception ex)
		{
			throw new BadRequestException(ex.Message);
		}
	}

	//   //  MenuId by MenuName using database for menu
	public static async Task<MenuDto?> MenuByNameAsync(ControllerBase filterContext, string menuName, IServiceManager? serviceManager = null)
	{
		try
		{
			if (filterContext is null)
				throw new GenericBadRequestException("Invalid controller context.");
			if (string.IsNullOrWhiteSpace(menuName))
				throw new GenericBadRequestException("Menu name is required.");

			var userIdClaim = filterContext.HttpContext.User.FindFirst("UserId")?.Value;
			if (string.IsNullOrWhiteSpace(userIdClaim))
				throw new GenericUnauthorizedException("User authentication required.");

			if (!int.TryParse(userIdClaim, out var userId) || userId <= 0)
				throw new GenericBadRequestException("Invalid user ID format.");

			var currentUser = serviceManager.Cache<UsersDto>(userId);
			if (currentUser == null)
				throw new GenericUnauthorizedException("User session expired.");

			var menus = await serviceManager.Menus.MenusByMenuNameAsync(menuName, false);
			var menu = menus.FirstOrDefault(m => string.Equals(m.MenuName.Trim().ToLower(), menuName.Trim().ToLower(), StringComparison.OrdinalIgnoreCase));
			return menu;
		}
		catch (GenericBadRequestException) { throw; }
		catch (Exception ex)
		{
			throw new BadRequestException(ex.Message);
		}
	}

	//  MenuId by MenuName for current user (permission aware via path)
	public static async Task<int?> MenuIdByNameForCurrentUserAsync(ControllerBase filterContext, string menuName, IServiceManager? serviceManager = null, CancellationToken cancellationToken = default)
	{
		try
		{
			if (filterContext is null)
				throw new GenericBadRequestException("Invalid controller context.");
			if (string.IsNullOrWhiteSpace(menuName))
				throw new GenericBadRequestException("Menu name is required.");

			serviceManager ??= filterContext.HttpContext.RequestServices.GetRequiredService<IServiceManager>();

			var userIdClaim = filterContext.HttpContext.User.FindFirst("UserId")?.Value;
			if (string.IsNullOrWhiteSpace(userIdClaim))
				throw new GenericUnauthorizedException("User authentication required.");
			if (!int.TryParse(userIdClaim, out var userId) || userId <= 0)
				throw new GenericBadRequestException("Invalid user ID format.");

			var currentUser = serviceManager.Cache<UsersDto>(userId);
			if (currentUser == null)
				throw new GenericUnauthorizedException("User session expired.");

			if (!MenuConstant.TryPath(menuName, out var menuPath))
				throw new GenericBadRequestException("Invalid menu name.");

			var rawUrl = $"..{menuPath}";
			var menuDto = await serviceManager.Groups.CheckMenuPermissionAsync(rawUrl, currentUser, cancellationToken);
			return menuDto?.MenuId;
		}
		catch (GenericBadRequestException) { throw; }
		catch (GenericUnauthorizedException) { throw; }
		catch (Exception ex)
		{
			throw new BadRequestException(ex.Message);
		}
	}
}




