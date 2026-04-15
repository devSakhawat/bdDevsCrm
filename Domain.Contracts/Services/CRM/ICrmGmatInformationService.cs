// ICrmGmatInformationService.cs
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.CRM;

/// <summary>
/// Service contract for CRM GMAT information management operations.
/// Defines methods for creating, updating, deleting, and retrieving GMAT information data.
/// </summary>
public interface ICrmGmatInformationService
{
	/// <summary>
	/// Creates a new GMAT information record.
	/// </summary>
	/// <param name="entityForCreate">The DTO containing data for the new GMAT information.</param>
	/// <param name="currentUser">The DTO containing current user information.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The created <see cref="GmatInformationDto"/> with the newly assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
	/// <exception cref="InvalidCreateOperationException">Thrown when GMATInformationId is not 0 for new creation.</exception>
	/// <exception cref="DuplicateRecordException">Thrown when GMAT information already exists for the applicant.</exception>
	Task<GmatInformationDto> CreateGMATInformationAsync(GmatInformationDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing GMAT information record.
	/// </summary>
	/// <param name="gmatInformationId">The ID of the GMAT information to update.</param>
	/// <param name="modelDto">The DTO containing updated field values.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="GmatInformationDto"/> reflecting the saved state.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no GMAT information is found for the given ID.</exception>
	Task<GmatInformationDto> UpdateGMATInformationAsync(int gmatInformationId, GmatInformationDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes a GMAT information record identified by the given ID.
	/// </summary>
	/// <param name="gmatInformationId">The ID of the GMAT information to delete.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The number of affected rows.</returns>
	/// <exception cref="IdParametersBadRequestException">Thrown when <paramref name="gmatInformationId"/> is zero or negative.</exception>
	/// <exception cref="NotFoundException">Thrown when no GMAT information record is found for the given ID.</exception>
	Task<int> DeleteGMATInformationAsync(int gmatInformationId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single GMAT information record by its ID.
	/// </summary>
	/// <param name="id">The ID of the GMAT information to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="GmatInformationDto"/> matching the specified ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no GMAT information is found for the given ID.</exception>
	Task<GmatInformationDto> GMATInformationAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves all GMAT information records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of all <see cref="GmatInformationDto"/> records.</returns>
	Task<IEnumerable<GmatInformationDto>> GMATInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves active GMAT information records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of active <see cref="GmatInformationDto"/> records.</returns>
	Task<IEnumerable<GmatInformationDto>> ActiveGMATInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves GMAT information by the specified applicant ID.
	/// </summary>
	/// <param name="applicantId">The ID of the applicant.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="GmatInformationDto"/> for the specified applicant.</returns>
	/// <exception cref="NotFoundException">Thrown when no GMAT information is found for the given applicant ID.</exception>
	Task<GmatInformationDto> GMATInformationByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a lightweight list of all GMAT informations suitable for use in dropdown lists.
	/// </summary>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="GmatInformationDto"/> for dropdown binding.</returns>
	Task<IEnumerable<GmatInformationDto>> GMATInformationForDDLAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated summary grid of all GMAT informations.
	/// </summary>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{GmatInformationDto}"/> containing the paged GMAT information data.</returns>
	Task<GridEntity<GmatInformationDto>> GMATInformationsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}

//using Application.Shared.Grid;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.CRM;

//namespace Domain.Contracts.Services.CRM;

//public interface ICrmGmatInformationService
//{
//  Task<IEnumerable<GmatInformationDto>> GmatinformationsDDLAsync(bool trackChanges = false);
//  Task<IEnumerable<GmatInformationDto>> ActiveGmatinformationsAsync(bool trackChanges = false);
//  Task<IEnumerable<GmatInformationDto>> GmatinformationsAsync(bool trackChanges = false);
//  Task<GmatInformationDto> GmatinformationAsync(int id, bool trackChanges = false);
//  Task<GmatInformationDto> GmatinformationByApplicantIdAsync(int applicantId, bool trackChanges = false);
//  Task<GmatInformationDto> CreateNewRecordAsync(GmatInformationDto dto, UsersDto currentUser);
//  Task<string> UpdateRecordAsync(int key, GmatInformationDto dto, bool trackChanges);
//  Task<string> DeleteRecordAsync(int key, GmatInformationDto dto);
//  Task<GridEntity<GmatInformationDto>> SummaryGrid(GridOptions options);
//}