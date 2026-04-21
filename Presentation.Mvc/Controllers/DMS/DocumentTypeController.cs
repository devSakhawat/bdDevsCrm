using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.DMS;

/// <summary>
/// DMS Document Type MVC Controller - Renders Document Type management page
/// </summary>
public class DocumentTypeController : Controller
{
    /// <summary>
    /// Document Type management page - GET
    /// </summary>
    [HttpGet]
    public IActionResult Index()
    {
        // Authentication check is handled by AuthenticationCheckMiddleware
        return View("~/Views/DMS/DocumentType/Index.cshtml");
    }
}
