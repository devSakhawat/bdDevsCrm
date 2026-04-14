using Domain.Entities.Entities.System;
using Domain.Contracts.Services.Core.SystemAdmin;
using bdDevCRM.s.Core.SystemAdmin;
using Application.Services.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevCRM.Shared.Exceptions;
using bdDevCRM.Utilities.OthersLibrary;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.Core.SystemAdmin;

/// <summary>
/// Query analyzer service implementing business logic for customized report management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class QueryAnalyzerService : IQueryAnalyzerService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<QueryAnalyzerService> _logger;
	private readonly IConfiguration _configuration;

	/// <summary>
	/// Initializes a new instance of <see cref="QueryAnalyzerService"/> with required dependencies.
	/// </summary>
	/// <param name="repository">The repository manager for data access operations.</param>
	/// <param name="logger">The logger for capturing service-level events.</param>
	/// <param name="configuration">The application configuration accessor.</param>
	public QueryAnalyzerService(IRepositoryManager repository, ILogger<QueryAnalyzerService> logger, IConfiguration configuration)
	{
		_repository = repository;
		_logger = logger;
		_configuration = configuration;
	}

	/// <summary>
	/// Retrieves all customized report information available in the system.
	/// </summary>
	public async Task<IEnumerable<QueryAnalyzerDto>> CustomizedReportInfoAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching customized report information. Time: {Time}", DateTime.UtcNow);

		IEnumerable<QueryAnalyzer> queryAnalyzers = await _repository.QueryAnalyzers
						.CustomizedReportsInfoAsync(trackChanges, cancellationToken);

		if (!queryAnalyzers.Any())
		{
			_logger.LogWarning("No customized reports found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<QueryAnalyzerDto>();
		}

		IEnumerable<QueryAnalyzerDto> queryAnalyzersDto = MyMapper.JsonCloneIEnumerableToList<QueryAnalyzer, QueryAnalyzerDto>(queryAnalyzers);

		_logger.LogInformation("Customized reports fetched successfully. Count: {Count}, Time: {Time}",
						queryAnalyzersDto.Count(),
						DateTime.UtcNow);

		return queryAnalyzersDto;
	}

	/// <summary>
	/// Retrieves customized reports accessible to a specific user based on their permissions.
	/// </summary>
	public async Task<IEnumerable<QueryAnalyzerDto>> CustomizedReportByPermissionAsync(UsersDto currentUser, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (currentUser is null)
		{
			_logger.LogWarning("CustomizedReportByPermissionAsync called with null currentUser. Time: {Time}", DateTime.UtcNow);
			throw new BadRequestException(nameof(UsersDto));
		}

		_logger.LogInformation("Fetching customized reports by permission for userId: {UserId}, Time: {Time}",
						currentUser.UserId,
						DateTime.UtcNow);

		Users usersEntity = await _repository.Users
						.FirstOrDefaultAsync(x => x.UserId.Equals(currentUser.UserId), trackChanges: false, cancellationToken)
						?? throw new NotFoundException("User", "UserId", currentUser.UserId.ToString());

		string condition = string.Empty;

		if (usersEntity.IsSystemUser == true)
		{
			_logger.LogInformation("System user detected. All reports will be accessible. UserId: {UserId}", usersEntity.UserId);
			condition = string.Empty;
		}
		else
		{
			var groupPermissionList = await _repository.QueryAnalyzers
							.GroupPermissionsForQueryAnalyzerReportAsync(usersEntity, cancellationToken);

			var ids = string.Join(",", groupPermissionList.Select(mn => mn.ReportHeaderId));

			condition = string.IsNullOrEmpty(ids)
							? string.Empty
							: $"WHERE ReportHeaderId IN ({ids})";

			_logger.LogInformation("Regular user detected. Permission filter applied. UserId: {UserId}, ReportCount: {Count}",
							usersEntity.UserId,
							groupPermissionList.Count());
		}

		IEnumerable<QueryAnalyzer> queryAnalyzers = await _repository.QueryAnalyzers
						.CustomizedReportsByPermissionAsync(usersEntity, condition, trackChanges, cancellationToken);

		if (!queryAnalyzers.Any())
		{
			_logger.LogWarning("No customized reports found for userId: {UserId}, Time: {Time}",
							currentUser.UserId,
							DateTime.UtcNow);
			return Enumerable.Empty<QueryAnalyzerDto>();
		}

		IEnumerable<QueryAnalyzerDto> queryAnalyzersDto = MyMapper.JsonCloneIEnumerableToList<QueryAnalyzer, QueryAnalyzerDto>(queryAnalyzers);

		_logger.LogInformation("Customized reports fetched successfully for userId: {UserId}, Count: {Count}, Time: {Time}",
						currentUser.UserId,
						queryAnalyzersDto.Count(),
						DateTime.UtcNow);

		return queryAnalyzersDto;
	}
}









//namespace bdDevCRM.Services.Core.SystemAdmin;


//internal sealed class QueryAnalyzerService : IQueryAnalyzerService
//{
//  private readonly IRepositoryManager _repository;
//  private readonly ILogger<QueryAnalyzerService> _logger;
//  private readonly IConfiguration _configuration;

//  public QueryAnalyzerService(IRepositoryManager repository, ILogger<QueryAnalyzerService> logger, IConfiguration configuration)
//  {
//    _repository = repository;
//    _logger = logger;
//    _configuration = configuration;
//  }



//  public async Task<IEnumerable<QueryAnalyzerDto>> CustomizedReportInfo(bool trackChanges)
//  {
//    IEnumerable<QueryAnalyzer> queryAnalyzers = await _repository.QueryAnalyzers.CustomizedReportInfo(trackChanges);
//    IEnumerable<QueryAnalyzerDto> queryAnalyzersDto = MyMapper.JsonCloneIEnumerableToList<QueryAnalyzer, QueryAnalyzerDto>(queryAnalyzers);

//    return queryAnalyzersDto;
//  }



//  public async Task<IEnumerable<QueryAnalyzerDto>> CustomizedReportByPermission(UsersDto currentUser, bool trackChanges)
//  {
//    if (currentUser == null) { return null; }
//    string condition = "";

//    Users usersEntity = await _repository.Users.ByIdAsync(predicate: x => x.UserId.Equals(currentUser.UserId), trackChanges);
//    if (usersEntity != null && usersEntity.IsSystemUser == true)
//    {
//      condition = "";
//    }
//    else
//    {
//      var groupPermissionList = await _repository.QueryAnalyzers.GroupPermissionForQueryAnalyzerReport(usersEntity);
//      var ids = string.Join(",", groupPermissionList.Select(mn => mn.ReportHeaderId));

//      condition = string.IsNullOrEmpty(ids) ? string.Empty : $"WHERE ReportHeaderId IN ({ids})";
//    }
//    IEnumerable<QueryAnalyzer> queryAnalyzers = await _repository.QueryAnalyzers.CustomizedReportByPermission(usersEntity, condition, trackChanges);
//    IEnumerable<QueryAnalyzerDto> queryAnalyzersDto = MyMapper.JsonCloneIEnumerableToList<QueryAnalyzer, QueryAnalyzerDto>(queryAnalyzers);

//    return queryAnalyzersDto;
//  }



//}
