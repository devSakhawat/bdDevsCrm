using Domain.Contracts.Repositories;
﻿using Domain.Entities.Entities.System;
using Domain.Contracts.Services.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Application.Services.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Domain.Exceptions;
using Application.Shared.Grid;
using Application.Services.Mappings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.Core.SystemAdmin;

/// <summary>
/// Menu service implementing business logic for menu management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class MenuService : IMenuService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<MenuService> _logger;
	private readonly IConfiguration _configuration;

	/// <summary>
	/// Initializes a new instance of <see cref="MenuService"/> with required dependencies.
	/// </summary>
	/// <param name="repository">The repository manager for data access operations.</param>
	/// <param name="logger">The logger for capturing service-level events.</param>
	/// <param name="configuration">The application configuration accessor.</param>
	public MenuService(IRepositoryManager repository, ILogger<MenuService> logger, IConfiguration configuration)
	{
		_repository = repository;
		_logger = logger;
		_configuration = configuration;
	}

	/// <summary>
	/// Creates a new menu record after validating for null input and duplicate menu name.
	/// </summary>
	/// <param name="entityForCreate">The DTO containing data for the new menu.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The created <see cref="MenuDto"/> with the newly assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
	/// <exception cref="DuplicateRecordException">Thrown when a menu with the same name already exists.</exception>
	public async Task<MenuDto> CreateMenuAsync(MenuDto entityForCreate, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(MenuDto));

		bool menuExists = await _repository.Menus.ExistsAsync(
				m => m.MenuName.Trim().ToLower() == entityForCreate.MenuName.Trim().ToLower(),
				cancellationToken: cancellationToken);

		if (menuExists)
			throw new DuplicateRecordException("Menu", "MenuName");

		Menu menuEntity = MyMapper.JsonClone<MenuDto, Menu>(entityForCreate);

		await _repository.Menus.CreateAsync(menuEntity, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("Menu could not be saved to the database.");

		_logger.LogInformation(
				"Menu created successfully. ID: {MenuId}, Name: {MenuName}, Time: {Time}",
				menuEntity.MenuId,
				menuEntity.MenuName,
				DateTime.UtcNow);

		return MyMapper.JsonClone<Menu, MenuDto>(menuEntity);
	}

	/// <summary>
	/// Updates an existing menu record by merging only the changed values from the provided DTO.
	/// Validates ID consistency, null input, record existence, and duplicate name constraints.
	/// </summary>
	/// <param name="menuId">The ID of the menu to update.</param>
	/// <param name="modelDto">The DTO containing updated field values.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="MenuDto"/> reflecting the saved state.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no menu is found for the given ID.</exception>
	/// <exception cref="DuplicateRecordException">Thrown when another menu with the same name already exists.</exception>
	public async Task<MenuDto> UpdateMenuAsync(int menuId, MenuDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(MenuDto));

		if (menuId != modelDto.MenuId)
			throw new BadRequestException(menuId.ToString(), nameof(MenuDto));

		Menu existingEntity = await _repository.Menus
				.FirstOrDefaultAsync(x => x.MenuId == menuId, trackChanges: false, cancellationToken)
				?? throw new NotFoundException("Menu", "MenuId", menuId.ToString());

		bool duplicateExists = await _repository.Menus.ExistsAsync(
				x => x.MenuName.Trim().ToLower() == modelDto.MenuName.Trim().ToLower()
					&& x.MenuId != menuId,
				cancellationToken: cancellationToken);

		if (duplicateExists)
			throw new DuplicateRecordException("Menu", "MenuName");
		Menu updatedEntity = MyMapper.MergeChangedValues<Menu, MenuDto>(existingEntity, modelDto);
		_repository.Menus.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("Menu", "MenuId", menuId.ToString());
		_logger.LogInformation(
				"Menu updated. ID: {MenuId}, Name: {MenuName}, Time: {Time}",
				updatedEntity.MenuId,
				updatedEntity.MenuName,
				DateTime.UtcNow);

		return MyMapper.JsonClone<Menu, MenuDto>(updatedEntity);
	}

	/// <summary>
	/// Deletes a menu record identified by the given ID.
	/// Validates that the ID is positive and that the record exists before deletion.
	/// </summary>
	/// <param name="menuId">The ID of the menu to delete.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <exception cref="IdParametersBadRequestException">Thrown when <paramref name="menuId"/> is zero or negative.</exception>
	/// <exception cref="NotFoundException">Thrown when no menu record is found for the given ID.</exception>
	public async Task<int> DeleteMenuAsync(int menuId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (menuId <= 0)
			throw new BadRequestException(menuId.ToString(), nameof(MenuDto));

		Menu menuEntity = await _repository.Menus
				.FirstOrDefaultAsync(x => x.MenuId == menuId, trackChanges, cancellationToken)
				?? throw new NotFoundException("Menu", "MenuId", menuId.ToString());

		await _repository.Menus.DeleteAsync(x => x.MenuId == menuId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("Menu", "MenuId", menuId.ToString());
		_logger.LogWarning(
				"Menu deleted. ID: {MenuId}, Name: {MenuName}, Time: {Time}",
				menuEntity.MenuId,
				menuEntity.MenuName,
				DateTime.UtcNow);
		return affected;
	}

	/// <summary>
	/// Retrieves all menus associated with the specified module ID.
	/// </summary>
	/// <param name="moduleId">The ID of the module whose menus are to be retrieved.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="MenuDto"/> belonging to the specified module.</returns>
	/// <exception cref="GenericListNotFoundException">Thrown when no menus are found for the given module ID.</exception>
	public async Task<IEnumerable<MenuDto>> MenusByModuleIdAsync(int moduleId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		IEnumerable<Menu> menus = await _repository.Menus.MenusByModuleIdAsync(moduleId, trackChanges, cancellationToken);
		if (!menus.Any())
		{
			_logger.LogWarning("No menus found for moduleId: {ModuleId}", moduleId);
			return Enumerable.Empty<MenuDto>();
		}

		return MyMapper.JsonCloneIEnumerableToList<Menu, MenuDto>(menus);
	}

	/// <summary>
	/// Retrieves all menus accessible to a specific user based on their permissions.
	/// Returns an empty collection if the user ID is invalid or no permissions are found.
	/// </summary>
	/// <param name="userId">The ID of the user whose menu permissions are to be fetched.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="MenuDto"/> the user has permission to access.</returns>
	public async Task<IEnumerable<MenuDto>> MenusByUserPermissionAsync(int userId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (userId <= 0)
		{
			_logger.LogWarning("MenusByUserPermissionAsync called with invalid userId: {UserId}", userId);
			return Enumerable.Empty<MenuDto>();
		}

		IEnumerable<Menu> menus = await _repository.Menus.MenusByUserPermissionAsync(userId, trackChanges, cancellationToken);

		if (!menus.Any())
		{
			_logger.LogWarning("No menu permissions found for userId: {UserId}", userId);
			return Enumerable.Empty<MenuDto>();
		}

		_logger.LogInformation("Menu permissions fetched for userId: {UserId}", userId);

		return MyMapper.JsonCloneIEnumerableToIEnumerable<Menu, MenuDto>(menus);
	}

	/// <summary>
	/// Retrieves multiple menu records by their IDs.
	/// Validates that all requested IDs are found in the database.
	/// </summary>
	/// <param name="ids">The collection of menu IDs to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="MenuDto"/> matching the provided IDs.</returns>
	/// <exception cref="IdParametersBadRequestException">Thrown when <paramref name="ids"/> is null.</exception>
	/// <exception cref="CollectionByIdsBadRequestException">Thrown when the number of found records does not match the requested IDs.</exception>
	public async Task<IEnumerable<MenuDto>> MenusByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (ids is null)
			throw new BadRequestException("Invalid request!");

		// Materialize once — avoid multiple Count() call 
		List<int> idList = ids.ToList();

		IEnumerable<Menu> menuEntities = await _repository.Menus.MenusByIdsAsync(idList, trackChanges, cancellationToken);
		//List<Menu> menuList = menuEntities.ToList();

		if (idList.Count != menuEntities.ToList().Count)
			throw new CollectionByIdsBadRequestException("Menus");

		return MyMapper.JsonCloneIEnumerableToIEnumerable<Menu, MenuDto>(menuEntities);
	}

	/// <summary>
	/// Retrieves the parent menus available for the specified user.
	/// Returns an empty collection if no parent menus are found.
	/// </summary>
	/// <param name="userId">The ID of the user for whom parent menus are to be fetched.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A list of <see cref="MenuDto"/> representing parent menus.</returns>
	public async Task<List<MenuDto>> ParentMenusByMenuAsync(int userId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		IEnumerable<Menu> menus = await _repository.Menus.ParentMenusByMenuAsync(userId, trackChanges, cancellationToken);

		if (!menus.Any())
		{
			_logger.LogWarning("No parent menus found for userId: {UserId}", userId);
			return new List<MenuDto>();
		}
		_logger.LogInformation("Parent menus fetched for userId: {UserId}", userId);
		return MyMapper.JsonCloneIEnumerableToList<Menu, MenuDto>(menus);
	}

	/// <summary>
	/// Retrieves all menu records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of all <see cref="MenuDto"/> records.</returns>
	public async Task<IEnumerable<MenuDto>> MenusAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		IEnumerable<Menu> menus = await _repository.Menus.MenusAsync(trackChanges, cancellationToken);
		if (!menus.Any())
		{
			_logger.LogWarning("No menus found");
			return Enumerable.Empty<MenuDto>();
		}
		_logger.LogInformation("Menus fetched successfully");
		return MyMapper.JsonCloneIEnumerableToIEnumerable<Menu, MenuDto>(menus);
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all menus with module and parent menu information.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{MenuDto}"/> containing the paged menu summary data.</returns>
	public async Task<GridEntity<MenuDto>> MenuSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default)
	{
		const string query =
				@"SELECT
            MenuId,
            Menu.ModuleId,
            MenuName,
            MenuPath,
            ISNULL(ParentMenu, 0) AS ParentMenu,
            ModuleName,
            ToDo,
            SORORDER,
            (SELECT MenuName FROM Menu mn WHERE mn.MenuId = Menu.ParentMenu) AS ParentMenuName,
            IsActive
          FROM Menu
          LEFT OUTER JOIN Module ON Module.ModuleId = Menu.ModuleId";
		const string orderBy = "ModuleName ASC, ParentMenu ASC, MenuName";

		return await _repository.Menus.AdoGridDataAsync<MenuDto>(query, options, orderBy, "", cancellationToken);
	}

	/// <summary>
	/// Retrieves a single menu record by its ID.
	/// </summary>
	/// <param name="id">The ID of the menu to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="MenuDto"/> matching the specified ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no menu is found for the given ID.</exception>
	public async Task<MenuDto> MenuAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		Menu menu = await _repository.Menus.MenuAsync(id, trackChanges, cancellationToken)
				?? throw new NotFoundException("Menu", "MenuId", id.ToString());

		_logger.LogInformation("Menu fetched successfully. ID: {MenuId}, Name: {MenuName}, Time: {Time}", menu.MenuId, menu.MenuName, DateTime.UtcNow);
		return MyMapper.JsonClone<Menu, MenuDto>(menu);
	}

	/// <summary>
	/// Retrieves all menus matching the specified menu name (case-insensitive).
	/// Returns an empty collection if no matches are found.
	/// </summary>
	/// <param name="menuName">The menu name to search for.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="MenuDto"/> matching the given name.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="menuName"/> is null or whitespace.</exception>
	public async Task<IEnumerable<MenuDto>> MenusByMenuNameAsync(string menuName, bool trackChanges = false, CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrWhiteSpace(menuName))
			throw new BadRequestException(menuName);

		IEnumerable<Menu> menus = await _repository.Menus.ListByConditionAsync(
				expression: x => x.MenuName.Trim().ToLower() == menuName.Trim().ToLower(),
				orderBy: x => x.MenuId,
				trackChanges: false,
				cancellationToken: cancellationToken);
		if (!menus.Any())
		{
			_logger.LogWarning("No menus found");
			return Enumerable.Empty<MenuDto>();
		}
		_logger.LogInformation("Menus fetched successfully for menuName: {MenuName}", menuName);

		return MyMapper.JsonCloneIEnumerableToIEnumerable<Menu, MenuDto>(menus);
	}

	/// <summary>
	/// Retrieves a lightweight list of all menus suitable for use in dropdown lists.
	/// Returns only the menu ID and name, ordered by sort order.
	/// </summary>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="MenuForDDLDto"/> for dropdown binding.</returns>
	public async Task<IEnumerable<MenuForDDLDto>> MenuForDDLAsync(CancellationToken cancellationToken = default)
	{
		IEnumerable<Menu> menus = await _repository.Menus.ListWithSelectAsync(
				x => new Menu
				{
					MenuId = x.MenuId,
					MenuName = x.MenuName
				},
				orderBy: x => x.Sororder,
				trackChanges: false,
				cancellationToken: cancellationToken);

		if (!menus.Any())
		{
			_logger.LogWarning("No menus found for dropdown list");
			return Enumerable.Empty<MenuForDDLDto>();
		}
		_logger.LogInformation("Menus fetched successfully for dropdown list");
		return MyMapper.JsonCloneIEnumerableToIEnumerable<Menu, MenuForDDLDto>(menus);
	}

}



