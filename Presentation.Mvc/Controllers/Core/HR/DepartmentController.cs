using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.Core.HR;

/// <summary>
/// Department MVC Controller - Renders Department management page
/// </summary>
public class DepartmentController : Controller
{
    /// <summary>
    /// Department management page - GET
    /// </summary>
    [HttpGet]
    public IActionResult Index()
    {
        // Authentication check is handled by AuthenticationCheckMiddleware
        return View("~/Views/Core/HR/Department.cshtml");
    }
}
