using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.Core.SystemAdmin;

public class DocumentTypeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/Core/SystemAdmin/DocumentType.cshtml");
    }
}
