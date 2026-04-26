using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class AdditionalInfoController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/AdditionalInfo/Index.cshtml");
    }
}
