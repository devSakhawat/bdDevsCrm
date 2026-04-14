using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;
using Domain.Contracts.Services.Core.SystemAdmin;
using Application.Services.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Domain.Exceptions;
using Application.Shared.Grid;
using Application.Services.Mappings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.Core.SystemAdmin;

/// <summary>
/// Module service implementing business logic for module management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class ModuleService : IModuleService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<ModuleService> _logger;
	private readonly IConfiguration _configuration;

	/// <summary>
	/// Initializes a new instance of <see cref="ModuleService"/> with required dependencies.
	/// </summary>
	/// <param name="repository">The repository manager for data access operations.</param>
	/// <param name="logger">The logger for capturing service-level events.</param>
	/// <param name="configuration">The application configuration accessor.</param>
	public ModuleService(IRepositoryManager repository, ILogger<ModuleService> logger, IConfiguration configuration)
	{
		_repository = repository;
		_logger = logger;
		_configuration = configuration;
	}

	/// <summary>
	/// Creates a new module record after validating for null input and duplicate module name.
	/// </summary>
	public async Task<ModuleDto> CreateModuleAsync(ModuleDto entityForCreate, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(ModuleDto));

		bool moduleExists = await _repository.Modules.ExistsAsync(
						m => m.ModuleName.Trim().ToLower() == entityForCreate.ModuleName.Trim().ToLower(),
						cancellationToken: cancellationToken);

		if (moduleExists)
			throw new DuplicateRecordException("Module", "ModuleName");

		Module moduleEntity = MyMapper.JsonClone<ModuleDto, Module>(entityForCreate);

		await _repository.Modules.CreateAsync(moduleEntity, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("Module could not be saved to the database.");

		_logger.LogInformation(
						"Module created successfully. ID: {ModuleId}, Name: {ModuleName}, Time: {Time}",
						moduleEntity.ModuleId,
						moduleEntity.ModuleName,
						DateTime.UtcNow);

		return MyMapper.JsonClone<Module, ModuleDto>(moduleEntity);
	}

	/// <summary>
	/// Updates an existing module record by merging only the changed values from the provided DTO.
	/// </summary>
	public async Task<ModuleDto> UpdateModuleAsync(int moduleId, ModuleDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(ModuleDto));

		if (moduleId != modelDto.ModuleId)
			throw new BadRequestException(moduleId.ToString(), nameof(ModuleDto));

		Module existingEntity = await _repository.Modules
						.FirstOrDefaultAsync(x => x.ModuleId == moduleId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("Module", "ModuleId", moduleId.ToString());

		bool duplicateExists = await _repository.Modules.ExistsAsync(
						x => x.ModuleName.Trim().ToLower() == modelDto.ModuleName.Trim().ToLower()
								&& x.ModuleId != moduleId,
						cancellationToken: cancellationToken);

		if (duplicateExists)
			throw new DuplicateRecordException("Module", "ModuleName");

		Module updatedEntity = MyMapper.MergeChangedValues<Module, ModuleDto>(existingEntity, modelDto);
		_repository.Modules.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("Module", "ModuleId", moduleId.ToString());

		_logger.LogInformation(
						"Module updated. ID: {ModuleId}, Name: {ModuleName}, Time: {Time}",
						updatedEntity.ModuleId,
						updatedEntity.ModuleName,
						DateTime.UtcNow);

		return MyMapper.JsonClone<Module, ModuleDto>(updatedEntity);
	}

	/// <summary>
	/// Deletes a module record identified by the given ID.
	/// </summary>
	public async Task<int> DeleteModuleAsync(int moduleId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (moduleId <= 0)
			throw new BadRequestException(moduleId.ToString(), nameof(ModuleDto));

		Module moduleEntity = await _repository.Modules
						.FirstOrDefaultAsync(x => x.ModuleId == moduleId, trackChanges, cancellationToken)
						?? throw new NotFoundException("Module", "ModuleId", moduleId.ToString());

		await _repository.Modules.DeleteAsync(x => x.ModuleId == moduleId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("Module", "ModuleId", moduleId.ToString());

		_logger.LogWarning(
						"Module deleted. ID: {ModuleId}, Name: {ModuleName}, Time: {Time}",
						moduleEntity.ModuleId,
						moduleEntity.ModuleName,
						DateTime.UtcNow);

		return affected;
	}

	/// <summary>
	/// Retrieves a single module record by its ID.
	/// </summary>
	public async Task<ModuleDto> ModuleAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		Module module = await _repository.Modules.ModuleAsync(id, trackChanges, cancellationToken)
						?? throw new NotFoundException("Module", "ModuleId", id.ToString());

		_logger.LogInformation("Module fetched successfully. ID: {ModuleId}, Name: {ModuleName}, Time: {Time}", module.ModuleId, module.ModuleName, DateTime.UtcNow);
		return MyMapper.JsonClone<Module, ModuleDto>(module);
	}

	/// <summary>
	/// Retrieves all module records from the database.
	/// </summary>
	public async Task<IEnumerable<ModuleDto>> ModulesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		IEnumerable<Module> modules = await _repository.Modules.ModulesAsync(trackChanges, cancellationToken);
		if (!modules.Any())
		{
			_logger.LogWarning("No modules found");
			return Enumerable.Empty<ModuleDto>();
		}
		_logger.LogInformation("Modules fetched successfully");
		return MyMapper.JsonCloneIEnumerableToIEnumerable<Module, ModuleDto>(modules);
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all modules.
	/// </summary>
	public async Task<GridEntity<ModuleDto>> ModuleSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default)
	{
		const string query =
						@"SELECT
            ModuleId,
            ModuleName,
            ModulePath,
            IsActive
          FROM Module";
		const string orderBy = "ModuleName ASC";

		return await _repository.Modules.AdoGridDataAsync<ModuleDto>(query, options, orderBy, "", cancellationToken);
	}

	/// <summary>
	/// Retrieves a lightweight list of all modules suitable for use in dropdown lists.
	/// </summary>
	public async Task<IEnumerable<ModuleForDDLDto>> ModuleForDDLAsync(CancellationToken cancellationToken = default)
	{
		IEnumerable<Module> modules = await _repository.Modules.ListWithSelectAsync(
						x => new Module
						{
							ModuleId = x.ModuleId,
							ModuleName = x.ModuleName
						},
						orderBy: x => x.ModuleName,
						trackChanges: false,
						cancellationToken: cancellationToken);

		if (!modules.Any())
		{
			_logger.LogWarning("No modules found for dropdown list");
			return Enumerable.Empty<ModuleForDDLDto>();
		}
		_logger.LogInformation("Modules fetched successfully for dropdown list");
		return MyMapper.JsonCloneIEnumerableToIEnumerable<Module, ModuleForDDLDto>(modules);
	}
}

