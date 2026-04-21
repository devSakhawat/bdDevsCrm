using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class YearController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/Year/Index.cshtml");
    }
}