//using Application.Shared.Grid;
//using Domain.Entities.Entities.System;
//using Domain.Contracts.Services.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using Application.Services.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using Domain.Exceptions;
//using Application.Services.Mappings;
//using Microsoft.Extensions.Configuration;
//using System.Linq.Expressions;
//using Microsoft.Extensions.Logging;

//namespace Application.Services.Core.SystemAdmin;


///// <summary>
///// Menu service implementing business logic for menu management.
///// Follows enterprise patterns with structured logging and exception handling.
///// </summary>
//internal sealed class MenuService : IMenuService
//{
//	private readonly IRepositoryManager _repository;
//	private readonly ILogger<MenuService> _logger;
//	private readonly IConfiguration _configuration;

//	public MenuService(IRepositoryManager repository, ILogger<MenuService> logger, IConfiguration configuration)
//	{
//		_repository = repository;
//		_logger = logger;
//		_configuration = configuration;
//	}

//	public async Task<IEnumerable<MenuDto>> SelectAllMenuByModuleId(int moduleId, bool trackChanges)
//	{
//		var menus = await _repository.Menus.SelectAllMenuByModuleId(moduleId, trackChanges);
//		if (menus.Count() == 0) throw new GenericListNotFoundException("Menu");
//		var menuDtos = MyMapper.JsonCloneIEnumerableToList<Menu, MenuDto>(menus);
//		return menuDtos;
//	}

