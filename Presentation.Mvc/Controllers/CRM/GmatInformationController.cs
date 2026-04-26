using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class GmatInformationController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/GmatInformation/Index.cshtml");
    }
}
