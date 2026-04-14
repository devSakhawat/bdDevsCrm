

// ICrmAdditionalDocumentService.cs
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.CRM;

/// <summary>
/// Service contract for CRM additional document management operations.
/// Defines methods for creating, updating, deleting, and retrieving additional document data.
/// </summary>
public interface ICrmAdditionalDocumentService
{
	/// <summary>
	/// Creates a new additional document record.
	/// </summary>
	/// <param name="entityForCreate">The DTO containing data for the new document.</param>
	/// <param name="currentUser">The DTO containing current user information.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The created <see cref="AdditionalDocumentDto"/> with the newly assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
	/// <exception cref="InvalidCreateOperationException">Thrown when AdditionalDocumentId is not 0 for new creation.</exception>
	Task<AdditionalDocumentDto> CreateAdditionalDocumentAsync(AdditionalDocumentDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing additional document record.
	/// </summary>
	/// <param name="additionalDocumentId">The ID of the document to update.</param>
	/// <param name="modelDto">The DTO containing updated field values.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="AdditionalDocumentDto"/> reflecting the saved state.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no document is found for the given ID.</exception>
	Task<AdditionalDocumentDto> UpdateAdditionalDocumentAsync(int additionalDocumentId, AdditionalDocumentDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes an additional document record identified by the given ID.
	/// </summary>
	/// <param name="additionalDocumentId">The ID of the document to delete.</param>
	/// <param name="modelDto">The DTO containing document data for validation.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The number of affected rows.</returns>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no document record is found for the given ID.</exception>
	Task<int> DeleteAdditionalDocumentAsync(int additionalDocumentId, AdditionalDocumentDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single additional document record by its ID.
	/// </summary>
	/// <param name="id">The ID of the document to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="AdditionalDocumentDto"/> matching the specified ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no document is found for the given ID.</exception>
	Task<AdditionalDocumentDto> AdditionalDocumentAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves all additional document records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of all <see cref="AdditionalDocumentDto"/> records.</returns>
	/// <exception cref="GenericListNotFoundException">Thrown when no documents are found.</exception>
	Task<IEnumerable<AdditionalDocumentDto>> AdditionalDocumentsAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves active additional document records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of active <see cref="AdditionalDocumentDto"/> records.</returns>
	/// <exception cref="GenericListNotFoundException">Thrown when no active documents are found.</exception>
	Task<IEnumerable<AdditionalDocumentDto>> ActiveAdditionalDocumentsAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves additional documents by the specified applicant ID.
	/// </summary>
	/// <param name="applicantId">The ID of the applicant.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="AdditionalDocumentDto"/> for the specified applicant.</returns>
	Task<IEnumerable<AdditionalDocumentDto>> AdditionalDocumentsByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a lightweight list of all additional documents suitable for use in dropdown lists.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="AdditionalDocumentDto"/> for dropdown binding.</returns>
	/// <exception cref="GenericListNotFoundException">Thrown when no documents are found.</exception>
	Task<IEnumerable<AdditionalDocumentDto>> AdditionalDocumentForDDLAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated summary grid of all additional documents.
	/// </summary>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{AdditionalDocumentDto}"/> containing the paged document data.</returns>
	Task<GridEntity<AdditionalDocumentDto>> AdditionalDocumentsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}

//using Application.Shared.Grid;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.CRM;

//namespace bdDevCRM.ServiceContract.CRM;

//public interface ICrmAdditionalDocumentService
//{
//  Task<IEnumerable<AdditionalDocumentDto>> AdditionalDocumentsDDLAsync(bool trackChanges = false);
//  Task<IEnumerable<AdditionalDocumentDto>> ActiveAdditionalDocumentsAsync(bool trackChanges = false);
//  Task<IEnumerable<AdditionalDocumentDto>> AdditionalDocumentsAsync(bool trackChanges = false);
//  Task<AdditionalDocumentDto> AdditionalDocumentAsync(int id, bool trackChanges = false);
//  Task<IEnumerable<AdditionalDocumentDto>> AdditionalDocumentsByApplicantIdAsync(int applicantId, bool trackChanges = false);
//  Task<AdditionalDocumentDto> CreateNewRecordAsync(AdditionalDocumentDto dto, UsersDto currentUser);
//  Task<string> UpdateRecordAsync(int key, AdditionalDocumentDto dto, bool trackChanges);
//  Task<string> DeleteRecordAsync(int key, AdditionalDocumentDto dto);
//  Task<GridEntity<AdditionalDocumentDto>> SummaryGrid(GridOptions options);

//  //Task<IEnumerable<AdditionalDocumentDto>> AdditionalDocumentsByApplicantId(int applicantId);
//}