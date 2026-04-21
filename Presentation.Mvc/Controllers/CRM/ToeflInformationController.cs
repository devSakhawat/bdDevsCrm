using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class ToeflInformationController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/ToeflInformation/Index.cshtml");
    }
}
