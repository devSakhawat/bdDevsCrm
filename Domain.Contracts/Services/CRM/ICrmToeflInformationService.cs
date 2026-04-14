// ICrmToeflInformationService.cs
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.CRM;

/// <summary>
/// Service contract for CRM TOEFL information management operations.
/// Defines methods for creating, updating, deleting, and retrieving TOEFL information data.
/// </summary>
public interface ICrmToeflInformationService
{
	/// <summary>
	/// Creates a new TOEFL information record.
	/// </summary>
	/// <param name="entityForCreate">The DTO containing data for the new TOEFL information.</param>
	/// <param name="currentUser">The DTO containing current user information.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The created <see cref="ToeflInformationDto"/> with the newly assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
	/// <exception cref="InvalidCreateOperationException">Thrown when TOEFLInformationId is not 0 for new creation.</exception>
	/// <exception cref="DuplicateRecordException">Thrown when TOEFL information already exists for the applicant.</exception>
	Task<ToeflInformationDto> CreateTOEFLInformationAsync(ToeflInformationDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing TOEFL information record.
	/// </summary>
	/// <param name="toeflInformationId">The ID of the TOEFL information to update.</param>
	/// <param name="modelDto">The DTO containing updated field values.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="ToeflInformationDto"/> reflecting the saved state.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no TOEFL information is found for the given ID.</exception>
	Task<ToeflInformationDto> UpdateTOEFLInformationAsync(int toeflInformationId, ToeflInformationDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

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
	/// <returns>The <see cref="ToeflInformationDto"/> matching the specified ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no TOEFL information is found for the given ID.</exception>
	Task<ToeflInformationDto> TOEFLInformationAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves all TOEFL information records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of all <see cref="ToeflInformationDto"/> records.</returns>
	Task<IEnumerable<ToeflInformationDto>> TOEFLInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves active TOEFL information records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of active <see cref="ToeflInformationDto"/> records.</returns>
	Task<IEnumerable<ToeflInformationDto>> ActiveTOEFLInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves TOEFL information by the specified applicant ID.
	/// </summary>
	/// <param name="applicantId">The ID of the applicant.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="ToeflInformationDto"/> for the specified applicant.</returns>
	/// <exception cref="NotFoundException">Thrown when no TOEFL information is found for the given applicant ID.</exception>
	Task<ToeflInformationDto> TOEFLInformationByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a lightweight list of all TOEFL informations suitable for use in dropdown lists.
	/// </summary>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="ToeflInformationDto"/> for dropdown binding.</returns>
	Task<IEnumerable<ToeflInformationDto>> TOEFLInformationForDDLAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated summary grid of all TOEFL informations.
	/// </summary>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{ToeflInformationDto}"/> containing the paged TOEFL information data.</returns>
	Task<GridEntity<ToeflInformationDto>> TOEFLInformationsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}


//using Application.Shared.Grid;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.CRM;

//namespace Domain.Contracts.Services.CRM;

//public interface ICrmToeflInformationService
//{
//  Task<IEnumerable<ToeflInformationDto>> ToeflinformationsDDLAsync(bool trackChanges = false);
//  Task<IEnumerable<ToeflInformationDto>> ActiveToeflinformationsAsync(bool trackChanges = false);
//  Task<IEnumerable<ToeflInformationDto>> ToeflinformationsAsync(bool trackChanges = false);
//  Task<ToeflInformationDto> ToeflinformationAsync(int id, bool trackChanges = false);
//  Task<ToeflInformationDto> ToeflinformationByApplicantIdAsync(int applicantId, bool trackChanges = false);
//  Task<ToeflInformationDto> CreateNewRecordAsync(ToeflInformationDto dto, UsersDto currentUser);
//  Task<string> UpdateRecordAsync(int key, ToeflInformationDto dto, bool trackChanges);
//  Task<string> DeleteRecordAsync(int key, ToeflInformationDto dto);
//  Task<GridEntity<ToeflInformationDto>> SummaryGrid(GridOptions options);
//}