using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.Core.SystemAdmin;

/// <summary>
/// Thana MVC Controller - Renders Thana management page
/// </summary>
public class ThanaController : Controller
{
    /// <summary>
    /// Thana management page - GET
    /// </summary>
    [HttpGet]
    public IActionResult Index()
    {
        // Authentication check is handled by AuthenticationCheckMiddleware
        return View("~/Views/Core/SystemAdmin/Thana.cshtml");
    }
}
