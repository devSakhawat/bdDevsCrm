using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.DMS;

public class DocumentFolderController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/DMS/DocumentFolder/Index.cshtml");
    }
}
