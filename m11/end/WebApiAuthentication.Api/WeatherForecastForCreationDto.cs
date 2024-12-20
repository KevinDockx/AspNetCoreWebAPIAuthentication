namespace WebApiAuthentication.Api;

public class WeatherForecastForCreationDto
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; } 

    public string? Summary { get; set; }
}
