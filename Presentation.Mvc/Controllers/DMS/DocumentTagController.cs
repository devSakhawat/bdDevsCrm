using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.DMS;

public class DocumentTagController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/DMS/DocumentTag/Index.cshtml");
    }
}
