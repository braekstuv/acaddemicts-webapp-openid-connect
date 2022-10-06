using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Net.Http.Headers;

namespace ImageGallery.Client;

public static class HostingExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllersWithViews()
             .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

        // create an HttpClient used for accessing the API
        builder.Services.AddHttpClient("APIClient", client =>
        {
            client.BaseAddress = new Uri("https://localhost:44366/");
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
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
        {
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.Authority = "https://localhost:5001";
            options.ClientId = "imagegalleryclient";
            options.ResponseType = "code";
            options.UsePkce = true; //set to true by default
            //options.CallbackPath = new PathString("...");
            options.SignedOutCallbackPath = "/signout-callback-oidc"; // by default
            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.SaveTokens = true;
            options.ClientSecret = "secret";
            options.GetClaimsFromUserInfoEndpoint = true;
        });
    }
}
