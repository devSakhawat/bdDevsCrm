using bdDevs.Shared.Constants;
using Domain.Entities.Entities.CRM;
using Domain.Entities.Entities.System;
using Domain.Contracts.Services.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Application.Services.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.CRM;
using Domain.Exceptions;
using Domain.Contracts.Repositories;
using Application.Shared.Grid;
using Application.Services.Mappings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.Core.SystemAdmin;

/// <summary>
/// Status service implementing business logic for workflow state and action management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class StatusService : IStatusService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<StatusService> _logger;
	private readonly IConfiguration _configuration;

	/// <summary>
	/// Initializes a new instance of <see cref="StatusService"/> with required dependencies.
	/// </summary>
	/// <param name="repository">The repository manager for data access operations.</param>
	/// <param name="logger">The logger for capturing service-level events.</param>
	/// <param name="configuration">The application configuration accessor.</param>
	public StatusService(IRepositoryManager repository, ILogger<StatusService> logger, IConfiguration configuration)
	{
		_repository = repository;
		_logger = logger;
		_configuration = configuration;
	}

	/// <summary>
	/// Creates a new workflow state record after validating for null input and duplicate state name.
	/// </summary>
	public async Task<WfStateDto> CreateStatusAsync(WfStateDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(WfStateDto));

		if (entityForCreate.WfStateId != 0)
			throw new InvalidCreateOperationException("WfStateId must be 0 for creating a new workflow state.");

		bool stateExists = await _repository.WfStates.ExistsAsync(
						x => x.StateName != null
								&& x.StateName.Trim().ToLower() == entityForCreate.StateName!.Trim().ToLower()
								&& x.MenuId == entityForCreate.MenuId,
						cancellationToken: cancellationToken);

		if (stateExists)
			throw new DuplicateRecordException("Workflow", "StateName");

		WfState wfStateEntity = MyMapper.JsonClone<WfStateDto, WfState>(entityForCreate);

		await _repository.WfStates.CreateAsync(wfStateEntity, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("Workflow state could not be saved to the database.");

		_logger.LogInformation(
						"Workflow state created successfully. ID: {WfStateId}, Name: {StateName}, UserId: {UserId}, Time: {Time}",
						wfStateEntity.WfStateId,
						wfStateEntity.StateName,
						currentUser?.UserId ?? 0,
						DateTime.UtcNow);

		return MyMapper.JsonClone<WfState, WfStateDto>(wfStateEntity);
	}

	/// <summary>
	/// Updates an existing workflow state record by merging only the changed values from the provided DTO.
	/// </summary>
	public async Task<WfStateDto> UpdateStatusAsync(int wfStateId, WfStateDto modelDto, bool trackChanges, UsersDto currentUser, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(WfStateDto));

		if (wfStateId != modelDto.WfStateId)
			throw new BadRequestException(wfStateId.ToString(), nameof(WfStateDto));

		WfState existingEntity = await _repository.WfStates
						.FirstOrDefaultAsync(x => x.WfStateId == wfStateId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("Status", "WfStateId", wfStateId.ToString());

		bool duplicateExists = await _repository.WfStates.ExistsAsync(
						x => x.StateName != null
								&& x.StateName.Trim().ToLower() == modelDto.StateName!.Trim().ToLower()
								&& x.MenuId == modelDto.MenuId
								&& x.WfStateId != wfStateId,
						cancellationToken: cancellationToken);

		if (duplicateExists)
			throw new DuplicateRecordException("Workflow", "StateName");

		WfState updatedEntity = MyMapper.MergeChangedValues<WfState, WfStateDto>(existingEntity, modelDto);
		_repository.WfStates.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("Status", "WfStateId", wfStateId.ToString());

		_logger.LogInformation(
						"Workflow state updated. ID: {WfStateId}, Name: {StateName}, UserId: {UserId}, Time: {Time}",
						updatedEntity.WfStateId,
						updatedEntity.StateName,
						currentUser?.UserId ?? 0,
						DateTime.UtcNow);

		return MyMapper.JsonClone<WfState, WfStateDto>(updatedEntity);
	}

	/// <summary>
	/// Deletes a workflow state record identified by the given ID.
	/// </summary>
	public async Task<int> DeleteStatusAsync(int wfStateId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (wfStateId <= 0)
			throw new BadRequestException(wfStateId.ToString(), nameof(WfStateDto));

		WfState wfStateEntity = await _repository.WfStates
						.FirstOrDefaultAsync(x => x.WfStateId == wfStateId, trackChanges, cancellationToken)
						?? throw new NotFoundException("WfState", "WfStateId", wfStateId.ToString());

		bool hasActions = await _repository.WfActions
						.ExistsAsync(x => x.WfStateId == wfStateId, cancellationToken: cancellationToken);

		if (hasActions)
			throw new GenericConflictException("Cannot delete workflow status. Action data exists for this statusId. Please delete action data first.");

		await _repository.WfStates.DeleteAsync(x => x.WfStateId == wfStateId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("WfState", "WfStateId", wfStateId.ToString());

		_logger.LogWarning(
						"Workflow status deleted. ID: {WfStateId}, Name: {StateName}, Time: {Time}",
						wfStateEntity.WfStateId,
						wfStateEntity.StateName,
						DateTime.UtcNow);

		return affected;
	}

	/// <summary>
	/// Retrieves a single workflow state record by its ID.
	/// </summary>
	public async Task<WfStateDto> StatusAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		WfState wfState = await _repository.WfStates
						.FirstOrDefaultAsync(x => x.WfStateId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("WfState", "WfStateId", id.ToString());

		_logger.LogInformation(
						"Workflow state fetched successfully. ID: {WfStateId}, Name: {StateName}, Time: {Time}",
						wfState.WfStateId,
						wfState.StateName,
						DateTime.UtcNow);

		return MyMapper.JsonClone<WfState, WfStateDto>(wfState);
	}

	/// <summary>
	/// Retrieves workflow statuses by the specified menu ID.
	/// </summary>
	public async Task<IEnumerable<WfStateDto>> StatusesByMenuIdAsync(int menuId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (menuId <= 0)
		{
			_logger.LogWarning("StatusesByMenuIdAsync called with invalid menuId: {MenuId}", menuId);
			throw new BadRequestException("Invalid request!");
		}

		_logger.LogInformation("Fetching statuses for menu ID: {MenuId}, Time: {Time}", menuId, DateTime.UtcNow);

		IEnumerable<WfState> wfStates = await _repository.WfStates.WfStatesByMenuIdAsync(menuId, trackChanges, cancellationToken);

		//IEnumerable<WfState> wfStates = await _repository.WfStates
		//				.StatusByMenuId(menuId, trackChanges, cancellationToken);

		if (!wfStates.Any())
		{
			_logger.LogWarning("No statuses found for menu ID: {MenuId}, Time: {Time}", menuId, DateTime.UtcNow);
			return Enumerable.Empty<WfStateDto>();
		}

		IEnumerable<WfStateDto> wfStatesDto = MyMapper.JsonCloneIEnumerableToList<WfState, WfStateDto>(wfStates);

		_logger.LogInformation(
						"Statuses fetched successfully. MenuId: {MenuId}, Count: {Count}, Time: {Time}",
						menuId,
						wfStatesDto.Count(),
						DateTime.UtcNow);

		return wfStatesDto;
	}

	/// <summary>
	/// Retrieves workflow actions by the specified status ID for group.
	/// </summary>
	public async Task<IEnumerable<WfActionDto>> ActionsByStatusIdForGroupAsync(int statusId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (statusId <= 0)
		{
			_logger.LogWarning("ActionsByStatusIdForGroupAsync called with invalid statusId: {StatusId}", statusId);
			throw new BadRequestException("Invalid request!");
		}

		_logger.LogInformation("Fetching actions for status ID: {StatusId}, Time: {Time}", statusId, DateTime.UtcNow);

		//IEnumerable<WfAction> wfActions = await _repository.WfStates.ActionsByStatusIdForGroup(statusId, trackChanges, cancellationToken);
		IEnumerable<WfAction> wfActions = await _repository.WfStates.WfActionsByStatusIdAsync(statusId, trackChanges, cancellationToken);

		if (!wfActions.Any())
		{
			_logger.LogWarning("No actions found for status ID: {StatusId}, Time: {Time}", statusId, DateTime.UtcNow);
			return Enumerable.Empty<WfActionDto>();
		}

		IEnumerable<WfActionDto> wfActionsDto = MyMapper.JsonCloneIEnumerableToList<WfAction, WfActionDto>(wfActions);

		_logger.LogInformation(
						"Actions fetched successfully. StatusId: {StatusId}, Count: {Count}, Time: {Time}",
						statusId,
						wfActionsDto.Count(),
						DateTime.UtcNow);

		return wfActionsDto;
	}

	/// <summary>
	/// Retrieves all workflow state records from the database.
	/// </summary>
	public async Task<IEnumerable<WfStateDto>> StatusesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all workflow states. Time: {Time}", DateTime.UtcNow);

		//IEnumerable<WfState> wfStates = await _repository.WfStates.StatusesAsync(trackChanges, cancellationToken);
		IEnumerable<WfState> wfStates = await _repository.WfStates.StatusesAsync(trackChanges, cancellationToken);

		if (!wfStates.Any())
		{
			_logger.LogWarning("No workflow states found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<WfStateDto>();
		}

		IEnumerable<WfStateDto> wfStatesDto = MyMapper.JsonCloneIEnumerableToIEnumerable<WfState, WfStateDto>(wfStates);

		_logger.LogInformation(
						"Workflow states fetched successfully. Count: {Count}, Time: {Time}",
						wfStatesDto.Count(),
						DateTime.UtcNow);

		return wfStatesDto;
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all workflow states with menu and module information.
	/// </summary>
	public async Task<GridEntity<WfStateDto>> WorkflowSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default)
	{
		const string query =
						@"SELECT 
                    WFSTATE.WfStateId,
                    WFSTATE.StateName,
                    WFSTATE.MenuId,
                    WFSTATE.IsDefaultStart,
                    WFSTATE.IsClosed,
                    MENU.ModuleId,
                    MENU.MenuName,
                    MODULE.ModuleName,
                    [Sequence] AS Sequence,
                    WFSTATE.IsClosed AS ClosingStateId,
                    CASE WFSTATE.IsClosed
                        WHEN 0 THEN 'Select Closing Status'
                        WHEN 1 THEN 'Open'
                        WHEN 2 THEN 'Possible Close'
                        WHEN 3 THEN 'Close'
                        WHEN 4 THEN 'Destroyed'
                        WHEN 5 THEN 'Draft'
                        WHEN 6 THEN 'Deligated'
                        WHEN 7 THEN 'Published'
                        WHEN 8 THEN 'Extended'
                        ELSE 'Unknown'
                    END AS ClosingStateName
                FROM WFSTATE
                INNER JOIN MENU ON WFSTATE.MenuId = MENU.MenuId
                INNER JOIN MODULE ON MENU.ModuleId = MODULE.ModuleId";

		const string orderBy = "MenuId, ModuleID, Sequence ASC";

		_logger.LogInformation("Fetching workflow summary grid. Time: {Time}", DateTime.UtcNow);

		//return await _repository.Workflows.GridData<WfStateDto>(query, options, orderBy, "", cancellationToken);
		return await _repository.Workflows.AdoGridDataAsync<WfStateDto>(query, options, orderBy, "", cancellationToken);

	}

	/// <summary>
	/// Creates a new workflow action record after validating for null input and duplicate action name.
	/// </summary>
	public async Task<WfActionDto> CreateWfActionAsync(WfActionDto entityForCreate, UsersDto currentUser, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(WfActionDto));

		if (entityForCreate.WfActionId != 0)
			throw new InvalidCreateOperationException("WfActionId must be 0 for creating a new workflow action.");

		bool actionExists = await _repository.WfActions.ExistsAsync(
						x => x.ActionName != null
								&& x.ActionName.Trim().ToLower() == entityForCreate.ActionName!.Trim().ToLower()
								&& x.WfStateId == entityForCreate.WfStateId,
						cancellationToken: cancellationToken);

		if (actionExists)
			throw new DuplicateRecordException("Workflow", "ActionName");

		WfAction wfActionEntity = MyMapper.JsonClone<WfActionDto, WfAction>(entityForCreate);

		await _repository.WfActions.CreateAsync(wfActionEntity, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("Workflow action could not be saved to the database.");

		_logger.LogInformation(
						"Workflow action created successfully. ID: {WfActionId}, Name: {ActionName}, UserId: {UserId}, Time: {Time}",
						wfActionEntity.WfActionId,
						wfActionEntity.ActionName,
						currentUser?.UserId ?? 0,
						DateTime.UtcNow);

		return MyMapper.JsonClone<WfAction, WfActionDto>(wfActionEntity);
	}

	/// <summary>
	/// Updates an existing workflow action record by merging only the changed values from the provided DTO.
	/// </summary>
	public async Task<WfActionDto> UpdateWfActionAsync(int wfActionId, WfActionDto modelDto, UsersDto currentUser, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(WfActionDto));

		if (wfActionId != modelDto.WfActionId)
			throw new BadRequestException(wfActionId.ToString(), nameof(WfActionDto));

		WfAction existingEntity = await _repository.WfActions
						.FirstOrDefaultAsync(x => x.WfActionId == wfActionId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("Status", "ActionId", wfActionId.ToString());

		WfAction updatedEntity = MyMapper.MergeChangedValues<WfAction, WfActionDto>(existingEntity, modelDto);
		_repository.WfActions.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("Status", "ActionId", wfActionId.ToString());

		_logger.LogInformation(
						"Workflow action updated. ID: {WfActionId}, Name: {ActionName}, UserId: {UserId}, Time: {Time}",
						updatedEntity.WfActionId,
						updatedEntity.ActionName,
						currentUser?.UserId ?? 0,
						DateTime.UtcNow);

		return MyMapper.JsonClone<WfAction, WfActionDto>(updatedEntity);
	}

	/// <summary>
	/// Deletes a workflow action record identified by the given ID.
	/// </summary>
	public async Task<int> DeleteWfActionAsync(int wfActionId, WfActionDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(WfActionDto));

		if (wfActionId != modelDto.WfActionId)
			throw new BadRequestException(wfActionId.ToString(), nameof(WfActionDto));

		WfAction wfActionEntity = await _repository.WfActions
						.FirstOrDefaultAsync(x => x.WfActionId == wfActionId, trackChanges, cancellationToken)
						?? throw new NotFoundException("WfAction", "ActionId", wfActionId.ToString());

		await _repository.WfActions.DeleteAsync(x => x.WfActionId == wfActionId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("WfAction", "ActionId", wfActionId.ToString());

		_logger.LogWarning(
						"Workflow action deleted. ID: {WfActionId}, Name: {ActionName}, Time: {Time}",
						wfActionEntity.WfActionId,
						wfActionEntity.ActionName,
						DateTime.UtcNow);

		return affected;
	}

	/// <summary>
	/// Retrieves a paginated grid of workflow actions by the specified status ID.
	/// </summary>
	public async Task<GridEntity<WfActionDto>> ActionsGridByStatusIdAsync(int stateId, GridOptions options, CancellationToken cancellationToken = default)
	{
		const string query =
						@"SELECT
                    WFActionId, 
                    WFAction.WFStateId, 
                    WfState.StateName, 
                    ActionName, 
                    NextStateId,
                    (SELECT StateName FROM WFState WHERE WfStateId = NextStateId) AS NextStateName,
                    EMAIL_ALERT, 
                    SMS_ALERT, 
                    AcSortOrder, 
                    MenuId, 
                    IsDefaultStart, 
                    IsClosed, 
                    sequence AS Sequence
                FROM WFAction 
                INNER JOIN WfState ON WfState.WFStateId = WfAction.WFStateId
                WHERE WFAction.WfStateId = {0}";

		const string orderBy = "AcSortOrder ASC";

		string formattedQuery = string.Format(query, stateId);

		_logger.LogInformation("Fetching actions grid for status ID: {StatusId}, Time: {Time}", stateId, DateTime.UtcNow);

		//return await _repository.WfActions.GridData<WfActionDto>(formattedQuery, options, orderBy, "", cancellationToken);
		return await _repository.WfActions.AdoGridDataAsync<WfActionDto>(formattedQuery, options, orderBy, "", cancellationToken);
	}

	/// <summary>
	/// Retrieves next workflow states by the specified menu ID.
	/// </summary>
	public async Task<IEnumerable<WfStateDto>> NextStatesByMenuIdAsync(int menuId, CancellationToken cancellationToken = default)
	{
		if (menuId <= 0)
		{
			_logger.LogWarning("NextStatesByMenuIdAsync called with invalid menuId: {MenuId}", menuId);
			throw new BadRequestException("Invalid request!");
		}

		_logger.LogInformation("Fetching next states for menu ID: {MenuId}, Time: {Time}", menuId, DateTime.UtcNow);

		IEnumerable<WfState> wfStates = await _repository.WfStates
						.ListByWhereWithSelectAsync(
										selector: x => new WfState
										{
											WfStateId = x.WfStateId,
											StateName = x.StateName
										},
										expression: x => x.MenuId == menuId,
										orderBy: x => x.Sequence,
										trackChanges: false,
										cancellationToken: cancellationToken);

		if (!wfStates.Any())
		{
			_logger.LogWarning("No next states found for menu ID: {MenuId}, Time: {Time}", menuId, DateTime.UtcNow);
			return Enumerable.Empty<WfStateDto>();
		}

		IEnumerable<WfStateDto> wfStatesDto = MyMapper.JsonCloneIEnumerableToIEnumerable<WfState, WfStateDto>(wfStates);

		_logger.LogInformation(
						"Next states fetched successfully. MenuId: {MenuId}, Count: {Count}, Time: {Time}",
						menuId,
						wfStatesDto.Count(),
						DateTime.UtcNow);

		return wfStatesDto;
	}

	/// <summary>
	/// Retrieves workflow states by user permission for the specified menu ID.
	/// </summary>
	public async Task<IEnumerable<WfStateDto>> StatusesByUserPermissionAsync(int menuId, int userId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching workflow states by user permission. MenuId: {MenuId}, UserId: {UserId}, Time: {Time}",
						menuId, userId, DateTime.UtcNow);

		//IEnumerable<WfState> wfStates = await _repository.WfStates.GetWFStateByUserPermission(menuId, userId, trackChanges, cancellationToken);
		IEnumerable<WfState> wfStates = await _repository.WfStates.WfStatesByUserPermissionAsync(menuId, userId, cancellationToken);

		if (!wfStates.Any())
		{
			_logger.LogWarning("No workflow states found for user permission. MenuId: {MenuId}, UserId: {UserId}, Time: {Time}",
							menuId, userId, DateTime.UtcNow);
			return Enumerable.Empty<WfStateDto>();
		}

		IEnumerable<WfStateDto> wfStatesDto = MyMapper.JsonCloneIEnumerableToIEnumerable<WfState, WfStateDto>(wfStates);

		_logger.LogInformation(
						"Workflow states fetched by user permission. MenuId: {MenuId}, UserId: {UserId}, Count: {Count}, Time: {Time}",
						menuId,
						userId,
						wfStatesDto.Count(),
						DateTime.UtcNow);

		return wfStatesDto;
	}

	/// <summary>
	/// Retrieves workflow states by user permission for the specified menu name.
	/// </summary>
	public async Task<IEnumerable<WfStateDto>> StatusesByMenuNameAndUserPermissionAsync(string menuName, int userId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrWhiteSpace(menuName))
		{
			_logger.LogWarning("StatusesByMenuNameAndUserPermissionAsync called with null or whitespace menuName");
			throw new BadRequestException(nameof(menuName));
		}

		_logger.LogInformation("Fetching workflow states by menu name and user permission. MenuName: {MenuName}, UserId: {UserId}, Time: {Time}",
						menuName, userId, DateTime.UtcNow);

		IEnumerable<WfState> wfStates = await _repository.WfStates.WfStatesByMenuAndUserPermissionAsync(menuName, userId, cancellationToken);

		if (!wfStates.Any())
		{
			_logger.LogWarning("No workflow states found for menu name and user permission. MenuName: {MenuName}, UserId: {UserId}, Time: {Time}",
							menuName, userId, DateTime.UtcNow);
			return Enumerable.Empty<WfStateDto>();
		}

		IEnumerable<WfStateDto> wfStatesDto = MyMapper.JsonCloneIEnumerableToIEnumerable<WfState, WfStateDto>(wfStates);

		_logger.LogInformation(
						"Workflow states fetched by menu name and user permission. MenuName: {MenuName}, UserId: {UserId}, Count: {Count}, Time: {Time}",
						menuName,
						userId,
						wfStatesDto.Count(),
						DateTime.UtcNow);

		return wfStatesDto;
	}

	/// <summary>
	/// Saves a workflow state with validation for default start status.
	/// </summary>
	public async Task<string> SaveWorkflowAsync(WfStateDto modelDto, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(WfStateDto));

		_logger.LogInformation("Saving workflow state. WfStateId: {WfStateId}, StateName: {StateName}, Time: {Time}",
						modelDto.WfStateId, modelDto.StateName, DateTime.UtcNow);

		if (modelDto.WfStateId == 0)
		{
			bool isDefaultExist = await _repository.WfStates.ExistsAsync(
							x => x.IsDefaultStart == false
									&& x.MenuId == modelDto.MenuId
									&& x.StateName != null
									&& x.StateName.Trim() == modelDto.StateName.ToString().Trim(),
							cancellationToken: cancellationToken);

			if (!isDefaultExist)
			{
				bool stateNameExists = await _repository.WfStates.ExistsAsync(
								x => x.MenuId == modelDto.MenuId
										&& x.StateName != null
										&& x.StateName.Trim() == modelDto.StateName.ToString().Trim(),
								cancellationToken: cancellationToken);

				if (!stateNameExists)
				{
					WfState wfState = MyMapper.JsonClone<WfStateDto, WfState>(modelDto);
					await _repository.WfStates.CreateAsync(wfState, cancellationToken);
					int affected = await _repository.SaveChangesAsync(cancellationToken);

					if (affected <= 0)
						throw new InvalidOperationException("Workflow state could not be saved to the database.");

					_logger.LogInformation("Workflow state created successfully. ID: {WfStateId}, Time: {Time}",
									wfState.WfStateId, DateTime.UtcNow);

					return OperationMessage.Success;
				}
				else
				{
					_logger.LogWarning("The state name already exists. StateName: {StateName}", modelDto.StateName);
					return "The state name already exist!";
				}
			}
			else
			{
				_logger.LogWarning("The state name already exists or only one StatusId can be IsDefault. StateName: {StateName}", modelDto.StateName);
				return "The state name already exist or Only one StatusId can be IsDefault!";
			}
		}
		else
		{
			bool isDefaultExist = await _repository.WfStates.ExistsAsync(
							x => x.IsDefaultStart == modelDto.IsDefaultStart
									&& x.WfStateId == modelDto.WfStateId,
							cancellationToken: cancellationToken);

			if (!isDefaultExist)
			{
				WfState wfState = MyMapper.JsonClone<WfStateDto, WfState>(modelDto);
				_repository.WfStates.UpdateByState(wfState);
				int affected = await _repository.SaveChangesAsync(cancellationToken);

				if (affected <= 0)
					throw new NotFoundException("WfState", "WfStateId", modelDto.WfStateId.ToString());

				_logger.LogInformation("Workflow state updated successfully. ID: {WfStateId}, Time: {Time}",
								wfState.WfStateId, DateTime.UtcNow);

				return OperationMessage.Success;
			}
			else
			{
				_logger.LogWarning("Only one StatusId can be IsDefault. WfStateId: {WfStateId}", modelDto.WfStateId);
				return "Only one StatusId can be IsDefault!";
			}
		}
	}

	/// <summary>
	/// Creates or updates a workflow action with validation for duplicate action name.
	/// </summary>
	public async Task<string> SaveWfActionAsync(WfActionDto modelDto, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(WfActionDto));

		_logger.LogInformation("Saving workflow action. WfActionId: {WfActionId}, ActionName: {ActionName}, Time: {Time}",
						modelDto.WfActionId, modelDto.ActionName, DateTime.UtcNow);

		if (modelDto.WfActionId == 0)
		{
			bool isActionExistByStateId = await _repository.WfActions.ExistsAsync(
							x => x.WfStateId == modelDto.WfStateId
									&& x.ActionName != null
									&& x.ActionName.ToLower().Trim() == modelDto.ActionName.ToLower().Trim(),
							cancellationToken: cancellationToken);

			if (!isActionExistByStateId)
			{
				WfAction wfAction = MyMapper.JsonClone<WfActionDto, WfAction>(modelDto);
				await _repository.WfActions.CreateAsync(wfAction, cancellationToken);
				int affected = await _repository.SaveChangesAsync(cancellationToken);

				if (affected <= 0)
					throw new InvalidOperationException("Workflow action could not be saved to the database.");

				_logger.LogInformation("Workflow action created successfully. ID: {WfActionId}, Time: {Time}",
								wfAction.WfActionId, DateTime.UtcNow);

				return OperationMessage.Success;
			}
			else
			{
				_logger.LogWarning("The action name already exists for the state. ActionName: {ActionName}", modelDto.ActionName);
				return "The action name already exist for the state!";
			}
		}
		else
		{
			bool isActionExistByStateId = await _repository.WfActions.ExistsAsync(
							x => x.WfStateId == modelDto.WfStateId
									&& x.ActionName == modelDto.ActionName
									&& x.WfActionId != modelDto.WfActionId,
							cancellationToken: cancellationToken);

			if (!isActionExistByStateId)
			{
				WfAction wfAction = MyMapper.JsonClone<WfActionDto, WfAction>(modelDto);
				_repository.WfActions.UpdateByState(wfAction);
				int affected = await _repository.SaveChangesAsync(cancellationToken);

				if (affected <= 0)
					throw new NotFoundException("WfAction", "WfActionId", modelDto.WfActionId.ToString());

				_logger.LogInformation("Workflow action updated successfully. ID: {WfActionId}, Time: {Time}",
								wfAction.WfActionId, DateTime.UtcNow);

				return OperationMessage.Success;
			}
			else
			{
				_logger.LogWarning("The action name already exists for the state. ActionName: {ActionName}", modelDto.ActionName);
				return "The action name already exist for the state!";
			}
		}
	}
}