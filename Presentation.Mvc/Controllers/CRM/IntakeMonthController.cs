using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class IntakeMonthController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/IntakeMonth/Index.cshtml");
    }
}
