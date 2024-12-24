using Duende.IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApiAuthentication.Client.Models;

namespace WebApiAuthentication.Client.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeController(ILogger<HomeController> logger,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CallApiApp()
    {
        var httpClient = _httpClientFactory.CreateClient("IdpClient");

        var discoveryDocument = await httpClient.GetDiscoveryDocumentAsync();
        if (discoveryDocument.IsError)
        { 
           throw discoveryDocument.Exception ?? new Exception("Problem reading the discovery document.");
        } 

        var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(
            new ClientCredentialsTokenRequest
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = "democlientclientcredentials",
                ClientSecret = "511536EF-F270-4058-80CA-1C89C192F69A",
                Scope = "claimsapi"
            });

        if (tokenResponse.IsError)
        { 
            Console.WriteLine(tokenResponse.Exception ?? new Exception("Problem getting access token."));
        }

        var resultFromCall = await CallApiWithTokenAsync(tokenResponse.AccessToken ?? string.Empty); 
        return View("Index", 
            new ApiClaimsViewModel() { 
                Claims = resultFromCall } );
      
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CallApiUser()
    { 
        var resultFromCall = await CallApiWithTokenAsync(string.Empty);
        return View("Index",
            new ApiClaimsViewModel()
            {
                Claims = resultFromCall
            });
    }
     
    private async Task<List<ClaimDto>> CallApiWithTokenAsync(string accessToken)
    {
        var httpClient = _httpClientFactory.CreateClient("ApiClient");
        var jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        var request = new HttpRequestMessage(
          HttpMethod.Get,
          "/api/claims/");

        if (accessToken != string.Empty)
        {
            httpClient.SetBearerToken(accessToken);
        }

        var response = await httpClient.SendAsync(
            request, HttpCompletionOption.ResponseHeadersRead);

        response.EnsureSuccessStatusCode();

        using (var responseStream = await response.Content.ReadAsStreamAsync())
        {
            return await JsonSerializer.DeserializeAsync<List<ClaimDto>>(responseStream,
                jsonSerializerOptions) ?? []; 
        }
    }

    public IActionResult Index()
    {
        return View(new ApiClaimsViewModel());
    } 
}
