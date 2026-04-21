using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class OthersInformationController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/OthersInformation/Index.cshtml");
    }
}
