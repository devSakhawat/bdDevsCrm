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
/// Module management endpoints.
///
/// [AuthorizeUser] at class-level ensures:
///    - Every request validates user via attribute
///    - CurrentUser / CurrentUserId available from BaseApiController
///    - No auth checks needed in controller methods
///    - Exceptions handled by StandardExceptionMiddleware
/// </summary>
[AuthorizeUser]
public class ModuleController : BaseApiController
{
	private readonly IMemoryCache _cache;
	private readonly ILinkFactory<ModuleDto> _linkFactory;

	public ModuleController(IServiceManager serviceManager, IMemoryCache cache, ILinkFactory<ModuleDto> linkFactory) : base(serviceManager)
	{
		_cache = cache;
		_linkFactory = linkFactory;
	}

	#region CUD
	/// <summary>
	/// Creates a new module.
	/// </summary>
	[HttpPost(RouteConstants.CreateModule)]
	[ServiceFilter(typeof(EmptyObjectFilterAttribute))]
	public async Task<IActionResult> CreateModuleAsync([FromBody] ModuleDto modelDto, CancellationToken cancellationToken = default)
	{
		var createdModule = await _serviceManager.Modules.CreateModuleAsync(modelDto, cancellationToken);

		if (createdModule.ModuleId <= 0)
			throw new InvalidCreateOperationException("Failed to create module record.");

		return Ok(ApiResponseHelper.Created(createdModule, "Module created successfully."));
	}

	/// <summary>
	/// Updates an existing module.
	/// </summary>
	[HttpPut(RouteConstants.UpdateModule)]
	[ServiceFilter(typeof(EmptyObjectFilterAttribute))]
	public async Task<IActionResult> UpdateModuleAsync([FromRoute] int key, [FromBody] ModuleDto modelDto, CancellationToken cancellationToken = default)
	{
		if (key != modelDto.ModuleId)
			throw new IdMismatchBadRequestException(key.ToString(), nameof(ModuleDto));

		var updatedModule = await _serviceManager.Modules.UpdateModuleAsync(key, modelDto, trackChanges: true, cancellationToken: cancellationToken);

		return Ok(ApiResponseHelper.Updated(updatedModule, "Module updated successfully."));
	}

	/// <summary>
	/// Deletes a module by ID.
	/// </summary>
	[HttpDelete(RouteConstants.DeleteModule)]
	public async Task<IActionResult> DeleteModuleAsync([FromRoute] int key, CancellationToken cancellationToken = default)
	{
		await _serviceManager.Modules.DeleteModuleAsync(key, trackChanges: true, cancellationToken: cancellationToken);
		return Ok(ApiResponseHelper.NoContent<object>("Module deleted successfully"));
	}

	#endregion CUD

	#region Read
	/// <summary>
	/// Retrieves paginated summary grid of modules.
	/// </summary>
	[HttpPost(RouteConstants.ModuleSummary)]
	public async Task<IActionResult> ModuleSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
	{
		if (options == null)
			throw new NullModelBadRequestException(nameof(GridOptions));

		var modules = await _serviceManager.Modules.ModuleSummaryAsync(trackChanges: false, options, cancellationToken);

		if (!modules.Items.Any())
			return Ok(ApiResponseHelper.Success(new GridEntity<ModuleDto>(), "No modules found."));

		return Ok(ApiResponseHelper.Success(modules, "Modules retrieved successfully"));
	}

	/// <summary>
	/// Retrieves all modules.
	/// </summary>
	[HttpGet(RouteConstants.ReadModules)]
	[ResponseCache(Duration = 60)]
	public async Task<IActionResult> ModulesAsync(CancellationToken cancellationToken = default)
	{
		var modules = await _serviceManager.Modules.ModulesAsync(trackChanges: false, cancellationToken: cancellationToken);

		if (!modules.Any())
			return Ok(ApiResponseHelper.Success(Enumerable.Empty<ModuleDto>(), "No modules found."));

		return Ok(ApiResponseHelper.Success(modules, "Modules retrieved successfully"));
	}
	/// <summary>
	/// Retrieves a module by ID.
	/// </summary>
	[HttpGet(RouteConstants.ReadModule)]
	public async Task<IActionResult> ModuleAsync([FromRoute] int id, CancellationToken cancellationToken = default)
	{
		if (id <= 0)
			throw new IdParametersBadRequestException();

		var module = await _serviceManager.Modules.ModuleAsync(id, trackChanges: false, cancellationToken: cancellationToken);

		return Ok(ApiResponseHelper.Success(module, "Module retrieved successfully"));
	}

	/// <summary>
	/// Retrieves modules for dropdown list.
	/// </summary>
	[HttpGet(RouteConstants.ModuleDDL)]
	public async Task<IActionResult> ModulesForDDLAsync(CancellationToken cancellationToken = default)
	{
		var modules = await _serviceManager.Modules.ModuleForDDLAsync(cancellationToken);

		if (!modules.Any())
			return Ok(ApiResponseHelper.Success(Enumerable.Empty<ModuleForDDLDto>(), "No modules found."));

		return Ok(ApiResponseHelper.Success(modules, "Modules retrieved successfully"));
	}
}
	#endregion read

