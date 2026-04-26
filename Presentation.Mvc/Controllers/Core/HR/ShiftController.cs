using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.Core.HR;

/// <summary>
/// Shift MVC Controller - Renders Shift management page
/// </summary>
public class ShiftController : Controller
{
    /// <summary>
    /// Shift management page - GET
    /// </summary>
    [HttpGet]
    public IActionResult Index()
    {
        // Authentication check is handled by AuthenticationCheckMiddleware
        return View("~/Views/Core/HR/Shift.cshtml");
    }
}
