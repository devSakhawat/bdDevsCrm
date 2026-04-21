using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class PaymentMethodController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/PaymentMethod/Index.cshtml");
    }
}
