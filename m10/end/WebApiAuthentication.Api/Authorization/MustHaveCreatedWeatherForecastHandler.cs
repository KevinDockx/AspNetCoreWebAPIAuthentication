using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebApiAuthentication.Api.Authorization;

public class MustHaveCreatedWeatherForecastHandler :
    AuthorizationHandler<MustHaveCreatedWeatherForecastRequirement>
{
    private readonly IWeatherForecastRepository _weatherForecastRepository;

    public MustHaveCreatedWeatherForecastHandler(
        IWeatherForecastRepository weatherForecastRepository)
    {
        _weatherForecastRepository = weatherForecastRepository;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        MustHaveCreatedWeatherForecastRequirement requirement)
    {
        // access the RouteValues from HttpContext
        var httpContext = (context.Resource as DefaultHttpContext);
        if (httpContext == null)
        {
            context.Fail();
            return;
        }

        var routeValues = httpContext.GetRouteData().Values;
        if (!routeValues.TryGetValue("id", out var idValue))
        {
            context.Fail();
            return;
        }

        var weatherForecastId = idValue as string;
        // get the username
        var userName = context.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (weatherForecastId == null || userName == null)
        {
            context.Fail();
            return;
        }

        if (!(await _weatherForecastRepository.UserCreatedWeatherForecast(weatherForecastId, userName)))
        {
            context.Fail();
        }
        context.Succeed(requirement);
    }
}

