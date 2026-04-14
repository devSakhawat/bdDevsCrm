using Domain.Entities.Entities.System;
using Domain.Contracts.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.Core.SystemAdmin;

/// <summary>
/// Repository for module data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class ModuleRepository : RepositoryBase<Module>, IModuleRepository
{
	public ModuleRepository(CRMContext context) : base(context) { }

	/// <summary>
	/// Retrieves modules by module ID using raw SQL asynchronously.
	/// </summary>
	public async Task<IEnumerable<Module>> ModulesByModuleIdAsync(int moduleId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		string query = $"SELECT * FROM Module WHERE ModuleId = {moduleId} ORDER BY SorOrder, ModuleName ASC";
		return await AdoExecuteListQueryAsync<Module>(query, null, cancellationToken);
	}

	/// <summary>
	/// Retrieves all modules asynchronously.
	/// </summary>
	public async Task<IEnumerable<Module>> ModulesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(m => m.ModuleId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves a single module by ID asynchronously.
	/// </summary>
	public async Task<Module?> ModuleAsync(int moduleId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(m => m.ModuleId.Equals(moduleId), trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves modules by a collection of IDs asynchronously.
	/// </summary>
	public async Task<IEnumerable<Module>> ModulesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(m => ids.Contains(m.ModuleId), trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves a module by ID with additional condition asynchronously.
	/// </summary>
	public async Task<Module?> ModuleWithConditionAsync(int moduleId, string additionalCondition, CancellationToken cancellationToken = default)
	{
		var query = $"SELECT * FROM Module WHERE ModuleId = {moduleId} {additionalCondition}";
		return await AdoExecuteSingleDataAsync<Module>(query, null, cancellationToken);
	}

	/// <summary>
	/// Retrieves module summary asynchronously.
	/// </summary>
	public async Task<List<Module>> ModulesSummaryAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		string query = "SELECT ModuleId, ModuleName FROM Module ORDER BY ModuleId";
		IEnumerable<Module> result = await AdoExecuteListQueryAsync<Module>(query, parameters: null, cancellationToken);
		return result.ToList();
	}

	/// <summary>
	/// Creates a new module.
	/// </summary>
	public async Task<Module> CreateModuleAsync(Module module, CancellationToken cancellationToken = default)
	{
		int moduleId = await CreateAndIdAsync(module, cancellationToken);
		module.ModuleId = moduleId;
		return module;
	}

	/// <summary>
	/// Updates an existing module.
	/// </summary>
	public void UpdateModule(Module module) => UpdateByState(module);

	/// <summary>
	/// Deletes a module.
	/// </summary>
	public async Task DeleteModuleAsync(Module module, bool trackChanges, CancellationToken cancellationToken = default)
		=> await DeleteAsync(x => x.ModuleId == module.ModuleId, trackChanges, cancellationToken);
}
