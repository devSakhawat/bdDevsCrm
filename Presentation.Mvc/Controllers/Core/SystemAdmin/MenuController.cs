using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.Core.SystemAdmin;

/// <summary>
/// Menu MVC Controller - Renders Menu management page
/// </summary>
public class MenuController : Controller
{
    /// <summary>
    /// Menu management page - GET
    /// </summary>
    [HttpGet]
    public IActionResult Index()
    {
        // Authentication check is handled by AuthenticationCheckMiddleware
        return View("~/Views/Core/SystemAdmin/Menu.cshtml");
    }
}
