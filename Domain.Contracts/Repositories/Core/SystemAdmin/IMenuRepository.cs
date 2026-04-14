

// Interface: IMenuRepository
using Domain.Entities.Entities.System;
using Domain.Contracts.Repositories;

namespace Domain.Contracts.Core.SystemAdmin
{
  public interface IMenuRepository : IRepositoryBase<Menu>
  {
    Task<IEnumerable<Menu>> MenusByModuleIdAsync(int moduleId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<Menu>> MenusByUserPermissionAsync(int userId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<List<Menu>> ParentMenusByMenuAsync(int parentMenuId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<List<Menu>> MenusSummaryAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<Menu>> MenusAsync(bool trackChanges, CancellationToken cancellationToken = default);
    IEnumerable<Menu> MenusByIds(IEnumerable<int> ids, bool trackChanges);
    Task<IEnumerable<Menu>> MenusByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellation);
    Menu? MenuById(int menuId, bool trackChanges);
    Task<Menu?> MenuAsync(int menuId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<Menu?> MenuWithAdditionalConditionAsync(int menuId, string additionalCondition, CancellationToken cancellationToken = default);
    Task<IEnumerable<Menu>> ModuleMenusAsync(int moduleId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<Menu> CreateMenuAsync(Menu menu, CancellationToken cancellationToken = default);
		void UpdateMenu(Menu menu);
    Task DeleteMenuAsync(Menu menu, bool trackChanges, CancellationToken cancellationToken = default);
  }
}


//using Domain.Entities.Entities.System;
//using Domain.Contracts.Core.SystemAdmin;
//using Domain.Contracts.Repositories;
//using Infrastructure.Sql.Context;
//using Microsoft.EntityFrameworkCore;

//namespace Infrastructure.Repositories.Core.SystemAdmin;

//public class MenuRepository : RepositoryBase<Menu>, IMenuRepository
//{
//	public MenuRepository(CRMContext context) : base(context) { }

//	private const string SELECT_ALL_MENU_BY_MODULEID_QUERY = "Select * from Menu where ModuleId = {0} order by SorOrder,menuName asc";

//	private const string SELECT_MENU_BY_USERS_PERMISSION_QUERY =
//						"SELECT DISTINCT Menu.MenuId,Menu.ModuleId, GroupMember.UserId, GroupPermission.PermissionTableName, Menu.MenuName, Menu.MenuPath, Menu.ParentMenu,SORORDER,ToDo FROM GroupMember INNER JOIN Groups ON GroupMember.GroupId = Groups.GroupId INNER JOIN GroupPermission ON Groups.GroupId = GroupPermission.GroupId INNER JOIN Menu ON GroupPermission.ReferenceID = Menu.MenuId WHERE (GroupMember.UserId ={0}) AND (GroupPermission.PermissionTableName = 'Menu') order by Sororder, Menu.MenuName";

//	public async Task<IEnumerable<Menu>> SelectAllMenuByModuleId(int moduleId, bool trackChanges)
//	{
//		//string query = string.Format(SELECT_ALL_MENU_BY_MODULEID_QUERY, moduleId);
//		//IEnumerable<Menu> menu = await ListOfDataByQuery<Menu>(query);

//		string query = string.Format(SELECT_ALL_MENU_BY_MODULEID_QUERY, moduleId);

//		IEnumerable<Menu> menu = await ExecuteListQuery<Menu>(query, null);
//		return menu.AsQueryable();
//	}

//	public async Task<IEnumerable<Menu>> SelectMenuByUserPermission(int userId, bool trackChanges)
//	{
//		string query = string.Format(SELECT_MENU_BY_USERS_PERMISSION_QUERY, userId);
//		IEnumerable<Menu> menu = await ExecuteListQuery<Menu>(query, null);

//		return menu.AsQueryable();
//	}

//	public async Task<List<Menu>> ParentMenuByMenu(int parentMenuId, bool trackChanges)
//	{
//		string menusByUserPermissionQuery = $"SELECT * FROM Menu WHERE MenuID = {parentMenuId}";
//		IEnumerable<Menu> menusDto = await ExecuteListQuery<Menu>(menusByUserPermissionQuery, null);
//		return menusDto.ToList();
//	}

//	public async Task<List<Menu>> MenuSummary(bool trackChanges)
//	{
//		string menuSummaryQuery = $"Select MenuId,Menu.ModuleId, MenuName, MenuPath, ISNULL(ParentMenu, 0) as ParentMenu ,ModuleName,ToDo,SORORDER\r\n,(Select MenuName from Menu mn where mn.MenuId = menu.ParentMenu) as ParentMenuName \r\nfrom Menu \r\nleft outer join Module on module.ModuleId = menu.ModuleId\r\norder by ModuleName asc,ParentMenu asc, MenuName";
//		IEnumerable<Menu> menusDto = await ExecuteListQuery<Menu>(menuSummaryQuery, null);
//		return menusDto.ToList();
//	}

//	public async Task<IEnumerable<Menu>> Menus(bool trackChanges) => await ListAsync(x => x.MenuName, trackChanges);

//	//public async Task<IEnumerable<Menu>> AllMenus(bool trackChanges) => await ListAsync(trackChanges).OrderBy(c => c.MenuName).ToList();


//	public IEnumerable<Menu> ByIds(IEnumerable<int> ids, bool trackChanges)
//		=> ListByIds(c => ids.Contains(c.MenuId), trackChanges).ToList();


//	public Menu? Menu(int MenuId, bool trackChanges) => FirstOrDefault(c => c.MenuId.Equals(MenuId), trackChanges);


//	//  all Menus
//	public async Task<IEnumerable<Menu>> MenusAsync(bool trackChanges)
//		=> await ListAsync(x => x.MenuId, trackChanges);

//	//  a single Menu by ID
//	public async Task<Menu> MenuAsync(int MenuId, bool trackChanges) => await FirstOrDefaultAsync(c => c.MenuId.Equals(MenuId), trackChanges);

//	public async Task<Menu?> MenuByMenuIdWithAdditionalCondition(int MenuId, string additionalCondition)
//	{
//		var query = string.Format("Select * from Menu where MenuId = {0} {1}", MenuId, additionalCondition);
//		Menu? objMenu = await ExecuteSingleData<Menu>(query);
//		return objMenu;
//	}

//	public async Task<IEnumerable<Menu>> MenusByModuleId(int moduleId, bool trackChanges)
//		=> await ListByConditionAsync(c => c.ModuleId.Equals(moduleId), orderBy: x => x.ModuleId, trackChanges: trackChanges);

//	// Add a new Menu
//	public void CreateMenu(Menu Menu) => Create(Menu);

//	// Update an existing Menu
//	public void UpdateMenu(Menu Menu) => UpdateByState(Menu);

//	// Delete a Menu by ID
//	public void DeleteMenu(Menu Menu) => Delete(Menu);



//}
