namespace WebApiAuthentication.Api;

public class WeatherForecastRepository : IWeatherForecastRepository
{

    public async Task<bool> UserCreatedWeatherForecast(string weatherForecastId, string userName)
    {
        // for the demo, return true. 
        return await Task.FromResult(true);
    }

}
