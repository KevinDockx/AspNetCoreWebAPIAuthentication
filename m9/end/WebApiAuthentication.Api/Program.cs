using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAuthorization(authorizationOptions =>
{ 
    authorizationOptions.AddPolicy(
        "MustBeGoldAndOlderThan21", policyBuilder =>
        { 
            policyBuilder.RequireAuthenticatedUser();
            policyBuilder.RequireClaim("subscriptionlevel", "gold");
            policyBuilder.RequireAssertion(context =>
            {
                var ageClaim = context.User.Claims.FirstOrDefault(c => c.Type == "age");
                if (ageClaim != null && int.TryParse(ageClaim.Value, out var age))
                {
                    return age >= 21;
                }
                return false;
            });
        }); 
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