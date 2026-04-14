// ICrmInstituteTypeService.cs
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.CRM;

/// <summary>
/// Service contract for CRM institute type management operations.
/// Defines methods for creating, updating, deleting, and retrieving institute type data.
/// </summary>
public interface ICrmInstituteTypeService
{
	/// <summary>
	/// Creates a new institute type record.
	/// </summary>
	/// <param name="entityForCreate">The DTO containing data for the new institute type.</param>
	/// <param name="currentUser">The DTO containing current user information.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The created <see cref="CrmInstituteTypeDto"/> with the newly assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
	/// <exception cref="InvalidCreateOperationException">Thrown when InstituteTypeId is not 0 for new creation.</exception>
	/// <exception cref="DuplicateRecordException">Thrown when an institute type with the same name already exists.</exception>
	Task<CrmInstituteTypeDto> CreateInstituteTypeAsync(CrmInstituteTypeDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing institute type record.
	/// </summary>
	/// <param name="instituteTypeId">The ID of the institute type to update.</param>
	/// <param name="modelDto">The DTO containing updated field values.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="CrmInstituteTypeDto"/> reflecting the saved state.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no institute type is found for the given ID.</exception>
	Task<CrmInstituteTypeDto> UpdateInstituteTypeAsync(int instituteTypeId, CrmInstituteTypeDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes an institute type record identified by the given ID.
	/// </summary>
	/// <param name="instituteTypeId">The ID of the institute type to delete.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The number of affected rows.</returns>
	/// <exception cref="IdParametersBadRequestException">Thrown when <paramref name="instituteTypeId"/> is zero or negative.</exception>
	/// <exception cref="NotFoundException">Thrown when no institute type record is found for the given ID.</exception>
	Task<int> DeleteInstituteTypeAsync(int instituteTypeId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves all institute type records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of all <see cref="CrmInstituteTypeDto"/> records.</returns>
	Task<IEnumerable<CrmInstituteTypeDto>> InstituteTypesAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a lightweight list of all institute types suitable for use in dropdown lists.
	/// </summary>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="CrmInstituteTypeDto"/> for dropdown binding.</returns>
	Task<IEnumerable<CrmInstituteTypeDto>> InstituteTypeForDDLAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated summary grid of all institute types.
	/// </summary>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{CrmInstituteTypeDto}"/> containing the paged institute type data.</returns>
	Task<GridEntity<CrmInstituteTypeDto>> InstituteTypesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);

	/// <summary>
	/// Saves an institute type record (create or update).
	/// </summary>
	/// <param name="instituteTypeId">The ID of the institute type (0 for create).</param>
	/// <param name="modelDto">The DTO containing institute type data.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A message indicating the result of the operation.</returns>
	Task<string> SaveOrUpdateInstituteTypeAsync(int instituteTypeId, CrmInstituteTypeDto modelDto, CancellationToken cancellationToken = default);
}


//using bdDevs.Shared.DataTransferObjects.CRM;
//using Application.Shared.Grid;

//namespace Domain.Contracts.Services.CRM;

//public interface ICrmInstituteTypeService
//{
//	Task<IEnumerable<CrmInstituteTypeDto>> InstituteTypesDDLAsync(bool trackChanges = false);
//	Task<GridEntity<CrmInstituteTypeDto>> SummaryGrid(GridOptions options);
//	Task<string> CreateNewRecordAsync(CrmInstituteTypeDto dto);
//	Task<string> UpdateRecordAsync(int key, CrmInstituteTypeDto dto, bool trackChanges);
//	Task<string> DeleteRecordAsync(int key, CrmInstituteTypeDto dto);
//	Task<string> SaveOrUpdateAsync(int key, CrmInstituteTypeDto dto);
//}
