using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class IeltsInformationController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/IeltsInformation/Index.cshtml");
    }
}
