using Presentation.AuthorizeAttributes;
using Domain.Contracts.Services;
using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Domain.Exceptions;
using bdDevs.Shared.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

[AuthorizeUser]
public class QueryAnalyzerController : BaseApiController
{
	//private readonly IServiceManager _serviceManager;
	private readonly IMemoryCache _cache;

	public QueryAnalyzerController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
	{
		//_serviceManager = serviceManager;
		_cache = cache;
	}

	//[HttpPost(RouteConstants.GroupSummary)]
	//public async Task<IActionResult> GroupSummary([FromBody] CRMGridOptions options)
	//{
	//  var groupSummary = await _serviceManager.Groups.GroupSummary(trackChanges: false, options);
	//  return (groupSummary != null) ? Ok(groupSummary) : NoContent();
	//}

	[HttpGet(RouteConstants.CustomizedReportInfo)]
	public async Task<IActionResult> CustomizedReportInfo(CancellationToken cancellationToken = default)
	{
		var userIdClaim = User.FindFirst("UserId")?.Value;
		if (string.IsNullOrEmpty(userIdClaim))
			throw new GenericUnauthorizedException("User authentication required.");

		if (!int.TryParse(userIdClaim, out int userId))
			throw new GenericBadRequestException("Invalid user ID format.");

		UsersDto currentUser = _serviceManager.Cache<UsersDto>(userId);
		if (currentUser == null)
			throw new GenericUnauthorizedException("User session expired.");

		IEnumerable<QueryAnalyzerDto> res = await _serviceManager.QueryAnalyzer.CustomizedReportByPermissionAsync(currentUser, trackChanges: false, cancellationToken: cancellationToken);
		//IEnumerable<QueryAnalyzerDto> res = await _serviceManager.QueryAnalyzer.CustomizedReportByPermission(currentUser, trackChanges: false);
		//return Ok(queryAnalyzers);

		if (res == null || !res.Any())
			return Ok(ApiResponseHelper.NoContent<IEnumerable<QueryAnalyzerDto>>("No reports found"));

		return Ok(ApiResponseHelper.Success(res, "reports retrieved successfully"));
	}



}