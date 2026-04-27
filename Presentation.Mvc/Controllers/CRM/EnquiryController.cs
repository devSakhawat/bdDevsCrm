using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class EnquiryController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/Enquiry/Index.cshtml");
    }
}
