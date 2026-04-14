using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

/// <summary>
/// Service contract for module management operations.
/// Defines methods for creating, updating, deleting, and retrieving module data.
/// </summary>
public interface IModuleService
{
	/// <summary>
	/// Creates a new module record after validating for null input and duplicate module name.
	/// </summary>
	/// <param name="entityForCreate">The DTO containing data for the new module.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The created <see cref="ModuleDto"/> with the newly assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
	/// <exception cref="DuplicateRecordException">Thrown when a module with the same name already exists.</exception>
	Task<ModuleDto> CreateModuleAsync(ModuleDto entityForCreate, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing module record by merging only the changed values from the provided DTO.
	/// Validates ID consistency, null input, record existence, and duplicate name constraints.
	/// </summary>
	/// <param name="moduleId">The ID of the module to update.</param>
	/// <param name="modelDto">The DTO containing updated field values.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="ModuleDto"/> reflecting the saved state.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no module is found for the given ID.</exception>
	/// <exception cref="DuplicateRecordException">Thrown when another module with the same name already exists.</exception>
	Task<ModuleDto> UpdateModuleAsync(int moduleId, ModuleDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes a module record identified by the given ID.
	/// Validates that the ID is positive and that the record exists before deletion.
	/// </summary>
	/// <param name="moduleId">The ID of the module to delete.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The number of affected rows.</returns>
	/// <exception cref="IdParametersBadRequestException">Thrown when <paramref name="moduleId"/> is zero or negative.</exception>
	/// <exception cref="NotFoundException">Thrown when no module record is found for the given ID.</exception>
	Task<int> DeleteModuleAsync(int moduleId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single module record by its ID.
	/// </summary>
	/// <param name="id">The ID of the module to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="ModuleDto"/> matching the specified ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no module is found for the given ID.</exception>
	Task<ModuleDto> ModuleAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves all module records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of all <see cref="ModuleDto"/> records.</returns>
	Task<IEnumerable<ModuleDto>> ModulesAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated summary grid of all modules.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{ModuleDto}"/> containing the paged module summary data.</returns>
	Task<GridEntity<ModuleDto>> ModuleSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a lightweight list of all modules suitable for use in dropdown lists.
	/// Returns only the module ID and name, ordered by name.
	/// </summary>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="ModuleForDDLDto"/> for dropdown binding.</returns>
	Task<IEnumerable<ModuleForDDLDto>> ModuleForDDLAsync(CancellationToken cancellationToken = default);
}






//using Application.Shared.Grid;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

//namespace Application.Services.Core.SystemAdmin;

//public interface IModuleService
//{
//    Task<GridEntity<ModuleDto>> ModuleSummary(bool trackChanges, GridOptions options);
//    Task<List<ModuleDto>> ModulesAsync(UsersDto currentUser, bool trackChanges);
//    Task<ModuleDto> CreateModuleAsync(ModuleDto moduleDto);
//    Task<ModuleDto> UpdateModuleAsync(int key, ModuleDto moduleDto);
//    Task DeleteModuleAsync(int key);
//}
