using Microsoft.AspNetCore.Authorization;

namespace WebApiAuthentication.Api.Authorization;

public class MustHaveCreatedWeatherForecastAttribute : AuthorizeAttribute, IAuthorizationRequirementData
{
    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        return [new MustHaveCreatedWeatherForecastRequirement()];
    }
} 