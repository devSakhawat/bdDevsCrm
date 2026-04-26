using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers;

public class AccountController : Controller
{
    /// <summary>
    /// Login page - GET
    /// </summary>
    [HttpGet]
    public IActionResult Login()
    {
        // If already authenticated, redirect to home
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    /// <summary>
    /// Logout action - POST
    /// </summary>
    [HttpPost]
    public IActionResult Logout()
    {
        // Frontend handles token clearing via AuthManager.logout()
        // This just provides a server-side endpoint if needed
        return RedirectToAction("Login");
    }
}
