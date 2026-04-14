// Class: MenuRepository
using Domain.Entities.Entities.System;
using bdDevCRM.RepositoriesContracts.Core.SystemAdmin;
using bdDevCRM.s.Core.SystemAdmin;
using bdDevCRM.Sql.Context;

namespace Infrastructure.Repositories.Core.SystemAdmin
{
	public class MenuRepository : RepositoryBase<Menu>, IMenuRepository
	{
		public MenuRepository(CRMContext context) : base(context) { }

		/// <summary>
		/// Retrieves menus by module ID using raw SQL.
		/// </summary>
		public async Task<IEnumerable<Menu>> MenusByModuleIdAsync(int moduleId, bool trackChanges, CancellationToken cancellationToken = default)
		{
			string query = string.Format("Select * from Menu where ModuleId = {0} order by SorOrder, menuName asc", moduleId);
			return await AdoExecuteListQueryAsync<Menu>(query, null, cancellationToken);
		}

		/// <summary>
		/// Retrieves menus accessible by a user based on permissions.
		/// </summary>
		public async Task<IEnumerable<Menu>> MenusByUserPermissionAsync(int userId, bool trackChanges, CancellationToken cancellationToken = default)
		{
			string query = string.Format(@"
                SELECT DISTINCT Menu.MenuId, Menu.ModuleId, GroupMember.UserId, GroupPermission.PermissionTableName, Menu.MenuName, Menu.MenuPath, Menu.ParentMenu, SORORDER, ToDo 
                FROM GroupMember 
                INNER JOIN Groups ON GroupMember.GroupId = Groups.GroupId 
                INNER JOIN GroupPermission ON Groups.GroupId = GroupPermission.GroupId 
                INNER JOIN Menu ON GroupPermission.ReferenceID = Menu.MenuId 
                WHERE (GroupMember.UserId = {0}) AND (GroupPermission.PermissionTableName = 'Menu') 
                ORDER BY Sororder, Menu.MenuName", userId);

			return await AdoExecuteListQueryAsync<Menu>(query, null, cancellationToken);
		}

		/// <summary>
		/// Retrieves parent menus by menu ID.
		/// </summary>
		public async Task<List<Menu>> ParentMenusByMenuAsync(int parentMenuId, bool trackChanges, CancellationToken cancellationToken = default)
		{
			string query = $"SELECT * FROM Menu WHERE MenuID = {parentMenuId}";
			IEnumerable<Menu> result = await AdoExecuteListQueryAsync<Menu>(query, null, cancellationToken);
			return result.ToList();
		}

		/// <summary>
		/// Retrieves a summary of menus.
		/// </summary>
		public async Task<List<Menu>> MenusSummaryAsync(bool trackChanges, CancellationToken cancellationToken = default)
		{
			string query = @"Select MenuId, Menu.ModuleId, MenuName, MenuPath, ISNULL(ParentMenu, 0) as ParentMenu, ModuleName, ToDo, SORORDER 
                              ,(Select MenuName from Menu mn where mn.MenuId = menu.ParentMenu) as ParentMenuName 
                              from Menu 
                              left outer join Module on module.ModuleId = menu.ModuleId
                              order by ModuleName asc, ParentMenu asc, MenuName";

			IEnumerable<Menu> result = await AdoExecuteListQueryAsync<Menu>(query, null, cancellationToken);
			return result.ToList();
		}

		/// <summary>
		/// s all menus using EF Core.
		/// </summary>
		public async Task<IEnumerable<Menu>> MenusAsync(bool trackChanges, CancellationToken cancellationToken = default)
		{
			return await ListAsync(x => x.MenuName, trackChanges, cancellationToken);
		}

		/// <summary>
		/// s menus by a collection of IDs.
		/// </summary>
		public IEnumerable<Menu> MenusByIds(IEnumerable<int> ids, bool trackChanges)
				=> ListByIds(c => ids.Contains(c.MenuId), trackChanges);

		/// <summary>
		/// s menus by a collection of IDs.
		/// </summary>
		public async Task<IEnumerable<Menu>> MenusByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellation)
				=> await ListByIdsAsync(c => ids.Contains(c.MenuId), trackChanges, cancellation);

