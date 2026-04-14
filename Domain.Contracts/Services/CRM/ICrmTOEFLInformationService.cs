// ICrmTOEFLInformationService.cs
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.CRM;

/// <summary>
/// Service contract for CRM TOEFL information management operations.
/// Defines methods for creating, updating, deleting, and retrieving TOEFL information data.
/// </summary>
public interface ICrmTOEFLInformationService
{
	/// <summary>
	/// Creates a new TOEFL information record.
	/// </summary>
	/// <param name="entityForCreate">The DTO containing data for the new TOEFL information.</param>
	/// <param name="currentUser">The DTO containing current user information.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The created <see cref="TOEFLInformationDto"/> with the newly assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
	/// <exception cref="InvalidCreateOperationException">Thrown when TOEFLInformationId is not 0 for new creation.</exception>
	/// <exception cref="DuplicateRecordException">Thrown when TOEFL information already exists for the applicant.</exception>
	Task<TOEFLInformationDto> CreateTOEFLInformationAsync(TOEFLInformationDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing TOEFL information record.
	/// </summary>
	/// <param name="toeflInformationId">The ID of the TOEFL information to update.</param>
	/// <param name="modelDto">The DTO containing updated field values.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="TOEFLInformationDto"/> reflecting the saved state.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no TOEFL information is found for the given ID.</exception>
	Task<TOEFLInformationDto> UpdateTOEFLInformationAsync(int toeflInformationId, TOEFLInformationDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes a TOEFL information record identified by the given ID.
	/// </summary>
	/// <param name="toeflInformationId">The ID of the TOEFL information to delete.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The number of affected rows.</returns>
	/// <exception cref="IdParametersBadRequestException">Thrown when <paramref name="toeflInformationId"/> is zero or negative.</exception>
	/// <exception cref="NotFoundException">Thrown when no TOEFL information record is found for the given ID.</exception>
	Task<int> DeleteTOEFLInformationAsync(int toeflInformationId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single TOEFL information record by its ID.
	/// </summary>
	/// <param name="id">The ID of the TOEFL information to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="TOEFLInformationDto"/> matching the specified ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no TOEFL information is found for the given ID.</exception>
	Task<TOEFLInformationDto> TOEFLInformationAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves all TOEFL information records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of all <see cref="TOEFLInformationDto"/> records.</returns>
	Task<IEnumerable<TOEFLInformationDto>> TOEFLInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves active TOEFL information records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of active <see cref="TOEFLInformationDto"/> records.</returns>
	Task<IEnumerable<TOEFLInformationDto>> ActiveTOEFLInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves TOEFL information by the specified applicant ID.
	/// </summary>
	/// <param name="applicantId">The ID of the applicant.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="TOEFLInformationDto"/> for the specified applicant.</returns>
	/// <exception cref="NotFoundException">Thrown when no TOEFL information is found for the given applicant ID.</exception>
	Task<TOEFLInformationDto> TOEFLInformationByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a lightweight list of all TOEFL informations suitable for use in dropdown lists.
	/// </summary>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="TOEFLInformationDto"/> for dropdown binding.</returns>
	Task<IEnumerable<TOEFLInformationDto>> TOEFLInformationForDDLAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated summary grid of all TOEFL informations.
	/// </summary>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{TOEFLInformationDto}"/> containing the paged TOEFL information data.</returns>
	Task<GridEntity<TOEFLInformationDto>> TOEFLInformationsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}


//using Application.Shared.Grid;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.CRM;

//namespace Domain.Contracts.Services.CRM;

//public interface ICrmTOEFLInformationService
//{
//  Task<IEnumerable<TOEFLInformationDto>> ToeflinformationsDDLAsync(bool trackChanges = false);
//  Task<IEnumerable<TOEFLInformationDto>> ActiveToeflinformationsAsync(bool trackChanges = false);
//  Task<IEnumerable<TOEFLInformationDto>> ToeflinformationsAsync(bool trackChanges = false);
//  Task<TOEFLInformationDto> ToeflinformationAsync(int id, bool trackChanges = false);
//  Task<TOEFLInformationDto> ToeflinformationByApplicantIdAsync(int applicantId, bool trackChanges = false);
//  Task<TOEFLInformationDto> CreateNewRecordAsync(TOEFLInformationDto dto, UsersDto currentUser);
//  Task<string> UpdateRecordAsync(int key, TOEFLInformationDto dto, bool trackChanges);
//  Task<string> DeleteRecordAsync(int key, TOEFLInformationDto dto);
//  Task<GridEntity<TOEFLInformationDto>> SummaryGrid(GridOptions options);
//}