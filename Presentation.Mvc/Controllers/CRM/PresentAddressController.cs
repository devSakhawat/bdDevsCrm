using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class PresentAddressController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/PresentAddress/Index.cshtml");
    }
}
