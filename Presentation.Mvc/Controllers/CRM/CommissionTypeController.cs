using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class CommissionTypeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/CommissionType/Index.cshtml");
    }
}
