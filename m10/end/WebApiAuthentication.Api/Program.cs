using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using WebApiAuthentication.Api;
using WebApiAuthentication.Api.Authorization;
using WebApiAuthentication.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAuthorization(authorizationOptions =>
{
    authorizationOptions.AddPolicy(
        AuthorizationPolicies.MustHaveGoldSubscriptionAndBeOver21
        , AuthorizationPolicies.MustBeGoldAndOlderThan21());

    authorizationOptions.AddPolicy(
           PolicyMetadata.MustHaveCreatedWeatherForecast,
            policyBuilder =>
            {
                policyBuilder.RequireAuthenticatedUser();
                policyBuilder.AddRequirements(
                        new MustHaveCreatedWeatherForecastRequirement());
            }); 
});

builder.Services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();
builder.Services.AddScoped<IAuthorizationHandler, MustHaveCreatedWeatherForecastHandler>();


builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer();  

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();