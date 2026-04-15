using Domain.Contracts.Services.Core.Infrastructure;
﻿using Application.Services.Core.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Application.Services.Core.Infrastructure;

/// <summary>
/// Concrete implementation of cookie management service.
/// Handles secure cookie operations for authentication tokens.
/// </summary>
public sealed class CookieManagementService : ICookieManagementService
{
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly IWebHostEnvironment _environment;
	private const string REFRESH_TOKEN_COOKIE_NAME = "refreshToken";

	public CookieManagementService(
		IHttpContextAccessor httpContextAccessor,
		IWebHostEnvironment environment)
	{
		_httpContextAccessor = httpContextAccessor;
		_environment = environment;
	}

	public void SetRefreshTokenCookie(string refreshToken, DateTime expiry)
	{
		var httpContext = _httpContextAccessor.HttpContext;
		if (httpContext == null)
			throw new InvalidOperationException("HttpContext is not available");

		var cookieOptions = new CookieOptions
		{
			HttpOnly = true,
			Secure = !_environment.IsDevelopment(),
			SameSite = SameSiteMode.Strict,
			Expires = expiry,
			Path = "/",
			IsEssential = true
		};

		httpContext.Response.Cookies.Append(REFRESH_TOKEN_COOKIE_NAME, refreshToken, cookieOptions);
	}

	public void ClearRefreshTokenCookie()
	{
		var httpContext = _httpContextAccessor.HttpContext;
		if (httpContext == null)
			return;

		var cookieOptions = new CookieOptions
		{
			HttpOnly = true,
			Secure = !_environment.IsDevelopment(),
			SameSite = SameSiteMode.Strict,
			Path = "/"
		};

		httpContext.Response.Cookies.Delete(REFRESH_TOKEN_COOKIE_NAME, cookieOptions);
	}

	public string? RefreshToken()
	{
		var httpContext = _httpContextAccessor.HttpContext;
		if (httpContext == null)
			return null;

		httpContext.Request.Cookies.TryGetValue(REFRESH_TOKEN_COOKIE_NAME, out var refreshToken);
		return refreshToken;
	}
}
