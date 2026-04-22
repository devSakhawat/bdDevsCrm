using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.Core.SystemAdmin;

public class AuditTrailController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/Core/SystemAdmin/AuditTrail.cshtml");
    }
}
