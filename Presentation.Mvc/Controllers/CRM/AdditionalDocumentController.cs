using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class AdditionalDocumentController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/AdditionalDocument/Index.cshtml");
    }
}
