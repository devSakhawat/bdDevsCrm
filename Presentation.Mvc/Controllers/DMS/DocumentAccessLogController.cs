using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.DMS;

public class DocumentAccessLogController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/DMS/DocumentAccessLog/Index.cshtml");
    }
}
