using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class LeadStageController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/LeadStage/Index.cshtml");
    }
}
