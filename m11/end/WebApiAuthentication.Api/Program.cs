using Microsoft.IdentityModel.Tokens;
using WebApiAuthentication.Api.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(PolicyMetadata.WeatherForecastRead, policyBuilder =>
            {
                policyBuilder.RequireAuthenticatedUser();
                policyBuilder.RequireClaim("scope", "weatherforecastapi.read");
            });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(PolicyMetadata.WeatherForecastWrite, policyBuilder =>
    {
        policyBuilder.RequireAuthenticatedUser();
        policyBuilder.RequireClaim("scope", "weatherforecastapi.write");
    });

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();