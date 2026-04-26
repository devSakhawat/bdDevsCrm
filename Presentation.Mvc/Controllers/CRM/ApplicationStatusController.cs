using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class ApplicationStatusController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/ApplicationStatus/Index.cshtml");
    }
}
