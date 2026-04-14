namespace Domain.Contracts.Services.Core.Infrastructure;

/// <summary>
/// Service for accessing HTTP context information.
/// Provides abstraction over HttpContext for testability and clean architecture.
/// </summary>
public interface IHttpContextService
{
	/// <summary>
	/// Retrieves the client's IP address from the current HTTP request.
	/// SECURITY: Only uses Connection.RemoteIpAddress (trusted source).
	/// Do NOT trust X-Forwarded-For headers unless behind a verified proxy.
	/// </summary>
	/// <returns>Client IP address or "Unknown" if unavailable</returns>
	string ClientIpAddress();
}