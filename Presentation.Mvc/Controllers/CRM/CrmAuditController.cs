using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers.CRM;

public class CrmAuditController : Controller
{
    [HttpGet]
    public IActionResult AuditLog()
    {
        return View("~/Views/CRM/CrmAudit/AuditLog.cshtml");
    }

    [HttpGet]
    public IActionResult SecurityDashboard()
    {
        return View("~/Views/CRM/CrmAudit/SecurityDashboard.cshtml");
    }
}
