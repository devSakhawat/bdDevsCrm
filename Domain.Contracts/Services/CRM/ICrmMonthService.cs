// ICrmMonthService.cs
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.CRM;

/// <summary>
/// Service contract for CRM month management operations.
/// Defines methods for creating, updating, deleting, and retrieving month data.
/// </summary>
public interface ICrmMonthService
{
	/// <summary>
	/// Creates a new month record using CRUD Record pattern.
	/// </summary>
	Task<CrmMonthDto> CreateAsync(CreateCrmMonthRecord record, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing month record using CRUD Record pattern.
	/// </summary>
	Task<CrmMonthDto> UpdateAsync(UpdateCrmMonthRecord record, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes a month record using CRUD Record pattern.
	/// </summary>
	Task DeleteAsync(DeleteCrmMonthRecord record, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single month record by its ID.
	/// </summary>
	/// <param name="id">The ID of the month to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="CrmMonthDto"/> matching the specified ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no month is found for the given ID.</exception>
	Task<CrmMonthDto> MonthAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves all month records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of all <see cref="CrmMonthDto"/> records.</returns>
	Task<IEnumerable<CrmMonthDto>> MonthsAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves active month records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of active <see cref="CrmMonthDto"/> records.</returns>
	Task<IEnumerable<CrmMonthDto>> ActiveMonthsAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a lightweight list of all months suitable for use in dropdown lists.
	/// </summary>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="CrmMonthDto"/> for dropdown binding.</returns>
	Task<IEnumerable<CrmMonthDto>> MonthForDDLAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves months by the specified applicant ID.
	/// </summary>
	/// <param name="applicantId">The ID of the applicant.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="CrmMonthDto"/> for the specified applicant.</returns>
	Task<IEnumerable<CrmMonthDto>> MonthsByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated summary grid of all months.
	/// </summary>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{CrmMonthDto}"/> containing the paged month data.</returns>
	Task<GridEntity<CrmMonthDto>> MonthsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}



//using Application.Shared.Grid;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.CRM;

//namespace Domain.Contracts.Services.CRM;

//public interface ICrmMonthService
//{
//  Task<IEnumerable<CrmMonthDto>> MonthsDDLAsync(bool trackChanges = false);
//  Task<IEnumerable<CrmMonthDto>> ActiveMonthsAsync(bool trackChanges = false);
//  Task<IEnumerable<CrmMonthDto>> MonthsAsync(bool trackChanges = false);
//  Task<CrmMonthDto> MonthAsync(int id, bool trackChanges = false);
//  Task<IEnumerable<CrmMonthDto>> MonthsByApplicantIdAsync(int applicantId, bool trackChanges = false);
//  Task<CrmMonthDto> CreateNewRecordAsync(CrmMonthDto dto, UsersDto currentUser);
//  Task<string> UpdateRecordAsync(int key, CrmMonthDto dto, bool trackChanges);
//  Task<string> DeleteRecordAsync(int key, CrmMonthDto dto);
//  Task<string> SaveOrUpdate(int key, CrmMonthDto modelDto, UsersDto currentUser);
//  Task<CrmMonthDto> CreateMonthAsync(CrmMonthDto entityForCreate);
//  Task<GridEntity<CrmMonthDto>> SummaryGrid(GridOptions options);
//}
