using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
 
builder.Services.AddControllersWithViews();

// HttpClient used for accessing the API
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(uriString: builder.Configuration["ApiRoot"]
        ?? throw new ArgumentNullException("uriString"));
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
}).AddUserAccessTokenHandler();

builder.Services.AddHttpClient("IdpClient", client =>
{
    client.BaseAddress = new Uri(uriString: builder.Configuration["IdpAuthority"]
        ?? throw new ArgumentNullException("uriString"));
    client.DefaultRequestHeaders.Clear(); 
});

builder.Services.AddAuthentication(
    options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
    {
        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.Authority = "https://localhost:5001";
        options.ClientId = "democlientcodepkce";
        options.ClientSecret = "49C1A7E1-0C79-4A89-A3D6-A37998FB86B0";
        options.ResponseType = "code";
        options.Scope.Add("claimsapi");
        options.SaveTokens = true; 
    });
 
builder.Services.AddOpenIdConnectAccessTokenManagement();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseStaticFiles();

app.UseAuthentication();
 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); 

app.Run();
