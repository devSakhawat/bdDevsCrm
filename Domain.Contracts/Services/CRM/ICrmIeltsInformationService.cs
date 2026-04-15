// ICrmIeltsInformationService.cs
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.CRM;

/// <summary>
/// Service contract for CRM IELTS information management operations.
/// Defines methods for creating, updating, deleting, and retrieving IELTS information data.
/// </summary>
public interface ICrmIeltsInformationService
{
	/// <summary>
	/// Creates a new IELTS information record.
	/// </summary>
	/// <param name="entityForCreate">The DTO containing data for the new IELTS information.</param>
	/// <param name="currentUser">The DTO containing current user information.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The created <see cref="IeltsInformationDto"/> with the newly assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
	/// <exception cref="InvalidCreateOperationException">Thrown when IELTSInformationId is not 0 for new creation.</exception>
	/// <exception cref="DuplicateRecordException">Thrown when IELTS information already exists for the applicant.</exception>
	Task<IeltsInformationDto> CreateIELTSInformationAsync(IeltsInformationDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing IELTS information record.
	/// </summary>
	/// <param name="ieltsInformationId">The ID of the IELTS information to update.</param>
	/// <param name="modelDto">The DTO containing updated field values.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="IeltsInformationDto"/> reflecting the saved state.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no IELTS information is found for the given ID.</exception>
	Task<IeltsInformationDto> UpdateIELTSInformationAsync(int ieltsInformationId, IeltsInformationDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes an IELTS information record identified by the given ID.
	/// </summary>
	/// <param name="ieltsInformationId">The ID of the IELTS information to delete.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The number of affected rows.</returns>
	/// <exception cref="IdParametersBadRequestException">Thrown when <paramref name="ieltsInformationId"/> is zero or negative.</exception>
	/// <exception cref="NotFoundException">Thrown when no IELTS information record is found for the given ID.</exception>
	Task<int> DeleteIELTSInformationAsync(int ieltsInformationId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single IELTS information record by its ID.
	/// </summary>
	/// <param name="id">The ID of the IELTS information to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="IeltsInformationDto"/> matching the specified ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no IELTS information is found for the given ID.</exception>
	Task<IeltsInformationDto> IELTSInformationAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves all IELTS information records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of all <see cref="IeltsInformationDto"/> records.</returns>
	Task<IEnumerable<IeltsInformationDto>> IELTSInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves active IELTS information records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of active <see cref="IeltsInformationDto"/> records.</returns>
	Task<IEnumerable<IeltsInformationDto>> ActiveIELTSInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves IELTS information by the specified applicant ID.
	/// </summary>
	/// <param name="applicantId">The ID of the applicant.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="IeltsInformationDto"/> for the specified applicant.</returns>
	/// <exception cref="NotFoundException">Thrown when no IELTS information is found for the given applicant ID.</exception>
	Task<IeltsInformationDto> IELTSInformationByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a lightweight list of all IELTS informations suitable for use in dropdown lists.
	/// </summary>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="IeltsInformationDto"/> for dropdown binding.</returns>
	Task<IEnumerable<IeltsInformationDto>> IELTSInformationForDDLAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated summary grid of all IELTS informations.
	/// </summary>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{IeltsInformationDto}"/> containing the paged IELTS information data.</returns>
	Task<GridEntity<IeltsInformationDto>> IELTSInformationsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}




//using Application.Shared.Grid;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.CRM;

//namespace Domain.Contracts.Services.CRM;

//public interface ICrmIeltsInformationService
//{
//  Task<IEnumerable<IeltsInformationDto>> IELTSinformationsDDLAsync(bool trackChanges = false);
//  Task<IEnumerable<IeltsInformationDto>> ActiveIELTSinformationsAsync(bool trackChanges = false);
//  Task<IEnumerable<IeltsInformationDto>> IELTSinformationsAsync(bool trackChanges = false);
//  Task<IeltsInformationDto> IELTSinformationAsync(int id, bool trackChanges = false);
//  Task<IeltsInformationDto> IELTSinformationByApplicantIdAsync(int applicantId, bool trackChanges = false);
//  Task<IeltsInformationDto> CreateNewRecordAsync(IeltsInformationDto dto, UsersDto currentUser);
//  Task<string> UpdateRecordAsync(int key, IeltsInformationDto dto, bool trackChanges);
//  Task<string> DeleteRecordAsync(int key, IeltsInformationDto dto);
//  Task<GridEntity<IeltsInformationDto>> SummaryGrid(GridOptions options);
//}