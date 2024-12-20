using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiAuthentication.Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{id}",Name = "GetWeatherForecast")]
    public ActionResult Get(string id)
    {
        // return dummy weatherforecast object.  This isn't 
        // important for the authorization policy.
        return Ok(new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        });
    }

    [HttpPost]
    public ActionResult<WeatherForecast> Post(
        WeatherForecastForCreationDto weatherForecastForCreationDto)
    {
        // create object to store from inputted dto...

        var objectToStore = new WeatherForecast()
        {
            TemperatureC = weatherForecastForCreationDto.TemperatureC,
            Date = weatherForecastForCreationDto.Date, 
            Summary = weatherForecastForCreationDto.Summary
        };

        // store the object... not implemented,
        // this isn't important for the authorization policy

        // return created weatherforecast object.  
        return CreatedAtRoute("GetWeatherForecast", 
            new { id = "abc123" }, 
            objectToStore); 
    }
}
