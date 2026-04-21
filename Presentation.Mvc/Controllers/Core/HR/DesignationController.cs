using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.Core.HR;

/// <summary>
/// Designation MVC Controller - Renders Designation management page
/// </summary>
public class DesignationController : Controller
{
    /// <summary>
    /// Designation management page - GET
    /// </summary>
    [HttpGet]
    public IActionResult Index()
    {
        // Authentication check is handled by AuthenticationCheckMiddleware
        return View("~/Views/Core/HR/Designation.cshtml");
    }
}
