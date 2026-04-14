// Interface: IModuleRepository
using Domain.Entities.Entities.System;
using Domain.Contracts.Repositories;

namespace Domain.Contracts.Core.SystemAdmin
{
  public interface IModuleRepository : IRepositoryBase<Module>
  {
    /// <summary>
    /// Retrieves modules by module ID using raw SQL asynchronously.
    /// </summary>
    Task<IEnumerable<Module>> ModulesByModuleIdAsync(int moduleId, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all modules asynchronously.
    /// </summary>
    Task<IEnumerable<Module>> ModulesAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single module by ID asynchronously.
    /// </summary>
    Task<Module?> ModuleAsync(int moduleId, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves modules by a collection of IDs asynchronously.
    /// </summary>
    Task<IEnumerable<Module>> ModulesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a module by ID with additional condition asynchronously.
    /// </summary>
    Task<Module?> ModuleWithConditionAsync(int moduleId, string additionalCondition, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves module summary asynchronously.
    /// </summary>
    Task<List<Module>> ModulesSummaryAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new module.
    /// </summary>
    Task<Module> CreateModuleAsync(Module module, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing module.
    /// </summary>
    void UpdateModule(Module module) => UpdateByState(module);

    /// <summary>
    /// Deletes a module.
    /// </summary>
    Task DeleteModuleAsync(Module module, bool trackChanges, CancellationToken cancellationToken = default);
  }
}





//using Domain.Entities.Entities;
//using Domain.Entities.Entities.System;
//using Domain.Contracts.Repositories;

//namespace bdDevCRM.RepositoriesContracts.Core.SystemAdmin;

//public interface IModuleRepository : IRepositoryBase<Module>
//{
//  Task<IEnumerable<Module>> AllModules(bool trackChanges);

//  Module Module(int ModuleId, bool trackChanges);

//  void CreateModule(Module Module);

//  IEnumerable<Module> ByIds(IEnumerable<int> ids, bool trackChanges);

//  Task<IEnumerable<Module>> ModulesAsync(bool trackChanges);

//  Task<Module> ModuleAsync(int ModuleId, bool trackChanges);

//  Task<Module?> ModuleByModuleIdWithAdditionalCondition(int ModuleId, string additionalCondition);

//  void UpdateModule(Module Module);

//  void DeleteModule(Module Module);

//  Task<List<Module>> ModuleSummary(bool trackChanges);
//}