//	public async Task<IEnumerable<MenuDto>> SelectMenuByUserPermission(int userid, bool trackChanges)
//	{
//		if (userid <= 0)
//		{
//			_logger.LogWarning("SelectMenuByUserPermission called with invalid userId: {UserId}", userid);
//			return Enumerable.Empty<MenuDto>();
//		}

//		_logger.LogInformation("Fetching menu permissions for user {UserId}", userid);
//		var menus = await _repository.Menus.SelectMenuByUserPermission(userid, trackChanges);

//		if (!menus.Any())
//		{
//			_logger.LogWarning("No menu permissions found for userId: {UserId}", userid);
//			return Enumerable.Empty<MenuDto>();
//		}

//		var menusDto = MyMapper.JsonCloneIEnumerableToList<Menu, MenuDto>(menus);
//		return menusDto;
//	}

//	public async Task<List<MenuDto>> ParentMenuByMenu(int userid, bool trackChanges)
//	{
//		var menus = await _repository.Menus.ParentMenuByMenu(userid, trackChanges);

//		//if(menus.Count() == 0) throw new GenericListNotFoundException("Menu");
//		var menusDto = MyMapper.JsonCloneIEnumerableToList<Menu, MenuDto>(menus);
//		return menusDto;
//	}

//	/// <summary>
//	/// Menu crud
//	/// </summary>
//	/// <param name="trackChanges"></param>
//	/// <param name="options"></param>
//	/// <returns></returns>
//	public async Task<GridEntity<MenuDto>> MenuSummary(bool trackChanges, GridOptions options)
//	{
//		string menuSummaryQuery = $"Select MenuId,Menu.ModuleId, MenuName, MenuPath, ISNULL(ParentMenu, 0) as ParentMenu ,ModuleName,ToDo,SORORDER\r\n,(Select MenuName from Menu mn where mn.MenuId = menu.ParentMenu) as ParentMenuName ,IsActive \r\nfrom Menu \r\nleft outer join Module on module.ModuleId = menu.ModuleId";
//		string orderBy = "ModuleName asc,ParentMenu asc, MenuName";