///// <summary>
///// Module service implementing business logic for module management.
///// Follows enterprise patterns with structured logging and exception handling.
///// </summary>
//internal sealed class ModuleService : IModuleService
//{
//    private readonly IRepositoryManager _repository;
//    private readonly ILogger<ModuleService> _logger;
//    private readonly IConfiguration _configuration;

//    public ModuleService(IRepositoryManager repository, ILogger<ModuleService> logger, IConfiguration configuration)
//    {
//        _repository = repository;
//        _logger = logger;
//        _configuration = configuration;
//    }

//    /// <summary>
//    /// Retrieves paginated summary grid of modules asynchronously.
//    /// </summary>
//    public async Task<GridEntity<ModuleDto>> ModuleSummaryAsync(bool trackChanges, GridOptions options)
//    {
//        _logger.LogInformation("Fetching module summary grid");

//        string query = "SELECT ModuleId, ModuleName, ModulePath, IsActive FROM Module ORDER BY ModuleName";
//        string orderBy = "ModuleName ASC";

//        var gridEntity = await _repository.Modules.AdoGridDataAsync<ModuleDto>(query, options, orderBy, "");
//        return gridEntity;
//    }

//    /// <summary>
//    /// Retrieves all modules asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<ModuleDto>> ModulesAsync(bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching all modules");

//        var modules = await _repository.Modules.ModulesAsync(trackChanges, cancellationToken);

//        if (!modules.Any())
//        {
//            _logger.LogWarning("No modules found");
//            return Enumerable.Empty<ModuleDto>();
//        }

//        var moduleDtos = MyMapper.JsonCloneIEnumerableToList<Module, ModuleDto>(modules);
//        return moduleDtos;
//    }

//    /// <summary>
//    /// Creates a new module asynchronously.
//    /// </summary>
//    public async Task<ModuleDto> CreateAsync(ModuleDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(ModuleDto));

