// Interface: IStatusRepository
using Domain.Entities.Entities.System;
using Domain.Contracts.Repositories;

namespace Domain.Contracts.Core.SystemAdmin;

public interface IStatusRepository : IRepositoryBase<WfState>
{

  Task<IEnumerable<WfState>> StatusesAsync(bool trackChanges, CancellationToken cancellationToken = default);
	/// <summary>
	/// Retrieves workflow states by menu ID asynchronously.
	/// </summary>
	Task<IEnumerable<WfState>> WfStatesByMenuIdAsync(int menuId, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves workflow actions by status ID asynchronously.
  /// </summary>
  Task<IEnumerable<WfAction>> WfActionsByStatusIdAsync(int statusId, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves workflow states by user permission asynchronously.
  /// </summary>
  Task<IEnumerable<WfState>> WfStatesByUserPermissionAsync(int menuId, int userId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves workflow states by menu name and user permission asynchronously.
  /// </summary>
  Task<IEnumerable<WfState>> WfStatesByMenuAndUserPermissionAsync(string menuName, int userId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Creates a new workflow state.
  /// </summary>
  Task<WfState> CreateWfStateAsync(WfState wfState, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an existing workflow state.
  /// </summary>
  void UpdateWfState(WfState wfState) => UpdateByState(wfState);

  /// <summary>
  /// Deletes a workflow state.
  /// </summary>
  Task DeleteWfStateAsync(WfState wfState, bool trackChanges, CancellationToken cancellationToken = default);
}




//using Domain.Entities.Entities;
//using Domain.Entities.Entities.System;
//using Domain.Contracts.Repositories;

//namespace bdDevCRM.RepositoriesContracts.Core.SystemAdmin;

//  public interface IStatusRepository : IRepositoryBase<WfState>
//  {
//  Task<IEnumerable<WfState>> StatusByMenuId(int menuId, bool trackChanges);
//  Task<IEnumerable<WfAction>> ActionsByStatusIdForGroup(int statusId, bool trackChanges);

//  Task<IEnumerable<WfState>> WFStateByUserPermission(int menuId, int userId);

//  Task<IEnumerable<WfState>> WFStateByMenuNUserPermission(string menuName, int userId);
//}
