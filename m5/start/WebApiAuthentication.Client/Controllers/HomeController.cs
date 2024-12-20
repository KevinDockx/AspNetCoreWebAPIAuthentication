using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiAuthentication.Client.Models;

namespace WebApiAuthentication.Client.Controllers;


[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult CallApiApp()
    {
        
        return View(new ApiClaimsViewModel());
    }

    public IActionResult CallApiUser()
    {
        return View(new ApiClaimsViewModel());
    }

    public IActionResult Index()
    {
        return View();
    } 
}
