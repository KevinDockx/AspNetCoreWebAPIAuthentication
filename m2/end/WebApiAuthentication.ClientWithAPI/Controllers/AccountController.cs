using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiAuthentication.ClientWithAPI.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;

namespace WebApiAuthentication.ClientWithAPI.Controllers;

public class AccountController : Controller
{ 
    public IActionResult Login()
    {
        return View();
    }
 

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (model.Username == "Kevin" && model.Password == "Pluralsight")
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,
                        model.Username),
                    new Claim(ClaimTypes.Email,
                        "kevin.dockx@gmail.com"),
                    new Claim(ClaimTypes.Country,
                        "Belgium")
                };

            var claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authProperties = new AuthenticationProperties
            {   
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20)                  
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
               claimsPrincipal,
               authProperties);

            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }


    [Authorize]
    public async Task Logout()
    { 
        // Clears the application cookie
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
