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
    public async Task<IActionResult> CallApiUser()
    {
        var httpClient = _httpClientFactory.CreateClient("ApiClient");
        var jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        var viewModel = new ApiClaimsViewModel();

        var request = new HttpRequestMessage(
          HttpMethod.Get,
          "/api/claims/");  

        var response = await httpClient.SendAsync(
            request, HttpCompletionOption.ResponseHeadersRead);

        response.EnsureSuccessStatusCode();

        using (var responseStream = await response.Content.ReadAsStreamAsync())
        {
            viewModel.Claims = await JsonSerializer.DeserializeAsync<List<ClaimDto>>(responseStream,
                jsonSerializerOptions) ?? [];
        }

        return View("Index", viewModel);
    }
         
    public IActionResult Index()
    {
        return View(new ApiClaimsViewModel());
    } 
}