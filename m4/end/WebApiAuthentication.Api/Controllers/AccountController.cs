using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebApiAuthentication.Api.Controllers;

[Route("account")]
[ApiController]
public class AccountController(IConfiguration configuration) : ControllerBase
{
    private readonly IConfiguration _configuration = configuration;

    public class AuthenticationRequestBody
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    [HttpPost("login")]
    public ActionResult<string> Login(AuthenticationRequestBody authenticationRequestBody)
    {
        if (authenticationRequestBody.Username == "Kevin"
            && authenticationRequestBody.Password == "Pluralsight")
        {
            var securityKey = new SymmetricSecurityKey(
                Convert.FromBase64String(_configuration["Authentication:SecretForKey"]
                    ?? throw new KeyNotFoundException("SecretForKey not found or invalid")));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>
            {
                new("sub", "1234"),
                new("given_name", "Kevin"),
                new("family_name", "Dockx"),
                new("email", "kevin.dockx@gmail.com")
            };

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler()
                          .WriteToken(jwtSecurityToken);
            return Ok(tokenToReturn);
        }
        return Unauthorized();
    }
}
