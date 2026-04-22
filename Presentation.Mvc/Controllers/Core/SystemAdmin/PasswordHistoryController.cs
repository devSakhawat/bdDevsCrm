using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.Core.SystemAdmin;

public class PasswordHistoryController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/Core/SystemAdmin/PasswordHistory.cshtml");
    }
}
