// ICrmStatementOfPurposeService.cs
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.CRM;

/// <summary>
/// Service contract for CRM statement of purpose management operations.
/// Defines methods for creating, updating, deleting, and retrieving statement of purpose data.
/// </summary>
public interface ICrmStatementOfPurposeService
{
	/// <summary>
	/// Creates a new statement of purpose record.
	/// </summary>
	/// <param name="entityForCreate">The DTO containing data for the new statement of purpose.</param>
	/// <param name="currentUser">The DTO containing current user information.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The created <see cref="StatementOfPurposeDto"/> with the newly assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
	/// <exception cref="InvalidCreateOperationException">Thrown when StatementOfPurposeId is not 0 for new creation.</exception>
	/// <exception cref="DuplicateRecordException">Thrown when statement of purpose already exists for the applicant.</exception>
	Task<StatementOfPurposeDto> CreateStatementOfPurposeAsync(StatementOfPurposeDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing statement of purpose record.
	/// </summary>
	/// <param name="statementOfPurposeId">The ID of the statement of purpose to update.</param>
	/// <param name="modelDto">The DTO containing updated field values.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="StatementOfPurposeDto"/> reflecting the saved state.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no statement of purpose is found for the given ID.</exception>
	Task<StatementOfPurposeDto> UpdateStatementOfPurposeAsync(int statementOfPurposeId, StatementOfPurposeDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes a statement of purpose record identified by the given ID.
	/// </summary>
	/// <param name="statementOfPurposeId">The ID of the statement of purpose to delete.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The number of affected rows.</returns>
	/// <exception cref="IdParametersBadRequestException">Thrown when <paramref name="statementOfPurposeId"/> is zero or negative.</exception>
	/// <exception cref="NotFoundException">Thrown when no statement of purpose record is found for the given ID.</exception>
	Task<int> DeleteStatementOfPurposeAsync(int statementOfPurposeId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single statement of purpose record by its ID.
	/// </summary>
	/// <param name="id">The ID of the statement of purpose to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="StatementOfPurposeDto"/> matching the specified ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no statement of purpose is found for the given ID.</exception>
	Task<StatementOfPurposeDto> StatementOfPurposeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves all statement of purpose records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of all <see cref="StatementOfPurposeDto"/> records.</returns>
	Task<IEnumerable<StatementOfPurposeDto>> StatementOfPurposesAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves active statement of purpose records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of active <see cref="StatementOfPurposeDto"/> records.</returns>
	Task<IEnumerable<StatementOfPurposeDto>> ActiveStatementOfPurposesAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves statement of purpose by the specified applicant ID.
	/// </summary>
	/// <param name="applicantId">The ID of the applicant.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="StatementOfPurposeDto"/> for the specified applicant.</returns>
	/// <exception cref="NotFoundException">Thrown when no statement of purpose is found for the given applicant ID.</exception>
	Task<StatementOfPurposeDto> StatementOfPurposeByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a lightweight list of all statement of purposes suitable for use in dropdown lists.
	/// </summary>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="StatementOfPurposeDto"/> for dropdown binding.</returns>
	Task<IEnumerable<StatementOfPurposeDto>> StatementOfPurposeForDDLAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated summary grid of all statement of purposes.
	/// </summary>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{StatementOfPurposeDto}"/> containing the paged statement of purpose data.</returns>
	Task<GridEntity<StatementOfPurposeDto>> StatementOfPurposesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}


//using Application.Shared.Grid;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.CRM;

//namespace bdDevCRM.ServiceContract.CRM;

//public interface ICrmStatementOfPurposeService
//{
//  Task<IEnumerable<StatementOfPurposeDto>> StatementOfPurposesDDLAsync(bool trackChanges = false);
//  Task<IEnumerable<StatementOfPurposeDto>> ActiveStatementOfPurposesAsync(bool trackChanges = false);
//  Task<IEnumerable<StatementOfPurposeDto>> StatementOfPurposesAsync(bool trackChanges = false);
//  Task<StatementOfPurposeDto> StatementOfPurposeAsync(int id, bool trackChanges = false);
//  Task<StatementOfPurposeDto> StatementOfPurposeByApplicantIdAsync(int applicantId, bool trackChanges = false);
//  Task<StatementOfPurposeDto> CreateNewRecordAsync(StatementOfPurposeDto dto, UsersDto currentUser);
//  Task<string> UpdateRecordAsync(int key, StatementOfPurposeDto dto, bool trackChanges);
//  Task<string> DeleteRecordAsync(int key, StatementOfPurposeDto dto);
//  Task<GridEntity<StatementOfPurposeDto>> SummaryGrid(GridOptions options);
//}