using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class StudentStatusController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/StudentStatus/Index.cshtml");
    }
}
