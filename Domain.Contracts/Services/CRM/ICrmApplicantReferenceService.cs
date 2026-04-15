// ICrmApplicantReferenceService.cs
using Domain.Contracts.Services.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.CRM;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.CRM;

/// <summary>
/// Service contract for CRM applicant reference management operations.
/// Defines methods for creating, updating, deleting, and retrieving applicant reference data.
/// </summary>
public interface ICrmApplicantReferenceService
{
	/// <summary>
	/// Creates a new applicant reference record.
	/// </summary>
	/// <param name="entityForCreate">The DTO containing data for the new reference.</param>
	/// <param name="currentUser">The DTO containing current user information.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The created <see cref="ApplicantReferenceDto"/> with the newly assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
	/// <exception cref="InvalidCreateOperationException">Thrown when ApplicantReferenceId is not 0 for new creation.</exception>
	Task<ApplicantReferenceDto> CreateApplicantReferenceAsync(ApplicantReferenceDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing applicant reference record.
	/// </summary>
	/// <param name="applicantReferenceId">The ID of the reference to update.</param>
	/// <param name="modelDto">The DTO containing updated field values.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="ApplicantReferenceDto"/> reflecting the saved state.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no reference is found for the given ID.</exception>
	Task<ApplicantReferenceDto> UpdateApplicantReferenceAsync(int applicantReferenceId, ApplicantReferenceDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes an applicant reference record identified by the given ID.
	/// </summary>
	/// <param name="applicantReferenceId">The ID of the reference to delete.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The number of affected rows.</returns>
	/// <exception cref="IdParametersBadRequestException">Thrown when <paramref name="applicantReferenceId"/> is zero or negative.</exception>
	/// <exception cref="NotFoundException">Thrown when no reference record is found for the given ID.</exception>
	Task<int> DeleteApplicantReferenceAsync(int applicantReferenceId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single applicant reference record by its ID.
	/// </summary>
	/// <param name="id">The ID of the reference to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="ApplicantReferenceDto"/> matching the specified ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no reference is found for the given ID.</exception>
	Task<ApplicantReferenceDto> ApplicantReferenceAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves all applicant reference records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of all <see cref="ApplicantReferenceDto"/> records.</returns>
	Task<IEnumerable<ApplicantReferenceDto>> ApplicantReferencesAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves active applicant reference records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of active <see cref="ApplicantReferenceDto"/> records.</returns>
	Task<IEnumerable<ApplicantReferenceDto>> ActiveApplicantReferencesAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves applicant references by the specified applicant ID.
	/// </summary>
	/// <param name="applicantId">The ID of the applicant.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="ApplicantReferenceDto"/> for the specified applicant.</returns>
	Task<IEnumerable<ApplicantReferenceDto>> ApplicantReferencesByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a lightweight list of all applicant references suitable for use in dropdown lists.
	/// </summary>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="ApplicantReferenceDto"/> for dropdown binding.</returns>
	Task<IEnumerable<ApplicantReferenceDDLDto>> ApplicantReferenceForDDLAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated summary grid of all applicant references.
	/// </summary>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{ApplicantReferenceDto}"/> containing the paged reference data.</returns>
	Task<GridEntity<ApplicantReferenceDto>> ApplicantReferencesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}

//using Application.Shared.Grid;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.CRM;

//namespace Domain.Contracts.Services.CRM;

//public interface ICrmApplicantReferenceService
//{
//  Task<IEnumerable<ApplicantReferenceDto>> ApplicantReferencesDDLAsync(bool trackChanges = false);
//  Task<IEnumerable<ApplicantReferenceDto>> ActiveApplicantReferencesAsync(bool trackChanges = false);
//  Task<IEnumerable<ApplicantReferenceDto>> ApplicantReferencesAsync(bool trackChanges = false);
//  Task<ApplicantReferenceDto> ApplicantReferenceAsync(int id, bool trackChanges = false);
//  Task<IEnumerable<ApplicantReferenceDto>> ApplicantReferencesByApplicantIdAsync(int applicantId, bool trackChanges = false);
//  Task<ApplicantReferenceDto> CreateNewRecordAsync(ApplicantReferenceDto dto, UsersDto currentUser);
//  Task<string> UpdateRecordAsync(int key, ApplicantReferenceDto dto, bool trackChanges);
//  Task<string> DeleteRecordAsync(int key, ApplicantReferenceDto dto);
//  Task<GridEntity<ApplicantReferenceDto>> SummaryGrid(GridOptions options);
//}