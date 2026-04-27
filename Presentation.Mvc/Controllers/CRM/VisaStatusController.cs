using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class VisaStatusController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/VisaStatus/Index.cshtml");
    }
}
