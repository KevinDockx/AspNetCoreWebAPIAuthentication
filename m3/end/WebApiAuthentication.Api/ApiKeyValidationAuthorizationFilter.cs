using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace WebApiAuthentication.Api;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class ApiKeyValidationAuthorizationFilter : Attribute, IAuthorizationFilter
{ 
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!IsValidApiKey(context))
        {
            context.Result = new UnauthorizedResult();
        }
    }

    private bool IsValidApiKey(AuthorizationFilterContext context)
    {
        // check if we've got the required input 
        var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var validApiKey = config.GetValue<string>("Authentication:ApiKey");

        if (string.IsNullOrWhiteSpace(validApiKey))
        {
            throw new KeyNotFoundException("ApiKey not found in configuration.");
        }

        // try and get the key from a custom request header "X-ApiKey"
        if (!context.HttpContext.Request.Headers.TryGetValue("X-ApiKey", out var extractedApiKey))
        {
            // as a fallback option, try getting it from the query string           
            if (!context.HttpContext.Request.Query.TryGetValue("X-ApiKey", out extractedApiKey))
            {
                return false;
            }
        }

        return validApiKey.Equals(extractedApiKey); 
    }
}