using Microsoft.AspNetCore.Mvc;
using WebApiAuthentication.ClientWithAPI.Models;

namespace WebApiAuthentication.ClientWithAPI.Controllers;

public class AccountController : Controller
{ 
    public IActionResult Login()
    {
        return View();
    }
 

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if (model.Username == "Kevin" && model.Password == "Pluralsight")
        {
           
            // TODO 

            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }

    public void Logout()
    { 
        // TODO
    }
}
