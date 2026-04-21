using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.Core.HR;

/// <summary>
/// Branch MVC Controller - Renders Branch management page
/// </summary>
public class BranchController : Controller
{
    /// <summary>
    /// Branch management page - GET
    /// </summary>
    [HttpGet]
    public IActionResult Index()
    {
        // Authentication check is handled by AuthenticationCheckMiddleware
        return View("~/Views/Core/HR/Branch.cshtml");
    }
}
