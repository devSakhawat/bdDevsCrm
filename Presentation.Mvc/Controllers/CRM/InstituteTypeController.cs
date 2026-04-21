using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class InstituteTypeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/InstituteType/Index.cshtml");
    }
}
