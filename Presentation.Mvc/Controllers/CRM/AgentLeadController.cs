using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class AgentLeadController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/AgentLead/Index.cshtml");
    }
}
