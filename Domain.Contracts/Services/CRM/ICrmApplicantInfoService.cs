// ICrmApplicantInfoService.cs
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.CRM;

/// <summary>
/// Service contract for CRM applicant info management operations.
/// Defines methods for creating, updating, deleting, and retrieving applicant info data.
/// </summary>
public interface ICrmApplicantInfoService
{
	/// <summary>
	/// Creates a new applicant info record.
	/// </summary>
	/// <param name="entityForCreate">The DTO containing data for the new applicant info.</param>
	/// <param name="currentUser">The DTO containing current user information.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The created <see cref="ApplicantInfoDto"/> with the newly assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
	/// <exception cref="InvalidCreateOperationException">Thrown when ApplicantId is not 0 for new creation.</exception>
	/// <exception cref="DuplicateRecordException">Thrown when an applicant with the same email or application ID already exists.</exception>
	Task<ApplicantInfoDto> CreateApplicantInfoAsync(ApplicantInfoDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing applicant info record.
	/// </summary>
	/// <param name="applicantId">The ID of the applicant info to update.</param>
	/// <param name="modelDto">The DTO containing updated field values.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="ApplicantInfoDto"/> reflecting the saved state.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no applicant info is found for the given ID.</exception>
	Task<ApplicantInfoDto> UpdateApplicantInfoAsync(int applicantId, ApplicantInfoDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes an applicant info record identified by the given ID.
	/// </summary>
	/// <param name="applicantId">The ID of the applicant info to delete.</param>
	/// <param name="modelDto">The DTO containing applicant info data for validation.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The number of affected rows.</returns>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no applicant info record is found for the given ID.</exception>
	Task<int> DeleteApplicantInfoAsync(int applicantId, ApplicantInfoDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single applicant info record by its ID.
	/// </summary>
	/// <param name="id">The ID of the applicant info to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="ApplicantInfoDto"/> matching the specified ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no applicant info is found for the given ID.</exception>
	Task<ApplicantInfoDto> ApplicantInfoAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single applicant info record by application ID.
	/// </summary>
	/// <param name="applicationId">The ID of the application.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="ApplicantInfoDto"/> matching the specified application ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no applicant info is found for the given application ID.</exception>
	Task<ApplicantInfoDto> ApplicantInfoByApplicationIdAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single applicant info record by email address.
	/// </summary>
	/// <param name="email">The email address to search for.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="ApplicantInfoDto"/> matching the specified email.</returns>
	/// <exception cref="NotFoundException">Thrown when no applicant info is found for the given email.</exception>
	Task<ApplicantInfoDto> ApplicantInfoByEmailAsync(string email, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves all applicant info records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of all <see cref="ApplicantInfoDto"/> records.</returns>
	/// <exception cref="GenericListNotFoundException">Thrown when no applicant infos are found.</exception>
	Task<IEnumerable<ApplicantInfoDto>> ApplicantInfosAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves active applicant info records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of active <see cref="ApplicantInfoDto"/> records.</returns>
	/// <exception cref="GenericListNotFoundException">Thrown when no active applicant infos are found.</exception>
	Task<IEnumerable<ApplicantInfoDto>> ActiveApplicantInfosAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a lightweight list of all applicant infos suitable for use in dropdown lists.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="ApplicantInfoDto"/> for dropdown binding.</returns>
	/// <exception cref="GenericListNotFoundException">Thrown when no applicant infos are found.</exception>
	Task<IEnumerable<ApplicantInfoDto>> ApplicantInfoForDDLAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated summary grid of all applicant infos.
	/// </summary>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{ApplicantInfoDto}"/> containing the paged applicant info data.</returns>
	Task<GridEntity<ApplicantInfoDto>> ApplicantInfosSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}


//using Application.Shared.Grid;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.CRM;

//namespace bdDevCRM.ServiceContract.CRM;

//public interface ICrmApplicantInfoService
//{
//  Task<IEnumerable<ApplicantInfoDto>> ApplicantInfosDDLAsync(bool trackChanges = false);
//  Task<IEnumerable<ApplicantInfoDto>> ActiveApplicantInfosAsync(bool trackChanges = false);
//  Task<IEnumerable<ApplicantInfoDto>> ApplicantInfosAsync(bool trackChanges = false);
//  Task<ApplicantInfoDto> ApplicantInfoAsync(int id, bool trackChanges = false);
//  Task<ApplicantInfoDto> ApplicantInfoByApplicationIdAsync(int applicationId, bool trackChanges = false);
//  Task<ApplicantInfoDto> CreateNewRecordAsync(ApplicantInfoDto dto, UsersDto currentUser);
//  Task<string> UpdateRecordAsync(int key, ApplicantInfoDto dto, bool trackChanges);
//  Task<string> DeleteRecordAsync(int key, ApplicantInfoDto dto);
//  Task<ApplicantInfoDto> ApplicantInfoByEmailAsync(string email, bool trackChanges = false);
//  Task<GridEntity<ApplicantInfoDto>> SummaryGrid(GridOptions options);
//}