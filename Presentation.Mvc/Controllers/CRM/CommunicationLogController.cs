using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class CommunicationLogController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/CommunicationLog/Index.cshtml");
    }
}