//		var gridEntity = await _repository.Menus.GridData<MenuDto>(menuSummaryQuery, options, orderBy, "");

//		return gridEntity;
//	}


//	public MenuDto Menu(int id, bool trackChanges)
//    {
//        var Menu = _repository.Menus.Menu(id, trackChanges);
//        //Check if the Menu is null
//        if (Menu == null) throw new NotFoundException("Menu", "MenuId", id.ToString());

//        var MenuDto = new MenuDto();
//        MenuDto = MyMapper.JsonClone<Menu, MenuDto>(Menu);
//        return MenuDto;
//    }

//    public MenuDto CreateMenu(MenuDto Menu)
//    {
//        Menu MenuEntity = MyMapper.JsonClone<MenuDto, Menu>(Menu);
//        _repository.Menus.CreateMenu(MenuEntity);
//        _repository.Save();

//        var MenuToReturn = MyMapper.JsonClone<Menu, MenuDto>(MenuEntity);
//        return MenuToReturn;
//    }

//    public IEnumerable<MenuDto> ByIds(IEnumerable<int> ids, bool trackChanges)
//    {
//        if (ids is null)
//            throw new BadRequestException("Invalid request!");
//        var MenuEntities = _repository.Menus.ByIds(ids, trackChanges);
//        if (ids.Count() != MenuEntities.Count())
//            throw new CollectionByIdsBadRequestException("Menus");
//        var MenusToReturn = MyMapper.JsonCloneIEnumerableToList<Menu, MenuDto>(MenuEntities);
//        return MenusToReturn;
//    }

