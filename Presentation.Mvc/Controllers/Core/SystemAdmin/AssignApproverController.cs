using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.Core.SystemAdmin;

public class AssignApproverController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/Core/SystemAdmin/AssignApprover.cshtml");
    }
}
