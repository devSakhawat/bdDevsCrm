namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new module.
/// </summary>
/// <param name="ModuleName">Name of the module.</param>
public record CreateModuleRecord(string ModuleName);

/// <summary>
/// Record for updating an existing module.
/// </summary>
/// <param name="ModuleId">ID of the module to update.</param>
/// <param name="ModuleName">Updated module name.</param>
public record UpdateModuleRecord(int ModuleId, string ModuleName);

/// <summary>
/// Record for deleting a module.
/// </summary>
/// <param name="ModuleId">ID of the module to delete.</param>
public record DeleteModuleRecord(int ModuleId);
