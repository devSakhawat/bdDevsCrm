using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class ApplicantInfoController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/ApplicantInfo/Index.cshtml");
    }
}
