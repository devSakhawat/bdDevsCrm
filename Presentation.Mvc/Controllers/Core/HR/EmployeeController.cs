using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.Core.HR;

/// <summary>
/// Employee MVC Controller - Renders Employee management page
/// Complex tabbed form for employee information management
/// </summary>
public class EmployeeController : Controller
{
    /// <summary>
    /// Employee management page - GET
    /// </summary>
    [HttpGet]
    public IActionResult Index()
    {
        // Authentication check is handled by AuthenticationCheckMiddleware
        return View("~/Views/Core/HR/Employee.cshtml");
    }
}
