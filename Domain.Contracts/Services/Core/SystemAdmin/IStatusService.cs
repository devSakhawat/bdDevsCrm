using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

/// <summary>
/// Service contract for workflow status and state management operations.
/// Defines methods for creating, updating, deleting, and retrieving workflow state and action data.
/// </summary>
public interface IStatusService
{
	/// <summary>
	/// Creates a new workflow state record after validating for null input and duplicate state name.
	/// </summary>
	/// <param name="entityForCreate">The DTO containing data for the new workflow state.</param>
	/// <param name="currentUser">The DTO containing current user information.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The created <see cref="WfStateDto"/> with the newly assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
	/// <exception cref="InvalidCreateOperationException">Thrown when WfStateId is not 0 for new creation.</exception>
	/// <exception cref="DuplicateRecordException">Thrown when a workflow state with the same name already exists for the menu.</exception>
	Task<WfStateDto> CreateStatusAsync(WfStateDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing workflow state record by merging only the changed values from the provided DTO.
	/// Validates ID consistency, null input, record existence, and duplicate name constraints.
	/// </summary>
	/// <param name="wfStateId">The ID of the workflow state to update.</param>
	/// <param name="modelDto">The DTO containing updated field values.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="currentUser">The DTO containing current user information.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="WfStateDto"/> reflecting the saved state.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no workflow state is found for the given ID.</exception>
	/// <exception cref="DuplicateRecordException">Thrown when another workflow state with the same name already exists.</exception>
	Task<WfStateDto> UpdateStatusAsync(int wfStateId, WfStateDto modelDto, bool trackChanges, UsersDto currentUser, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes a workflow state record identified by the given ID.
	/// Validates that the ID is positive, record exists, and no associated actions exist before deletion.
	/// </summary>
	/// <param name="wfStateId">The ID of the workflow state to delete.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The number of affected rows.</returns>
	/// <exception cref="IdParametersBadRequestException">Thrown when <paramref name="wfStateId"/> is zero or negative.</exception>
	/// <exception cref="NotFoundException">Thrown when no workflow state record is found for the given ID.</exception>
	/// <exception cref="GenericConflictException">Thrown when action data exists for this status.</exception>
	Task<int> DeleteStatusAsync(int wfStateId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single workflow state record by its ID.
	/// </summary>
	/// <param name="id">The ID of the workflow state to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="WfStateDto"/> matching the specified ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no workflow state is found for the given ID.</exception>
	Task<WfStateDto> StatusAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves workflow statuses by the specified menu ID.
	/// </summary>
	/// <param name="menuId">The ID of the menu whose statuses are to be retrieved.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="WfStateDto"/> belonging to the specified menu.</returns>
	/// <exception cref="IdParametersBadRequestException">Thrown when <paramref name="menuId"/> is zero or negative.</exception>
	Task<IEnumerable<WfStateDto>> StatusesByMenuIdAsync(int menuId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves workflow actions by the specified status ID for group.
	/// </summary>
	/// <param name="statusId">The ID of the status whose actions are to be retrieved.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="WfActionDto"/> belonging to the specified status.</returns>
	/// <exception cref="IdParametersBadRequestException">Thrown when <paramref name="statusId"/> is zero or negative.</exception>
	Task<IEnumerable<WfActionDto>> ActionsByStatusIdForGroupAsync(int statusId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves all workflow state records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of all <see cref="WfStateDto"/> records.</returns>
	Task<IEnumerable<WfStateDto>> StatusesAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated summary grid of all workflow states with menu and module information.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{WfStateDto}"/> containing the paged workflow summary data.</returns>
	Task<GridEntity<WfStateDto>> WorkflowSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default);

	/// <summary>
	/// Creates a new workflow action record after validating for null input and duplicate action name.
	/// </summary>
	/// <param name="entityForCreate">The DTO containing data for the new workflow action.</param>
	/// <param name="currentUser">The DTO containing current user information.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The created <see cref="WfActionDto"/> with the newly assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
	/// <exception cref="InvalidCreateOperationException">Thrown when WfActionId is not 0 for new creation.</exception>
	/// <exception cref="DuplicateRecordException">Thrown when a workflow action with the same name already exists for the state.</exception>
	Task<WfActionDto> CreateWfActionAsync(WfActionDto entityForCreate, UsersDto currentUser, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing workflow action record by merging only the changed values from the provided DTO.
	/// </summary>
	/// <param name="wfActionId">The ID of the workflow action to update.</param>
	/// <param name="modelDto">The DTO containing updated field values.</param>
	/// <param name="currentUser">The DTO containing current user information.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="WfActionDto"/> reflecting the saved state.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no workflow action is found for the given ID.</exception>
	Task<WfActionDto> UpdateWfActionAsync(int wfActionId, WfActionDto modelDto, UsersDto currentUser, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes a workflow action record identified by the given ID.
	/// </summary>
	/// <param name="wfActionId">The ID of the workflow action to delete.</param>
	/// <param name="modelDto">The DTO containing workflow action data for validation.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The number of affected rows.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no workflow action record is found for the given ID.</exception>
	Task<int> DeleteWfActionAsync(int wfActionId, WfActionDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated grid of workflow actions by the specified status ID.
	/// </summary>
	/// <param name="stateId">The ID of the status whose actions are to be retrieved.</param>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{WfActionDto}"/> containing the paged action data.</returns>
	Task<GridEntity<WfActionDto>> ActionsGridByStatusIdAsync(int stateId, GridOptions options, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves next workflow states by the specified menu ID.
	/// </summary>
	/// <param name="menuId">The ID of the menu whose next states are to be retrieved.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="WfStateDto"/> representing next states.</returns>
	/// <exception cref="IdParametersBadRequestException">Thrown when <paramref name="menuId"/> is zero or negative.</exception>
	Task<IEnumerable<WfStateDto>> NextStatesByMenuIdAsync(int menuId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves workflow states by user permission for the specified menu ID.
	/// </summary>
	/// <param name="menuId">The ID of the menu.</param>
	/// <param name="userId">The ID of the user.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="WfStateDto"/> the user has permission to access.</returns>
	Task<IEnumerable<WfStateDto>> StatusesByUserPermissionAsync(int menuId, int userId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves workflow states by user permission for the specified menu name.
	/// </summary>
	/// <param name="menuName">The name of the menu.</param>
	/// <param name="userId">The ID of the user.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="WfStateDto"/> the user has permission to access.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="menuName"/> is null or whitespace.</exception>
	Task<IEnumerable<WfStateDto>> StatusesByMenuNameAndUserPermissionAsync(string menuName, int userId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Saves a workflow state with validation for default start status.
	/// </summary>
	/// <param name="modelDto">The DTO containing workflow state data.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A message indicating the result of the operation.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="DuplicateRecordException">Thrown when a workflow state with the same name already exists.</exception>
	Task<string> SaveWorkflowAsync(WfStateDto modelDto, CancellationToken cancellationToken = default);

	/// <summary>
	/// Creates or updates a workflow action with validation for duplicate action name.
	/// </summary>
	/// <param name="modelDto">The DTO containing workflow action data.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A message indicating the result of the operation.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="DuplicateRecordException">Thrown when an action with the same name already exists for the state.</exception>
	Task<string> SaveWfActionAsync(WfActionDto modelDto, CancellationToken cancellationToken = default);
}



//using Application.Shared.Grid;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using System.Threading.Tasks;

//namespace Application.Services.Core.SystemAdmin;

//public interface IStatusService
//{
//  Task<IEnumerable<WfStateDto>> StatusByMenuId(int menuId, bool trackChanges);
//  Task<IEnumerable<WfActionDto>> ActionsByStatusIdForGroup(int statusId, bool trackChanges);
//  Task<GridEntity<WfStateDto>> WorkflowSummary(bool trackChanges, GridOptions options);
//  Task<WfStateDto> CreateNewRecordAsync(WfStateDto modelDto, UsersDto currentUser);
//  Task<string> UpdateRecordAsync(int key, WfStateDto modelDto, bool trackChanges, UsersDto currentUser);
//  Task<WfActionDto> CreateWfActionNewRecordAsync(WfActionDto modelDto, UsersDto currentUser, bool trackChanges);
//  Task<string> UpdateWfActionRecordAsync(int key, WfActionDto modelDto, UsersDto currentUser, bool trackChanges = false);

//  Task<string> SaveWorkflow(WfStateDto modelDto);
//  Task<string> CreateActionAsync(WfActionDto modelDto);
//  Task<string> DeleteAction(int key, WfActionDto modelDto);
//  Task<string> DeleteWorkflow(int key);

//  Task<IEnumerable<WfStateDto>> NextStatesByMenu(int menuId);

//  Task<GridEntity<WfActionDto>> ActionByStatusId(int stateId, GridOptions options);

//  Task<IEnumerable<WfStateDto>> WFStateByUserPermission(int menuId, int userId);

//  Task<IEnumerable<WfStateDto>> WFStateByMenuNUserPermission(string menuName, int userId);
//}
