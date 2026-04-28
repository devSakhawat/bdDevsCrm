using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class AgentController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/Agent/Index.cshtml");
    }

    [HttpGet]
    public IActionResult Performance()
    {
        return View("~/Views/CRM/Agent/Performance.cshtml");
    }
}
