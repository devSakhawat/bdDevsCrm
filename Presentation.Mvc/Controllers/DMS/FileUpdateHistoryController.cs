using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.DMS;

public class FileUpdateHistoryController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/DMS/FileUpdateHistory/Index.cshtml");
    }
}
