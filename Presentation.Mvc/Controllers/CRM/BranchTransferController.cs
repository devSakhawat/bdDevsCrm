using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class BranchTransferController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/CRM/BranchTransfer/Index.cshtml");
    }
}
