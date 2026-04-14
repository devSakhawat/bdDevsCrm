using Domain.Entities.Entities.System;
using bdDevCRM.RepositoriesContracts.Core.SystemAdmin;
using bdDevCRM.s.Core.SystemAdmin;
using bdDevCRM.Sql.Context;

namespace Infrastructure.Repositories.Core.SystemAdmin;

/// <summary>
/// Repository for workflow state data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class StatusRepository : RepositoryBase<WfState>, IStatusRepository
{
	public StatusRepository(CRMContext context) : base(context) { }

	public async Task<IEnumerable<WfState>> StatusesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(x => x.MenuId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves workflow states by menu ID asynchronously.
	/// </summary>
	public async Task<IEnumerable<WfState>> WfStatesByMenuIdAsync(int menuId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		string query = $@"SELECT WFState.*, Menu.MenuName 
            FROM WFState 
            INNER JOIN Menu ON Menu.MenuID = WFState.MenuId
            WHERE WFState.MenuId = {menuId}
            ORDER BY WFState.Sequence";

		return await AdoExecuteListQueryAsync<WfState>(query, null, cancellationToken);
	}

	/// <summary>
	/// Retrieves workflow actions by status ID asynchronously.
	/// </summary>
	public async Task<IEnumerable<WfAction>> WfActionsByStatusIdAsync(int statusId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		string query = $@"SELECT *, 
            (SELECT StateName FROM WFState WHERE WfStateId = WFAction.NextStateId) AS NextStateName 
            FROM WFAction 
            WHERE WfStateId = {statusId} 
            ORDER BY AcSortOrder";

		return await AdoExecuteListQueryAsync<WfAction>(query, null, cancellationToken);
	}

	/// <summary>
	/// Retrieves workflow states by user permission asynchronously.
	/// </summary>
	public async Task<IEnumerable<WfState>> WfStatesByUserPermissionAsync(int menuId, int userId, CancellationToken cancellationToken = default)
	{
		string query = $@"SELECT DISTINCT * FROM WFState 
            WHERE WfStateId IN (
                SELECT ReferenceId FROM GroupPermission 
                WHERE PermissionTableName = 'Status' 
                AND GroupId IN (SELECT GroupId FROM GroupMember WHERE UserId = {userId})
            ) AND WFState.MenuId = {menuId} 
            ORDER BY Sequence";

		return await AdoExecuteListQueryAsync<WfState>(query, null, cancellationToken);
	}

	/// <summary>
	/// Retrieves workflow states by menu name and user permission asynchronously.
	/// </summary>
	public async Task<IEnumerable<WfState>> WfStatesByMenuAndUserPermissionAsync(string menuName, int userId, CancellationToken cancellationToken = default)
	{
		string query = $@"SELECT DISTINCT ws.*
            FROM WFState ws
            INNER JOIN Menu m ON ws.MenuId = m.MenuId
            WHERE ws.WfStateId IN (
                SELECT ReferenceId FROM GroupPermission 
                WHERE PermissionTableName = 'Status' 
                AND GroupId IN (SELECT GroupId FROM GroupMember WHERE UserId = {userId})
            ) AND m.MenuName = '{menuName}'
            ORDER BY ws.Sequence";

		return await AdoExecuteListQueryAsync<WfState>(query, null, cancellationToken);
	}

	/// <summary>
	/// Creates a new workflow state.
	/// </summary>
	public async Task<WfState> CreateWfStateAsync(WfState wfState, CancellationToken cancellationToken = default)
	{
		int wfStateId = await CreateAndIdAsync(wfState, cancellationToken);
		wfState.WfStateId = wfStateId;
		return wfState;
	}

	/// <summary>
	/// Updates an existing workflow state.
	/// </summary>
	public void UpdateWfState(WfState wfState) => UpdateByState(wfState);

	/// <summary>
	/// Deletes a workflow state.
	/// </summary>
	public async Task DeleteWfStateAsync(WfState wfState, bool trackChanges, CancellationToken cancellationToken = default)
		=> await DeleteAsync(x => x.WfStateId == wfState.WfStateId, trackChanges, cancellationToken);
}
