using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class StatementOfPurposeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/StatementOfPurpose/Index.cshtml");
    }
}
