using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class LeadStatusController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/LeadStatus/Index.cshtml");
    }
}
