using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class IntakeYearController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/IntakeYear/Index.cshtml");
    }
}