//    public async Task<MenuDto> CreateMenuAsync(MenuDto entityForCreate)
//    {
//        Menu Menu = MyMapper.JsonClone<MenuDto, Menu>(entityForCreate);
//        _repository.Menus.CreateMenu(Menu);
//        await _repository.SaveAsync();
//        return entityForCreate;
//    }

//    public async Task<MenuDto> CreateAsync(MenuDto modelDto)
//    {
//        if (modelDto == null) throw new BadRequestException(new MenuDto().Type().Name.ToString());
//        bool ismenuExists = await _repository.Menus.ExistsAsync(m => m.MenuName.Trim().ToLower() == modelDto.MenuName.Trim().ToLower());
//        if (ismenuExists) throw new DuplicateRecordException("Menu", "MenuName");

//        Menu entity = MyMapper.JsonClone<MenuDto, Menu>(modelDto);
//        modelDto.MenuId = await _repository.Menus.CreateAndIdAsync(entity);
//        await _repository.SaveAsync();
//        return modelDto;
//    }

//    public async Task UpdateMenuAsync(int MenuId, MenuDto MenuForUpdate, bool trackChanges)
//    {
//        Expression<Func<Menu, bool>> expression = e => e.MenuId == MenuId;
//        bool exists = await _repository.Menus.ExistsAsync(expression);
//        if (!exists) throw new NotFoundException("Menu", "MenuId", MenuId.ToString());

//        Menu Menu = MyMapper.JsonClone<MenuDto, Menu>(MenuForUpdate);
//        _repository.Menus.UpdateMenu(Menu);
//        await _repository.SaveAsync();
//    }

//    public async Task DeleteMenuAsync(int menuId, bool trackChanges)
//    {
//        var Menu = await _repository.Menus.FirstOrDefaultAsync(x => x.MenuId.Equals(menuId));
//        _logger.LogWarning("Menu with Id: {MenuId} is about to be deleted from the database.", menuId);
//        if (Menu != null) _repository.Menus.DeleteMenu(Menu);
//        await _repository.SaveAsync();
//    }

//    public async Task<IEnumerable<MenuDto>> MenusAsync(bool trackChanges)
//    {
//        IEnumerable<Menu> Menus = await _repository.Menus.MenusAsync(trackChanges);
//        List<MenuDto> Menus = MyMapper.JsonCloneIEnumerableToList<Menu, MenuDto>(Menus);
//        return Menus;
//    }

//    public async Task<MenuDto> MenuAsync(int MenuId, bool trackChanges)
//    {
//        if (MenuId <= 0) throw new ArgumentOutOfRangeException(nameof(MenuId), "Menu ID must be be zero or non-negative integer.");

