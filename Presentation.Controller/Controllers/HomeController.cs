using bdDevs.Shared.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class HomeController : ControllerBase
{
  [HttpGet]
  public IActionResult Index()
  {
    return Ok("Welcome to bdDevCRM");
  }

  [HttpPost(RouteConstants.Logout)]
  public async Task<IActionResult> Logout(
    CancellationToken ct = default)
  {
    // Token extract করো
    var token = HttpContext.Request.Headers["Authorization"]
        .FirstOrDefault()?["Bearer ".Length..]?.Trim();

    if (!string.IsNullOrEmpty(token))
    {
      // JWT expiry বের করো
      var expiry = GetTokenExpiry(token); // JWT-এর exp claim

      // Blacklist-এ যোগ করো
      await _serviceManager.TokenBlacklist
          .BlacklistTokenAsync(token, expiry, ct);
    }

    return OkResponse(null, "Logged out successfully.");
  }
}
