using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.Core.SystemAdmin;

public class TokenBlacklistController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/Core/SystemAdmin/TokenBlacklist.cshtml");
    }
}