//        Menu Menu = await _repository.Menus.MenuAsync(MenuId, trackChanges);
//        if (Menu == null) throw new NotFoundException("Menu", "MenuId", MenuId.ToString());

//        MenuDto MenuDto = MyMapper.JsonClone<Menu, MenuDto>(Menu);
//        return MenuDto;
//    }

//    public Task<IEnumerable<MenuDto>> ByIdsAsync(IEnumerable<int> ids, bool trackChanges)
//    {
//        throw new NotImplementedException();
//    }

//    public Task<(IEnumerable<MenuDto> Menus, string ids)> CreateMenuCollectionAsync(IEnumerable<MenuDto> MenuCollection)
//    {
//        throw new NotImplementedException();
//    }

//    public async Task<IEnumerable<MenuDto>> MenusByModuleId(int moduleId, bool trackChanges)
//    {
//        if (moduleId < 0) throw new ArgumentOutOfRangeException(nameof(moduleId), "Module ID must be a positive integer.");

//        IEnumerable<Menu> menus = await _repository.Menus.MenusByModuleId(moduleId, trackChanges);
//        //if (menus.Count() = 0) throw new NotFoundException("Menu", "MenuId", moduleId.ToString());
//        if (menus.Count() == 0) return new List<MenuDto>();

//        List<MenuDto> menusDto = MyMapper.JsonCloneIEnumerableToList<Menu, MenuDto>(menus);
//        return menusDto;
//    }

//    public async Task<IEnumerable<MenuDto>> MenusByMenuName(string menuName, bool trackChanges = false)
//    {
//        if (string.IsNullOrWhiteSpace(menuName)) throw new BadRequestException(menuName);

//        IEnumerable<Menu> menus = await _repository.Menus.ListByConditionAsync(expression: x => x.MenuName.Trim().ToLower() == menuName.Trim().ToLower(), orderBy: x => x.MenuId, trackChanges: false);
//        if (menus.Count() == 0) return new List<MenuDto>();

//        List<MenuDto> menusDto = MyMapper.JsonCloneIEnumerableToList<Menu, MenuDto>(menus);
//        return menusDto;
//    }



//    public async Task<MenuDto> UpdateAsync(int key, MenuDto modelDto)
//    {
//        if (modelDto == null) throw new BadRequestException(new MenuDto().Type().Name.ToString());
//        if (key != modelDto.MenuId) throw new BadRequestException(key.ToString(), new MenuDto().Type().Name.ToString());

//        Menu entity = await _repository.Menus.ByIdAsync(m => m.MenuId == modelDto.MenuId, trackChanges: false);
//        //if (entity.MenuName == modelDto.MenuName) throw new DuplicateRecordException();

//        entity = MyMapper.JsonClone<MenuDto, Menu>(modelDto);
//        _repository.Menus.UpdateByState(entity);
//        await _repository.SaveAsync();
//        modelDto = MyMapper.JsonClone<Menu, MenuDto>(entity);
//        return modelDto;
//    }

//    public async Task DeleteAsync(int key)
//    {
//        if (key <= 0)
//            throw new ArgumentOutOfRangeException(nameof(key), "Menu ID must be a positive integer.");

//        Menu entity = await _repository.Menus.ByIdAsync(m => m.MenuId == key, trackChanges: false);
//        if (entity == null)
//            throw new NotFoundException("Menu", "MenuId", key.ToString());

//        // Soft delete - set IsActive to 0
//        //entity.IsActive = 0;

//        await _repository.Menus.DeleteAsync(x => x.MenuId == entity.MenuId, trackChanges: false);
//        await _repository.SaveAsync();
//    }

//    public async Task<IEnumerable<MenuForDDLDto>> MenuForDDL()
//    {
//        // Corrected the initialization of the Menu object to use a constructor instead of a collection initializer.
//        IEnumerable<Menu> menus = await _repository.Menus.ListWithSelectAsync(
//            x => new Menu
//            {
//                MenuId = x.MenuId,
//                MenuName = x.MenuName
//            },
//            orderBy: x => x.Sororder,
//            trackChanges: false
//        );

//        if (menus.Count() == 0) return new List<MenuForDDLDto>();

//        IEnumerable<MenuForDDLDto> menusForDDLDto = MyMapper.JsonCloneIEnumerableToList<Menu, MenuForDDLDto>(menus);
//        return menusForDDLDto;
//    }

//}
