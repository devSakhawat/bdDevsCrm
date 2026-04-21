using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.Core.SystemAdmin;

/// <summary>
/// Workflow MVC Controller - Renders Workflow management page
/// </summary>
public class WorkflowController : Controller
{
    /// <summary>
    /// Workflow management page - GET
    /// </summary>
    [HttpGet]
    public IActionResult Index()
    {
        // Authentication check is handled by AuthenticationCheckMiddleware
        return View("~/Views/Core/SystemAdmin/Workflow.cshtml");
    }
}
