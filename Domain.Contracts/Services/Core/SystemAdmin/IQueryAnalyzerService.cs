using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

namespace Domain.Contracts.Services.Core.SystemAdmin;

/// <summary>
/// Service contract for query analyzer and customized report operations.
/// Defines methods for retrieving report data based on user permissions.
/// </summary>
public interface IQueryAnalyzerService
{
	/// <summary>
	/// Retrieves all customized report information available in the system.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="QueryAnalyzerDto"/> containing report information.</returns>
	Task<IEnumerable<QueryAnalyzerDto>> CustomizedReportInfoAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves customized reports accessible to a specific user based on their permissions.
	/// System users receive all reports, while regular users receive only permitted reports.
	/// </summary>
	/// <param name="currentUser">The DTO containing current user information.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="QueryAnalyzerDto"/> the user has permission to access.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="currentUser"/> is null.</exception>
	/// <exception cref="NotFoundException">Thrown when the user is not found in the database.</exception>
	Task<IEnumerable<QueryAnalyzerDto>> CustomizedReportByPermissionAsync(UsersDto currentUser, bool trackChanges, CancellationToken cancellationToken = default);
}



//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

//namespace Application.Services.Core.SystemAdmin;

//public interface IQueryAnalyzerService
//{
//	Task<IEnumerable<QueryAnalyzerDto>> CustomizedReportInfo(bool trackChanges);
//	Task<IEnumerable<QueryAnalyzerDto>> CustomizedReportByPermission(UsersDto currentUser, bool trackChanges);
//}
