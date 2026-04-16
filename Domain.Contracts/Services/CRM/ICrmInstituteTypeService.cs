// ICrmInstituteTypeService.cs
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.CRM;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.CRM;

/// <summary>
/// Service contract for CRM institute type management operations.
/// Defines methods for creating, updating, deleting, and retrieving institute type data.
/// </summary>
public interface ICrmInstituteTypeService
{
	/// <summary>
	/// Creates a new institute type record using CRUD Record pattern.
	/// </summary>
	Task<CrmInstituteTypeDto> CreateAsync(CreateCrmInstituteTypeRecord record, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing institute type record using CRUD Record pattern.
	/// </summary>
	Task<CrmInstituteTypeDto> UpdateAsync(UpdateCrmInstituteTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes an institute type record using CRUD Record pattern.
	/// </summary>
	Task DeleteAsync(DeleteCrmInstituteTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default);

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
