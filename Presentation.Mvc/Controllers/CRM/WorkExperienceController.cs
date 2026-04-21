using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class WorkExperienceController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/WorkExperience/Index.cshtml");
    }
}
