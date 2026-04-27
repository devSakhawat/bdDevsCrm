using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class InstituteController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/Institute/Index.cshtml");
    }
}
