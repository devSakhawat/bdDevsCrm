using bdDevCRM.ServiceContract.Core.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Core.Infrastructure;

/// <summary>
/// Concrete implementation of HTTP context service.
/// Provides safe access to HTTP context information.
/// </summary>
public sealed class HttpContextService : IHttpContextService
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public HttpContextService(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public string ClientIpAddress()
	{
		var httpContext = _httpContextAccessor.HttpContext;
		if (httpContext == null)
			return "Unknown";

		// SECURITY: Do NOT trust X-Forwarded-For or X-Real-IP headers unless behind a trusted proxy!
		var remoteIp = httpContext.Connection.RemoteIpAddress;
		return remoteIp?.ToString() ?? "Unknown";
	}
}
