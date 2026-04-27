using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class FollowUpController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/FollowUp/Index.cshtml");
    }
}
