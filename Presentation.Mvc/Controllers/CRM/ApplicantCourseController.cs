using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class ApplicantCourseController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/ApplicantCourse/Index.cshtml");
    }
}
