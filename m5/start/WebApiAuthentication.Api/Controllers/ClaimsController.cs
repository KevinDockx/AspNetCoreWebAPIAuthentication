using Microsoft.AspNetCore.Mvc;

namespace WebApiAuthentication.Api.Controllers;

[Route("api/claims")]
[ApiController]
public class ClaimsController : ControllerBase
{ 
    [HttpGet]
    public IActionResult GetClaims()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value });
        return  Ok(claims); 
    } 
}
