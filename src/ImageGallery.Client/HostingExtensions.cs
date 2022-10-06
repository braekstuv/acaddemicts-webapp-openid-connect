using System.IdentityModel.Tokens.Jwt;
using IdentityModel;
using ImageGallery.Client.HttpHandlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

namespace ImageGallery.Client;

public static class HostingExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllersWithViews()
             .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddTransient<BearerTokenHandler>();

        // create an HttpClient used for accessing the API
        builder.Services.AddHttpClient("APIClient", client =>
        {
            client.BaseAddress = new Uri("https://localhost:44366/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        }).AddHttpMessageHandler<BearerTokenHandler>();

        // create an HttpClient used for accessing the IDP
        builder.Services.AddHttpClient("IDPClient", client =>
        {
            client.BaseAddress = new Uri("https://localhost:5001/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        });

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // Will prevent unwanted mapping of claimtypes

        builder.Services.AddAuthentication(options =>
        {
            // the default scheme should be unique for every app on the same domain,
            // so taht cookies don't interfere with eachother
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.AccessDeniedPath = "/Authorization/AccessDenied";
        })
        .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
        {
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.Authority = "https://localhost:5001";
            options.ClientId = "imagegalleryclient";
            options.ResponseType = "code";
            //Removed duplicate code. For more info, see:
            // https://github.com/dotnet/aspnetcore/blob/3ea008c80d5cc63de7f90ddfd6823b7b006251ff/src/Security/Authentication/OpenIdConnect/src/OpenIdConnectOptions.cs
            options.Scope.Add("address");
            options.Scope.Add("roles");
            options.Scope.Add("imagegalleryapi");
            options.ClaimActions.DeleteClaim("sid");
            options.ClaimActions.DeleteClaim("idp");
            options.ClaimActions.DeleteClaim("s_hash");
            options.ClaimActions.DeleteClaim("auth_time");
            options.ClaimActions.MapUniqueJsonKey("role", "role");
            options.SaveTokens = true;
            options.ClientSecret = "secret";
            options.GetClaimsFromUserInfoEndpoint = true;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                NameClaimType = JwtClaimTypes.GivenName,
                RoleClaimType = JwtClaimTypes.Role,
            };
        });
    }
}
