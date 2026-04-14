// ICrmApplicationService.cs
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.CRM;

/// <summary>
/// Service contract for CRM application management operations.
/// Defines methods for creating, updating, deleting, and retrieving CRM application data with all nested entities.
/// </summary>
public interface ICrmApplicationService
{
	/// <summary>
	/// Creates a new CRM application record with all nested entities.
	/// </summary>
	/// <param name="entityForCreate">The DTO containing data for the new application.</param>
	/// <param name="currentUser">The DTO containing current user information.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The created <see cref="CrmApplicationDto"/> with the newly assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
	/// <exception cref="InvalidCreateOperationException">Thrown when ApplicationId is not 0 for new creation.</exception>
	Task<CrmApplicationDto> CreateApplicationAsync(CrmApplicationDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing CRM application record with all nested entities.
	/// </summary>
	/// <param name="applicationId">The ID of the application to update.</param>
	/// <param name="modelDto">The DTO containing updated field values.</param>
	/// <param name="currentUser">The DTO containing current user information.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="CrmApplicationDto"/> reflecting the saved state.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no application is found for the given ID.</exception>
	Task<CrmApplicationDto> UpdateApplicationAsync(int applicationId, CrmApplicationDto modelDto, UsersDto currentUser, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single CRM application record by its ID with all related data.
	/// </summary>
	/// <param name="applicationId">The ID of the application to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="GetApplicationDto"/> matching the specified ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no application is found for the given ID.</exception>
	Task<ApplicationDto> ApplicationAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated summary grid of all CRM applications with permission-based filtering.
	/// </summary>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="statusId">The status ID to filter applications.</param>
	/// <param name="currentUser">The DTO containing current user information.</param>
	/// <param name="menuDto">The DTO containing menu information.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{CrmApplicationGridDto}"/> containing the paged application data.</returns>
	Task<GridEntity<CrmApplicationGridDto>> ApplicationsSummaryAsync(GridOptions options, int statusId, UsersDto currentUser, MenuDto menuDto, CancellationToken cancellationToken = default);

	/// <summary>
	/// Saves a CRM application record (create or update) with all nested entities and transaction support.
	/// </summary>
	/// <param name="modelDto">The DTO containing application data.</param>
	/// <param name="currentUser">The DTO containing current user information.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The saved <see cref="CrmApplicationDto"/> with the assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="InvalidCreateOperationException">Thrown when validation fails.</exception>
	Task<CrmApplicationDto> SaveApplicationAsync(CrmApplicationDto modelDto, UsersDto currentUser, CancellationToken cancellationToken = default);
}