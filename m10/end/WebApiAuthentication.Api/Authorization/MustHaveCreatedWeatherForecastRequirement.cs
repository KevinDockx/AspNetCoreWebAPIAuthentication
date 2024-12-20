using Microsoft.AspNetCore.Authorization;

namespace WebApiAuthentication.Api.Authorization;

public class MustHaveCreatedWeatherForecastRequirement : IAuthorizationRequirement
{
}
