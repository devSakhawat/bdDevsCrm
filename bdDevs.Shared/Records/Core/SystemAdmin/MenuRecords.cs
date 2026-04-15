namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new menu.
/// </summary>
/// <param name="ModuleId">Parent module ID.</param>
/// <param name="MenuName">Name of the menu.</param>
/// <param name="MenuPath">Navigation path for the menu.</param>
/// <param name="ParentMenu">Parent menu ID for hierarchical menus.</param>
/// <param name="Sororder">Sort order for display sequence.</param>
/// <param name="Todo">Todo flag.</param>
/// <param name="IsActive">Active status (1 = active, 0 = inactive).</param>
public record CreateMenuRecord(
    int ModuleId,
    string MenuName,
    string? MenuPath,
    int? ParentMenu,
    int? Sororder,
    int? Todo,
    int? IsActive);

/// <summary>
/// Record for updating an existing menu.
/// </summary>
/// <param name="MenuId">ID of the menu to update.</param>
/// <param name="ModuleId">Updated parent module ID.</param>
/// <param name="MenuName">Updated menu name.</param>
/// <param name="MenuPath">Updated navigation path.</param>
/// <param name="ParentMenu">Updated parent menu ID.</param>
/// <param name="Sororder">Updated sort order.</param>
/// <param name="Todo">Updated todo flag.</param>
/// <param name="IsActive">Updated active status.</param>
public record UpdateMenuRecord(
    int MenuId,
    int ModuleId,
    string MenuName,
    string? MenuPath,
    int? ParentMenu,
    int? Sororder,
    int? Todo,
    int? IsActive);

/// <summary>
/// Record for deleting a menu.
/// </summary>
/// <param name="MenuId">ID of the menu to delete.</param>
public record DeleteMenuRecord(int MenuId);