//        _logger.LogInformation("Creating new module: {ModuleName}", modelDto.ModuleName);

//        // Check for duplicate module name
//        bool moduleExists = await _repository.Modules.ExistsAsync(
//            m => m.ModuleName.Trim().ToLower() == modelDto.ModuleName.Trim().ToLower());

//        if (moduleExists)
//            throw new DuplicateRecordException("Module", "ModuleName");

//        // Map and create
//        Module entity = MyMapper.JsonClone<ModuleDto, Module>(modelDto);
//        modelDto.ModuleId = await _repository.Modules.CreateAndIdAsync(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("Module created successfully with ID: {ModuleId}", modelDto.ModuleId);

//        return modelDto;
//    }

//    /// <summary>
//    /// Updates an existing module asynchronously.
//    /// </summary>
//    public async Task<ModuleDto> UpdateAsync(int key, ModuleDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(ModuleDto));

//        if (key != modelDto.ModuleId)
//            throw new BadRequestException(key.ToString(), nameof(ModuleDto));

//        _logger.LogInformation("Updating module with ID: {ModuleId}", key);

//        // Check if module exists
//        var existingModule = await _repository.Modules.ByIdAsync(
//            m => m.ModuleId == key, trackChanges: false);

//        if (existingModule == null)
//            throw new NotFoundException("Module", "ModuleId", key.ToString());

//        // Check for duplicate name (excluding current record)
//        bool duplicateExists = await _repository.Modules.ExistsAsync(
//            m => m.ModuleName.Trim().ToLower() == modelDto.ModuleName.Trim().ToLower() 
//                 && m.ModuleId != key);

//        if (duplicateExists)
//            throw new DuplicateRecordException("Module", "ModuleName");

//        // Map and update
//        Module entity = MyMapper.JsonClone<ModuleDto, Module>(modelDto);
//        _repository.Modules.UpdateByState(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("Module updated successfully: {ModuleId}", key);

//        return modelDto;
//    }

//    /// <summary>
//    /// Deletes a module by ID asynchronously.
//    /// </summary>
//    public async Task DeleteAsync(int key)
//    {
//        if (key <= 0)
//            throw new BadRequestException("Invalid request!");

//        _logger.LogInformation("Deleting module with ID: {ModuleId}", key);

//        var module = await _repository.Modules.ByIdAsync(
//            m => m.ModuleId == key, trackChanges: false);

//        if (module == null)
//            throw new NotFoundException("Module", "ModuleId", key.ToString());

//        await _repository.Modules.DeleteAsync(m => m.ModuleId == key, trackChanges: false);
//        await _repository.SaveAsync();

//        _logger.LogInformation("Module deleted successfully: {ModuleId}", key);
//    }

//    /// <summary>
//    /// Retrieves a module by ID asynchronously.
//    /// </summary>
//    public async Task<ModuleDto> ModuleAsync(int moduleId, bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        if (moduleId <= 0)
//        {
//            _logger.LogWarning("ModuleAsync called with invalid id: {ModuleId}", moduleId);
//            throw new BadRequestException("Invalid request!");
//        }

//        _logger.LogInformation("Fetching module with ID: {ModuleId}", moduleId);

//        var module = await _repository.Modules.ModuleAsync(moduleId, trackChanges, cancellationToken);

//        if (module == null)
//        {
//            _logger.LogWarning("Module not found with ID: {ModuleId}", moduleId);
//            throw new NotFoundException("Module", "ModuleId", moduleId.ToString());
//        }

//        var moduleDto = MyMapper.JsonClone<Module, ModuleDto>(module);
//        return moduleDto;
//    }

//    /// <summary>
//    /// Retrieves modules for dropdown list asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<ModuleForDDLDto>> ModulesForDDLAsync()
//    {
//        _logger.LogInformation("Fetching modules for dropdown list");

//        var modules = await _repository.Modules.ListWithSelectAsync(
//            x => new Module
//            {
//                ModuleId = x.ModuleId,
//                ModuleName = x.ModuleName
//            },
//            orderBy: x => x.ModuleName,
//            trackChanges: false
//        );

//        if (!modules.Any())
//            return new List<ModuleForDDLDto>();

//        var modulesForDDLDto = MyMapper.JsonCloneIEnumerableToList<Module, ModuleForDDLDto>(modules);
//        return modulesForDDLDto;
//    }
//}
