
namespace WebApiAuthentication.Api;

public interface IWeatherForecastRepository
{
    Task<bool> UserCreatedWeatherForecast(string weatherForecastId, string userName);
}