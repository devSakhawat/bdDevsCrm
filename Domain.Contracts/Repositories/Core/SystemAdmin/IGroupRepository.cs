// Interface: IGroupRepository
using Domain.Entities.Entities.System;
using Domain.Contracts.Repositories;
using Domain.Contracts.Repositories;

namespace Domain.Contracts.Core.SystemAdmin
{
	public interface IGroupRepository : IRepositoryBase<Groups>
	{
		Task<Menu> CheckMenuPermission(string rawUrl, Users objUser, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves group permissions by group ID asynchronously.
		/// </summary>
		Task<IEnumerable<GroupPermission>> GroupPermissionsByGroupIdAsync(int groupId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves all access controls asynchronously.
		/// </summary>
		Task<IEnumerable<AccessControl>> AccessControlsAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Checks menu permission for a specific user and URL asynchronously.
		/// </summary>
		Task<Menu> CheckMenuPermissionAsync(string rawUrl, Users objUser, CancellationToken cancellationToken = default);

		/// <summary>
		/// Creates a new group.
		/// </summary>
		Task<Groups> CreateGroupAsync(Groups group, CancellationToken cancellationToken = default);

		/// <summary>
		/// Updates an existing group.
		/// </summary>
		void UpdateGroup(Groups group);

		/// <summary>
		/// Deletes a group.
		/// </summary>
		Task DeleteGroupAsync(Groups group, bool trackChanges, CancellationToken cancellationToken = default);
	}
}





//using Domain.Entities.Entities;
//using Domain.Entities.Entities.System;
//using bdDevs.Shared.DataTransferObjects.Core;
//using Domain.Contracts.Repositories;

//namespace Domain.Contracts.Core.SystemAdmin;

//public interface IGroupRepository : IRepositoryBase<Groups>
//{
//  Task<List<Groups>> GroupSummary(bool trackChanges);
//  Task<IEnumerable<GroupPermission>> GroupPermisionsbyGroupId(int groupId);
//  Task<IEnumerable<AccessControl>> Accesses();

//  Task<Menu> CheckMenuPermission(string rawUrl, Users objUser);

//  Task<IEnumerable<GroupPermission>> AccessPermisionForCurrentUser(int moduleId, int userId);
//  //Task<IEnumerable<Menu>> Menus(bool trackChanges);

//  //Menu? Menu(int MenuId, bool trackChanges);
//  //Task<Menu> MenuAsync(int MenuId, bool trackChanges);
//  //void CreateMenu(Menu Menu);

//  //IEnumerable<Menu> ByIds(IEnumerable<int> ids, bool trackChanges);


//  //Task<IEnumerable<Menu>> MenusAsync(bool trackChanges);
//  //Task<Menu?> MenuByMenuIdWithAdditionalCondition(int MenuId, string additionalCondition);
//  //Task<IEnumerable<Menu>> MenusByModuleId(int moduleId, bool trackChanges);
//  //void UpdateMenu(Menu Menu);
//  //void DeleteMenu(Menu Menu);
//}
