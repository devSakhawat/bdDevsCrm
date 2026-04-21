using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class EducationHistoryController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/EducationHistory/Index.cshtml");
    }
}
