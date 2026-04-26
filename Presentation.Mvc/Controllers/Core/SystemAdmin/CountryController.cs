using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.Core.SystemAdmin;

/// <summary>
/// Country MVC Controller - Renders Country management page
/// </summary>
public class CountryController : Controller
{
    /// <summary>
    /// Country management page - GET
    /// </summary>
    [HttpGet]
    public IActionResult Index()
    {
        // Authentication check is handled by AuthenticationCheckMiddleware
        return View("~/Views/Core/SystemAdmin/Country.cshtml");
    }
}
