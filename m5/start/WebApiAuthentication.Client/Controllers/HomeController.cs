using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApiAuthentication.Client.Models;

namespace WebApiAuthentication.Client.Controllers;


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
        var resultFromCall = await CallApiWithTokenAsync(null); 
        return View(new ApiClaimsViewModel() { 
            ClaimsAsString = resultFromCall } );
      
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CallApiUser()
    {
        var resultFromCall = await CallApiWithTokenAsync(null);
        return View(new ApiClaimsViewModel()
        {
            ClaimsAsString = resultFromCall
        });
    }
     
    private async Task<string> CallApiWithTokenAsync(string? accessToken)
    {
        var httpClient = _httpClientFactory.CreateClient("ApiClient");

        var request = new HttpRequestMessage(
          HttpMethod.Get,
          "/api/claims/");

        var response = await httpClient.SendAsync(
            request, HttpCompletionOption.ResponseHeadersRead);

        response.EnsureSuccessStatusCode();

        using (var responseStream = await response.Content.ReadAsStreamAsync())
        {
            var body = await JsonSerializer.DeserializeAsync<string>(responseStream);
            return body ?? string.Empty;
        }
    }

    public IActionResult Index()
    {
        return View(new ApiClaimsViewModel());
    } 
}
