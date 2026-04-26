using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.DMS;

/// <summary>
/// DMS Document MVC Controller - Renders Document management page
/// </summary>
public class DocumentController : Controller
{
    /// <summary>
    /// Document management page - GET
    /// </summary>
    [HttpGet]
    public IActionResult Index()
    {
        // Authentication check is handled by AuthenticationCheckMiddleware
        return View("~/Views/DMS/Document/Index.cshtml");
    }
}
