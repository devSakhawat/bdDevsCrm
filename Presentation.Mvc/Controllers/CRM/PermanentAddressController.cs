using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class PermanentAddressController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/PermanentAddress/Index.cshtml");
    }
}