		/// <summary>
		/// s a single menu by ID (Synchronous).
		/// </summary>
		public Menu? MenuById(int menuId, bool trackChanges) => FirstOrDefault(c => c.MenuId.Equals(menuId), trackChanges);

		/// <summary>
		/// s a single menu by ID (Asynchronous).
		/// </summary>
		public async Task<Menu?> MenuAsync(int menuId, bool trackChanges, CancellationToken cancellationToken = default)
				=> await FirstOrDefaultAsync(c => c.MenuId.Equals(menuId), trackChanges, cancellationToken);

		/// <summary>
		/// s a menu by ID with an additional condition.
		/// </summary>
		public async Task<Menu?> MenuWithAdditionalConditionAsync(int menuId, string additionalCondition, CancellationToken cancellationToken = default)
		{
			var query = string.Format("Select * from Menu where MenuId = {0} {1}", menuId, additionalCondition);
			return await AdoExecuteSingleDataAsync<Menu>(query, null, cancellationToken);
		}

		/// <summary>
		/// s menus by Module ID using EF Core.
		/// </summary>
		public async Task<IEnumerable<Menu>> ModuleMenusAsync(int moduleId, bool trackChanges, CancellationToken cancellationToken = default)
				=> await ListByConditionAsync(c => c.ModuleId.Equals(moduleId), orderBy: x => x.ModuleId, trackChanges: trackChanges, cancellationToken: cancellationToken);

		/// <summary>
		/// Creates a new menu.
		/// </summary>
		public async Task<Menu> CreateMenuAsync(Menu menu, CancellationToken cancellationToken = default)
		{
			int menuId = await CreateAndIdAsync(menu, cancellationToken);
			menu.MenuId = menuId;
			return menu;
		}

		/// <summary>
		/// Updates an existing menu.
		/// </summary>
		public void UpdateMenu(Menu menu) => UpdateByState(menu);

		/// <summary>
		/// Deletes a menu.
		/// </summary>
		public async Task DeleteMenuAsync(Menu menu, bool trackChanges, CancellationToken cancellationToken = default)
				=> await DeleteAsync(x => x.MenuId == menu.MenuId, trackChanges, cancellationToken);
	}
}


//using bdDevCRM.Entities.Entities;
//using Domain.Entities.Entities.System;
//using bdDevCRM.s.Core;
//using bdDevCRM.s.Core.SystemAdmin;

//namespace bdDevCRM.RepositoriesContracts.Core.SystemAdmin;

//public interface IMenuRepository : IRepositoryBase<Menu>
//{
//  Task<IEnumerable<Menu>> SelectAllMenuByModuleId(int moduleId, bool trackChanges);
//  Task<IEnumerable<Menu>> SelectMenuByUserPermission(int userId, bool trackChanges);
//  Task<List<Menu>> ParentMenuByMenu(int parentMenuId, bool trackChanges);
//  Task<List<Menu>> MenuSummary(bool trackChanges);



//  Task<IEnumerable<Menu>> Menus(bool trackChanges);

//  Menu? Menu(int MenuId, bool trackChanges);
//  Task<Menu> MenuAsync(int MenuId, bool trackChanges);
//  void CreateMenu(Menu Menu);

//  IEnumerable<Menu> ByIds(IEnumerable<int> ids, bool trackChanges);


//  Task<IEnumerable<Menu>> MenusAsync(bool trackChanges);
//  Task<Menu?> MenuByMenuIdWithAdditionalCondition(int MenuId, string additionalCondition);
//  Task<IEnumerable<Menu>> MenusByModuleId(int moduleId, bool trackChanges);
//  void UpdateMenu(Menu Menu);
//  void DeleteMenu(Menu Menu);

//}
