using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class ApplicantReferenceController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/ApplicantReference/Index.cshtml");
    }
}
