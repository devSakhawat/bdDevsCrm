// ICrmPteInformationService.cs
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.CRM;

/// <summary>
/// Service contract for CRM PTE information management operations.
/// Defines methods for creating, updating, deleting, and retrieving PTE information data.
/// </summary>
public interface ICrmPteInformationService
{
	/// <summary>
	/// Creates a new PTE information record.
	/// </summary>
	/// <param name="entityForCreate">The DTO containing data for the new PTE information.</param>
	/// <param name="currentUser">The DTO containing current user information.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The created <see cref="PteInformationDto"/> with the newly assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
	/// <exception cref="InvalidCreateOperationException">Thrown when PTEInformationId is not 0 for new creation.</exception>
	/// <exception cref="DuplicateRecordException">Thrown when PTE information already exists for the applicant.</exception>
	Task<PteInformationDto> CreatePTEInformationAsync(PteInformationDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing PTE information record.
	/// </summary>
	/// <param name="pteInformationId">The ID of the PTE information to update.</param>
	/// <param name="modelDto">The DTO containing updated field values.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="PteInformationDto"/> reflecting the saved state.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no PTE information is found for the given ID.</exception>
	Task<PteInformationDto> UpdatePTEInformationAsync(int pteInformationId, PteInformationDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes a PTE information record identified by the given ID.
	/// </summary>
	/// <param name="pteInformationId">The ID of the PTE information to delete.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The number of affected rows.</returns>
	/// <exception cref="IdParametersBadRequestException">Thrown when <paramref name="pteInformationId"/> is zero or negative.</exception>
	/// <exception cref="NotFoundException">Thrown when no PTE information record is found for the given ID.</exception>
	Task<int> DeletePTEInformationAsync(int pteInformationId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single PTE information record by its ID.
	/// </summary>
	/// <param name="id">The ID of the PTE information to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="PteInformationDto"/> matching the specified ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no PTE information is found for the given ID.</exception>
	Task<PteInformationDto> PTEInformationAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves all PTE information records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of all <see cref="PteInformationDto"/> records.</returns>
	Task<IEnumerable<PteInformationDto>> PTEInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves active PTE information records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of active <see cref="PteInformationDto"/> records.</returns>
	Task<IEnumerable<PteInformationDto>> ActivePTEInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves PTE information by the specified applicant ID.
	/// </summary>
	/// <param name="applicantId">The ID of the applicant.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="PteInformationDto"/> for the specified applicant.</returns>
	/// <exception cref="NotFoundException">Thrown when no PTE information is found for the given applicant ID.</exception>
	Task<PteInformationDto> PTEInformationByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a lightweight list of all PTE informations suitable for use in dropdown lists.
	/// </summary>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="PteInformationDto"/> for dropdown binding.</returns>
	Task<IEnumerable<PteInformationDto>> PTEInformationForDDLAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated summary grid of all PTE informations.
	/// </summary>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{PteInformationDto}"/> containing the paged PTE information data.</returns>
	Task<GridEntity<PteInformationDto>> PTEInformationsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}


//using Application.Shared.Grid;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.CRM;

//namespace Domain.Contracts.Services.CRM;

//public interface ICrmPteInformationService
//{
//  Task<IEnumerable<PteInformationDto>> PTEInformationsDDLAsync(bool trackChanges = false);
//  Task<IEnumerable<PteInformationDto>> ActivePTEInformationsAsync(bool trackChanges = false);
//  Task<IEnumerable<PteInformationDto>> PTEInformationsAsync(bool trackChanges = false);
//  Task<PteInformationDto> PTEInformationAsync(int id, bool trackChanges = false);
//  Task<PteInformationDto> PTEInformationByApplicantIdAsync(int applicantId, bool trackChanges = false);
//  Task<PteInformationDto> CreateNewRecordAsync(PteInformationDto dto, UsersDto currentUser);
//  Task<string> UpdateRecordAsync(int key, PteInformationDto dto, bool trackChanges);
//  Task<string> DeleteRecordAsync(int key, PteInformationDto dto);
//  Task<GridEntity<PteInformationDto>> SummaryGrid(GridOptions options);
//}