using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

/// <summary>
/// CRM Application MVC Controller - Renders CRM Application management page
/// Complex tabbed form for student application processing
/// </summary>
public class ApplicationController : Controller
{
    /// <summary>
    /// CRM Application management page - GET
    /// </summary>
    [HttpGet]
    public IActionResult Index()
    {
        // Authentication check is handled by AuthenticationCheckMiddleware
        return View("~/Views/CRM/Application/Index.cshtml");
    }
}
