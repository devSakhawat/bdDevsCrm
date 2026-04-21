using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class CourseIntakeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/CourseIntake/Index.cshtml");
    }
}
