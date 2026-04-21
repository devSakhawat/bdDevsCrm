using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class MonthController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/Month/Index.cshtml");
    }
}
