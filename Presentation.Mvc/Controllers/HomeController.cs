using Microsoft.AspNetCore.Mvc;
using Presentation.Mvc.Models;
using System.Diagnostics;

namespace Presentation.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Dashboard";
            ViewBag.PageTitle = "Employee Management Dashboard";
            ViewBag.PageSubtitle = "A reusable HRIS shell with document-driven layout, dashboard widgets, filters and action patterns.";
            ViewBag.Breadcrumb = new[] { "Home", "HR", "Employee Management" };
            ViewBag.PageActions = new List<PageActionItem>
            {
                new()
                {
                    Text = "Add Employee",
                    Variant = "primary",
                    ActionName = "add-employee",
                    IconSvg = "+"
                },
                new()
                {
                    Text = "Export Excel",
                    Variant = "secondary",
                    ActionName = "export-report",
                    IconSvg = "⇩"
                },
                new()
                {
                    Text = "Refresh",
                    Variant = "ghost",
                    ActionName = "refresh-dashboard",
                    IconSvg = "↻"
                }
            };

            return View();
        }

        public IActionResult Privacy()
        {
            ViewData["Title"] = "Privacy";
            ViewBag.PageTitle = "Privacy & Compliance";
            ViewBag.PageSubtitle = "Policy, access and retention details for enterprise CRM data.";
            ViewBag.Breadcrumb = new[] { "Home", "Privacy" };
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
