using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class CommunicationTemplateController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/CommunicationTemplate/Index.cshtml");
    }
}
