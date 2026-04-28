using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class CommissionController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/Commission/Index.cshtml");
    }

    [HttpGet]
    public IActionResult Dashboard()
    {
        return View("~/Views/CRM/Commission/Dashboard.cshtml");
    }
}
