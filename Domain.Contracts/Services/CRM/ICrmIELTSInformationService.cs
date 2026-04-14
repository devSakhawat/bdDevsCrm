// ICrmIELTSInformationService.cs
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.CRM;

/// <summary>
/// Service contract for CRM IELTS information management operations.
/// Defines methods for creating, updating, deleting, and retrieving IELTS information data.
/// </summary>
public interface ICrmIELTSInformationService
{
	/// <summary>
	/// Creates a new IELTS information record.
	/// </summary>
	/// <param name="entityForCreate">The DTO containing data for the new IELTS information.</param>
	/// <param name="currentUser">The DTO containing current user information.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The created <see cref="IELTSInformationDto"/> with the newly assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
	/// <exception cref="InvalidCreateOperationException">Thrown when IELTSInformationId is not 0 for new creation.</exception>
	/// <exception cref="DuplicateRecordException">Thrown when IELTS information already exists for the applicant.</exception>
	Task<IELTSInformationDto> CreateIELTSInformationAsync(IELTSInformationDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing IELTS information record.
	/// </summary>
	/// <param name="ieltsInformationId">The ID of the IELTS information to update.</param>
	/// <param name="modelDto">The DTO containing updated field values.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="IELTSInformationDto"/> reflecting the saved state.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no IELTS information is found for the given ID.</exception>
	Task<IELTSInformationDto> UpdateIELTSInformationAsync(int ieltsInformationId, IELTSInformationDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

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
	/// <returns>The <see cref="IELTSInformationDto"/> matching the specified ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no IELTS information is found for the given ID.</exception>
	Task<IELTSInformationDto> IELTSInformationAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves all IELTS information records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of all <see cref="IELTSInformationDto"/> records.</returns>
	Task<IEnumerable<IELTSInformationDto>> IELTSInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves active IELTS information records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of active <see cref="IELTSInformationDto"/> records.</returns>
	Task<IEnumerable<IELTSInformationDto>> ActiveIELTSInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves IELTS information by the specified applicant ID.
	/// </summary>
	/// <param name="applicantId">The ID of the applicant.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="IELTSInformationDto"/> for the specified applicant.</returns>
	/// <exception cref="NotFoundException">Thrown when no IELTS information is found for the given applicant ID.</exception>
	Task<IELTSInformationDto> IELTSInformationByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a lightweight list of all IELTS informations suitable for use in dropdown lists.
	/// </summary>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="IELTSInformationDto"/> for dropdown binding.</returns>
	Task<IEnumerable<IELTSInformationDto>> IELTSInformationForDDLAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated summary grid of all IELTS informations.
	/// </summary>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{IELTSInformationDto}"/> containing the paged IELTS information data.</returns>
	Task<GridEntity<IELTSInformationDto>> IELTSInformationsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}




//using Application.Shared.Grid;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.CRM;

//namespace Domain.Contracts.Services.CRM;

//public interface ICrmIELTSInformationService
//{
//  Task<IEnumerable<IELTSInformationDto>> IELTSinformationsDDLAsync(bool trackChanges = false);
//  Task<IEnumerable<IELTSInformationDto>> ActiveIELTSinformationsAsync(bool trackChanges = false);
//  Task<IEnumerable<IELTSInformationDto>> IELTSinformationsAsync(bool trackChanges = false);
//  Task<IELTSInformationDto> IELTSinformationAsync(int id, bool trackChanges = false);
//  Task<IELTSInformationDto> IELTSinformationByApplicantIdAsync(int applicantId, bool trackChanges = false);
//  Task<IELTSInformationDto> CreateNewRecordAsync(IELTSInformationDto dto, UsersDto currentUser);
//  Task<string> UpdateRecordAsync(int key, IELTSInformationDto dto, bool trackChanges);
//  Task<string> DeleteRecordAsync(int key, IELTSInformationDto dto);
//  Task<GridEntity<IELTSInformationDto>> SummaryGrid(GridOptions options);
//}