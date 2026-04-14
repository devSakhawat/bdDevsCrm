using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IMenuService
{

	Task<MenuDto> CreateMenuAsync(MenuDto entityForCreate, CancellationToken cancellationToken = default);
	Task<int> DeleteMenuAsync(int menuId, bool trackChanges, CancellationToken cancellationToken = default);
	Task<MenuDto> UpdateMenuAsync(int menuId, MenuDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);
	Task<IEnumerable<MenuDto>> MenusByModuleIdAsync(int moduleId, bool trackChanges, CancellationToken cancellationToken = default);
	Task<IEnumerable<MenuDto>> MenusByUserPermissionAsync(int userId, bool trackChanges, CancellationToken cancellationToken = default);
	Task<IEnumerable<MenuDto>> MenusByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
	Task<List<MenuDto>> ParentMenusByMenuAsync(int parentMenuId, bool trackChanges, CancellationToken cancellationToken = default);
	Task<IEnumerable<MenuDto>> MenusAsync(bool trackChanges, CancellationToken cancellationToken = default);
	Task<GridEntity<MenuDto>> MenuSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default);
	Task<MenuDto> MenuAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
	Task<IEnumerable<MenuDto>> MenusByMenuNameAsync(string menuName, bool trackChanges = false, CancellationToken cancellationToken = default);
	Task<IEnumerable<MenuForDDLDto>> MenuForDDLAsync(CancellationToken cancellationToken = default);


	//Task<(IEnumerable<MenuDto> Menus, string ids)> CreateMenuCollectionAsync(IEnumerable<MenuDto> MenuCollection);
	//object MenuSummary(bool trackChanges, GridOptions options);
	//IEnumerable<MenuDto> MenusByIds(IEnumerable<int> ids, bool trackChanges);
	//Task<MenuDto> MenuAsync(int MenuId, bool trackChanges);
	//Task<IEnumerable<MenuDto>> ByIdsAsync(IEnumerable<int> ids, bool trackChanges);
	//Task<IEnumerable<MenuDto>> MenusByModuleId(int moduleId, bool trackChanges);
	//MenuDto CreateMenu(MenuDto Menu);
	//Task<MenuDto> CreateAsync(MenuDto modelDto, CancellationToken cancellationToken = default);
	//Task<MenuDto> UpdateAsync(int key, MenuDto modelDto, CancellationToken cancellationToken = default);
	//Task DeleteAsync(int key, MenuDto modelDto);
	//Task DeleteAsync(int key, CancellationToken cancellationToken = default);
}
