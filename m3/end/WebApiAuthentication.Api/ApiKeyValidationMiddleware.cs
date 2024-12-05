namespace WebApiAuthentication.Api;

public class ApiKeyValidationMiddleware
{
    private readonly RequestDelegate _next; 

    public ApiKeyValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var config = context.RequestServices.GetRequiredService<IConfiguration>();
        var validApiKey = config.GetValue<string>("Authentication:ApiKey");

        if (string.IsNullOrWhiteSpace(validApiKey))
        {
            throw new KeyNotFoundException("ApiKey not found in configuration.");
        }

        // try and get the key from a custom request header "X-ApiKey"
        if (!context.Request.Headers.TryGetValue("X-ApiKey", out var extractedApiKey))
        {           
            // as a fallback option, try getting it from the query string           
            if (!context.Request.Query.TryGetValue("X-ApiKey", out extractedApiKey))
            { 
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("No ApiKey provided.");
                return;
            }
        }         

        if (!validApiKey.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid ApiKey provided.");
            return;
        }

        await _next(context);
    }
}