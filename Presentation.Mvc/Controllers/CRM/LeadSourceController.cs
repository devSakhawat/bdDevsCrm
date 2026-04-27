using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class LeadSourceController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/LeadSource/Index.cshtml");
    }
}
