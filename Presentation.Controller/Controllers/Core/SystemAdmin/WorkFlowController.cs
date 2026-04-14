using Presentation.AuthorizeAttributes;
using Presentation.Extensions;
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
/// Status and workflow management endpoints.
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
public class WorkFlowController : BaseApiController
{
  private readonly IMemoryCache _cache;
  private readonly ILinkFactory<WfStateDto> _linkFactory;

  public WorkFlowController(IServiceManager serviceManager, IMemoryCache cache, ILinkFactory<WfStateDto> linkFactory) : base(serviceManager)
  {
    _cache = cache;
    _linkFactory = linkFactory;
  }

	#region ── CUD Operations (First) ─────────────────────────────

	/// <summary>
	/// Creates a new workflow state.
	/// POST: /bdDevs-crm/workflow-state
	/// </summary>
	[HttpPost(RouteConstants.CreateWorkflowState)]
	[ServiceFilter(typeof(EmptyObjectFilterAttribute))]
	public async Task<IActionResult> CreateWorkflowStateAsync([FromBody] WfStateDto modelDto, CancellationToken cancellationToken = default)
	{
		var currentUser = CurrentUser;
		var createdState = await _serviceManager.WfState.CreateStatusAsync( modelDto, currentUser, cancellationToken: cancellationToken);

		if (createdState.WfStateId <= 0)
			throw new InvalidCreateOperationException("Failed to create workflow state record.");

		// Returns plain DTO
		return Ok(ApiResponseHelper.Created(createdState, "Workflow state created successfully."));
	}

	/// <summary>
	/// Updates an existing workflow state.
	/// PUT: /bdDevs-crm/workflow-state/{key}
	/// </summary>
	[HttpPut(RouteConstants.UpdateWorkflowState)]
	[ServiceFilter(typeof(EmptyObjectFilterAttribute))]
	public async Task<IActionResult> UpdateWorkflowStateAsync( [FromRoute] int key, [FromBody] WfStateDto modelDto, CancellationToken cancellationToken = default)
	{
		if (key != modelDto.WfStateId)
			throw new IdMismatchBadRequestException(key.ToString(), nameof(WfStateDto));

		var updatedState = await _serviceManager.WfState.UpdateStatusAsync(key, modelDto, trackChanges: false, CurrentUser, cancellationToken: cancellationToken);

		if (updatedState.WfStateId <= 0)
			throw new InvalidUpdateOperationException("Failed to update workflow state record.");

		// Returns plain DTO
		return Ok(ApiResponseHelper.Updated(updatedState, "Workflow state updated successfully."));
	}

	/// <summary>
	/// Deletes a workflow state by ID.
	/// DELETE: /bdDevs-crm/workflow-state/{key}
	/// </summary>
	[HttpDelete(RouteConstants.DeleteWorkflowState)]
	public async Task<IActionResult> DeleteWorkflowStateAsync([FromRoute] int key, CancellationToken cancellationToken = default)
	{
		await _serviceManager.WfState.DeleteStatusAsync(key,trackChanges: false, cancellationToken: cancellationToken);

		return Ok(ApiResponseHelper.NoContent<object>("Workflow state deleted successfully"));
	}

	#endregion ── CUD Operations End ──────────────────────────────


	/// <summary>
	/// Retrieves paginated summary grid of workflows.
	/// </summary>
	[HttpPost(RouteConstants.WorkflowSummary)]
	public async Task<IActionResult> WorkflowSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
	{
		if (options == null)
			throw new NullModelBadRequestException(nameof(GridOptions));

		var workflows = await _serviceManager.WfState.WorkflowSummaryAsync(trackChanges: false, options, cancellationToken: cancellationToken);

		if (!workflows.Items.Any())
			return Ok(ApiResponseHelper.Success(new GridEntity<WfStateDto>(), "No workflows found."));

		return Ok(ApiResponseHelper.Success(workflows, "Workflows retrieved successfully"));
	}


	/// <summary>
	/// Retrieves workflow statuses by menu ID.
	/// </summary>
	[HttpGet(RouteConstants.StatusByMenuId)]
  public async Task<IActionResult> StatusesByMenuIdAsync([FromRoute] int menuId, CancellationToken cancellationToken = default)
  {
    if (menuId <= 0)
      throw new IdParametersBadRequestException();

    var statuses = await _serviceManager.WfState.StatusesByMenuIdAsync(menuId, trackChanges: false, cancellationToken: cancellationToken);

    if (!statuses.Any())
      return Ok(ApiResponseHelper.Success(Enumerable.Empty<WfStateDto>(), "No statuses found for this menu."));

    return Ok(ApiResponseHelper.Success(statuses, "Statuses retrieved successfully"));
  }

  /// <summary>
  /// Retrieves workflow actions by status ID for group.
  /// </summary>
  [HttpGet(RouteConstants.ActionsByStatusIdForGroup)]
  public async Task<IActionResult> ActionsByStatusIdForGroupAsync([FromRoute] int statusId, CancellationToken cancellationToken = default)
  {
    if (statusId <= 0)
      throw new IdParametersBadRequestException();

    var actions = await _serviceManager.WfState.ActionsByStatusIdForGroupAsync(statusId, trackChanges: false, cancellationToken: cancellationToken);

    if (!actions.Any())
      return Ok(ApiResponseHelper.Success(Enumerable.Empty<WfActionDto>(), "No actions found for this status."));

    return Ok(ApiResponseHelper.Success(actions, "Actions retrieved successfully"));
  }


  /// <summary>
  /// Creates a new workflow action.
  /// </summary>
  [HttpPost(RouteConstants.CreateWorkflowAction)]
  [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
  public async Task<IActionResult> CreateWorkflowActionAsync([FromBody] WfActionDto modelDto, CancellationToken cancellationToken = default)
  {
var currentUser = CurrentUser;
		var createdState = await _serviceManager.WfState.CreateWfActionAsync(modelDto, currentUser, trackChanges: true, cancellationToken: cancellationToken);

    if (createdState.WfStateId <= 0)
      throw new InvalidCreateOperationException("Failed to create workflow state record.");

    return Ok(ApiResponseHelper.Created(createdState, "Workflow state created successfully."));
  }

  /// <summary>
  /// Updates an existing workflow action.
  /// </summary>
  [HttpPut(RouteConstants.UpdateWorkflowAction)]
  [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
  public async Task<IActionResult> UpdateWorkflowActionAsync([FromRoute] int key, [FromBody] WfActionDto modelDto , CancellationToken cancellationToken = default)
  {
    if (key != modelDto.WfActionId)
      throw new IdMismatchBadRequestException(key.ToString(), nameof(WfActionDto));

    var updatedState = await _serviceManager.WfState.UpdateWfActionAsync(key, modelDto ,currentUser: CurrentUser, trackChanges: true , cancellationToken: cancellationToken);

    return Ok(ApiResponseHelper.Updated(updatedState, "Workflow state updated successfully."));
  }

  /// <summary>
  /// Deletes a workflow action by ID.
  /// </summary>
  [HttpDelete(RouteConstants.DeleteWorkflowAction)]
  public async Task<IActionResult> DeleteWorkflowActionAsync([FromRoute] int key ,WfActionDto wfActionDto ,CancellationToken cancellation = default)
  {
		await _serviceManager.WfState.DeleteWfActionAsync(key, wfActionDto, trackChanges: true, cancellationToken: cancellation);
		return Ok(ApiResponseHelper.NoContent<object>("Workflow action deleted successfully"));
  }
}
