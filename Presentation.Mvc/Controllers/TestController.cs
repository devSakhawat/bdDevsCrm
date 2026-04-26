using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers;

/// <summary>
/// Test Controller - For testing and debugging purposes
/// </summary>
public class TestController : Controller
{
    /// <summary>
    /// Session Management Testing Page - GET
    /// </summary>
    [HttpGet]
    public IActionResult SessionTest()
    {
        return View();
    }
}
