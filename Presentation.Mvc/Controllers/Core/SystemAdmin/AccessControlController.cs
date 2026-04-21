using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.Core.SystemAdmin;

/// <summary>
/// Access Control MVC Controller - Renders Access Control management page
/// </summary>
public class AccessControlController : Controller
{
    /// <summary>
    /// Access Control management page - GET
    /// </summary>
    [HttpGet]
    public IActionResult Index()
    {
        // Authentication check is handled by AuthenticationCheckMiddleware
        return View("~/Views/Core/SystemAdmin/AccessControl.cshtml");
    }
}
